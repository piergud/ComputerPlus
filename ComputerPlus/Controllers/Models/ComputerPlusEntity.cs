using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;
using ComputerPlus.Extensions.Rage;
using ComputerPlus.Extensions;

namespace ComputerPlus.Controllers.Models
{
    enum EntityTypes { Ped = 0, Vehicle = 1 }
    public enum PersonaTypes {  Vanilla = 0, BPS = 1 }
    public class ComputerPlusEntity: IPersistable
    {
        public static PersonaTypes PersonaType;
        public Ped Ped
        {
            get;
            private set;
        }
        internal Persona PedPersona
        {
            get;
            set;
        }

        public Vehicle Vehicle
        {
            get;
            private set;
        }

        private VehiclePersona VehiclePersona
        {
            get;
            set;
        }

        internal System.Object RawVehiclePersona
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Vehicle) || !Vehicle) return null;
                return VehiclePersona.RawPersona;
            }
        }

        internal System.Object RawPedPersona
        {
            get
            {
                return PedPersona;
            }
        }

        public String FullName
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return String.Empty;
                switch(PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaFullName(PedPersona);
                    default: return PedPersona.FullName;
                }
            }
        }

        public String FirstName
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return String.Empty;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaForeName(PedPersona);
                    default: return PedPersona.Forename;
                }
            }
        }

        public String LastName
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return String.Empty;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaSurName(PedPersona); ;
                    default: return PedPersona.Surname;
                }
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
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Vehicle) return String.Empty;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetVehicleLicensePlate(VehiclePersona.RawPersona);
                    default: return Vehicle.LicensePlate;
                }
            }
        }

        public String DOBString
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return String.Empty;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaDOBString(PedPersona);
                    default: return PedPersona.BirthDay.ToDateTimeString(Extensions.Gwen.TextBoxExtensions.DateOutputPart.DATE, false);
                }
            }
        }

        public int Age
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return 0;
                DateTime birthDate;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: birthDate = BritishPolicingFunctions.GetPedPersonaBirthDay(PedPersona); break;
                    default: birthDate = PedPersona.BirthDay; break;
                }
                return  (int)((DateTime.Today - birthDate).Days / 365.25m);
            }
        }

        public String AgeString
        {
            get
            {
                return Age.ToString();
            }
        }

        public int TimesStopped
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return 0;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaTimesStopped(PedPersona);
                    default: return PedPersona.TimesStopped;
                }
            }
        }

        public LSPD_First_Response.Gender Gender
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return LSPD_First_Response.Gender.Random;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaGender(PedPersona);
                    default: return PedPersona.Gender;
                }
            }
        }

        public String GenderString
        {
            get
            {
                return Gender.ToFriendlyString();
            }
        }

        public char GenderId
        {
            get
            {
                return Gender.ToFriendlyString().First();
            }
        }

        public bool IsWanted
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return false;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaIsWanted(PedPersona);
                    default: return PedPersona.Wanted;
                }
            }
        }

        public String WantedReason
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return String.Empty;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaWantedReason(PedPersona);
                    default: return Ped.GetWantedReason();
                }
            }
        }

        public GunPermitInfo GunPermitInformation
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return new GunPermitInfo(false);
                return Ped.GetGunPermitInfo();
            }
        }

        public bool IsLicenseValid
        {
            get
            {              
                  
                if (PersonaType == PersonaTypes.BPS)
                {
                    return BritishPolicingFunctions.IsLicenseValid(PedPersona);
                }
                else {
                    switch (PedPersona.LicenseState)
                    {
                        case ELicenseState.None:
                        case ELicenseState.Suspended:
                        case ELicenseState.Expired: return false;
                        default: return true;
                    }
                }
            }
        }

        public String LicenseStateString
        {
            get
            {
                if (PersonaType == PersonaTypes.BPS)
                {
                    return BritishPolicingFunctions.GetLicenseStateString(PedPersona);
                }
                else
                {
                    switch (PedPersona.LicenseState)
                    {
                        case ELicenseState.Expired: return "Expired";
                        case ELicenseState.Suspended: return "Suspended";
                        case ELicenseState.Valid: return "Valid";
                        case ELicenseState.None: return "None";
                        default: return "N/A";
                    }
                }
            }
        }

        public bool IsAgent
        {
            get
            {

                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return false;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaIsAgent(PedPersona);
                    default: return PedPersona.IsAgent;
                }
            }
        }
        public bool IsCop
        {
            get
            {

                if (!CreatedWith.HasFlag(EntityTypes.Ped) || !Ped) return false;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.GetPedPersonaIsCop(PedPersona);
                    default: return PedPersona.IsCop;
                }
            }
        }
        
        //Vehicle Persona

        public bool HasInsurance
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Vehicle) || !Vehicle) return true;
                return VehiclePersona.HasInsurance.HasValue ? VehiclePersona.HasInsurance.Value : true;
                //switch (PersonaType)
                //{
                //    case PersonaTypes.BPS: return (RawVehiclePersona as British_Policing_Script.VehicleRecords).Insured;
                //    default: return VehiclePersona.HasInsurance.HasValue ? VehiclePersona.HasInsurance.Value : true;
                //}
            }
        }


        public bool IsRegistered
        {
            get
            {
                if (!CreatedWith.HasFlag(EntityTypes.Vehicle) || !Vehicle) return true;
                switch (PersonaType)
                {
                    case PersonaTypes.BPS: return BritishPolicingFunctions.IsVehicleRegistered(VehiclePersona.RawPersona);
                    default: return VehiclePersona.IsRegistered.HasValue ? VehiclePersona.IsRegistered.Value : true;
                }
            }
        }

        public String VehicleAlert
        {
            get
            {
                return VehiclePersona.Alert;
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
        static ComputerPlusEntity()
        {
             PersonaType = PersonaTypes.Vanilla;
        }
        private ComputerPlusEntity(Ped ped, Persona persona)
        {
            this.CreatedWith = EntityTypes.Ped;
            this.Ped = ped;
            this.PedPersona = persona;
        }

        private ComputerPlusEntity(Vehicle vehicle, VehiclePersona persona)
        {
            this.CreatedWith = EntityTypes.Vehicle;
            this.Vehicle = vehicle;
            this.VehiclePersona = persona;
        }


        private ComputerPlusEntity(Ped ped, Persona persona, Vehicle vehicle, VehiclePersona vehiclePersona)
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
                return this.Ped != null && this.Ped.IsValid() && this.Vehicle != null && this.Vehicle.IsValid();
            }
            else if (this.CreatedWith == EntityTypes.Ped){
                return this.Ped != null && this.Ped.IsValid();
            }
            else if (this.CreatedWith == EntityTypes.Vehicle)
            {
                return this.Vehicle != null && this.Vehicle.IsValid();
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
            return entity == null ? false : entity.Validate();
        }

        public static ComputerPlusEntity CloneFrom(ComputerPlusEntity entity, Vehicle vehicle, VehiclePersona vehiclePersona)
        {
            return new ComputerPlusEntity(entity.Ped, entity.PedPersona, vehicle, vehiclePersona);
        }

        public static ComputerPlusEntity CreateFrom(Ped ped)
        {
            if (!ped) return null;
            return new ComputerPlusEntity(ped, GetPersonaForPed(ped));
        }

        public static ComputerPlusEntity CreateFrom(Vehicle vehicle)
        {
            if (!vehicle) return null;
            return new ComputerPlusEntity(vehicle, GetPersonaForVehicle(vehicle));
        }

        public static ComputerPlusEntity CreateFrom(Ped ped, Vehicle vehicle)
        {
            if (!ped || !vehicle) return null;
            return new ComputerPlusEntity(ped, GetPersonaForPed(ped), vehicle, GetPersonaForVehicle(vehicle));
        }

        public static Persona GetPersonaForPed(Ped ped)
        {
            if (PersonaType == PersonaTypes.BPS)
            {
                return BritishPolicingFunctions.GetBritishPersona(ped);
            }
            else
            {
                return LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(ped);
            }
        }

        public static VehiclePersona GetPersonaForVehicle(Vehicle vehicle)
        {
            if (PersonaType == PersonaTypes.BPS)
            {
                return BritishPolicingFunctions.CreateVehiclePersona(vehicle);
            }
            else
            {
                var vehiclePersona = new VehiclePersona();
                if (Function.IsTrafficPolicerRunning())
                {
                    vehiclePersona.HasInsurance = TrafficPolicerFunction.GetVehicleInsuranceStatus(vehicle) == EVehicleStatus.Valid ? true : false;
                    vehiclePersona.IsRegistered = TrafficPolicerFunction.GetVehicleRegistrationStatus(vehicle) == EVehicleStatus.Valid ? true : false;
                }
                else
                {
                    vehiclePersona.HasInsurance = false;
                    vehiclePersona.IsRegistered = false;
                }
                return vehiclePersona;
            }
        }
    }
}
