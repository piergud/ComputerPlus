using System;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using Rage;
using Rage.Forms;
using Gwen.Control;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;

namespace ComputerPlus
{
    internal class ComputerCurrentCallDetails : GwenForm
    {
        private Button btn_main;
        private MultilineTextBox output_info;
        internal static GameFiber form_main = new GameFiber(OpenMainMenuForm);

        public ComputerCurrentCallDetails() : base(typeof(ComputerCurrentCallDetailsTemplate))
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.btn_main.Clicked += this.MainMenuButtonClickedHandler;
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            output_info.KeyboardInputEnabled = false;

            FillCallDetails();
        }

        private void MainMenuButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
            form_main = new GameFiber(OpenMainMenuForm);
            form_main.Start();
        }

        private static void OpenMainMenuForm()
        {
            GwenForm main = new ComputerMain();
            main.Show();
            while (main.Window.IsVisible)
                GameFiber.Yield();
        }

        private void FillCallDetails()
        {
            String callText = "";

            if (EntryPoint.gActiveCallout != null)
            {
                callText += EntryPoint.gActiveCallout.FullName;
                callText += Environment.NewLine;
                callText += Environment.NewLine;
                callText += EntryPoint.gActiveCallout.Description;
            }

            output_info.Text = callText;
        }
    }
}