using System;

using Rage;
using ComputerPlus.Interfaces.ComputerPedDB;
using ComputerPlus.Controllers.Models;

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

        internal static GunPermitInfo GetGunPermitInfo(this Ped ped)
        {
            if (ped != null && ped.IsValid())
            {
                if (ped.Metadata.hasGunPermit == null)
                {
                    if (Globals.Random.Next(0, 100) < 25)
                    {
                        string gunLicense = "Handguns";
                        int randomNum = Globals.Random.Next(0, 100);
                        if (randomNum < 30) gunLicense = "Long guns";
                        else if (randomNum < 50) gunLicense = "Handguns and Long guns";

                        string gunPermit = "Concealed";
                        randomNum = Globals.Random.Next(0, 100);
                        if (randomNum < 20) gunPermit = "Public";

                        ped.Metadata.hasGunPermit = true;
                        ped.Metadata.gunLicense = gunLicense;
                        ped.Metadata.gunPermit = gunPermit;
                        return new GunPermitInfo(true, gunLicense, gunPermit);
                    }
                    else
                    {
                        ped.Metadata.hasGunPermit = false;
                        return new GunPermitInfo(false);
                    }
                }
                else
                {
                    if (ped.Metadata.hasGunPermit == true)
                    {
                        return new GunPermitInfo(true, ped.Metadata.gunLicense, ped.Metadata.gunPermit);
                    }
                    else
                    {
                        return new GunPermitInfo(false);
                    }
                }
            } else
            {
                return new GunPermitInfo(false);
            }
        }

        internal static String GetDrivingLicenseExpirationDuration(this Ped ped)
        {
            if (ped.Metadata.DrivingLicenseExpirationDuration == null) {
                ped.Metadata.DrivingLicenseExpirationDuration = Globals.Random.Next(2, 720).ToString();
            }
            return ped.Metadata.DrivingLicenseExpirationDuration;
        }
    }
}
