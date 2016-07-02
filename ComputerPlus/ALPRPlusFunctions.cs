using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stealth.Plugins.ALPRPlus;
using Stealth.Plugins.ALPRPlus.API;
using Stealth.Plugins.ALPRPlus.API.Types;
using Rage;
using ComputerPlus.Interfaces.ComputerVehDB;
using Stealth.Plugins.ALPRPlus.API.Types.Enums;
using ComputerPlus.Controllers.Models;

namespace ComputerPlus { 

   
    
    internal static class ALPRPlusFunctions
    {
        internal static event EventHandler<ALPR_Arguments> OnAlprPlusMessage;
        internal static void RegisterForEvents()
        {

            Events.ALPRResultDisplayed += Events_ALPRResultDisplayed;
            Events.ALPRFlagGenerated += Events_ALPRFlagGenerated;
        }

        private static void Events_ALPRFlagGenerated(Vehicle veh, ALPREventArgs e)
        {
            Game.LogVerboseDebug("Events_ALPRFlagGenerated");
        }

        private static void Events_ALPRResultDisplayed(Rage.Vehicle veh, Stealth.Plugins.ALPRPlus.API.Types.ALPREventArgs e)
        {
            Game.LogVerboseDebug("ALPRPlusFunctions Events_ALPRResultDisplayed");
            EventHandler<ALPR_Arguments> handler = (EventHandler<ALPR_Arguments>)OnAlprPlusMessage;
            if (handler != null)
            {
                ALPR_Position position = ALPR_Position.FRONT;
                switch (e.Camera)
                {
                    case ECamera.Driver_Front:
                        position = ALPR_Position.FRONT_DRIVER;
                        break;
                    case ECamera.Passenger_Front:
                        position = ALPR_Position.FRONT_PASSENGER;
                        break;
                    case ECamera.Driver_Rear:
                        position = ALPR_Position.REAR_DRIVER;
                        break;
                    case ECamera.Passenger_Rear:
                        position = ALPR_Position.REAR_PASSENGER;
                        break;                    
                }
                Game.LogVerboseDebug("ALPRPlusFunctions sending to handler");
                handler(null, new ALPR_Arguments(e.Result.Vehicle, position, e.Result.Result));
            }
            else
                Game.LogVerboseDebug("ALPRPlusFunctions has no handler");
        }
    }
}
