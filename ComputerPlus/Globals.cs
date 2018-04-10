using ComputerPlus.API;
using System;
using System.Collections.Generic;
using System.Linq;
using ComputerPlus.Controllers;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.DB;

namespace ComputerPlus
{
    internal class Styles
    {
        internal Gwen.Font RegularFont;
        internal Gwen.Font BoldFont;
        internal Gwen.Font LabelHeaderFont;
        internal Gwen.Font LabelHeaderFontBold;
    }
    internal static class Globals
    {
        internal static Random Random = new Random();
        internal static bool IsPlayerOnDuty = false;
        internal static List<CalloutData> CallQueue = new List<CalloutData>();
        internal static Guid ActiveCallID = Guid.Empty;
        internal static bool IsCalloutActive = false;
        internal static int WebAPIFileId = 11453;
        internal static Guid ActiveExternalUI_ID = Guid.Empty;
        internal static List<ExternalUI> ExternalUI = new List<API.ExternalUI>();
        internal readonly static String EmptyImageVehicle = "no_vehicle_image.jpg";
        internal readonly static String EmptyImagePed = "no_ped_image.jpg";
        internal readonly static String DefaultBackgroundImage = "generic.jpg";

        private static bool mPauseGameWhenOpen = true;
        internal static bool PauseGameWhenOpen
        {
            get { return mPauseGameWhenOpen; }
            set
            {
                mPauseGameWhenOpen = value;
                BlockInputNeeded = value;
            }
        }
        internal static bool ShowBackgroundWhenOpen = true;
        internal static bool CloseRequested = false;
        internal static bool OpenRequested = false;
        internal static Rage.Object tablet = null;
        internal static NavigationController Navigation = new NavigationController();
        internal static String SimpleNotepadText = String.Empty;
        private static String Clipboard = String.Empty;
        internal static readonly String DefaultAssetPath = @"Plugins\LSPDFR\ComputerPlus\";
        internal static Storage Store;
        internal static readonly Version SchemaVersion = new Version("1.0.2");
        internal static bool? BlockInputNeeded;
        internal static List<Rage.Entity> persistedRageEntities = new List<Rage.Entity>();

        internal static Styles Style = new Styles();


        internal static void OpenStore()
        {
            if (Store == null)
            {
                Store = new Storage();
                Store.initDB();
            }
        }

        static public ArrestReport PendingArrestReport
        {
            get;
            internal set;
        } = new ArrestReport();

        static public TrafficCitation PendingTrafficCitation
        {
            get;
            internal set;
        } = null;

        static private Dictionary<Rage.Ped, List<TrafficCitation>> TicketsInPosession = new Dictionary<Rage.Ped, List<TrafficCitation>>();

        public static ChargeCategories ChargeDefinitions
        {
            get;
            internal set;
        } = null;

        public static CitationCategories CitationDefinitions
        {
            get;
            internal set;
        } = null;

        public static VehicleDefinitions VehicleDefinitions
        {
            get;
            internal set;
        } = null;

        public static List<String> WantedReasons
        {
            get;
            internal set;
        } = null;

        public static List<CitationDefinition> CitationList
        {
            get;
            internal set;
        } = null;


        internal static bool HasTrafficTicketsInHand()
        {
            return TicketsInPosession.Any(x => x.Key && x.Value.Count > 0);
        }

        internal static List<TrafficCitation> GetTrafficCitationsInHandForPed(Rage.Ped ped)
        {
            if (!ped || !TicketsInPosession.ContainsKey(ped)) return null;
            return TicketsInPosession[ped];
        }


        internal static void RemoveTrafficCitationsInHandForPed(Rage.Ped ped)
        {
            if (ped != null) //dont care about game validility of Ped
            {
                if (TicketsInPosession.ContainsKey(ped))
                {
                    TicketsInPosession.Remove(ped);
                }
            }
        }

        internal static void ClearTrafficCitationsInHand()
        {
            TicketsInPosession.Clear();
        }

        internal static void AddTrafficCitationsInHandForPed(Rage.Ped ped, TrafficCitation citation)
        {
            if (ped)
            {
                if (TicketsInPosession.ContainsKey(ped))
                {
                    TicketsInPosession[ped].Add(citation);
                }
                else
                {
                    TicketsInPosession.Add(ped, new List<TrafficCitation>() { citation });
                }
            }
        }

        /// <summary>
        /// Returns the active callout from the queue.
        /// This property is readonly, and should NOT be used for updating data.
        /// </summary>
        internal static CalloutData ActiveCallout
        {
            get
            {
                if (ActiveCallID == Guid.Empty)
                    return null;
                else
                    return (from x in CallQueue where x.ID == ActiveCallID select x).FirstOrDefault();
            }
        }

        internal static IOrderedEnumerable<ExternalUI> SortedExternalUI
        {
            get
            {
                return ExternalUI.OrderBy(x => x.DisplayName);
            }
        }

        internal static ExternalUI ActiveExternalUI
        {
            get
            {
                if (ActiveExternalUI_ID == Guid.Empty)
                    return null;
                var results = ExternalUI.Where(x => x.Identifier == ActiveExternalUI_ID).DefaultIfEmpty(null).FirstOrDefault();
                if (results != null) return results;
                Function.Log("ActiveExternalUI_ID is not Empty, but there is no matching interface");
                return null;
            }
        }

        internal static String GetClipboard(bool clearAfter = false)
        {
            var clip = Clipboard;
            if (clearAfter) SetClipboard(null);
            return clip;
        }

        internal static void SetClipboard(String text)
        {
            if (text == null) Clipboard = String.Empty;
            else Clipboard = String.Empty;
        }

    }
}