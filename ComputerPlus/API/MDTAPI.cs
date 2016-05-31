using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace ComputerPlus
{
    /// <summary>
    /// This is the API for the Citation form.
    /// </summary>
    
    public static class CitationAPI
    {
        internal static string perpfirst;
        internal static string perplast;
        internal static string vehplate;
        internal static bool speeding;
        internal static bool citcomplete;  

        /// <summary>
        /// Checks the name of the ped in the citation form.  Using LSPDFR API.
        /// </summary>
        /// <param name="ped">The ped to be checked</param>
        /// <returns>True or false: if true the name (tolower()) in the citation form matches the Ped in the callout</returns>
        public static bool CheckCitationPedName(Ped ped)
        {
            if (ReportMain.form_citation.IsAlive)
            {
                perpfirst = CitationInformationCode.firstname.ToLower();
                perplast = CitationInformationCode.lastname.ToLower();
            }
            Persona pers = Functions.GetPersonaForPed(ped);
            if (pers.Forename.ToLower() == perpfirst && pers.Surname.ToLower() == perplast)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks the information on the vehicle of the citation.
        /// </summary>
        /// <param name="veh">The vehicle who's plate to check</param>
        /// <returns>True or false: if true the license plate in the citation form matches the vehicle in the callout</returns>
        public static bool CheckCitationVehiclePlate(Vehicle veh)
        {
            if (CitationInformationCode.form_citationviolation.IsAlive)
            {
                Game.LogTrivial("citation violation alive");
                if (veh.Exists())
                {
                    Game.LogTrivial("veh exists");
                    if (CitationViolationCode.state == CitationViolationCode.SubmitCheck.submitted)
                    {
                        Game.LogTrivial("veh plate set");
                        CitationAPI.vehplate = CitationViolationCode.vehplate;
                    }
                }
            }
            if (veh.LicensePlate.ToString().ToLower() == vehplate)
            {
                Game.LogTrivial("vehplate == true");
                return true;
            }
            else
            {
                Game.LogTrivial("vehplate == false");
                return false;
            }
        }
        /// <summary>
        /// Checks whether or not the officer declared a speeding violation.
        /// </summary>
        /// <param name="veh">The vehicle who may have been speeding</param>
        /// <returns>True or false: if true the vehicle was determined by the officer to be speeding (option was checked in the citation form)</returns>
        public static bool CheckVehicleSpeedViolation(Vehicle veh)
        {
            if (CitationInformationCode.form_citationviolation.IsAlive && CitationViolationCode.SpeedCheckisChecked == true)
            {
                Game.LogTrivial("Citation violation is alive and it's checked");
                speeding = true;
            }
            else if (!CitationInformationCode.form_citationviolation.IsAlive)
            {
                return false;
            }
            if (CitationViolationCode.SpeedCheckisChecked == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks whether the citation was successfully submitted. Use this to call the end to your callout once the citation has been submitted
        /// Note that submitting the citation will not cause the player to go back to the game, so a check for the game being paused is included.
        /// When the game is unpaused and the citation was completed the bool will return it's true value.
        /// </summary>
        /// <returns>True or false: if true the citation was submitted</returns>
        public static bool IsCitationSubmitted()
        {
            if (CitationInformationCode.form_citationviolation.IsAlive)
            {
                Game.LogTrivial("Citation form is alive");
                citcomplete = CitationViolationCode.CitationComplete();
            }
            else if (!CitationInformationCode.form_citationviolation.IsAlive)
            {
                return false;
            }
            if (citcomplete == true)
            {
                Game.LogTrivial("Citationcomplete = true");
                return true;
            }
            else
            {
                Game.LogTrivial("Citationcomplete = false");
                return false;
            }
        }
    }
    /// <summary>
    /// This is used to access the Field Interview information
    /// </summary>
    public class FIAPI
    {
        internal static string perpfirst;
        internal static string perplast;
        internal static string vehplate;
        internal static bool ficomplete;

        /// <summary>
        /// Checks the name of the ped in the field interview form.  Using LSPDFR API.
        /// </summary>
        /// <param name="ped">The ped to be checked</param>
        /// <returns>True or false: if true the name (tolower()) in the FI form matches the Ped in the callout</returns>
        public static bool CheckFIPedName(Ped ped)
        {
            if (FIEnvironmentCode.form_fipersonal.IsAlive)
            {
                perpfirst = FIPersonalInfoCode.firstname.ToLower();
                perplast = FIPersonalInfoCode.lastname.ToLower();
            }
            Persona pers = Functions.GetPersonaForPed(ped);
            if (pers.Forename.ToLower() == perpfirst && pers.Surname.ToLower() == perplast)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks the information on the vehicle of the field interaction.
        /// </summary>
        /// <param name="veh">The vehicle who's plate to check</param>
        /// <returns>True or false: if true the license plate in the FI form matches the vehicle in the callout</returns>
        public static bool CheckFIVehiclePlate(Vehicle veh)
        {
            if (FIEnvironmentCode.form_fipersonal.IsAlive)
            {
                if (veh.Exists())
                {
                    Game.LogTrivial("veh exists");
                    if (FIPersonalInfoCode.state == FIPersonalInfoCode.SubmitCheck.submitted)
                    {
                        Game.LogTrivial("veh plate set");
                        FIAPI.vehplate = FIPersonalInfoCode.vehplate;
                    }
                }
            }
            if (veh.LicensePlate.ToString().ToLower() == vehplate)
            {
                Game.LogTrivial("vehplate == true");
                return true;
            }
            else
            {
                Game.LogTrivial("vehplate == false");
                return false;
            }
        }
        /// <summary>
        /// Checks whether the FI was successfully submitted. Use this to call the end to your callout once the FI has been submitted
        /// Note that submitting the FI will not cause the player to go back to the game however the game is paused during this time.
        /// </summary>
        /// <returns>True or false: if true the FI was submitted</returns>
        public static bool IsFISubmitted()
        {
            if (FIEnvironmentCode.form_fipersonal.IsAlive)
            {
                Game.LogTrivial("FI form is alive");
                ficomplete = FIPersonalInfoCode.FIComplete();
            }
            else if (!FIEnvironmentCode.form_fipersonal.IsAlive)
            {
                return false;
            }
            if (ficomplete == true)
            {
                Game.LogTrivial("Citationcomplete = true");
                return true;
            }
            else
            {
                Game.LogTrivial("Citationcomplete = false");
                return false;
            }
        }
    }
}
