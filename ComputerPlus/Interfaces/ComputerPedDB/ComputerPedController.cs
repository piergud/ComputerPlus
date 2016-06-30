using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using System.Drawing;
using Rage.Forms;

namespace ComputerPlus.Interfaces.ComputerPedDB
{
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

    internal static class ListBoxExtension
    {
        internal static void AddPed(this Gwen.Control.ListBox listBox, Tuple<Ped, Persona> ped)
        {
            // "{0} {1} {2} {3}" ped.Item2.Wanted ? String.Empty : "WANTED "
            listBox.AddRow(
                String.Format("({0}) {1} | {2}", ped.Item2.Gender == Gender.Male ? "M" : "F", ped.Item2.FullName, ped.Item2.BirthDay.ToString("MMMM dd yyyy")),
                String.Format("{0}_{1}", ped.Item2.Forename, ped.Item2.Surname),
                ped);
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
                return World.GetAllPeds().Where(x => {
                    return (Functions.IsPedStoppedByPlayer(x) || (pulledOverSuspect != null && pulledOverSuspect == x));
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
            peds.RemoveAll(p => !p || !p.IsValid());
            peds.OrderBy(p => p.DistanceTo(Game.LocalPlayer.Character.Position));
            var ped = peds.Where(p => p && Functions.GetPersonaForPed(p).FullName.ToLower() == name).DefaultIfEmpty(null).FirstOrDefault();
            return LookupPersona(ped);
        }
       
        internal Tuple<Ped, Persona> LookupPersona(Ped ped)
        {
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
            //For some reason it's not updating the view for second ped lookups
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
