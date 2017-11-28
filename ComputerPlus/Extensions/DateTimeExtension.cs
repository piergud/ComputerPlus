using System;
using static ComputerPlus.Extensions.Gwen.TextBoxExtensions;

namespace ComputerPlus.Extensions
{
    internal static class DateTimeExtension
    {
        internal static String ToLocalTimeString(this DateTime date, DateOutputPart output = DateOutputPart.ALL)
        {
            return date.ToDateTimeString(output, true);
        }

        internal static String ToDateTimeString(this DateTime date, DateOutputPart output = DateOutputPart.ALL, bool convertToLocal = false)
        {
            var local = date;
            if (convertToLocal) local = date.ToLocalTime();
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
