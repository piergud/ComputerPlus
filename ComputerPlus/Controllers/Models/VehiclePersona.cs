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
    }
}
