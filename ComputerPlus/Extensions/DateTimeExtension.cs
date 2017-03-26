using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ComputerPlus.Extensions.Gwen.TextBoxExtensions;

namespace ComputerPlus.Extensions
{
    internal static class DateTimeExtension
    {
        internal static String ToLocalTimeString(this DateTime date, DateOutputPart output = DateOutputPart.ALL)
        {
            var local = date.ToLocalTime();
            switch (output)
            {
                case DateOutputPart.DATE: return local.ToShortDateString();
                case DateOutputPart.TIME: return local.ToShortTimeString();
                case DateOutputPart.ISO: return local.ToString("g");
                default: return local.ToString("f");
            }
        }
    }
}
