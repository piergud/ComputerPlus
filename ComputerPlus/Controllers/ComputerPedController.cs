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
using ComputerPlus.Controllers;
using ComputerPlus.Interfaces.Reports.Models;

namespace ComputerPlus.Interfaces.ComputerPedDB
{


    class ComputerPedController
    {
        private readonly static List<ComputerPlusEntity> RecentSearches = new List<ComputerPlusEntity>();       

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
            var ped = peds.Where(p => p 
                && Functions.GetPersonaForPed(p).FullName.ToLower().Equals(name, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();
            if (ped == null) return null;
            return ComputerPlusEntity.CreateFrom(ped);
        }
       
        internal ComputerPlusEntity LookupPersona(Ped ped)
        {
            if (ped == null) return null;
            var entity = ComputerPlusEntity.CreateFrom(ped);
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


        internal static void ShowPedSearch()
        {
            Globals.Navigation.Push(new ComputerPedSearch());
        }

        internal async static void ShowPedView()
        {
            if (!LastSelected || !LastSelected.Validate()) return;
            try
            {
                var reports = await ComputerReportsController.GetArrestReportsForPedAsync(LastSelected);
                var trafficCitations = await ComputerReportsController.GetTrafficCitationsForPedAsync(LastSelected);
                if (trafficCitations != null) Function.Log("Found citations for ped");
                else Function.Log("Citations for ped are null");
                Globals.Navigation.Push(new ComputerPedViewExtended(new DetailedEntity(LastSelected, reports, trafficCitations)));
            } catch (Exception e)
            {
                Function.Log(e.ToString());
            }
        }

        protected internal static void ActivatePedView()
        {
            //@TODO This method may not be needed
            ShowPedView();
        }

        public static ComputerPedController Instance
        {
            get;
            private set;
        } = new ComputerPedController();

    }
}
