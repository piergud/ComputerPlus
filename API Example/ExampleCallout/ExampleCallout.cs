using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using ComputerPlus;
using Rage;

namespace Example
{
    [CalloutInfo("Example Callout", CalloutProbability.Medium)]
    public class ExampleCallout : Callout
    {
        Vector3 location;
        Guid callID;
        Blip blip;
        Vehicle vehicle;
        Ped driver;
        bool hasArrived;
        bool computerPlusRunning;

        public override bool OnBeforeCalloutDisplayed()
        {
            location = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));

            vehicle = new Vehicle("adder", location);
            driver = vehicle.CreateRandomDriver();

            vehicle.IsPersistent = true;
            driver.IsPersistent = true;

            this.CalloutMessage = "Example Callout";
            this.CalloutPosition = location;
            hasArrived = false;

            blip = new Blip(vehicle);
            blip.Color = Color.Yellow;

            // Check if Computer+ 1.3 or higher is running
            // ComputerPlus+ 1.2.2 and below does not have an API, so the version check is necessary
            computerPlusRunning = Function.IsLSPDFRPluginRunning("ComputerPlus", new Version("1.3.0.0"));

            // Creates the callout within the computer
            if (computerPlusRunning)
                callID = ComputerPlusFuncs.CreateCallout("Example Callout", "EXAMPLE CALLOUT", location,
                    (int)EResponseType.Code_3, "This is an example callout.");

            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            // Updates the callout's status to "Dispatched" when the player sees the callout on screen
            if (computerPlusRunning)
                ComputerPlusFuncs.UpdateCalloutStatus(callID, (int)ECallStatus.Dispatched);
            base.OnCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            // Updates the callout's status to "Unit Responding" when the player accepts
            if (computerPlusRunning)
                ComputerPlusFuncs.SetCalloutStatusToUnitResponding(callID);
            blip.EnableRoute(Color.Yellow);
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            // Assigns the callout to an AI unit when the player ignores
            if (computerPlusRunning)
                ComputerPlusFuncs.AssignCallToAIUnit(callID);
            CleanUp();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();
            {
                if (Game.LocalPlayer.Character.DistanceTo(blip.Position) <= 20f && !hasArrived)
                {
                    /* Once the player is on scene (approx. 20 metres from the call's location),
                     the callout's status will change to "At Scene". It will also add an update
                     saying "This is an update." and will include both the vehicle and the driver
                     on the computer's call details screen for easy searching */
                    if (computerPlusRunning)
                    {
                        ComputerPlusFuncs.SetCalloutStatusToAtScene(callID);
                        ComputerPlusFuncs.AddUpdateToCallout(callID, "This is an update.");
                        ComputerPlusFuncs.AddPedToCallout(callID, driver);
                        ComputerPlusFuncs.AddVehicleToCallout(callID, vehicle);
                    }
                    if (blip)
                        blip.DisableRoute();
                    hasArrived = true;
                }
            }
        }

        public override void End()
        {
            if (computerPlusRunning)
            {
                // Changes the call's status to "Concluded" when the callout ends
                ComputerPlusFuncs.ConcludeCallout(callID);
            }
            CleanUp();
            base.End();
        }

        public void CleanUp()
        {
            if (blip)
                blip.Delete();
            if (driver)
                driver.Delete();
            if (vehicle)
                vehicle.Delete();
        }
    }
}
