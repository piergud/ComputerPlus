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
            ALPRPlusFunctions.OnAlprPlusMessage += Events_ALPRResultDisplayed;

        }

        private static void Events_ALPRResultDisplayed(object sender, ALPR_Arguments e)
        {
            Game.LogVerboseDebug("ALPRPlusFunctions Events_ALPRResultDisplayed");
            EventHandler<ALPR_Arguments> handler = (EventHandler<ALPR_Arguments>)OnAlprPlusMessage;
            if (handler != null)
            {
                ALPR_Position position = ALPR_Position.FRONT;
                switch (e.Position)
                {
                    case ALPR_Position.FRONT_DRIVER:
                        position = ALPR_Position.FRONT_DRIVER;
                        break;
                    case ALPR_Position.FRONT_PASSENGER:
                        position = ALPR_Position.FRONT_PASSENGER;
                        break;
                    case ALPR_Position.REAR_DRIVER:
                        position = ALPR_Position.REAR_DRIVER;
                        break;
                    case ALPR_Position.REAR_PASSENGER:
                        position = ALPR_Position.REAR_PASSENGER;
                        break;                    
                }
                Game.LogVerboseDebug("ALPRPlusFunctions sending to handler");
                handler(null, new ALPR_Arguments(e.Vehicle, position, e.Message));
            }
            else
                Game.LogVerboseDebug("ALPRPlusFunctions has no handler");
        }
    }
}
