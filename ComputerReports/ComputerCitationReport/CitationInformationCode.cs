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

namespace ComputerPlus
{
    public class CitationInformationCode2 : GwenForm
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
        private TextBox SuspectLastBox;
        private TextBox SuspectFirstBox;
        private TextBox CitationPerpDOBBox;
        private TextBox CitationPerpStreetBox;
        private TextBox CurrentStoppedBox1;
        private TextBox CurrentStoppedBox2;
        private TextBox CurrentStoppedBox3;
        private TextBox CurrentStoppedBox4;
        // Button
        private Button CitationContinueButton;
        private Button CitationAutoLookupButton;

        public CitationInformationCode2()
            : base(typeof(CitationForm2))
        {

        }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                Game.LogTrivial("Initializing Citation Information");
                this.CitationAutoLookupButton.Clicked += this.OnCitationAutoLookupClick;
                this.CitationContinueButton.Clicked += this.OnCitationContinueClick;
                CitationDateBox.Text = DateTime.Now.ToShortDateString();
                CitationTimeBox.Text = DateTime.Now.ToShortTimeString();
                int l = CommonStuff.RandomNumber.r.Next(1, 14000);
                CitationNumberBox.Text = l.ToString("D5");
                CitationIssuedOfficerNameBox.Text = CommonStuff.Settings.OfficerName;
                CitationIssuedOfficerBox.Text = CommonStuff.Settings.OfficerNumber;

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

            int PerpNumber = CommonStuff.RandomNumber.r.Next(1, 1200);

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

            int PerpStreet = CommonStuff.RandomNumber.r.Next(PerpAddress.Count);

            CitationPerpStreetBox.Text = PerpNumber.ToString("D4") + " " + (string)PerpAddress[PerpStreet];
        }

        private void OnCitationContinueClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Game.LogTrivial("Citation page 1 submission begin...");
            using (StreamWriter Information = new StreamWriter("LSPDFR/MDT/MDT Citations.txt", true))
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
            Window.Close();
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(0500);
                GwenForm form8 = new CitationViolationCode2();
                Game.IsPaused = true;
                form8.Show();
                form8.Position = new System.Drawing.Point(500, 250);
                while (form8.Window.IsVisible)
                {
                    GameFiber.Yield();
                }

                Game.IsPaused = true;
            });
        }
    }
}
