using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Controllers.Models
{
    public struct GunPermitInfo
    {
        public bool HasGunPermit;
        public String GunLicense;
        public String GunPermit;


        public GunPermitInfo(bool hasGunPermit, String gunLicense = null, String gunPermit = null)
        {
            HasGunPermit = hasGunPermit;
            GunLicense = gunLicense;
            GunPermit = gunPermit;
        }
    }
}
