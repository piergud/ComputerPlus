using System;
using System.Collections.Generic;
using System.Linq;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage.Forms;
using Rage.Native;
using ComputerPlus.Interfaces.ComputerPedDB;
using System.Drawing;
using System.Timers;
using ComputerPlus.Controllers.Models;
using ComputerPlus.Extensions.Rage;
using System.Threading.Tasks;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Controllers;
using ComputerPlus.Interfaces.Reports.Models;

namespace ComputerPlus.Interfaces.ComputerVehDB
{
  
    internal static class ComputerVehicleController
    {
        private readonly static List<ComputerPlusEntity> RecentSearches = new List<ComputerPlusEntity>();
        internal static readonly List<ALPR_Arguments> ALPR_Detected = new List<ALPR_Arguments>(10);
        private static ComputerPlusEntity _LastSelected = null;
        public static ComputerPlusEntity LastSelected
        {
            get
            {
                if(_LastSelected != null && _LastSelected.Validate())
                {
                    return _LastSelected;
                }
                return null;
            }
            set
            {
                _LastSelected = value;
            }
        }

        static internal ComputerPlusEntity CurrentlyPulledOver
        {
            get
            {
                var handle = Functions.GetCurrentPullover();
                if (handle != null)
                {
                    Ped ped = Functions.GetPulloverSuspect(handle);
                    Vehicle vehicle = FindPedVehicle(ped);                    
                    if (vehicle == null || !vehicle.Exists()) return null;

                    ComputerPlusEntity vehEntity = null;
                    bool isVehicleLookupSuccess = false;
                    while (!isVehicleLookupSuccess)
                    {
                        try
                        {
                            if (vehicle != null && vehicle.IsValid())
                            {
                                vehEntity = LookupVehicle(vehicle);
                                if (vehEntity.FirstName != null && vehEntity.LastName != null && vehEntity.DOBString != null)
                                {
                                    isVehicleLookupSuccess = true;
                                } else
                                {
                                    GameFiber.Sleep(500);
                                }
                            }
                            else
                            {
                                GameFiber.Sleep(500);
                            }
                        }
                        catch (Exception e)
                        {
                            Function.Log("Error while getting vehicle persona");
                            Function.LogCatch(e.ToString());
                            GameFiber.Sleep(500);
                        }
                    }
                    return vehEntity;
                }
                return null;
            }
        }

        private static Dictionary<Blip, Vehicle> _Blips = new Dictionary<Blip, Vehicle>();
        private static  Dictionary<Blip, Vehicle> Blips
        {
            get
            {                
                return _Blips;
            }
        }

        private static Timer timer = new Timer(3500);
        
        public readonly static GameFiber VehicleSearchGameFiber = new GameFiber(ShowVehicleSearch);
        public readonly static GameFiber VehicleDetailsGameFiber = new GameFiber(ShowVehicleDetails);
        //public readonly static GameFiber VanillaAlprGameFiber = new GameFiber(VanillaALPR);

        //public static event EventHandler<ALPR_Arguments> OnAlprVanillaMessage;


        //static event EventHandler OnStopAlprVanilla;
        //static readonly float ReadDistanceThreshold = 5f;

        static ComputerVehicleController()
        {
           
        }        

        static DateTime RandomDay()
        {
            Random gen = new Random();
            DateTime start = new DateTime(1985, 1, 1);
            int range = (DateTime.Today.AddYears(-16) - start).Days;
            return start.AddDays(gen.Next(range));
        }

        internal static Vehicle FindPedVehicle(Ped ped)
        {
            return World.EnumerateVehicles().Where(x => x.HasDriver && x.Driver == ped).DefaultIfEmpty(null).ToList().FirstOrDefault();
        }

        internal static ComputerPlusEntity LookupVehicle(String vehicleTag)
        {
           var vehicle = World.EnumerateVehicles().Where(x => x.LicensePlate.Equals(vehicleTag, StringComparison.CurrentCultureIgnoreCase)).DefaultIfEmpty(null).ToList().FirstOrDefault();
            return vehicle ? LookupVehicle(vehicle) : null;
        }

        private static ComputerPlusEntity generateVehicleOwner(string ownerName)
        {
            Ped ped = null;
            while (ped == null)
            {
                //Last ditch effort to make C+ happy by just providing any ped as the owner and setting them as the owner
                ped = FindRandomPed();
                if (ped != null && !ped.IsValid()) ped = null;
            }
            var parts = ownerName.Split(' ');
            while (parts.Length < 2)
            {
                parts = Persona.GetRandomFullName().Split(' ');
            }
            int timesStopped = Globals.Random.Next(0, 4);
            var persona = new Persona(ped, Gender.Random, RandomDay(), Globals.Random.Next(0, timesStopped + 1), 
                parts[0], parts[1], ELicenseState.Valid, timesStopped, false, false, false);
            Functions.SetPersonaForPed(ped, persona);
            return ComputerPlusEntity.CreateFrom(ped);
        }

        internal static ComputerPlusEntity LookupVehicle(Vehicle vehicle)
        {
            if (!vehicle) return null;
            var vehiclePersona = ComputerPlusEntity.GetPersonaForVehicle(vehicle);

            string ownerName = Functions.GetVehicleOwnerName(vehicle);
            ComputerPlusEntity owner = null;
            var driver = vehicle.HasDriver ? vehicle.Driver : null;
            if (driver != null)
            {
                Persona driverPersona = Functions.GetPersonaForPed(driver);
                if (!driverPersona.FullName.Equals(ownerName))
                {
                    owner = ComputerPedController.Instance.LookupPersona(ownerName);
                    if (owner == null) owner = generateVehicleOwner(ownerName);
                }
                else
                {
                    owner = ComputerPedController.Instance.LookupPersona(driver);
                }
            }
            else
            {
                owner = ComputerPedController.Instance.LookupPersona(ownerName);
                if (owner == null) owner = generateVehicleOwner(ownerName);
            }
            return ComputerPlusEntity.CloneFrom(owner, vehicle, vehiclePersona);
        } 
        
        private static Ped FindRandomPed()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            var peds = World.EnumeratePeds().Take(50).ToArray();
            Ped ped = null;
            while (!ped.Exists() || ped.RelationshipGroup == "COP")
            {
                int position = rnd.Next(0, peds.Count() - 1);
                ped = peds.ElementAt(position);
            }
            return ped;
        }       

        internal static void Cleanup()
        {
            var blips = Blips.Keys.Where(x => x != null && x.Exists()).ToList();
            blips.ForEach(x => x.Delete());            
            Blips.Clear();
            timer.Stop();
        }

       

        internal static Blip BlipVehicle(Vehicle vehicle, Color color)
        {
            if (_Blips.ContainsValue(vehicle)) return _Blips.Single(x => x.Value == vehicle).Key;
            else if (vehicle.GetAttachedBlip()) return vehicle.GetAttachedBlip();
            var blip = vehicle.AddBlipSafe(color);
            if (blip != null && (vehicle != null && vehicle.IsValid())) _Blips.Add(blip, vehicle);

            GameFiber.StartNew(() =>
            {
                var stopAt = DateTime.Now.AddMilliseconds(30000);
                while (DateTime.Now < stopAt) GameFiber.Yield();
                try {
                    if (blip != null && blip.IsValid()) blip.Delete();
                } catch (Exception e)
                {
                    Function.Log(e.Message);
                }

            });
            return blip;
        }
/*
        public static void RunVanillaAlpr()
        {
           Function.LogDebug("RunVanillaAlpr");
            if (VanillaAlprGameFiber.IsHibernating)
            {
               Function.LogDebug("Wake RunVanillaAlpr");
                EventHandler handler = (EventHandler)OnStopAlprVanilla;
                if (handler != null)
                {
                    handler(null, null);
                }

                VanillaAlprGameFiber.Wake();
            }
            else if (!VanillaAlprGameFiber.IsAlive && !VanillaAlprGameFiber.IsSleeping)
            {
               Function.LogDebug("Start RunVanillaAlpr");
                VanillaAlprGameFiber.Start();
            }
        }

        public static void StopVanillaAlpr()
        {
           Function.LogDebug("StopVanillaAlpr");
            if (!VanillaAlprGameFiber.IsHibernating && VanillaAlprGameFiber.IsAlive)
            {
                EventHandler handler = (EventHandler)OnStopAlprVanilla;
                if(handler != null)
                {
                   Function.LogDebug("StopVanillaAlpr handler");
                    handler(null, null);
                }
                else
                {
                   Function.LogDebug("StopVanillaAlpr no handler");
                }
            }
        }
*/
        public static void AddAlprScan(ALPR_Arguments args)
        {            
          ALPR_Detected.Add(args);
        }

        /*
        private static void  VanillaALPR()
        {
           Function.LogDebug("Executing VanillaALPR");
            bool shouldRun = true;
            OnStopAlprVanilla += (sender, args) =>
            {
                shouldRun = !shouldRun;
            };
            while (true)
            {
                while (shouldRun)
                {
                    var vehicle = Game.LocalPlayer.LastVehicle;
                    if (vehicle != null && vehicle.Exists() && vehicle.HasDriver && vehicle.Driver == Game.LocalPlayer.Character)
                    {
                        Vector3 front = Game.LocalPlayer.Character.GetOffsetPositionFront(ReadDistanceThreshold);
                        Vector3 rear = Game.LocalPlayer.Character.GetOffsetPositionFront((0 - vehicle.Width) - ReadDistanceThreshold);
                        Vector3 driver = Game.LocalPlayer.Character.GetOffsetPositionRight((0 - vehicle.Length) - ReadDistanceThreshold);
                        Vector3 passenger = Game.LocalPlayer.Character.GetOffsetPositionRight(ReadDistanceThreshold);
                        var nearVehicles = World.EnumerateVehicles()
                            .Where(x => x != vehicle && !ALPR_Detected.Exists(y => y.Vehicle == x) && x.IsOnScreen)
                            .Where(x => x.DistanceTo(Game.LocalPlayer.Character.Position) <= ReadDistanceThreshold * 3)
                            .Where(x => x.IsCar && x.ShouldVehiclesYieldToThisVehicle)                            
                            .Select(x =>
                            {
                               Function.LogDebug(String.Format("Detected plate: {0}", x.LicensePlate));
                                return x;
                            });
                        if (nearVehicles != null)
                        {

                            var handler = (EventHandler<ALPR_Arguments>)OnAlprVanillaMessage;
                            foreach (var x in nearVehicles)
                            {
                                ALPR_Arguments entry = null;
                                if (x.Position.DistanceTo(front) <= ReadDistanceThreshold)
                                {
                                    Function.LogDebug("Vehicle detected FRONT");
                                    entry = new ALPR_Arguments(x, ALPR_Position.FRONT);

                                }
                                else if (x.Position.DistanceTo(rear) <= ReadDistanceThreshold)
                                {
                                    Function.LogDebug("Vehicle detected REAR");
                                    entry = new ALPR_Arguments(x, ALPR_Position.REAR);
                                }
                                else if (x.Position.DistanceTo(driver) <= ReadDistanceThreshold)
                                {
                                    Function.LogDebug("Vehicle detected DRIVER");
                                    entry = new ALPR_Arguments(x, ALPR_Position.DRIVER);
                                }
                                else if (x.Position.DistanceTo(passenger) <= ReadDistanceThreshold)
                                {
                                    Function.LogDebug("Vehicle detected PASSENGER");
                                    entry = new ALPR_Arguments(x, ALPR_Position.PASSENGER);
                                }

                                if (entry != null)
                                {                                    
                                    AddAlprScan(entry);
                                    var data = LookupVehicle(entry.Vehicle);
                                    if (data != null && data.IsWanted)
                                    {
                                        var msg = String.Format("~r~Wanted Owner:~w~ {0} {1} {2}", data.Vehicle.Model.Name, data.Vehicle.LicensePlate, data.FullName);
                                        Game.DisplayNotification(msg);
                                       Function.Log(msg);
                                    }
                                    if (handler != null)
                                        handler(null, entry);
                                }
                            }
                        }
                    }
                    GameFiber.Yield();
                }
                GameFiber.Hibernate();
            }
        }
        */
        
        internal static void ShowVehicleSearch()
        {
            Globals.Navigation.Push(new ComputerVehicleSearch());
        }

        internal static void ShowVehicleDetails()
        {
            if (!LastSelected || !LastSelected.Validate()) return;
            var reports = ComputerReportsController.GetArrestReportsForPed(LastSelected);
            var trafficCitations = ComputerReportsController.GetTrafficCitationsForPed(LastSelected);

            if (LastSelected != null && LastSelected.Vehicle != null && LastSelected.Vehicle.IsValid() 
                && LastSelected.Ped != null && LastSelected.Ped.IsValid())
            {
                Globals.Navigation.Push(new ComputerVehicleViewExtendedContainer(new DetailedEntity(LastSelected, reports, trafficCitations)));
            }
        }
    }
}
