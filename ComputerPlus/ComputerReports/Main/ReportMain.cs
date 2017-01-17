using Rage;
using Rage.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Notsolethalpolicing.MDT;
using LSPD_First_Response;
using LSPD_First_Response.Mod.API;
using System.IO;
using LSPD_First_Response.Engine.Scripting.Entities;
using System.Drawing;

namespace ComputerPlus
{
    public class ReportMain : GwenForm
    {
        // Possible status updates?
        private TextBox CurrentStatus;

        // Home
        private Label MDTLabel2;
        private Button HomeButton;
        private Label InfoLabel1;
        private Button FIButton;

        // Report
        private Button ReportCitation;
        private Button ReportArrest;

        public ReportMain()
            : base(typeof(MainForm))
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            Game.LogTrivial("Initializing Reports");
            this.HomeButton.Clicked += this.OnHomeButtonClick;
            this.ReportArrest.Clicked += this.OnReportArrestClick;
            this.ReportCitation.Clicked += this.OnReportCitationClick;
            this.FIButton.Clicked += this.OnFIButtonClick;
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
        }


        public void OnHomeButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            this.Window.Close();
        }

        private void OnReportCitationClick(Base sender, ClickedEventArgs arguments)
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(0500);
                Game.LogTrivial("Loading Citation");
                GwenForm CitaitonForm = new CitationInformationCode();
                Game.IsPaused = true;
                CitaitonForm.Show();
                CitaitonForm.Position = new System.Drawing.Point(500, 250);
                while (CitaitonForm.Window.IsVisible)
                    GameFiber.Yield();

                Game.IsPaused = true;
            });
        }

        private void OnReportArrestClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(0500);
                Game.LogTrivial("Loading Arrest Form");
                GwenForm ArrestForm = new ArrestDataCode();
                Game.IsPaused = true;
                ArrestForm.Show();
                ArrestForm.Position = new System.Drawing.Point(500, 250);
                while (ArrestForm.Window.IsVisible)
                    GameFiber.Yield();

                Game.IsPaused = true;
            });
        }

        private void OnFIButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(0500);
                Game.LogTrivial("Loading FIs");
                GwenForm FIForm = new FIEnvironmentCode();
                Game.IsPaused = true;
                FIForm.Show();
                FIForm.Position = new System.Drawing.Point(500, 250);
                while (FIForm.Window.IsVisible)
                    GameFiber.Yield();

                Game.IsPaused = true;
            });
        }
    }
}
