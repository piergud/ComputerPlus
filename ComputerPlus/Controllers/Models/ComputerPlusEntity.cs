using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace ComputerPlus.Controllers.Models
{
    class ComputerPlusEntity: IPersistable
    {
        internal Ped Ped
        {
            get;
            private set;
        }
        internal Persona PedPersona
        {
            get;
            set;
        }

        internal Vehicle Vehicle
        {
            get;
            private set;
        }
        internal VehiclePersona VehiclePersona
        {
            get;
            set;
        }

        private bool _IsPersistent;
        public bool IsPersistent
        {
            get
            {
                return _IsPersistent;
            }

            set
            {
                try
                {
                    if (!_IsPersistent)
                    {
                        Ped.IsPersistent = true;
                        Vehicle.IsPersistent = true;
                        _IsPersistent = true;
                    }
                }
                catch
                {
                    _IsPersistent = false;
                }
            }
        }

        internal ComputerPlusEntity(Ped ped, Persona persona, Vehicle vehicle, VehiclePersona vehiclePersona)
        {
            this.Ped = ped;
            this.PedPersona = persona;
            this.Vehicle = vehicle;
            this.VehiclePersona = vehiclePersona;

            if(!Validate())
            {
                throw new ArgumentException("ComputerPlusEntity failed validation");
            }
        }

        public bool Validate()
        {
            return Ped != null
                && Ped.Exists()
                && Vehicle != null
                && Vehicle.Exists();
        }

        public void Dismiss()
        {
            try
            {
                Ped.Dismiss();
                Vehicle.Dismiss();
            }
            catch
            {
                Game.LogVerboseDebug("ComputerPlusEntity caught exception during Dismiss");
            }
        }
    }
}
