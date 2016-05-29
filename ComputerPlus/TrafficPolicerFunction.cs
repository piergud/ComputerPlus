using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using Traffic_Policer.API;

namespace ComputerPlus
{
    internal static class TrafficPolicerFunction
    {
        internal static bool GetVehicleInsured(Vehicle veh)
        {
            return Functions.IsVehicleInsured(veh);
        }
    }
}
