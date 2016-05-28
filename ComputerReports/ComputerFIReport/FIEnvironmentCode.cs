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
using Gwen.ControlInternal;
using System.Media;

namespace ComputerPlus
{
    public class FIEnvironmentCode2 : GwenForm
    {
        DateTime BDay;
        Persona PedPersona;
        Persona PedPersona1;
        Persona PedPersona2;
        Persona PedPersona3;
        Persona PedPersona4;
        string PedName1;
        string PedName2;
        string PedName3;
        string PedName4;
        string PedName5;
        private Vector3 CurrentLocation;

        // Housekeeping
        private Label FiskeyLabel;
        private Label MDTLabel2;
        private Label MenuLabel1;
        private Label MenuLabel2;
        private Label MenuLabel3;
        private Label MenuLabel4;
        private Label MenuLabel5;
        private ProgressBar ProgressBar;

        /// <summary>
        /// FI Section
        /// </summary>
        // Labels
        private Label FIInfo;
        private Label FIEnvironment;
        private Label FIDate;
        private Label FITime;
        private Label FILocation;
        private Label FICounty;
        private Label FIOfficerNumber;
        private Label FIOfficerName;
        private Label SuspectLast;
        private Label SuspectFirst;
        private Label CurrentStoppedLabel;
        private Label CurrentStreet;
        // Boxes
        private TextBox FIDateBox;
        private TextBox FITimeBox;
        private TextBox FILocationBox;
        private TextBox FICountyBox;
        private TextBox FIOfficerNumberBox;
        private TextBox FIOfficerNameBox;
        private TextBox SuspectLastBox;
        private TextBox SuspectFirstBox;
        private TextBox CurrentStoppedBox1;
        private TextBox CurrentStoppedBox2;
        private TextBox CurrentStoppedBox3;
        private TextBox CurrentStoppedBox4;
        private TextBox CurrentStreetBox;
        // Buttons
        private Button FIContinueButton;
        private Button FIBackButton;

        public FIEnvironmentCode2()
            : base(typeof(FIEnvironmentForm))
        {

        }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                Game.LogTrivial("Initializing FI Main");
                FIContinueButton.Clicked += OnFIContinueButtonClick;
                FIBackButton.Clicked += OnFIBackButtonClick;
                FIDateBox.Text = DateTime.Now.ToShortDateString();
                FITimeBox.Text = DateTime.Now.ToShortTimeString();
                FIOfficerNameBox.Text = CommonStuff.Settings.OfficerName;
                FIOfficerNumberBox.Text = CommonStuff.Settings.OfficerNumber;

                CurrentLocation = Game.LocalPlayer.Character.Position;
                string currentstreet = Rage.World.GetStreetName(CurrentLocation);
                CurrentStreetBox.Text = currentstreet;

                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped.Exists())
                    {
                        Persona pers = Functions.GetPersonaForPed(ped);
                        if (Functions.IsPedStoppedByPlayer(ped) == true)
                        {
                            PedPersona1 = pers;
                            PedName1 = pers.FullName;
                            CurrentStoppedBox1.Text = PedName1;
                        }
                    }
                }
                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped.Exists())
                    {
                        Persona pers = Functions.GetPersonaForPed(ped);
                        if (Functions.IsPedStoppedByPlayer(ped) == true && pers.FullName != PedPersona1.FullName)
                        {
                            PedPersona2 = pers;
                            PedName2 = pers.FullName;
                            CurrentStoppedBox2.Text = PedName2;
                        }
                    }
                }
                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped.Exists())
                    {
                        Persona pers = Functions.GetPersonaForPed(ped);
                        if (Functions.IsPedStoppedByPlayer(ped) == true && pers.FullName != PedPersona1.FullName && pers.FullName != PedPersona2.FullName)
                        {
                            PedPersona3 = pers;
                            PedName3 = pers.FullName;
                            CurrentStoppedBox3.Text = PedName3;
                        }
                    }
                }
                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped.Exists())
                    {
                        Persona pers = Functions.GetPersonaForPed(ped);
                        if (Functions.IsPedStoppedByPlayer(ped) == true && pers.FullName != PedPersona1.FullName && pers.FullName != PedPersona2.FullName && pers.FullName != PedPersona3.FullName)
                        {
                            PedPersona4 = pers;
                            PedName4 = pers.FullName;
                            CurrentStoppedBox4.Text = PedName4;
                        }
                    }
                }
                GameFiber.Yield();
            });
        }

        public void OnFIContinueButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            GameFiber.StartNew(delegate
            {
                Game.LogTrivial("FI environment saving for: " + SuspectLastBox.Text.ToString() + " " + SuspectFirstBox.Text.ToString());
                Game.DisplayNotification("Page 1 of 3 saved. Continuing Field Interaction form...");
                using (StreamWriter Information = new StreamWriter("LSPDFR/MDT/FIs/" + SuspectLastBox.Text.ToLower() + SuspectFirstBox.Text.ToLower() + ".txt", true))
                {
                    // 8 Lines
                    Information.WriteLine(" ");
                    Information.WriteLine(" ");
                    Information.WriteLine("---ENVIRONMENT---");
                    Information.WriteLine("Date: " + FIDateBox.Text);
                    Information.WriteLine("Time: " + FITimeBox.Text);
                    Information.WriteLine("Location: " + FILocation.Text);
                    Information.WriteLine("Officer Name and Number: " + FIOfficerNameBox.Text + " " + FIOfficerNumberBox.Text);
                    Information.WriteLine("Individual Full Name: " + SuspectFirstBox.Text + " " + SuspectLastBox.Text);
                }
                Game.LogTrivial("Successfully written to .txt");
                Window.Close();
            });
            GameFiber.StartNew(delegate
            {
                    GameFiber.Sleep(0500);
                    GwenForm form5 = new FIPersonalInfoCode2();
                    Game.IsPaused = true;
                    form5.Show();
                    form5.Position = new System.Drawing.Point(500, 250);
                    while (form5.Window.IsVisible)
                    {
                        GameFiber.Yield();
                    }

                    Game.IsPaused = true;
            });
        }

        private void OnFIBackButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Window.Close();
        }
    }
}
