using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using ComputerPlus.Interfaces.ComputerPedDB;

namespace ComputerPlus.Extensions.Rage
{
    internal static class PedExtension
    {
        internal static String GetHomeAddress(this Ped ped)
        {
            if (ped.Metadata.HomeAddress == null) ped.Metadata.HomeAddress = ComputerPedController.GetRandomStreetAddress();
            return ped.Metadata.HomeAddress;
        }

        internal static String GetWantedReason(this Ped ped)
        {
            if (ped.Metadata.WantedReason == null) ped.Metadata.WantedReason = ComputerPedController.GetRandomWantedReason();
            return ped.Metadata.WantedReason;
        }

        internal static String GetDrivingLicenseExpirationDuration(this Ped ped)
        {
            if (ped.Metadata.DrivingLicenseExpirationDuration == null) {
                ped.Metadata.DrivingLicenseExpirationDuration = Globals.Random.Next(2, 360).ToString();
            }
            return ped.Metadata.DrivingLicenseExpirationDuration;
        }
    }
}
