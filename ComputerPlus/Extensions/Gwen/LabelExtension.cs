using Gwen.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Extensions.Gwen
{
    static class LabelExtension
    {
        internal static void Error(this Label label, String message)
        {
            if (label == null || String.IsNullOrEmpty(message)) return;
            label.TextColorOverride = System.Drawing.Color.Red;
            label.SetToolTipText(message);
            label.UpdateColors();
        }

        internal static void Warn(this Label label, String message)
        {
            if (label == null || String.IsNullOrEmpty(message)) return;
            label.TextColorOverride = System.Drawing.Color.Red;
            label.SetText(message);
            label.UpdateColors();
        }

        internal static void ClearWarn(this Label label)
        {
            if (label == null) return;
            label.TextColorOverride = label.TextColor;
            label.MakeColorNormal();
            label.UpdateColors();
        }

        internal static void ClearError(this Label label)
        {
            if (label == null) return;
            label.TextColorOverride = label.TextColor;
            label.SetToolTipText(String.Empty);
            label.UpdateColors();
        }

        internal static void Errors(this RichLabel label, List<String> messages, bool newLines = true)
        {
            if (label == null) return;
            StringBuilder sb = new StringBuilder();
            foreach(var message in messages) {

                if (!String.IsNullOrEmpty(message))
                {
                    var formatted = newLines ? String.Format("{0}{1}", message, Environment.NewLine) : message;
                    sb.Append(formatted);                                        
                }
            }
            label.AddText(sb.ToString(), label.Skin.Colors.Label.Default);
        }

        internal static void Error(this RichLabel label, String message)
        {
            if (label == null || String.IsNullOrEmpty(message)) return;
            label.AddText(message, System.Drawing.Color.Red);
            label.AddLineBreak();
        }

        internal static void Warn(this RichLabel label, String message)
        {
            if (label == null || String.IsNullOrEmpty(message)) return;
            label.AddText(message, System.Drawing.Color.Yellow);
            label.AddLineBreak();
        }

        internal static void ClearText(this RichLabel label)
        {
            if (label == null) return;
            label.DeleteAllChildren();
        }

    }
}
