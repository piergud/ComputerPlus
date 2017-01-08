using System;
using System.Collections.Generic;
using System.Linq;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage.Forms;
using ComputerPlus.Controllers.Models;

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
        private readonly static List<ComputerPlusEntity> RecentSearches = new List<ComputerPlusEntity>();       
        public static GameFiber PedSearchGameFiber = new GameFiber(ShowPedSearch);
        public static GameFiber PedViewGameFiber = new GameFiber(ShowPedView);

        private static ComputerPlusEntity _LastSelected = null;
        public static ComputerPlusEntity LastSelected
        {
            get
            {
                if (_LastSelected != null && _LastSelected.Validate())
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

        internal ComputerPlusEntity LookupPersona(String name)
        {
            List<Ped> peds = World.GetAllPeds().ToList();
            peds.RemoveAll(p => !p || !p.Exists());
            peds.OrderBy(p => p.DistanceTo(Game.LocalPlayer.Character.Position));
            var ped = peds.Where(p => p && Functions.GetPersonaForPed(p).FullName.ToLower().Equals(name.ToLower())).FirstOrDefault();
            return LookupPersona(ped);
        }
       
        internal ComputerPlusEntity LookupPersona(Ped ped)
        {
            if (!ped)
            {
                return null;
            }
            var entity = new ComputerPlusEntity(ped, Functions.GetPersonaForPed(ped));            
            RecentSearches.Add(entity);
            return entity;
        }

        internal List<ComputerPlusEntity> GetRecentSearches()
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
                Game.LogVerboseDebug("ShowPedSearch Show");
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
                var form = new ComputerPedView(LastSelected);                
                form.Show();
                Game.LogVerboseDebug("Show ComputerPedView");
                while (form.IsOpen())
                    GameFiber.Yield();
                Game.LogVerboseDebug("Close ComputerPedView");
                form.Close();
                Game.LogVerboseDebug("ShowPedView Hibernating");
                GameFiber.Hibernate();
            }
        }

        protected internal static void ActivatePedView()
        {
            var fiber = ComputerPedController.PedViewGameFiber;
            if (fiber.IsHibernating) fiber.Wake();
            else if (!fiber.IsAlive) fiber.Start();
        }

        private static ComputerPedController _instance = new ComputerPedController();
        public static ComputerPedController Instance
        {
            get { return _instance; }
        }

    }
}
