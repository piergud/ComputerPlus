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

namespace ComputerPlus
{
    public class NotebookCode2 : GwenForm
    {
        private Label FiskeyLabel;
        private Label InfoLabel1;
        private Label InfoLabel2;
        private MultilineTextBox MainBox;
        private Button SaveButton;
        private Label Sure;
        private Button EraseButton;
        private Button NoButton;
        private Button YesButton;
        private Button OpenMDT;

        public NotebookCode2()
            : base(typeof(NotebookForm))
        {

        }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                Game.LogTrivial("Initializing Police Notebook");
                this.SaveButton.Clicked += this.OnSaveButtonClick;
                this.EraseButton.Clicked += this.OnEraseButtonClick;
                this.NoButton.Clicked += this.OnNoButtonClick;
                this.YesButton.Clicked += this.OnYesButtonClick;
                this.OpenMDT.Clicked += this.OnOpenMDTButtonClick;
                Sure.Hide();
                NoButton.Hide();
                YesButton.Hide();
                Game.LogTrivial("Pulling previous information");
                string Exists = "LSPDFR/Police Notebook/Notebook.txt";
                if (File.Exists(Exists))
                {
                    string Notes = File.ReadAllText("LSPDFR/Police Notebook/Notebook.txt");
                    MainBox.Text = Notes;
                }
                GameFiber.Yield();
            });
        }

        private void OnSaveButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            using (StreamWriter Information = new StreamWriter("LSPDFR/Police Notebook/Notebook.txt", true))
            {
                Information.WriteLine(MainBox.Text);
            }
        }

        private void OnEraseButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Sure.Show();
            YesButton.Show();
            NoButton.Show();
        }
        private void OnNoButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Sure.Hide();
            NoButton.Hide();
            YesButton.Hide();
        }
        private void OnYesButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            GameFiber.StartNew(delegate
            {
                Sure.Hide();
                NoButton.Hide();
                YesButton.Hide();
                File.WriteAllText("LSPDFR/Police Notebook/Notebook.txt", String.Empty);
                GameFiber.Sleep(0500);
                string Exists = "LSPDFR/Police Notebook/Notebook.txt";
                if (File.Exists(Exists))
                {
                    string Notes = File.ReadAllText("LSPDFR/Police Notebook/Notebook.txt");
                    MainBox.Text = Notes;
                }
            });
        }
        private void OnOpenMDTButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Game.LogTrivial("Loading MDT via Notebook");
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(1000);
                Rage.Forms.GwenForm form11 = new ReportMain();
                Game.IsPaused = true;
                form11.Show();
                form11.Position = new System.Drawing.Point(500, 250);
                while (form11.Window.IsVisible)
                {
                    Game.IsPaused = true;
                    GameFiber.Yield();
                }

                Game.IsPaused = false;
            });
        }
    }
}
