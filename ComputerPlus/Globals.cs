using ComputerPlus.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Controllers;
namespace ComputerPlus
{
    internal sealed class Globals
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
        internal readonly static String DefaultBackgroundImage = "lspd.jpg";
        internal static bool PauseGameWhenOpen = true;
        internal static bool ShowBackgroundWhenOpen = true;
        internal static bool CloseRequested = false;
        internal static NavigationController Navigation = new NavigationController();

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
    }
}