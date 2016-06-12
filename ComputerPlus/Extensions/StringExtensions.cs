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
    internal static class StringExtensions
    {
        internal static string WordWrap(this String inputString, int width, string fontFamily)
        {
            inputString = WordWrapText(inputString,
                width,
                fontFamily,
                9f);

            return inputString;
        }

        private static string WordWrapText(string text, double pixels, string fontFamily, float size)
        {
            double emSize = size;
            emSize = (96.0f / 72.0f) * size;

            string[] originalLines = text.Split(new string[] { " " },
                 StringSplitOptions.None);

            List<string> separatedLines = new List<string>();
            List<string> wrappedLines = new List<string>();

            foreach (var item in originalLines)
            {
                string[] words = item.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (words.Length == 1)
                {
                    separatedLines.Add(words[0]);
                }
                else if (words.Length == 2)
                {
                    separatedLines.Add(words[0]);
                    separatedLines.Add("");
                    separatedLines.Add(words[1]);
                }
                else
                {
                    foreach (string w in words)
                    {
                        separatedLines.Add(w);
                    }
                }
            }

            StringBuilder actualLine = new StringBuilder();
            double actualWidth = 0;

            foreach (var item in separatedLines)
            {
                FormattedText formatted = new FormattedText(item,
                     CultureInfo.GetCultureInfo("en-us"),
                     System.Windows.FlowDirection.LeftToRight,
                     new Typeface(fontFamily), emSize, System.Windows.Media.Brushes.Black);

                actualLine.Append(item + " ");
                actualWidth += formatted.Width;

                if (actualWidth > pixels && item != "")
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