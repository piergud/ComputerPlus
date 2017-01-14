using Gwen.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Extensions.Gwen
{
    internal static class TextBoxExtensions
    {
        internal static void Error(this TextBox textbox, String message)
        {
            if (textbox == null) return;
            textbox.TextColorOverride = System.Drawing.Color.Red;
            textbox.SetToolTipText(message);
            textbox.UpdateColors();
        }

        internal static void ClearError(this TextBox textbox)
        {
            if (textbox == null) return;
            textbox.TextColorOverride = textbox.TextColor;
            textbox.ToolTip = null;
            textbox.UpdateColors();
        }
    }
}
