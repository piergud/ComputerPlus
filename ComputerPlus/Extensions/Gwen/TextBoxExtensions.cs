using Gwen.Control;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Extensions.Gwen
{
    internal static class TextBoxExtensions
    {
        internal enum DateOutputPart { ALL = 0, DATE = 1, TIME = 2, ISO = 4 }
        internal static void Error(this TextBox textbox, String message)
        {
            if (textbox == null || String.IsNullOrEmpty(message)) return;
            textbox.TextColorOverride = System.Drawing.Color.Red;
            textbox.SetToolTipText(message);
            textbox.UpdateColors();
        }

        internal static void Warn(this TextBox textbox, String message)
        {
            if (textbox == null || String.IsNullOrEmpty(message)) return;
            //textbox.PaddingOutlineColor = System.Drawing.Color.Yellow;
            //textbox.MarginOutlineColor = System.Drawing.Color.Yellow;
            textbox.TextColorOverride = System.Drawing.Color.Red;
            textbox.SetText(message);
            textbox.UpdateColors();
        }

        internal static void ClearWarn(this TextBox textbox)
        {
            if (textbox == null) return;
            //textbox.TextColorOverride = textbox.TextColor;
            textbox.MakeColorNormal();
        }

        internal static void ClearError(this TextBox textbox)
        {
            if (textbox == null) return;
            textbox.TextColorOverride = textbox.TextColor;
            textbox.SetToolTipText(String.Empty);
            textbox.UpdateColors();
        }

        private static String DateFormatForPart(DateOutputPart part)
        {
            switch (part)
            {
                case DateOutputPart.DATE: return "d";
                case DateOutputPart.TIME: return "t";
                case DateOutputPart.ISO: return "o";
                default: return "g";
            }
        }

        internal static void LocalDateTime(this Label textbox, String dateTime, DateOutputPart part = DateOutputPart.ALL)
        {
            if (textbox == null) return;
            DateTime parsed;
            
            if (DateTime.TryParseExact(dateTime, DateFormatForPart(part), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsed))
                textbox.LocalDateTime(parsed, part);
            else
                textbox.SetText(dateTime);
        }

        internal static void LocalDateTime(this Label textbox, String dateTime, DateOutputPart input = DateOutputPart.ALL, DateOutputPart output = DateOutputPart.ALL)
        {
            if (textbox == null) return;
            DateTime parsed;

            if (DateTime.TryParseExact(dateTime, DateFormatForPart(input), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsed))
                textbox.LocalDateTime(parsed, output);
            else
                textbox.SetText(dateTime);
        }

        internal static void LocalDateTime(this Label textbox)
        {
            textbox.LocalDateTime(DateTime.Now.ToLocalTime(), DateOutputPart.ALL);
        }

        internal static void LocalDateTime(this Label textbox, DateOutputPart part = DateOutputPart.ALL)
        {
            textbox.LocalDateTime(DateTime.Now.ToLocalTime(), part);
        }

        internal static void LocalDateTime(this Label textbox, DateTime date, DateOutputPart output = DateOutputPart.ALL)
        {
            if (textbox == null) return;
            var local = date.ToLocalTime();
            switch (output)
            {                
                case DateOutputPart.DATE: textbox.Text = local.ToShortDateString(); break;
                case DateOutputPart.TIME: textbox.Text = local.ToShortTimeString(); break;
                case DateOutputPart.ISO: textbox.Text = local.ToString("g"); break;
                default: textbox.Text = local.ToString("f"); break;
            }
        }

        internal static void AppendText(this MultilineTextBox textbox, String message, bool appendNewLine = true)
        {
            textbox.SetText(String.Format("{0}{1}{2}",
                        textbox.Text,
                        String.IsNullOrEmpty(textbox.Text) ? String.Empty : appendNewLine ? Environment.NewLine : " ",
                        message
                    )
            );
        }
    }
}
