using System;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace ComputerPlus.Controllers.Models
{
    enum ALPR_Position { FRONT = 0, REAR = 1, DRIVER = 2, PASSENGER = 3, FRONT_DRIVER = 4, FRONT_PASSENGER = 5, REAR_DRIVER = 6, REAR_PASSENGER = 7 }
    internal class ALPR_Arguments : EventArgs
    {
        internal Vehicle Vehicle
        {
            get;
            private set;
        }
        internal String Message
        {
            get;
            private set;
        }

        internal ALPR_Position Position
        {
            get;
            private set;
        }

        internal ALPR_Arguments(Vehicle vehicle, ALPR_Position position, String message = null)
        {
            this.Vehicle = vehicle;
            this.Message = message;
            this.Position = position;
        }
    }
}
