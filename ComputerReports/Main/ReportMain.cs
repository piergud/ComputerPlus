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
        private Button PoliceNotebook;
        public static GameFiber form_main = new GameFiber(OpenMainMenuForm);

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
            this.PoliceNotebook.Clicked += this.OnPoliceNotebookClick;
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
        }

        private static void OpenMainMenuForm()
        {
            GwenForm main = new ComputerMain();
            main.Show();
            while (main.Window.IsVisible)
                GameFiber.Yield();
        }

        public void OnHomeButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            this.Window.Close();
            form_main = new GameFiber(OpenMainMenuForm);
            form_main.Start();
        }

        private void OnReportCitationClick(Base sender, ClickedEventArgs arguments)
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(0500);
                Game.LogTrivial("Loading Citation -- Based on SearchForm");
                GwenForm CitaitonForm = new CitationInformationCode2();
                Game.IsPaused = true;
                CitaitonForm.Show();
                CitaitonForm.Position = new System.Drawing.Point(500, 250);
                while (CitaitonForm.Window.IsVisible)
                {
                    GameFiber.Yield();
                }
                Game.IsPaused = true;
            });
        }

        private void OnReportArrestClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            // In progress
        }

        private void OnFIButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(0500);
                Game.LogTrivial("Loading FIs -- Based on CitationForm");
                GwenForm FIForm = new FIEnvironmentCode2();
                Game.IsPaused = true;
                FIForm.Show();
                FIForm.Position = new System.Drawing.Point(500, 250);
                while (FIForm.Window.IsVisible)
                {
                    GameFiber.Yield();
                }

                Game.IsPaused = true;
            });
        }

        private void OnPoliceNotebookClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Game.LogTrivial("Loading Police Notebook via MDT");
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(0500);
                Rage.Forms.GwenForm PNotebook = new NotebookCode2();
                Game.IsPaused = true;
                PNotebook.Show();
                PNotebook.Position = new System.Drawing.Point(900, 100);
                while (PNotebook.Window.IsVisible)
                {
                    Game.IsPaused = true;
                    GameFiber.Yield();
                }
                Game.IsPaused = false;
            });
        }
    }
}
