using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;
using ComputerPlus.Extensions.Rage;

namespace ComputerPlus.Controllers.Models
{
    enum EntityTypes { Ped = 0, Vehicle = 1 }
    public class ComputerPlusEntity: IPersistable
    {
        public Ped Ped
        {
            get;
            private set;
        }
        public Persona PedPersona
        {
            get;
            internal set;
        }

        public Vehicle Vehicle
        {
            get;
            private set;
        }

        public VehiclePersona VehiclePersona
        {
            get;
            internal set;
        }

        public String FullName
        {
            get
            {
                return (!Ped) ? String.Empty : PedPersona.FullName;
            }
        }

        public String FirstName
        {
            get
            {
                return (!Ped) ? String.Empty : PedPersona.Forename;
            }
        }

        public String LastName
        {
            get
            {
                return (!Ped) ? String.Empty : PedPersona.Surname;
            }
        }


        public String Address
        {
            get
            {
                return !Ped ? String.Empty : Ped.GetHomeAddress();
            }
        }

        public String VehicleTag
        {
            get
            {
                return (!Vehicle) ? String.Empty : Vehicle.LicensePlate;
            }
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
                Function.LogDebug("ComputerPlusEntity caught exception during Dismiss");
            }
        }

        public static implicit operator bool (ComputerPlusEntity entity)
        {
            return entity.Validate();
        }
    }
}
