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
                Game.LogVerboseDebug(String.Format("Set LastSelected with {0}", value == null));
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
            LHandle handle = null;
            Ped suspect = null;
            if (_Blips.Count == 0) return;
            handle = Functions.GetCurrentPullover();
            if (handle != null)
                suspect = Functions.GetPulloverSuspect(handle);
            lock (_Blips) {
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

        

        static DateTime RandomDay()
        {
            Random gen = new Random();
            DateTime start = new DateTime(1985, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        internal static Vehicle FindPedVehicle(Ped ped)
        {
            return World.GetAllVehicles().Where(x => x.HasDriver && x.Driver == ped).First();
        }

        internal static ComputerPlusEntity LookupVehicle(String vehicleTag)
        {
            var vehicle = World.EnumerateVehicles().Where(x => x.LicensePlate.Equals(vehicleTag.ToUpper())).First();
            return vehicle != null ? LookupVehicle(vehicle) : null;
        }

        internal static ComputerPlusEntity LookupVehicle(Vehicle vehicle)
        {
            var ownerName = Functions.GetVehicleOwnerName(vehicle);
            var driver = vehicle.HasDriver ? vehicle.Driver : null;           
            Tuple<Ped, Persona> owner = ComputerPedController.Instance.LookupPersona(ownerName);
            if(owner == null || owner.Item1 == null)
            {
                Game.LogVerboseDebug(String.Format("LookupVehicle owner was null, performing fixup on {0}", ownerName));

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
                    Game.LogVerboseDebug("Selecting a new random ped for fixup");
                    int position = rnd.Next(0, peds.Count() - 1);
                    ped = peds.ElementAt(position);
                }
                Game.LogVerboseDebug("Found a new ped for fixup");
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
                owner = new Tuple<Ped, Persona>(ped, persona);
            }
            bool? hasInsurance = null, isRegistered = null;
            if (Function.IsTrafficPolicerRunning())
            {
                hasInsurance = TrafficPolicerFunction.GetVehicleInsuranceStatus(vehicle) == EVehicleStatus.Valid ? true : false;
                isRegistered = TrafficPolicerFunction.GetVehicleRegistrationStatus(vehicle) == EVehicleStatus.Valid ? true : false;
            }
            //Mark the ped as persistent so they do not get GC
            //owner.Item1.MakePersistent();
            return new ComputerPlusEntity(
                owner.Item1,
                owner.Item2,
                vehicle,                
                new VehiclePersona(hasInsurance, isRegistered)
            );
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
           // if (!timer.Enabled)
            //    timer.Start();
            var blip = vehicle.AddBlipSafe(color);
            _Blips.Add(blip, vehicle);
            return blip;
        }
        //private static Vector3 lastPosition = Vector3.Zero;
        //private static bool hasBlip = false, hasCheckpoint = false;

        public static void RunVanillaAlpr()
        {
            Game.LogVerboseDebug("RunVanillaAlpr");
            if (VanillaAlprGameFiber.IsHibernating)
            {
                Game.LogVerboseDebug("Wake RunVanillaAlpr");
                EventHandler handler = (EventHandler)OnStopAlprVanilla;
                if (handler != null)
                {
                    handler(null, null);
                }

                VanillaAlprGameFiber.Wake();
            }
            else if (!VanillaAlprGameFiber.IsAlive && !VanillaAlprGameFiber.IsSleeping)
            {
                Game.LogVerboseDebug("Start RunVanillaAlpr");
                VanillaAlprGameFiber.Start();
            }
        }
        public static void StopVanillaAlpr()
        {
            Game.LogVerboseDebug("StopVanillaAlpr");
            if (!VanillaAlprGameFiber.IsHibernating && VanillaAlprGameFiber.IsAlive)
            {
                EventHandler handler = (EventHandler)OnStopAlprVanilla;
                if(handler != null)
                {
                    Game.LogVerboseDebug("StopVanillaAlpr handler");
                    handler(null, null);
                }
                else
                {
                    Game.LogVerboseDebug("StopVanillaAlpr no handler");
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
            Game.LogVerboseDebug("Executing VanillaALPR");
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
                                Game.LogVerboseDebug(String.Format("Detected plate: {0}", x.LicensePlate));
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
                                    Game.LogVeryVerboseDebug("Vehicle detected FRONT");
                                    entry = new ALPR_Arguments(x, ALPR_Position.FRONT);

                                }
                                else if (x.Position.DistanceTo(rear) <= ReadDistanceThreshold)
                                {
                                    Game.LogVeryVerboseDebug("Vehicle detected REAR");
                                    entry = new ALPR_Arguments(x, ALPR_Position.REAR);
                                }
                                else if (x.Position.DistanceTo(driver) <= ReadDistanceThreshold)
                                {
                                    Game.LogVeryVerboseDebug("Vehicle detected DRIVER");
                                    entry = new ALPR_Arguments(x, ALPR_Position.DRIVER);
                                }
                                else if (x.Position.DistanceTo(passenger) <= ReadDistanceThreshold)
                                {
                                    Game.LogVeryVerboseDebug("Vehicle detected PASSENGER");
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
                                        Game.LogVerboseDebug(msg);
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
                Game.LogVerboseDebug("Init new ComputerVehicleSearch");
                form.Show();
                while (form.IsOpen())
                    GameFiber.Yield();
                Game.LogVerboseDebug("Close ComputerVehicleSearch");
                form.Close();
                Game.LogVerboseDebug("ShowVehicleSearch Hibernating");
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
                    Game.LogVerboseDebug("Init new ComputerVehicleDetails");
                    form.Show();
                    while (form.IsOpen())
                        GameFiber.Yield();
                    Game.LogVerboseDebug("Close ComputerVehicleDetails");
                    form.Close();
                }
                Game.LogVerboseDebug("ShowVehicleDetails Hibernating");
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
