using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.API;
using Rage;
using Rage.Forms;

namespace ExampleExternalUI
{
    internal static class ComputerPlusWrapper
    {
        /**
         * Normally you'd have the full wrapper, but for the purposes of this example, it's omitted
         *
         *   public static Guid CreateCallout(string CallName, string ShortName, Vector3 Location, int ResponseType, string Description = "", int CallStatus = 1, List<Ped> CallPeds = null, List<Vehicle> CallVehicles = null)
         *   {
         *       return Functions.CreateCallout(new CalloutData(CallName, ShortName, Location, (EResponseType)ResponseType, Description, (ECallStatus)CallStatus, CallPeds, CallVehicles));
         *   }
         *
         *   public static void UpdateCalloutStatus(Guid ID, int Status)
         *   {
         *       Functions.UpdateCalloutStatus(ID, (ECallStatus)Status);
         *   }
         **/
       
        public static void RegisterComputerPlusInterface(String name, String author, Func<GwenForm> creator, Action OnOpen = null)
        {
            Functions.RegisterInterface(name, author, creator, OnOpen);
        }

     
    }
}
