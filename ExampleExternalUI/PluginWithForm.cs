using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
//using ComputerPlus.API;
using Rage;

namespace ExampleExternalUI
{
    public class PluginWithForm : Plugin
    {
        bool ComputerPlusRunning;
      
        public override void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(EntryPoint.LSPDFRResolveEventHandler);
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }

        private void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            ComputerPlusRunning = EntryPoint.IsLSPDFRPluginRunning("ComputerPlus", new Version("1.3.0.0"));
            if (ComputerPlusRunning)
            {
                ComputerPlusWrapper.RegisterComputerPlusInterface("Test", "ainesophaur", () => new ExampleForm(), 
                () =>
                {
                    Game.DisplayNotification("On Open");
                });
                Game.DisplayNotification("Registered");

                ComputerPlusWrapper.RegisterComputerPlusInterface("Another", "ainesophaur", () => new ExampleTwoForm());
            }                
            else
                Game.DisplayNotification("ComputerPlus is not running");
        }

        public override void Finally()
        {
        }

        
    }
}
