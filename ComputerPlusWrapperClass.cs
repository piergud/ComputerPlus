using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using ComputerPlus.API;
using ComputerPlus;

namespace AssortedCallouts.API
{
    internal static class ComputerPlusFuncs
    {
        public static Guid CreateCallout(string CallName, string ShortName, Vector3 Location, int ResponseType, string Description = "", int CallStatus = 1, List<Ped>CallPeds = null, List<Vehicle>CallVehicles = null)
        {
            return Functions.CreateCallout(new CalloutData(CallName, ShortName, Location, (ComputerPlus.EResponseType)ResponseType, Description, (ComputerPlus.ECallStatus)CallStatus, CallPeds, CallVehicles));
        }

        public static void UpdateCalloutStatus(Guid ID, int Status)
        {
            Functions.UpdateCalloutStatus(ID, (ECallStatus)Status);
        }

        public static void UpdateCalloutDescription(Guid ID, string Description)
        {
            Functions.UpdateCalloutDescription(ID, Description);
            
        }

        public static void SetCalloutStatusToAtScene(Guid ID)
        {
            Functions.SetCalloutStatusToAtScene(ID);
        }

        public static void ConcludeCallout(Guid ID)
        {
            Functions.ConcludeCallout(ID);
        }

        public static void CancelCallout(Guid ID)
        {
            Functions.CancelCallout(ID);
        }

        public static void SetCalloutStatusToUnitResponding(Guid ID)
        {
            Functions.SetCalloutStatusToUnitResponding(ID);
        }

        public static void AddPedToCallout(Guid ID, Ped PedToAdd)
        {
            Functions.AddPedToCallout(ID, PedToAdd);
        }

        public static void AddUpdateToCallout(Guid ID, string Update)
        {
            Functions.AddUpdateToCallout(ID, Update);
        }

        public static void AddVehicleToCallout(Guid ID, Vehicle VehicleToAdd)
        {
            Functions.AddVehicleToCallout(ID, VehicleToAdd);
        }

        public static void AssignCallToAIUnit(Guid ID)
        {
            Functions.AssignCallToAIUnit(ID);
        }

    } 
}
