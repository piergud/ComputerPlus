using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.ComputerReports
{
    [Serializable]
    class PedData
    {
        public bool ComboBox { get; set; } = false;
        public bool isPed { get; set; } = false;
        public Ped Suspect { get; set; }
        public Persona Sus_Persona { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }

        public PedData() { }

        public PedData(Ped ped)
        {
            isPed = true;
            Sus_Persona = Functions.GetPersonaForPed(ped);
            FirstName = Sus_Persona.Forename;
            LastName = Sus_Persona.Surname;
            DOB = Sus_Persona.BirthDay.ToShortDateString();
        }
    }
}
