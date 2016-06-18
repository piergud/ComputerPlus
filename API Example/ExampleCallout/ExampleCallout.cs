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

            if (Function.IsLSPDFRPluginRunning("ComputerPlus", new Version("1.3.0.0")))
                callID = ComputerPlusFuncs.CreateCallout("Example Callout", "EXAMPLE CALLOUT", location,
                    (int)EResponseType.Code_3, "This is an example callout.");

            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutDisplayed()
        {
            if (Function.IsLSPDFRPluginRunning("ComputerPlus", new Version("1.3.0.0")))
                ComputerPlusFuncs.UpdateCalloutStatus(callID, (int)ECallStatus.Dispatched);
            base.OnCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            if (Function.IsLSPDFRPluginRunning("ComputerPlus", new Version("1.3.0.0")))
                ComputerPlusFuncs.SetCalloutStatusToUnitResponding(callID);
            blip.EnableRoute(Color.Yellow);
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (Function.IsLSPDFRPluginRunning("ComputerPlus", new Version("1.3.0.0")))
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
                    if (Function.IsLSPDFRPluginRunning("ComputerPlus", new Version("1.3.0.0")))
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
            if (Function.IsLSPDFRPluginRunning("ComputerPlus", new Version("1.3.0.0")))
            {
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
