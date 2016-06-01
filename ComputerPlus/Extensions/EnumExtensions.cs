using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Extensions
{
    internal static class EnumExtensions
    {
        internal static string ToFriendlyString(this Enum pEnum)
        {
            return pEnum.ToString().Replace("_", " ");
        }
    }
}