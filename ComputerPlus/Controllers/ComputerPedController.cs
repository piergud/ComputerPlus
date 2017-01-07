using System;
using System.Collections.Generic;
using System.Linq;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage.Forms;

namespace ComputerPlus.Interfaces.ComputerPedDB
{
    internal static class PedExtension
    {
        internal static String GetHomeAddress(this Ped ped)
        {
            if (ped.Metadata.HomeAddress == null) ped.Metadata.HomeAddress = ComputerPedController.GetRandomStreetAddress();
            return ped.Metadata.HomeAddress;
        }
    }
    internal static class ListExtension
    {
        internal static Tuple<Ped, Persona> AddPed(this List<Tuple<Ped, Persona>> list, Ped ped, Persona persona, bool allowDuplicates = true)
        {
            if (allowDuplicates == false && list.Exists(x => x.Item1 == ped || x.Item2.FullName.Equals(persona.FullName)))
            {
                return list.First(x => x.Item1 == ped || x.Item2.FullName.Equals(persona.FullName));
            }
            var t = new Tuple<Ped, Persona>(ped, persona);
            list.Add(t);
            return t;
        }
    }

    internal static class GwenExtension
    {
        internal static void Close(this GwenForm form)
        {
            if (form.Window.IsClosable)
                form.Window.Close();
        }
        internal static bool IsOnTop(this GwenForm form)
        {
            try
            {
                return form.Window.IsOnTop;
            }
            catch
            {
                return false;
            }
        }

        internal static bool IsOpen(this GwenForm form)
        {
            try
            {
                return form.Window.IsVisible;
            }
            catch
            {
                return false;
            }
        }
    }

    class ComputerPedController
    {
        private List<Tuple<Ped, Persona>> RecentSearches = new List<Tuple<Ped, Persona>>();
        public static Tuple<Ped, Persona> LastSelected = null;

        public static GameFiber PedSearchGameFiber = new GameFiber(ShowPedSearch);
        public static GameFiber PedViewGameFiber = new GameFiber(ShowPedView);



        internal List<Ped> PedsCurrentlyStoppedByPlayer
        {
            get
            {
                LHandle pulloverHandle = Functions.GetCurrentPullover();
                
                Ped pulledOverSuspect = (pulloverHandle != null) ? Functions.GetPulloverSuspect(pulloverHandle) : null;
                return World.EnumeratePeds().Where(x => {
                    return (Functions.IsPedArrested(x) || Functions.IsPedGettingArrested(x) || Functions.IsPedStoppedByPlayer(x) || (pulledOverSuspect != null && pulledOverSuspect == x));
                }).ToList();
            }
        }

        private ComputerPedController()
        {
            LastSelected = null;
        }

        internal Tuple<Ped, Persona> LookupPersona(String name)
        {
            List<Ped> peds = World.GetAllPeds().ToList();
            peds.RemoveAll(p => !p || !p.Exists());
            peds.OrderBy(p => p.DistanceTo(Game.LocalPlayer.Character.Position));
            var ped = peds.Where(p => p && Functions.GetPersonaForPed(p).FullName.ToLower().Equals(name.ToLower())).FirstOrDefault();
            var persona =  LookupPersona(ped);
            return persona;
        }
       
        internal Tuple<Ped, Persona> LookupPersona(Ped ped)
        {
            if (ped == null || !ped.Exists())
            {
                return null;
            }
            var search = new Tuple<Ped, Persona>(ped, Functions.GetPersonaForPed(ped));
            
            RecentSearches.Add(search);
            return search;
        }

        internal List<Tuple<Ped,Persona>> GetRecentSearches()
        {
            return RecentSearches;
        }

        internal static String GetRandomStreetAddress()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int streetNum = rnd.Next(0, 5000);
            var street = World.GetStreetName(World.GetStreetHash(World.GetRandomPositionOnStreet()));
            return String.Format("{0} {1}", streetNum, street);
        }


        private static void ShowPedSearch()
        {
            while (true)
            {
                var form = new ComputerPedSearch();
                form.Show();
                while (form.IsOpen())
                    GameFiber.Yield();                
                form.Close();
                Game.LogVerboseDebug("ShowPedSearch Hibernating");
                GameFiber.Hibernate();
            }
        }

        private static void ShowPedView()
        {
            while (true)
            {
                var form = new ComputerPedView(LastSelected.Item2, LastSelected.Item1);
                Game.LogVerboseDebug(String.Format("Init new ComputerPedView for {0}", LastSelected.Item2.FullName));
                form.Show();
                while (form.IsOpen())
                    GameFiber.Yield();
                Game.LogVerboseDebug("Close ComputerPedView");
                form.Close();
                Game.LogVerboseDebug("ShowPedView Hibernating");
                GameFiber.Hibernate();
            }
        }

        private static ComputerPedController _instance = new ComputerPedController();
        public static ComputerPedController Instance
        {
            get { return _instance; }
        }

    }
}
