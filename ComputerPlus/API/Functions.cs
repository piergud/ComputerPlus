using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.API
{
    public static class Functions
    {
        /// <summary>
        /// Sets the active callout on the computer.
        /// (This function will not normally need to be used)
        /// </summary>
        /// <param name="callID">The CalloutData object to set as active.</param>
        public static void SetActiveCallout(Guid callID)
        {
            Globals.ActiveCallID = callID;
        }

        /// <summary>
        /// Registers the callout with the computer. This function also calls Functions.SetActiveCallout(Guid callID).
        /// The computer will automatically set your callout to "Completed" status when it ends, and a "Code 4" update will be added in the computer.
        /// </summary>
        /// <param name="data">The CalloutData object to register.</param>
        public static void CreateCallout(CalloutData data)
        {
            Globals.CallQueue.Add(data);
            SetActiveCallout(data.ID);
        }

        /// <summary>
        /// Add an update/comment to the call via the computer. This is an "in-character" function.
        /// </summary>
        /// <param name="callID">The ID of the callout.</param>
        /// <param name="text">The text of the update (A timestamp will be prefixed automatically).</param>
        public static void AddUpdateToCallout(Guid callID, string text)
        {
            // Must be done with a for loop, to update the CalloutData element in the CallQueue List
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
                x.AddUpdate(text);
        }

        /// <summary>
        /// Update the status of the call. AddUpdateToCallout() will also be called, to register the status update in the computer.
        /// This is an "in-character" function, but obviously, the CalloutData.Status property can be used for logic purposes as well.
        /// </summary>
        /// <param name="callID">The ID of the callout.</param>
        /// <param name="status">The status code.</param>
        public static void UpdateCalloutStatus(Guid callID, ECallStatus status)
        {
            // Must be done with a for loop, to update the CalloutData element in the CallQueue List
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
                x.UpdateStatus(status);
        }

        /// <summary>
        /// Update the description of the callout. This is an "in-character" function; this function will call AddUpdateToCallout() to signify in the computer that Dispatch (or the officer) updated the callout.
        /// The Description field is intended to contain the inital 'synopsis' from Dispatch; use AddUpdateToCallout() whenever possible.
        /// </summary>
        /// <param name="callID"></param>
        /// <param name="text"></param>
        public static void UpdateCalloutDescription(Guid callID, string text)
        {
            // Must be done with a for loop, to update the CalloutData element in the CallQueue List
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
                x.UpdateDescription(text);
        }

        /// <summary>
        /// Add a Rage.Ped object to the call. This is an "out of character" function (AddUpdateToCallout() will not be called).
        /// </summary>
        /// <param name="callID">The ID of the callout.</param>
        /// <param name="ped">The Rage.Ped to be added.</param>
        public static void AddPedToCallout(Guid callID, Ped ped)
        {
            // Must be done with a for loop, to update the CalloutData element in the CallQueue List
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
                x.AddPed(ped);
        }

        /// <summary>
        /// Add a Rage.Vehicle object to the call. This is an "out of character" function (AddUpdateToCallout() will not be called).
        /// </summary>
        /// <param name="callID">The ID of the callout.</param>
        /// <param name="veh">The Rage.Vehicle to be added.</param>
        public static void AddVehicleToCallout(Guid callID, Vehicle veh)
        {
            // Must be done with a for loop, to update the CalloutData element in the CallQueue List
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
                x.AddVehicle(veh);
        }
    }
}