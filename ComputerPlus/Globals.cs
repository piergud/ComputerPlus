using ComputerPlus.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Controllers;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.DB;
using Gwen;

namespace ComputerPlus
{
    internal class Styles
    {
        internal Font RegularFont;
        internal Font BoldFont;
        internal Font LabelHeaderFont;
        internal Font LabelHeaderFontBold;
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
        internal static bool PauseGameWhenOpen = true;
        internal static bool ShowBackgroundWhenOpen = true;
        internal static bool CloseRequested = false;
        internal static bool OpenRequested = false;
        internal static NavigationController Navigation = new NavigationController();
        internal static String SimpleNotepadText = String.Empty;
        private static String Clipboard = String.Empty;
        internal static readonly String DefaultAssetPath = @"Plugins\LSPDFR\ComputerPlus\";
        internal static Storage Store;
        internal static readonly Version SchemaVersion = new Version("1.0.0");

        internal static Styles Style = new Styles();

        internal async static Task OpenStore()
        {
            if (Store == null)
                Store = await Storage.ReadOrInit();
        }

        static public ArrestReport PendingArrestReport
        {
            get;
            internal set;
        } = new ArrestReport();

        public static ChargeCategories ChargeCategoryList
        {
            get;
            internal set;
        } = null;

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