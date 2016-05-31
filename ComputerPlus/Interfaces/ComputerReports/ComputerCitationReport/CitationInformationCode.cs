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

namespace ComputerPlus
{
    internal class CitationInformationCode : GwenForm
    {
        internal static string FirstName;
        internal static string LastName;
        internal static DateTime BDay;
        internal static Persona PedPersona;
        internal static Persona PedPersona1;
        internal static Persona PedPersona2;
        internal static Persona PedPersona3;
        internal static Persona PedPersona4;
        internal static string PedName1;
        internal static string PedName2;
        internal static string PedName3;
        internal static string PedName4;
        internal static string PedName5;
        internal string first;
        internal string last;
        internal static string firstname;
        internal static string lastname;

        // Housekeeping
        private Label FiskeyLabel;
        private Label MDTLabel2;
        private Label MenuLabel1;
        private Label MenuLabel2;
        private Label MenuLabel3;
        private Label MenuLabel4;
        private Label MenuLabel5;
        private ProgressBar ProgressBar;
        internal static GameFiber form_citationviolation = new GameFiber(OpenCitationViolationForm);

        /// <summary>
        /// Citation Section
        /// </summary>
        // Labels
        private Label InfoLabel;
        private Label CitationNumber;
        private Label RelatedReport;
        private Label Date;
        private Label Time;
        private Label SuspectLast;
        private Label SuspectFirst;
        private Label SuspectDOB;
        private Label SuspectAddress;
        private Label OfficerNumber;
        private Label OfficerName;
        // Boxes
        private TextBox CitationNumberBox;
        private TextBox CitationDateBox;
        private TextBox CitationTimeBox;
        private TextBox CitationRelatedBox;
        private TextBox CitationIssuedOfficerBox;
        private TextBox CitationIssuedOfficerNameBox;
        private TextBox CitationStreetInfoBox;
        private TextBox CitationSpeedBox;
        internal TextBox SuspectLastBox;
        internal TextBox SuspectFirstBox;
        private TextBox CitationPerpDOBBox;
        private TextBox CitationPerpStreetBox;
        internal TextBox CurrentStoppedBox1;
        internal TextBox CurrentStoppedBox2;
        internal TextBox CurrentStoppedBox3;
        internal TextBox CurrentStoppedBox4;
        // Button
        private Button CitationContinueButton;
        private Button CitationAutoLookupButton;
        private Button BackButton;

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
                this.CitationAutoLookupButton.Clicked += this.OnCitationAutoLookupClick;
                this.CitationContinueButton.Clicked += this.OnCitationContinueClick;
                BackButton.Clicked += OnBackButtonClick;
                CitationDateBox.Text = DateTime.Now.ToShortDateString();
                CitationTimeBox.Text = DateTime.Now.ToShortTimeString();
                int l = Configs.RandomNumber.r.Next(1, 14000);
                CitationNumberBox.Text = l.ToString("D5");
                CitationIssuedOfficerNameBox.Text = Configs.OfficerName;
                CitationIssuedOfficerBox.Text = Configs.OfficerNumber;
                PedCheck();
                GameFiber.Yield();
            });
        }

        internal void PedCheck()
        {
            if (Functions.IsPlayerPerformingPullover() == true)
            {
                LHandle pullover = Functions.GetCurrentPullover();
                Ped pulloverped = Functions.GetPulloverSuspect(pullover);
                if (pulloverped.Exists())
                {
                    Persona pers = Functions.GetPersonaForPed(pulloverped);
                    SuspectFirstBox.Text = pers.Forename.ToString();
                    SuspectLastBox.Text = pers.Surname.ToString();
                }
            }

            foreach (Ped ped in World.GetAllPeds())
            {
                if (ped.Exists())
                {
                    Persona pers = Functions.GetPersonaForPed(ped);
                    if (Functions.IsPedStoppedByPlayer(ped) == true)
                    {
                        PedPersona1 = pers;
                        CurrentStoppedBox1.Text = pers.FullName;
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
                        CurrentStoppedBox2.Text = pers.FullName;
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
                        CurrentStoppedBox3.Text = pers.FullName;
                    }
                }
            }
            foreach (Ped ped in World.GetAllPeds())
            {
                if (ped.Exists())
                {
                    Persona pers = Functions.GetPersonaForPed(ped);
                    if (Functions.IsPedGettingArrested(ped) == true || Functions.IsPedArrested(ped))
                    {
                        CurrentStoppedBox4.Text = pers.FullName;
                    }
                }
            }
        }

        private void OnCitationAutoLookupClick(Base sender, ClickedEventArgs arguments)
        {
            foreach (Ped ped in World.GetAllPeds())
            {
                if (ped.Exists())
                {
                    Persona pers = Functions.GetPersonaForPed(ped);
                    if (pers.FullName.ToLower() == SuspectFirstBox.Text.ToLower() + " " + SuspectLastBox.Text.ToLower())
                    {
                        PedPersona = pers;
                        BDay = PedPersona.BirthDay;
                        break;
                    }
                }
            }
            CitationPerpDOBBox.Text = BDay.ToShortDateString();

            int PerpNumber = Configs.RandomNumber.r.Next(1, 1200);

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

            int PerpStreet = Configs.RandomNumber.r.Next(PerpAddress.Count);

            CitationPerpStreetBox.Text = PerpNumber.ToString("D4") + " " + (string)PerpAddress[PerpStreet];
        }

        private void OnCitationContinueClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Check();
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
                Information.WriteLine("Offender Full Name: " + SuspectFirstBox.Text + " " + SuspectLastBox.Text);
                Information.WriteLine("Offender DOB: " + CitationPerpDOBBox.Text);
                Information.WriteLine("Offender Residence: " + CitationPerpStreetBox.Text);
                Information.WriteLine("Issuing Officer #: " + CitationIssuedOfficerBox.Text);
                Information.WriteLine("Issuing Officer Name: " + CitationIssuedOfficerNameBox.Text);
            }
            Game.LogTrivial("Citation page 1 submission success!");
            Game.DisplayNotification("Citation page 1 of 2 complete...");

            this.Window.Close();
            form_citationviolation = new GameFiber(OpenCitationViolationForm);
            form_citationviolation.Start();
        }

        internal void Check()
        {
            firstname = SuspectFirstBox.Text.ToLower();
            lastname = SuspectLastBox.Text.ToLower();
        }

        internal static void OpenCitationViolationForm()
        {
            GwenForm CitationViolation = new CitationViolationCode();
            CitationViolation.Show();
            while (CitationViolation.Window.IsVisible)
                GameFiber.Yield();
        }

        private void OnBackButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            this.Window.Close();
            ComputerMain.form_report = new GameFiber(ComputerMain.OpenReportMenuForm);
            ComputerMain.form_report.Start();
        }
    }
}
