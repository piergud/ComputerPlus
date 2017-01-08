using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace ComputerPlus.Controllers.Models
{
    enum EntityTypes { Ped = 0, Vehicle = 1 }
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

        private readonly EntityTypes CreatedWith;

        internal ComputerPlusEntity(Ped ped, Persona persona)
        {
            this.CreatedWith = EntityTypes.Ped;
            this.Ped = ped;
            this.PedPersona = persona;
        }

        internal ComputerPlusEntity(Vehicle vehicle, VehiclePersona persona)
        {
            this.CreatedWith = EntityTypes.Vehicle;
            this.Vehicle = vehicle;
            this.VehiclePersona = persona;
        }


        internal ComputerPlusEntity(Ped ped, Persona persona, Vehicle vehicle, VehiclePersona vehiclePersona)
        {
            this.CreatedWith = EntityTypes.Ped | EntityTypes.Vehicle;
            this.Ped = ped;
            this.PedPersona = persona;
            this.Vehicle = vehicle;
            this.VehiclePersona = vehiclePersona;         
        }

        public bool Validate()
        {
            if (this.CreatedWith.HasFlag(EntityTypes.Ped) && this.CreatedWith.HasFlag(EntityTypes.Vehicle)) {
                return this.Ped && this.Vehicle;
            }
            else if (this.CreatedWith == EntityTypes.Ped){
                return this.Ped;
            }
            else if (this.CreatedWith == EntityTypes.Vehicle)
            {
                return this.Vehicle;
            }
            return false;
        }

        public void Dismiss()
        {
            try
            {
                if (Ped) Ped.Dismiss();
                if (Vehicle) Vehicle.Dismiss();
            }
            catch
            {
                Game.LogVerboseDebug("ComputerPlusEntity caught exception during Dismiss");
            }
        }
    }
}
