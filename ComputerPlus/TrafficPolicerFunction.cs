using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using Traffic_Policer;
using Traffic_Policer.API;

namespace ComputerPlus
{
    internal static class TrafficPolicerFunction
    {
        internal static EVehicleStatus GetVehicleRegistrationStatus(Vehicle veh)
        {
            if (veh.Exists())
            {
                return FromTrafficPolicer(Functions.GetVehicleRegistrationStatus(veh));
            }
            else
            {
                return EVehicleStatus.Valid;
            }
        }

        internal static EVehicleStatus GetVehicleInsuranceStatus(Vehicle veh)
        {
            if (veh.Exists())
            {
                return FromTrafficPolicer(Functions.GetVehicleInsuranceStatus(veh));
            }
            else
            {
                return EVehicleStatus.Valid;
            }
        }

        internal static void SetVehicleRegistrationStatus(Vehicle veh, EVehicleStatus RegistrationStatus)
        {
            if (veh.Exists())
            {
                Functions.SetVehicleRegistrationStatus(veh, ToTrafficPolicer(RegistrationStatus));
            }
        }

        internal static void SetVehicleInsuranceStatus(Vehicle veh, EVehicleStatus InsuranceStatus)
        {
            if (veh.Exists())
            {
                Functions.SetVehicleInsuranceStatus(veh, ToTrafficPolicer(InsuranceStatus));
            }
        }

        private static EVehicleStatus FromTrafficPolicer(EVehicleDetailsStatus status)
        {
            if (status == EVehicleDetailsStatus.Expired)
                return EVehicleStatus.Expired;
            else if (status == EVehicleDetailsStatus.None)
                return EVehicleStatus.None;
            else
                return EVehicleStatus.Valid;
        }

        private static EVehicleDetailsStatus ToTrafficPolicer(EVehicleStatus status)
        {
            if (status == EVehicleStatus.Expired)
                return EVehicleDetailsStatus.Expired;
            else if (status == EVehicleStatus.None)
                return EVehicleDetailsStatus.None;
            else
                return EVehicleDetailsStatus.Valid;
        }
    }
}
