using System;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;
using British_Policing_Script;
using British_Policing_Script.API;
using ComputerPlus.Extensions;
using LSPD_First_Response;
using ComputerPlus.Controllers.Models;

namespace ComputerPlus
{
    internal static class BritishPolicingFunctions
    {


        internal static Persona GetBritishPersona(Ped ped)
        {
            return Functions.GetBritishPersona(ped);
        }

        internal static String GetPedPersonaFullName(Persona persona)
        {
            return (persona as BritishPersona).FullName;
        }

        internal static String GetPedPersonaForeName(Persona persona)
        {
            return (persona as BritishPersona).Forename;
        }

        internal static String GetPedPersonaSurName(Persona persona)
        {
            return (persona as BritishPersona).Surname;
        }

        internal static String GetPedPersonaDOBString(Persona persona)
        {
            return (persona as BritishPersona).BirthDay.ToLocalTimeString(Extensions.Gwen.TextBoxExtensions.DateOutputPart.DATE);
        }

        internal static DateTime GetPedPersonaBirthDay(Persona persona)
        {
            return (persona as BritishPersona).BirthDay;
        }

        internal static int GetPedPersonaTimesStopped(Persona persona)
        {
            return (persona as BritishPersona).TimesStopped;
        }

        internal static Gender GetPedPersonaGender(Persona persona)
        {
            return (persona as BritishPersona).Gender;
        }

        internal static bool GetPedPersonaIsWanted(Persona persona)
        {
            return (persona as BritishPersona).Wanted;
        }

        internal static bool GetPedPersonaIsAgent(Persona persona)
        {
            return (persona as BritishPersona).IsAgent;
        }

        internal static bool GetPedPersonaIsCop(Persona persona)
        {
            return (persona as BritishPersona).IsCop;
        }

        internal static String GetPedPersonaWantedReason(Persona persona)
        {
            return (persona as BritishPersona).WantedReason;
        }

        internal static String GetVehicleLicensePlate(System.Object rawPersona)
        {
            return (rawPersona as VehicleRecords).LicencePlate;
        }

        internal static bool IsVehicleRegistered(System.Object rawPersona)
        {
            return (rawPersona as VehicleRecords).IsTaxed;
        }

        internal static bool IsVehicleInsured(System.Object rawPersona)
        {
            return (rawPersona as VehicleRecords).Insured;
        }

        internal static bool DoesVehicleHaveMOT(System.Object rawPersona)
        {
            return (rawPersona as VehicleRecords).HasMOT;
        }

        internal static bool DoesVehicleHaveSORN(System.Object rawPersona)
        {
            return (rawPersona as VehicleRecords).HasSORN;
        }

        internal static bool IsPedInsuredToDriveVehicle(System.Object rawPedPersona, System.Object rawVehiclePersona)
        {
            var pedPersona = rawPedPersona as BritishPersona;
            var vehiclePersona = rawVehiclePersona as VehicleRecords;
            return pedPersona.IsInsuredToDriveVehicle(vehiclePersona);
        }

        internal static bool IsLicenseValid(Persona pedPersona)
        {
            var persona = pedPersona as BritishPersona;
            switch (persona.LicenceStatus)
            {
                case BritishPersona.LicenceStatuses.Disqualified:
                case BritishPersona.LicenceStatuses.Expired:
                case BritishPersona.LicenceStatuses.Revoked: return false;
                default: return true;
            }
        }

        internal static String GetLicenseStateString(Persona pedPersona)
        {
            var persona = pedPersona as BritishPersona;
            switch (persona.LicenceStatus)
            {
                case BritishPersona.LicenceStatuses.Disqualified: return "Disqualified";
                case BritishPersona.LicenceStatuses.Expired: return "Expired";
                case BritishPersona.LicenceStatuses.Revoked: return "Revoked";
                default: return "Valid";
            }
        }

        internal static VehiclePersona CreateVehiclePersona(Vehicle veh)
        {
            VehicleRecords records = Functions.GetVehicleRecords(veh);

            VehiclePersona vehiclePersona = new VehiclePersona();
            vehiclePersona.HasInsurance = records.Insured;
            vehiclePersona.IsRegistered = null;
            vehiclePersona.Color = records.CarColour;
            vehiclePersona.Alert = new System.Text.RegularExpressions.Regex(@"~\w~").Replace(records.DetermineFlags(), String.Empty);
            vehiclePersona.HasValidEmissions = records.HasMOT;
            vehiclePersona.IsOffroadOnly = records.HasSORN;
            vehiclePersona.IsTaxed = records.IsTaxed;
            vehiclePersona.RawPersona = records;

            return vehiclePersona;
        }
    }
}
