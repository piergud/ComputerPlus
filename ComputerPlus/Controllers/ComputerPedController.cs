using System;
using System.Collections.Generic;
using System.Linq;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage.Forms;
using ComputerPlus.Controllers.Models;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus.Interfaces.ComputerPedDB
{
   

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
            var ped = peds.Where(p => p && Functions.GetPersonaForPed(p).FullName.ToLower().Equals(name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (ped == null) return null;
            return LookupPersona(ped);
        }
       
        internal ComputerPlusEntity LookupPersona(Ped ped)
        {
            if (ped == null) return null;
            var persona = Functions.GetPersonaForPed(ped);
            if (persona == null) return null;
            var entity = new ComputerPlusEntity(ped, persona);            
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
                Function.LogDebug("ShowPedSearch Show");
                form.Show();
                while (form.IsOpen() && !Globals.CloseRequested)
                    GameFiber.Yield();                
                form.Close();
                Function.LogDebug("ShowPedSearch Hibernating");
                GameFiber.Hibernate();
            }
        }

        private static void ShowPedView()
        {
            while (true)
            {
                var form = new ComputerPedView(LastSelected);                
                form.Show();
                Function.LogDebug("Show ComputerPedView");
                while (form.IsOpen() && !Globals.CloseRequested)
                    GameFiber.Yield();
                Function.LogDebug("Close ComputerPedView");
                form.Close();
                Function.LogDebug("ShowPedView Hibernating");
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
