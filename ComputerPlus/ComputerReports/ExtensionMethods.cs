using Gwen.Control;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.ComputerReports
{
    class ExtensionMethods
    {
        public static void IncreaseProgressBar(ProgressBar progressBar, float value = 0.05f)
        {
            Game.LogTrivial("Increasing Progress Bar");
            float currentvalue = progressBar.Value;
            progressBar.Value = currentvalue + value;
        }
        public static void DecreaseProgressBar(ProgressBar progressBar, float value = 0.05f)
        {
            Game.LogTrivial("Decreasing Progress Bar");
            float currentvalue = progressBar.Value;
            progressBar.Value = currentvalue - value;
        }
    }
}
