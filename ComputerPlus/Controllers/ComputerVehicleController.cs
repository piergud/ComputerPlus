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

namespace ComputerPlus.Interfaces.ComputerVehDB
{
    internal static class ComputerVehicleController
    {
        private readonly static List<ComputerPlusEntity> RecentSearches = new List<ComputerPlusEntity>();
        internal static readonly List<ALPR_Arguments> _ALPR_Detected = new List<ALPR_Arguments>(10);
        internal static List<ALPR_Arguments> ALPR_Detected 
        {
            get {
                lock (_ALPR_Detected)
                {
                    var data = _ALPR_Detected.ToList().Where(x => x.Vehicle != null && x.Vehicle.Exists()).ToList();
                    _ALPR_Detected.Clear();
                    _ALPR_Detected.AddRange(data);
                    return data;
                }
            }
        }
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
                Function.Log("CurrentPulledOver get handle");
                var handle = Functions.GetCurrentPullover();
                Function.Log("CurrentPulledOver got handle");
                if (handle != null)
                {
                    Function.Log("CurrentPulledOver get suspect handle");
                    Ped ped = Functions.GetPulloverSuspect(handle);
                    Function.Log("CurrentPulledOver got suspect handle");
                    Vehicle vehicle = FindPedVehicle(ped);
                    Function.Log("CurrentPulledOver found vehicle");
                    if (vehicle == null || !vehicle.Exists()) return null;
                    Function.Log("CurrentPulledOver return LookupVehicle");
                    return LookupVehicle(vehicle);
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
        public readonly static GameFiber VanillaAlprGameFiber = new GameFiber(VanillaALPR);

        public static event EventHandler<ALPR_Arguments> OnAlprVanillaMessage;


        static event EventHandler OnStopAlprVanilla;
        static readonly float ReadDistanceThreshold = 5f;

        static ComputerVehicleController()
        {
         //   timer.Elapsed += BlipCleanup;
        }

        private static void BlipCleanup(object sender, ElapsedEventArgs e)
        {
            try
            {
                LHandle handle = null;
                Ped suspect = null;
                if (_Blips.Count == 0) return;
                handle = Functions.GetCurrentPullover();
                if (handle != null)
                    suspect = Functions.GetPulloverSuspect(handle);
                lock (_Blips)
                {
                    var data = _Blips.ToList();
                    _Blips.Clear();
                    data.Where(x => (x.Key != null && x.Key.Exists()) && (x.Value != null && x.Value.Exists()))
                    .ToList()
                    .ForEach(x =>
                    {

                        if (suspect != null && suspect.Exists())
                        {

                            if (suspect.LastVehicle != null && suspect.LastVehicle == x.Value && x.Key != null && x.Key.Exists())
                            {
                                x.Key.Delete();
                            }
                            else
                            {
                                _Blips.Add(x.Key, x.Value);
                            }
                        }
                        else
                            _Blips.Add(x.Key, x.Value);
                    });
                }
            }
            catch { }
        }

        

        static DateTime RandomDay()
        {
            Random gen = new Random();
            DateTime start = new DateTime(1985, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        internal static Vehicle FindPedVehicle(Ped ped)
        {
            return World.EnumerateVehicles().Where(x => x.HasDriver && x.Driver == ped).DefaultIfEmpty(null).ToList().FirstOrDefault();
        }

        internal static ComputerPlusEntity LookupVehicle(String vehicleTag)
        {
           var vehicle = World.EnumerateVehicles().Where(x => x.LicensePlate.Equals(vehicleTag.ToUpper())).DefaultIfEmpty(null).ToList().FirstOrDefault();
            return vehicle ? LookupVehicle(vehicle) : null;
        }

        internal static ComputerPlusEntity LookupVehicle(Vehicle vehicle)
        {
            if (!vehicle) return null;
            var vehiclePersona = new VehiclePersona();
            if (Function.IsTrafficPolicerRunning())
            {
                vehiclePersona.HasInsurance = TrafficPolicerFunction.GetVehicleInsuranceStatus(vehicle) == EVehicleStatus.Valid ? true : false;
                vehiclePersona.IsRegistered = TrafficPolicerFunction.GetVehicleRegistrationStatus(vehicle) == EVehicleStatus.Valid ? true : false;
            }
            
            var ownerName = Functions.GetVehicleOwnerName(vehicle);
            var driver = vehicle.HasDriver ? vehicle.Driver : null;           
            ComputerPlusEntity owner = ComputerPedController.Instance.LookupPersona(ownerName);
            if(!owner.Validate())
            {
                Function.Log(String.Format("LookupVehicle owner was null, performing fixup on {0}", ownerName));

                var parts = ownerName.Split(' ');
                while(parts.Length < 2)
                {
                    parts = LSPD_First_Response.Engine.Scripting.Entities.Persona.GetRandomFullName().Split(' ');
                }
                Functions.SetVehicleOwnerName(vehicle, String.Format("{0} {1}", parts[0], parts[1]));
                //Work some magic to fix the fact that the ped hasn't been spawned in game
                //@TODO parse ped model name for age group and randomize other props
                var rnd = new Random(DateTime.Now.Millisecond);
                
                var peds = World.GetAllPeds();
                Ped ped = null;
                while (ped == null || !ped.Exists())
                {
                    Function.Log("Selecting a new random ped for fixup");
                    int position = rnd.Next(0, peds.Count() - 1);
                    ped = peds.ElementAt(position);
                }
                Function.Log("Found a new ped for fixup");
                var persona = new Persona(
                    ped,
                    Gender.Random,
                    RandomDay(),
                    3,
                    parts[0],
                    parts[1],
                    ELicenseState.Valid,
                    1,
                    false,
                    false,
                    false
                    );
                Functions.SetPersonaForPed(ped, persona);
                owner.PedPersona = persona;
            }
           
          
            return new ComputerPlusEntity(owner.Ped, owner.PedPersona, vehicle, vehiclePersona);
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

        public static void AddAlprScan(ALPR_Arguments args)
        {
            lock(_ALPR_Detected)
                _ALPR_Detected.Add(args);
        }

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
                                    if (data != null && data.PedPersona.Wanted)
                                    {
                                        var msg = String.Format("~r~Wanted Owner:~w~ {0} {1} {2}", data.Vehicle.Model.Name, data.Vehicle.LicensePlate, data.PedPersona.FullName);
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
        
        private static void ShowVehicleSearch()
        {
            while (true)
            {
                var form = new ComputerVehicleSearch();
               Function.LogDebug("Init new ComputerVehicleSearch");
                form.Show();
                while (form.IsOpen())
                    GameFiber.Yield();
               Function.LogDebug("Close ComputerVehicleSearch");
                form.Close();
               Function.LogDebug("ShowVehicleSearch Hibernating");
                GameFiber.Hibernate();
            }
        }

        private static void ShowVehicleDetails()
        {
            
            while (true)
            {
                if (LastSelected != null && LastSelected.Validate())
                {
                    var form = new ComputerVehicleDetails(LastSelected);
                   Function.LogDebug("Init new ComputerVehicleDetails");
                    form.Show();
                    while (form.IsOpen())
                        GameFiber.Yield();
                   Function.LogDebug("Close ComputerVehicleDetails");
                    form.Close();
                }
               Function.LogDebug("ShowVehicleDetails Hibernating");
                GameFiber.Hibernate();
            }
        }
    }

    public static class EqualityComparerFactory<T>
    {
        private class MyComparer : IEqualityComparer<T>
        {
            private readonly Func<T, int> _getHashCodeFunc;
            private readonly Func<T, T, bool> _equalsFunc;

            public MyComparer(Func<T, int> getHashCodeFunc, Func<T, T, bool> equalsFunc)
            {
                _getHashCodeFunc = getHashCodeFunc;
                _equalsFunc = equalsFunc;
            }

            public bool Equals(T x, T y)
            {
                return _equalsFunc(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _getHashCodeFunc(obj);
            }
        }

        public static IEqualityComparer<T> CreateComparer(Func<T, int> getHashCodeFunc, Func<T, T, bool> equalsFunc)
        {
            if (getHashCodeFunc == null)
                throw new ArgumentNullException("getHashCodeFunc");
            if (equalsFunc == null)
                throw new ArgumentNullException("equalsFunc");

            return new MyComparer(getHashCodeFunc, equalsFunc);
        }
    }
}
