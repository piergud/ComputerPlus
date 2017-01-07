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
        /// Registers the callout with the computer. This function also calls Functions.SetActiveCallout(Guid callID).
        /// The computer will automatically set your callout to "Completed" status when it ends, and a "Code 4" update will be added in the computer.
        /// </summary>
        /// <param name="data">The CalloutData object to register.</param>
        /// <returns>The ID of your callout object (returns it for convenience...its created by the CalloutData constructor)</returns>
        public static Guid CreateCallout(CalloutData data)
        {
            Globals.CallQueue.Add(data);

            Globals.ActiveCallID = data.ID;
            Globals.IsCalloutActive = true;

            return data.ID;
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
        /// Set the status of a call to Unit_Responding; also adds an update in the computer signifying that the player's unit accepted the call.
        /// </summary>
        /// <param name="callID">The ID of the callout.</param>
        public static void SetCalloutStatusToUnitResponding(Guid callID)
        {
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
            {
                x.UpdateStatus(ECallStatus.Unit_Responding);
                x.AddUpdate(String.Format("Unit {0} is responding.", Configs.UnitNumber));
            }
        }

        /// <summary>
        /// Set the status of a call to At_Scene; also adds an update in the computer signifying that the player's unit arrived on scene.
        /// </summary>
        /// <param name="callID">The ID of the callout.</param>
        public static void SetCalloutStatusToAtScene(Guid callID)
        {
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
            {
                x.UpdateStatus(ECallStatus.At_Scene);
                x.AddUpdate(String.Format("Unit {0} is on scene.", Configs.UnitNumber));
            }
        }

        /// <summary>
        /// Set the status of a call to Completed; if this function is not called, it will be called automatically when your callout ends.
        /// </summary>
        /// <param name="callID">The ID of the callout.</param>
        public static void ConcludeCallout(Guid callID)
        {
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
            {
                x.ConcludeCallout();
            }
        }

        /// <summary>
        /// Set the status of a call to Cancelled; can be used to gracefully 'end' or 'Code 4' a call if it crashes before the player responds to it.
        /// </summary>
        /// <param name="callID">The ID of the callout.</param>
        public static void CancelCallout(Guid callID)
        {
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
            {
                x.CancelCallout();
            }
        }

        /// <summary>
        /// Assign the call to an AI unit if the player does not accept it. (Sets status to Unit_Responding, and removes the call from the 'Active Call' tab)
        /// </summary>
        /// <param name="callID">The ID of the callout.</param>
        public static void AssignCallToAIUnit(Guid callID)
        {
            foreach (var x in Globals.CallQueue.Where(x => x.ID == callID))
            {
                x.AssignCallToAIUnit();
            }

            Function.ClearActiveCall(callID);
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


        /// <summary>
        /// Register a Gwen Form that will be added to the extras drop down on the main screen.
        /// </summary>
        /// <param name="displayName">The name to display in the drop down list.</param>
        /// <param name="author">The author's name to display in the drop down list as $displayName - $author</param>
        /// <param name="creator">The creator function which will return the Gwen Form</param>
        /// <param name="onOpen">An optional callback that will be excuted when the form.Show() method is called.</param>
        public static Guid RegisterInterface(String displayName, String author, Func<Rage.Forms.GwenForm> creator, Action onOpen = null, Action onClose = null)
        {
            if (String.IsNullOrWhiteSpace(displayName) || String.IsNullOrWhiteSpace(author) || creator == null) return Guid.Empty;
            //Ensure there are no other UIs registered with the same params, if so, return it
            var conflict = Globals.ExternalUI.Where(x => x.DisplayName.Equals(displayName) && x.Author.Equals(author)).DefaultIfEmpty(null).FirstOrDefault();
            if (conflict != null) return conflict.Identifier;
            //Create a new ExternalUI entry
            var guid = Guid.NewGuid();
            Globals.ExternalUI.Add(new ExternalUI(guid, displayName, author, creator, onOpen, onClose));
            return guid;
        }
    }
}