using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;
using ComputerPlus.API;

namespace ComputerPlus
{
    class CustomPersona :  Persona
    {
        public CustomPersona(Ped ped) : base(ped, British_Policing_Script.API.Functions.GetBritishPersona(ped).LSPDFRPersona.Gender, British_Policing_Script.API.Functions.GetBritishPersona(ped).LSPDFRPersona.BirthDay, 0, British_Policing_Script.API.Functions.GetBritishPersona(ped).LSPDFRPersona.Forename, British_Policing_Script.API.Functions.GetBritishPersona(ped).LSPDFRPersona.Surname, British_Policing_Script.API.Functions.GetBritishPersona(ped).LSPDFRPersona.LicenseState, British_Policing_Script.API.Functions.GetBritishPersona(ped).LSPDFRPersona.TimesStopped, British_Policing_Script.API.Functions.GetBritishPersona(ped).LSPDFRPersona.Wanted, British_Policing_Script.API.Functions.GetBritishPersona(ped).LSPDFRPersona.IsAgent, British_Policing_Script.API.Functions.GetBritishPersona(ped).LSPDFRPersona.IsCop)
        {

        }
    }
}
