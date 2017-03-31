using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Controllers.Models
{
    public struct VehiclePersona
    {
        public bool? HasInsurance;
        public bool? IsRegistered;
        public String Alert;
        public String Color;
        public bool? IsOffroadOnly;
        public bool? HasValidEmissions;
        public bool? IsTaxed;
        public Object RawPersona;
        

        public VehiclePersona(bool? hasInsurance, bool? isRegistered, String alert = null, Object rawPersona = null)
        {
            HasInsurance = hasInsurance;
            IsRegistered = isRegistered;
            Alert = alert;
            Color = String.Empty;
            IsOffroadOnly = null;
            HasValidEmissions = null;
            IsTaxed = null;
            RawPersona = rawPersona;
        }      
        
        public VehiclePersona(British_Policing_Script.VehicleRecords records)
        {
            IsRegistered = null;
            HasInsurance = records.Insured;
            Color = records.CarColour;

            Alert = new System.Text.RegularExpressions.Regex(@"~\w~").Replace(records.DetermineFlags(), String.Empty);
            HasValidEmissions = records.HasMOT;
            IsOffroadOnly = records.HasSORN;
            IsTaxed = records.IsTaxed;
            RawPersona = records;
        }
    }
}
