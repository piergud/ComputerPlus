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

        public VehiclePersona(bool? hasInsurance, bool? isRegistered, String alert = null)
        {
            HasInsurance = hasInsurance;
            IsRegistered = isRegistered;
            Alert = alert;
        }       
    }
}
