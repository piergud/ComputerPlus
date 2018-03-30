using ComputerPlus.Interfaces.Common;
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
            textbox.TextColorOverride = System.Drawing.Color.Red;
            textbox.SetText(message);
            textbox.UpdateColors();
        }

        internal static void Valid(this TextBox textbox, String message)
        {
            if (textbox == null || String.IsNullOrEmpty(message)) return;
            textbox.TextColorOverride = System.Drawing.Color.Green;
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

        

        internal static void LocalDateTime(this Label textbox, String dateTime, DateOutputPart part = DateOutputPart.ALL)
        {
            if (textbox == null) return;
            DateTime parsed;
            
            if (DateTime.TryParseExact(dateTime, Function.DateFormatForPart(part), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsed))
                textbox.LocalDateTime(parsed, part);
            else
                textbox.SetText(dateTime);
        }

        internal static void LocalDateTime(this Label textbox, String dateTime, DateOutputPart input = DateOutputPart.ALL, DateOutputPart output = DateOutputPart.ALL)
        {
            if (textbox == null) return;
            DateTime parsed;

            if (DateTime.TryParseExact(dateTime, Function.DateFormatForPart(input), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsed))
                textbox.LocalDateTime(parsed, output);
            else
                textbox.SetText(dateTime);
        }
        /*
        internal static void LocalDateTime(this Label textbox)
        {
            textbox.LocalDateTime(DateTime.Now.ToLocalTime(), DateOutputPart.ALL);
        }

        internal static void LocalDateTime(this Label textbox, DateOutputPart part = DateOutputPart.ALL)
        {
            textbox.LocalDateTime(DateTime.Now.ToLocalTime(), part);
        }
        */
        internal static void LocalDateTime(this Label textbox, DateTime date, DateOutputPart output = DateOutputPart.ALL)
        {
            if (textbox == null) return;
            textbox.Text = Function.ToLocalDateString(date, output);
        }

        internal static String GetAppendText(this MultilineTextBox textbox, String message, bool appendNewLine = true)
        {
            if (textbox.Text.Length == 0)
            {
                return message;
            }
            var hasText = !String.IsNullOrEmpty(textbox.Text);            
            var prevText = hasText ? textbox.Text : String.Empty;
            var lastString = hasText ? prevText.Substring(prevText.Length - 1) : null;
            var prevTextEndsInSpace = !hasText || lastString == null ? false : lastString.Equals(" ");
            var prevTextEndsInNewLine = !hasText ? false : lastString == null ? true : lastString == Environment.NewLine || lastString == "";
            var newText = String.Format("{0}{1}", hasText ? String.Empty : appendNewLine ? Environment.NewLine : prevTextEndsInSpace ? "" : prevTextEndsInNewLine ? "" : " ", message);
            return newText;
        }

        internal static void NormalSize(this TextBox textbox)
        {
            textbox.SetSize(166, 21);
        }

        internal static void LongSize(this TextBox textbox)
        {
            textbox.SetSize(332, 21);
        }

        internal static void MediumSize(this TextBox textbox)
        {
            textbox.SetSize(125, 21);
        }

        internal static void SmallSize(this TextBox textbox)
        {
            textbox.SetSize(84, 21);
        }

        internal static void NormalSize(this StateControlledTextbox textbox)
        {
            textbox.SetSize(166, 21);
        }

        internal static void LongSize(this StateControlledTextbox textbox)
        {
            textbox.SetSize(332, 21);
        }

        internal static void MediumSize(this StateControlledTextbox textbox)
        {
            textbox.SetSize(125, 21);
        }

        internal static void SmallSize(this StateControlledTextbox textbox)
        {
            textbox.SetSize(84, 21);
        }

    }
}
