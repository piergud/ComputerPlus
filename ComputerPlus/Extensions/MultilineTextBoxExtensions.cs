using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Media;
using Gwen.Control;
using Rage;

namespace ComputerPlus.Extensions
{
    internal static class MultilineTextBoxExtensions
    {
        internal static void WordWrap(this MultilineTextBox textbox, int width)
        {
            textbox.Text = WordWrapText(textbox.Text,
                width,
                textbox.Font.FaceName.ToString(),
                9f);
        }

        private static string WordWrapText(string text, double pixels, string fontFamily, float size)
        {
            string[] originalLines = text.Split(new string[] { " " }, StringSplitOptions.None);

            List<string> wrappedLines = new List<string>();

            StringBuilder actualLine = new StringBuilder();
            double actualWidth = 0;

            foreach (var item in originalLines)
            {
                int width = (int)Math.Round(Graphics.MeasureText(item, fontFamily, size).Width);

                actualLine.Append(item + " ");
                actualWidth += width;

                if (actualWidth > pixels)
                {
                    wrappedLines.Add(actualLine.ToString());
                    actualLine.Clear();
                    actualWidth = 0;
                }
            }

            if (actualLine.Length > 0)
                wrappedLines.Add(actualLine.ToString());

            string allLines = string.Join(Environment.NewLine, wrappedLines.ToArray());

            return allLines;
        }
    }
}