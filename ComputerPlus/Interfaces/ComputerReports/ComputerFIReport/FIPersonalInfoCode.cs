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
using System.Drawing;

namespace ComputerPlus
{
    internal class FIPersonalInfoCode : GwenForm
    {
        internal DateTime BDay;
        internal Persona PedPersona;
        internal Persona PedPersona1;
        internal Persona PedPersona2;
        internal Persona PedPersona3;
        internal Persona PedPersona4;
        internal string PedName1;
        internal string PedName2;
        internal string PedName3;
        internal string PedName4;
        internal string PedName5;
        internal string FITotal;
        internal string FirstName;
        internal string LastName;
        internal int FINumber;
        internal bool PedWanted;
        internal string LicenseState;
        internal ELicenseState PedLicense;
        internal static SubmitCheck state;
        internal static string firstname;
        internal static string lastname;
        internal static string vehplate;

        // Housekeeping
        private Label FiskeyLabel;
        private Label MDTLabel2;
        private Label MenuLabel1;
        private Label MenuLabel2;
        private Label MenuLabel3;
        private Label MenuLabel4;
        private Label MenuLabel5;
        private ProgressBar ProgressBar;
        internal static GameFiber form_firemarks = new GameFiber(OpenFIRemarksForm);

        /// <summary>
        /// FI Section
        /// </summary>
        // Labels
        private Label FIInfo;
        private Label FIPersonalInfo;
        private Label CreditLabel;
        private Label SuspectLast;
        private Label SuspectFirst;
        private Label SuspectDOB;
        private Label SuspectSSN;
        private Label SuspectOccupation;
        private Label SuspectAddress;
        private Label SuspectCity;
        private Label SuspectSex;
        private Label SuspectRace;
        private Label SuspectHair;
        private Label SuspectEyes;
        private Label SuspectMark;
        private Label SuspectLicense;
        private Label SuspectVehicle;
        private Label SuspectPlate;
        private Label CurrentStoppedLabel;
        private Label Wanted;
        // Boxes
        internal TextBox SuspectLastBox;
        internal TextBox SuspectFirstBox;
        private TextBox SuspectDOBBox;
        private TextBox SuspectSSNBox;
        private TextBox SuspectOccupationBox;
        private TextBox SuspectAddressBox;
        private TextBox SuspectCityBox;
        private TextBox SuspectSexBox;
        private TextBox SuspectRaceBox;
        private TextBox SuspectHairBox;
        private TextBox SuspectEyesBox;
        private TextBox SuspectMarkBox;
        private TextBox SuspectLicenseBox;
        private TextBox SuspectVehicleBox;
        private TextBox SuspectPlateBox;
        private TextBox CurrentStoppedBox1;
        private TextBox CurrentStoppedBox2;
        private TextBox CurrentStoppedBox3;
        private TextBox CurrentStoppedBox4;
        private TextBox FITotalBox;
        private MultilineTextBox FIBox;
        // Button
        private Button FIContinueButton;
        private Button FILookupButton;
        private Button ShowFIButton;
        private Button FIBackButton;

        public FIPersonalInfoCode()
            : base(typeof(FIPersonalInfoForm))
        {

        }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                state = SubmitCheck.inprogress;
                Game.LogTrivial("Initializing FI Personal Info");
                this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
                FIContinueButton.Clicked += OnFIContinueButtonClick;
                FILookupButton.Clicked += OnFILookupButtonClick;
                ShowFIButton.Clicked += OnShowFIButtonClick;
                FIBackButton.Clicked += OnFIBackButtonClick;
                ShowFIButton.Hide();
                FIBox.Hide();
                Wanted.Hide();
                PedCheck();
                VehCheck();
                GameFiber.Yield();
            });
        }
        internal void VehCheck()
        {
            if (Functions.IsPlayerPerformingPullover() == true)
            {
                Vehicle[] vehs = World.GetAllVehicles();
                for (int i = 0; i < vehs.Length; i++)
                {

                    SuspectPlateBox.Text = vehs[i].LicensePlate;
                }
            }
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
                    PedPersona1 = pers;
                    FirstName = pers.Forename;
                    LastName = pers.Surname;
                    SuspectLastBox.Text = LastName;
                    SuspectFirstBox.Text = FirstName;
                    CurrentStoppedBox1.Text = pers.FullName;
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
        }

        internal void OnFIContinueButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            GameFiber.StartNew(delegate
            {
                Check();
                Game.LogTrivial("FI personal info saving for: " + SuspectLastBox.Text.ToString() + " " + SuspectFirstBox.Text.ToString());
                Game.DisplayNotification("Page 2 of 3 saved. Continuing Field Interaction form...");
                using (StreamWriter Information = new StreamWriter("Plugins/LSPDFR/ComputerPlus/field interviews/" + SuspectLastBox.Text.ToLower() + SuspectFirstBox.Text.ToLower() + ".txt", true))
                {
                    // 13 lines
                    Information.WriteLine("---PERSONAL INFORMATION---");
                    Information.WriteLine("DOB: " + SuspectDOBBox.Text);
                    Information.WriteLine(SuspectSSNBox.Text);
                    Information.WriteLine("Occupation: " + SuspectOccupationBox.Text);
                    Information.WriteLine("Address: " + SuspectAddressBox.Text + " " + SuspectCityBox.Text);
                    Information.WriteLine("Sex: " + SuspectSexBox.Text);
                    Information.WriteLine("Race: " + SuspectRaceBox.Text);
                    Information.WriteLine("Hair: " + SuspectHairBox.Text);
                    Information.WriteLine("Eyes: " + SuspectEyesBox.Text);
                    Information.WriteLine("Scars/Tattoos: " + SuspectMarkBox.Text);
                    Information.WriteLine("License Status: " + SuspectLicenseBox.Text);
                    Information.WriteLine("Vehicle Make, Model, Color: " + SuspectVehicleBox.Text);
                    Information.WriteLine("Vehicle Plate: " + SuspectPlateBox.Text);
                }
                Game.LogTrivial("Successfully written to .txt");
            });
            state = SubmitCheck.submitted;
            FIComplete();
            this.Window.Close();
            form_firemarks = new GameFiber(OpenFIRemarksForm);
            form_firemarks.Start();
        }

        internal static void OpenFIRemarksForm()
        {
            GwenForm FIRemarks = new FIRemarksCode();
            FIRemarks.Show();
            while (FIRemarks.Window.IsVisible)
                GameFiber.Yield();
        }
        internal static bool FIComplete()
        {
            return true;
        }
        internal void Check()
        {
            firstname = SuspectFirstBox.Text.ToLower();
            lastname = SuspectLastBox.Text.ToLower();
        }

        /// <summary>
        /// Crash noticed with FITotalNumber -- will investigate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arguments"></param>
        internal void OnFILookupButtonClick(Base sender, ClickedEventArgs arguments)
        {
            int PerpNumber = Configs.RandomNumber.r.Next(1, 1200);

            Game.LogTrivial("Initializing FI Lookup");
            string Exists = "Plugins/LSPDFR/ComputerPlus/field interviews/" + SuspectLastBox.Text.ToLower() + SuspectFirstBox.Text.ToLower() + ".txt";   
            if (File.Exists(Exists))
            {
                try
                {
                    double FINumber = File.ReadAllLines("Plugins/LSPDFR/ComputerPlus/field interviews/" + SuspectLastBox.Text.ToLower() + SuspectFirstBox.Text.ToLower() + ".txt").Count();
                    double FITotalNumber = (double)FINumber / (double)32;
                    double FIRounded = Math.Round(FITotalNumber, 1, MidpointRounding.AwayFromZero);
                    FITotalBox.Text = FIRounded.ToString() + " FIs Found";
                }
                catch (System.IndexOutOfRangeException e)
                {
                    FITotalBox.Text = e.ToString();
                }

                ShowFIButton.Show();
            }
            else
            {
                FITotalBox.Text = "None Found";
            }
            // SSN
            int SSN1 = Configs.RandomNumber.r.Next(100, 999);
            int SSN2 = Configs.RandomNumber.r.Next(10, 99);
            int SSN3 = Configs.RandomNumber.r.Next(1000, 9999);
            SuspectSSNBox.Text = "SSN: " + SSN1 + " - " + SSN2 + " - " + SSN3;

            // Getting Address
            int q = Configs.RandomNumber.r.Next(1, 3);
            List<string> PerpAddress = new List<string>();
            if (q == 1)
            {
                PerpAddress.Add("Alta Street");
                PerpAddress.Add("Amarillo Way");
                PerpAddress.Add("Banham Canyon Drive");
                PerpAddress.Add("Bay City Avenue");
                PerpAddress.Add("Bridge Street");
                PerpAddress.Add("Capital Boulevard");
                PerpAddress.Add("Clinton Avenue");
                PerpAddress.Add("Dutch London Street");
                PerpAddress.Add("El Rancho Boulevard");
                PerpAddress.Add("Glory Way");
                PerpAddress.Add("Lake Vinewood Drive");
                PerpAddress.Add("Magellan Avenue");
                PerpAddress.Add("Melanoma Street");
                PerpAddress.Add("Normandy Drive");
                PerpAddress.Add("Rub Street");
                PerpAddress.Add("Sinner Street");
                PerpAddress.Add("Spanish Avenue");
                PerpAddress.Add("Swiss Street");
                PerpAddress.Add("Tower Way");
                PerpAddress.Add("Vespucci Boulevard");
                PerpAddress.Add("West Eclipse Boulevard");
                PerpAddress.Add("West Mirror Drive");

                SuspectCityBox.Text = "Los Santos";
            }
            else
            {

                PerpAddress.Add("Algonquin Boulevard");
                PerpAddress.Add("Calafia Road");
                PerpAddress.Add("Cholla Road");
                PerpAddress.Add("Joshua Road");
                PerpAddress.Add("Marina Drive");
                PerpAddress.Add("North Calafia Way");
                PerpAddress.Add("ONeil Way");
                PerpAddress.Add("Panorama Drive");
                PerpAddress.Add("Procopio Drive");
                PerpAddress.Add("Raton Pass");
                PerpAddress.Add("Senora Way");
                PerpAddress.Add("Union Road");

                SuspectCityBox.Text = "Blaine County";
            }
            int PerpStreet = Configs.RandomNumber.r.Next(PerpAddress.Count);

            SuspectAddressBox.Text = PerpNumber.ToString("D4") + " " + (string)PerpAddress[PerpStreet];

            // Getting Occupation
            List<string> PerpOccupation = new List<string>();
            PerpOccupation.Add("Doctor");
            PerpOccupation.Add("Nurse");
            PerpOccupation.Add("Construction Worker");
            PerpOccupation.Add("Ammunation Salesmen");
            PerpOccupation.Add("Police Officer");
            PerpOccupation.Add("Firefighter");
            PerpOccupation.Add("Laid-Off");
            PerpOccupation.Add("Sales Clerk");
            PerpOccupation.Add("Gas Station Attendent");
            PerpOccupation.Add("Secretary");
            PerpOccupation.Add("Teacher");
            PerpOccupation.Add("Professional Tutor");
            PerpOccupation.Add("Professional Dancer");
            PerpOccupation.Add("Scientist");
            PerpOccupation.Add("Coroner");
            int PerpJob = Configs.RandomNumber.r.Next(PerpAddress.Count);
            SuspectOccupationBox.Text = (string)PerpOccupation[PerpJob];

            foreach (Ped ped in World.GetAllPeds())
            {
                if (ped.Exists())
                {
                    Persona pers = Functions.GetPersonaForPed(ped);
                    if (pers.FullName.ToLower() == SuspectFirstBox.Text.ToLower() + " " + SuspectLastBox.Text.ToLower())
                    {
                        PedPersona = pers;
                        BDay = PedPersona.BirthDay;
                        PedLicense = PedPersona.LicenseState;
                        PedWanted = PedPersona.Wanted;
                        SuspectDOBBox.Text = BDay.ToShortDateString();
                        if (PedLicense == ELicenseState.Valid)
                        { LicenseState = "Valid"; }
                        else if (PedLicense == ELicenseState.Suspended)
                        { LicenseState = "Suspended"; }
                        else if (PedLicense == ELicenseState.Expired)
                        { LicenseState = "Expired"; }
                        else
                        { LicenseState = "None"; }
                        SuspectLicenseBox.Text = LicenseState;
                        if (PedWanted == true)
                        { Wanted.Show(); }
                        break;
                    }
                }
            }
        }

        internal void OnShowFIButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            string Exists = "Plugins/LSPDFR/ComputerPlus/field interviews/" + SuspectLastBox.Text.ToLower() + SuspectFirstBox.Text.ToLower() + ".txt";
            if (File.Exists(Exists))
            {
                FIBox.Show();
                FIBackButton.Show();
                string FILines = File.ReadAllText("Plugins/LSPDFR/ComputerPlus/field interviews/" + SuspectLastBox.Text.ToLower() + SuspectFirstBox.Text.ToLower() + ".txt");
                FIBox.Text = FILines;
            }
            else
            {
                Game.DisplayNotification("~r~Please check your spelling, as there are no records found");
            }
        }

        internal void OnFIBackButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            FIBox.Hide();
            FIBackButton.Hide();
        }

        internal enum SubmitCheck
        {
            submitted,
            inprogress
        }
    }
}
