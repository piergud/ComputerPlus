using Gwen.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen;
using SystemDrawing = System.Drawing;

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

        internal static Label StyleHeader(this Label label)
        {
            if (Globals.Style.LabelHeaderFont == null)
            {
                Globals.Style.LabelHeaderFont = label.Font.Copy();
                Globals.Style.LabelHeaderFont.FaceName = Configs.RegularFontName;
                Globals.Style.LabelHeaderFont.Size = 20;
            }
            
            label.Font = Globals.Style.LabelHeaderFont;
            return label;
        }

        internal static Label StyleHeaderBold(this Label label)
        {
            if (Globals.Style.LabelHeaderFontBold == null)
            {
                Globals.Style.LabelHeaderFontBold = label.Font.Copy();
                Globals.Style.LabelHeaderFontBold.FaceName = Configs.RegularBoldFontName;
                Globals.Style.LabelHeaderFontBold.Size = 20;
            }
            label.Font = Globals.Style.LabelHeaderFontBold;
            return label;
        }

        internal static Label StyleRegularBold(this Label label)
        {
            if (Globals.Style.BoldFont == null)
            {
                Globals.Style.BoldFont = label.Font.Copy();
                Globals.Style.BoldFont.FaceName = Configs.RegularBoldFontName;
                Globals.Style.BoldFont.Size = 16;
            }
            label.Font = Globals.Style.BoldFont;
            return label;
        }

        internal static Label StyleRegular(this Label label)
        {
            if (Globals.Style.RegularFont == null)
            {
                Globals.Style.RegularFont = label.Font.Copy();
                Globals.Style.RegularFont.FaceName = Configs.RegularFontName;
                Globals.Style.RegularFont.Size = 16;
            }

            label.Font = Globals.Style.RegularFont;
            return label;
        }

        internal static RichLabel Text(this RichLabel label, String text, SystemDrawing.Color color = default(SystemDrawing.Color), Font textFont = default(Font))
        {
            color = color == SystemDrawing.Color.Empty ? SystemDrawing.Color.Black : color;
            label.AddText(text, color, textFont);
            return label;
        }

        internal static RichLabel Text(this RichLabel label, String text, SystemDrawing.Color color = default(SystemDrawing.Color))
        {
            color = color == SystemDrawing.Color.Empty ? SystemDrawing.Color.Black : color;
            if (Globals.Style.RegularFont == null)
            {
                Globals.Style.RegularFont = new Font(label.Skin.Renderer, Configs.RegularFontName, 16);
            }
            label.Text(text, color, Globals.Style.RegularFont);
            return label;
        }

        internal static RichLabel TextBold(this RichLabel label, String text, SystemDrawing.Color color = default(SystemDrawing.Color))
        {
            color = color == SystemDrawing.Color.Empty ? SystemDrawing.Color.Black : color;
            if (Globals.Style.BoldFont == null)
            {
                Globals.Style.BoldFont = new Font(label.Skin.Renderer, Configs.RegularBoldFontName, 16);                
            }
            var measure = label.Skin.Renderer.MeasureText(Globals.Style.BoldFont, text);
            int? width = null, height = null;
            if (measure.X > label.Width) width = label.Width + measure.X;
            if (measure.Y > label.Height) height = label.Height + measure.Y;
            if (width.HasValue || height.HasValue)
            {
                label.SetSize(width.HasValue ? width.Value : label.Width, height.HasValue ? height.Value : label.Height);
            }
            label.Text(text, color, Globals.Style.BoldFont);
            return label;
        }

    }
}
