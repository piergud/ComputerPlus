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
using System.Drawing;
using ComputerPlus.Interfaces.ComputerReports;

namespace ComputerPlus
{
    internal class CitationInformationCode : GwenForm
    {
        private ProgressBar ProgressBar;

        internal static GameFiber form_citationviolation = new GameFiber(OpenCitationViolationForm);

        private Label InfoLabel, CitationNumber, RelatedReport, Date, Time, 
            SuspectLast, SuspectFirst, SuspectDOB, SuspectAddress, OfficerNumber, OfficerName;

        private TextBox CitationNumberBox, CitationDateBox, CitationTimeBox, CitationRelatedBox, CitationIssuedOfficerBox,
            CitationIssuedOfficerNameBox, CitationStreetInfoBox, CitationSpeedBox, CitationPerpDOBBox, CitationPerpStreetBox, last_box, first_box;

        private ComboBox combo_last, combo_first;

        private Button CitationContinueButton, BackButton;

        private bool isPullover = false, ComboBox = false;

        public CitationInformationCode()
            : base(typeof(CitationForm2))
        {

        }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                Game.LogTrivial("Initializing Citation Information");
                this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);

                MethodStart();

                HideStuff();

                BoxFill();

                PedCheck();

                if (!ComboBox)
                {
                    NoComboBox();
                }

                GameFiber.Yield();
            });
        }

        private void MethodStart()
        {
            this.BackButton.Clicked += OnBackButtonClick;
            this.CitationContinueButton.Clicked += OnCitationContinueClick;
            this.combo_first.ItemSelected += AddtoProgressBarSelected;
            this.combo_first.ItemSelected += UpdateInformation;
            this.combo_last.ItemSelected += AddtoProgressBarSelected;
            this.combo_last.ItemSelected += UpdateInformation;
            this.first_box.TextChanged += UpdateInformation;
            this.last_box.TextChanged += UpdateInformation;
            this.CitationPerpDOBBox.TextChanged += AddtoProgressBarText;
            this.CitationPerpStreetBox.TextChanged += AddtoProgressBarText;
            this.CitationIssuedOfficerBox.TextChanged += AddtoProgressBarText;
            this.CitationIssuedOfficerNameBox.TextChanged += AddtoProgressBarText;
        }

        private void HideStuff()
        {
            combo_first.Hide();
            combo_last.Hide();
            first_box.Hide();
            last_box.Hide();
        }

        private void BoxFill()
        {
            CitationDateBox.Text = DateTime.Now.ToShortDateString();
            CitationTimeBox.Text = DateTime.Now.ToShortTimeString();
            int l = MathHelper.GetRandomInteger(1, 14000);
            CitationNumberBox.Text = l.ToString("D5");
            CitationIssuedOfficerNameBox.Text = Configs.Username;
            CitationIssuedOfficerBox.Text = Configs.UnitNumber;
            ProgressBar.Value = 0;
            ExtensionMethods.IncreaseProgressBar(ProgressBar, 0.15f);
        }
        
        private void AddtoProgressBarText (Base sender, EventArgs arguments)
        {
            ExtensionMethods.IncreaseProgressBar(ProgressBar);
        }

        private void AddtoProgressBarSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            ExtensionMethods.IncreaseProgressBar(ProgressBar);
        }

        private void PedCheck()
        {
            if (Functions.IsPlayerPerformingPullover() == true)
            {
                isPullover = true;
                LHandle pullover = Functions.GetCurrentPullover();
                Ped pulloverped = Functions.GetPulloverSuspect(pullover);
                if (pulloverped.Exists())
                {
                    Persona pers = Functions.GetPersonaForPed(pulloverped);
                    combo_first.AddItem(pers.Forename.ToString(), pers.Forename.ToString());
                    combo_last.AddItem(pers.Surname.ToString(), pers.Surname.ToString());
                    combo_first.Show();
                    combo_last.Show();
                    UpdateInfo(pers);
                    ComboBox = true;
                }
            }

            if (!isPullover)
            {
                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped.Exists())
                    {
                        Persona pers = Functions.GetPersonaForPed(ped);
                        if (Functions.IsPedStoppedByPlayer(ped) == true)
                        {
                            combo_first.AddItem(pers.Forename.ToString(), pers.Forename.ToString());
                            combo_last.AddItem(pers.Surname.ToString(), pers.Surname.ToString());
                            combo_first.Show();
                            combo_last.Show();
                            ComboBox = true;
                        }
                    }
                }
            }
        }

        private void NoComboBox()
        {
            first_box.Show();
            last_box.Show();
        }

        private void UpdateInformation(Base sender, EventArgs arguments)
        {
            foreach (Ped ped in World.GetAllPeds())
            {
                if (ped.Exists())
                {
                    Persona pers = Functions.GetPersonaForPed(ped);
                    if (sender == combo_last)
                    {
                        if (pers.Surname.ToLower() == combo_last.SelectedItem.Text.ToLower())
                        {
                            Game.LogTrivial("Match found -- changing first name");
                            combo_first.Text = pers.Forename;
                            UpdateInfo(pers);
                            break;
                        }
                    }
                    else if (sender == combo_first)
                    {
                        if (pers.Forename.ToLower() == combo_first.SelectedItem.Text.ToLower())
                        {
                            Game.LogTrivial("Match found -- changing last name");
                            combo_last.Text = pers.Surname;
                            UpdateInfo(pers);
                            break;
                        }
                    }
                    else if (sender == last_box)
                    {
                        if (pers.Surname.ToLower() == combo_last.SelectedItem.Text.ToLower())
                        {
                            Game.LogTrivial("Match found -- changing first name");
                            first_box.Text = pers.Forename;
                            UpdateInfo(pers);
                            break;
                        }
                    }
                    else if (sender == first_box)
                    {
                        if (pers.Forename.ToLower() == combo_first.SelectedItem.Text.ToLower())
                        {
                            Game.LogTrivial("Match found -- changing last name");
                            last_box.Text = pers.Surname;
                            UpdateInfo(pers);
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateInfo(Persona persona)
        {
            CitationPerpDOBBox.Text = persona.BirthDay.ToShortDateString();

            CitationPerpStreetBox.Text = GetRandomAddress();
        }
        
        private void OnCitationContinueClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            WriteData();
            
            this.Window.Close();
            form_citationviolation = new GameFiber(OpenCitationViolationForm);
            form_citationviolation.Start();
        }

        private void WriteData()
        {
            Game.LogTrivial("Citation page 1 submission begin...");
            using (StreamWriter Information = new StreamWriter("Plugins/LSPDFR/ComputerPlus/citations/completedcitations.txt", true))
            {
                Information.WriteLine(" ");
                Information.WriteLine(" ");
                Information.WriteLine("---INFORMATION---");
                Information.WriteLine("Citation Number: " + CitationNumberBox.Text);
                Information.WriteLine("Related Report Number: " + CitationRelatedBox.Text);
                Information.WriteLine("Date of incident: " + CitationDateBox.Text);
                Information.WriteLine("Time of incident: " + CitationTimeBox.Text);
                if (!ComboBox)
                {
                    Information.WriteLine("Individual Full Name: " + first_box.Text + " " + last_box.Text);
                }
                else
                {
                    Information.WriteLine("Individual Full Name: " + combo_first.Text + " " + combo_last.Text);
                }
                Information.WriteLine("Offender DOB: " + CitationPerpDOBBox.Text);
                Information.WriteLine("Offender Residence: " + CitationPerpStreetBox.Text);
                Information.WriteLine("Issuing Officer #: " + CitationIssuedOfficerBox.Text);
                Information.WriteLine("Issuing Officer Name: " + CitationIssuedOfficerNameBox.Text);
            }
            Game.LogTrivial("Citation page 1 submission success!");
        }

        private bool susFound = false;

        private static void OpenCitationViolationForm()
        {
            GwenForm CitationViolation = new CitationViolationCode();
            CitationViolation.Show();
            while (CitationViolation.Window.IsVisible)
                GameFiber.Yield();
        }

        private void OnBackButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            this.Window.Close();
        }

        private string GetRandomAddress()
        {
            int PerpNumber = MathHelper.GetRandomInteger(1, 1200);

            List<string> PerpAddress = new List<string>();
            PerpAddress.Add("Alta Street, LSC");
            PerpAddress.Add("Amarillo Way, LSC");
            PerpAddress.Add("Banham Canyon Drive, LSC");
            PerpAddress.Add("Bay City Avenue, LSC");
            PerpAddress.Add("Bridge Street, LSC");
            PerpAddress.Add("Capital Boulevard, LSC");
            PerpAddress.Add("Clinton Avenue, LSC");
            PerpAddress.Add("Dutch London Street, LSC");
            PerpAddress.Add("El Rancho Boulevard, LSC");
            PerpAddress.Add("Glory Way, LSC");
            PerpAddress.Add("Lake Vinewood Drive, LSC");
            PerpAddress.Add("Magellan Avenue, LSC");
            PerpAddress.Add("Melanoma Street, LSC");
            PerpAddress.Add("Normandy Drive, LSC");
            PerpAddress.Add("Rub Street, LSC");
            PerpAddress.Add("Sinner Street, LSC");
            PerpAddress.Add("Spanish Avenue, LSC");
            PerpAddress.Add("Swiss Street, LSC");
            PerpAddress.Add("Tower Way, LSC");
            PerpAddress.Add("Vespucci Boulevard, LSC");
            PerpAddress.Add("West Eclipse Boulevard, LSC");
            PerpAddress.Add("West Mirror Drive, LSC");
            PerpAddress.Add("Algonquin Boulevard, BC");
            PerpAddress.Add("Calafia Road, BC");
            PerpAddress.Add("Cholla Road, BC");
            PerpAddress.Add("Joshua Road, BC");
            PerpAddress.Add("Marina Drive, BC");
            PerpAddress.Add("North Calafia Way, BC");
            PerpAddress.Add("ONeil Way, BC");
            PerpAddress.Add("Panorama Drive, BC");
            PerpAddress.Add("Procopio Drive, BC");
            PerpAddress.Add("Raton Pass, BC");
            PerpAddress.Add("Senora Way, BC");
            PerpAddress.Add("Union Road, BC");

            int PerpStreet = MathHelper.GetRandomInteger(PerpAddress.Count);

            return PerpNumber.ToString("D4") + " " + (string)PerpAddress[PerpStreet];
        }
    }
}
