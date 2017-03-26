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
    }
}
