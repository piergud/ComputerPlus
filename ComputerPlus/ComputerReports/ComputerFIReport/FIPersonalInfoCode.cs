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
using ComputerPlus.Interfaces.ComputerReports;

namespace ComputerPlus
{
    internal class FIPersonalInfoCode : GwenForm
    {
        internal DateTime BDay;
        internal Persona PedPersona, PedPersona1;
        internal string FITotal, FirstName, LastName, firstname, lastname, vehplate, LicenseState;
        internal int FINumber;
        internal bool PedWanted;
        internal ELicenseState PedLicense;

        private PedData _data;
        
        internal static GameFiber form_firemarks = new GameFiber(OpenFIRemarksForm);
        
        private Label FIInfo, FIPersonalInfo, CreditLabel, SuspectLast, SuspectFirst, SuspectDOB, SuspectSSN,
            SuspectOccupation, SuspectAddress, SuspectCity, SuspectSex, SuspectRace, SuspectHair, SuspectEyes, 
            SuspectMark, SuspectLicense, SuspectVehicle, SuspectPlate, CurrentStoppedLabel, Wanted, FITotalLbl;

        internal TextBox SuspectDOBBox, SuspectSSNBox, SuspectOccupationBox,
            SuspectAddressBox, SuspectCityBox, SuspectSexBox, SuspectRaceBox, SuspectHairBox, SuspectEyesBox,
            SuspectMarkBox, SuspectLicenseBox, SuspectVehicleBox, SuspectPlateBox, first_box, last_box;
        private MultilineTextBox FIBox;

        private Button FIContinueButton, FILookupButton, ShowFIButton, FIBackButton;
        
        private bool isPullover = false;

        public FIPersonalInfoCode()
            : base(typeof(FIPersonalInfoForm))
        {

        }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();

                Setup();

                MethodStart();

                HideStuff();

                VehCheck();

                GameFiber.Yield();
            });
        }
        
        private void Setup()
        {
            Game.LogTrivial("Initializing FI Personal Info");
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);

            _data = Carryover.Data;

            first_box.Text = _data.FirstName;
            last_box.Text = _data.LastName;

            if (_data.isPed)
            {
                SuspectDOBBox.Text = _data.Sus_Persona.BirthDay.ToShortDateString();
                SuspectAddressBox.Text = GetRandomAddress();
                SuspectOccupationBox.Text = GetRandomOccupation();
                SuspectSSNBox.Text = GetRandomSSN();
                SuspectSexBox.Text = _data.Sus_Persona.Gender.ToString();
                SuspectLicenseBox.Text = _data.Sus_Persona.LicenseState.ToString();
            }
        }

        private void MethodStart()
        {
            FIContinueButton.Clicked += OnFIContinueButtonClick;
            FILookupButton.Clicked += OnFILookupButtonClick;
            ShowFIButton.Clicked += OnShowFIButtonClick;
            FIBackButton.Clicked += OnFIBackButtonClick;
        }

        private void HideStuff()
        {
            ShowFIButton.Hide();
            FIBox.Hide();
            Wanted.Hide();
            VehCheck();
            FIBackButton.Hide();
        }

        private void VehCheck()
        {
            if (Functions.IsPlayerPerformingPullover() == true)
            {
                Vehicle[] vehs = World.GetAllVehicles();
                for (int i = 0; i < vehs.Length; i++)
                {
                    SuspectPlateBox.Text = vehs[i].LicensePlate;
                    SuspectVehicleBox.Text = vehs[i].Model.Name;
                }
            }
        }

        #region Button_Clicks
        private void OnShowFIButtonClick(Base sender, ClickedEventArgs e)
        {
            FileCheck(Type.ShowCheck);
        }

        private void OnFIBackButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            FIBox.Hide();
            FIBackButton.Hide();
        }

        private void OnFILookupButtonClick(Base sender, ClickedEventArgs arguments)
        {
            Game.LogTrivial("Initializing FI Lookup");
            FileCheck(Type.NumberCheck);
        }

        private void OnFIContinueButtonClick(Base sender, ClickedEventArgs e)
        {
            Game.LogTrivial("FI personal info saving for: " + first_box.Text.ToString() + " " + last_box.Text.ToString());

            WriteData();

            CloseForm();
        }
        #endregion

        private void WriteData()
        {
            using (StreamWriter Information = new StreamWriter("Plugins/LSPDFR/ComputerPlus/field interviews/" + last_box.Text.ToLower() + first_box.Text.ToLower() + ".txt", true))
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
        }

        private void CloseForm()
        {
            this.Window.Close();
            form_firemarks = new GameFiber(OpenFIRemarksForm);
            form_firemarks.Start();
        }

        private static void OpenFIRemarksForm()
        {
            GwenForm FIRemarks = new FIRemarksCode();
            FIRemarks.Show();
            while (FIRemarks.Window.IsVisible)
                GameFiber.Yield();
        }

        private void FileCheck(Type type)
        {
            string Exists = "Plugins/LSPDFR/ComputerPlus/field interviews/" + last_box.Text.ToLower() + first_box.Text.ToLower() + ".txt";   
            if (File.Exists(Exists))
            {
                if (type == Type.NumberCheck)
                {
                    GetFINumber();
                }
                else if (type == Type.ShowCheck)
                {
                    ShowFI();
                }
            }
            else
            {
                FITotalLbl.Text = "None Found";
            }
        }

        private void GetFINumber()
        {
            try
            {
                var FINumber = File.ReadAllLines("Plugins/LSPDFR/ComputerPlus/field interviews/" + last_box.Text.ToLower() + first_box.Text.ToLower() + ".txt").Count();
                var FITotalNumber = (double)FINumber / (double)32;
                var FIRounded = Math.Round(FITotalNumber, 0, MidpointRounding.AwayFromZero);
                FITotalLbl.Text = FIRounded.ToString() + " FIs Found";

                if (FINumber > 0)
                    ShowFIButton.Show();
            }
            catch (IndexOutOfRangeException e)
            {
                FITotalLbl.Text = e.ToString();
            }
        }

        private void ShowFI()
        {
            FIBox.Show();
            FIBackButton.Show();
            string FILines = File.ReadAllText("Plugins/LSPDFR/ComputerPlus/field interviews/" + last_box.Text.ToLower() + first_box.Text.ToLower() + ".txt");
            FIBox.Text = FILines;
        }
        
        private string GetRandomSSN()
        {
            int SSN1 = MathHelper.GetRandomInteger(100, 999);
            int SSN2 = MathHelper.GetRandomInteger(10, 99);
            int SSN3 = MathHelper.GetRandomInteger(1000, 9999);
            return (SSN1.ToString() + " " + SSN2.ToString() + " " + SSN3.ToString());
        }

        private string GetRandomAddress()
        {
            int q = MathHelper.GetRandomInteger(1, 3);
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
            int PerpStreet = MathHelper.GetRandomInteger(PerpAddress.Count);
            int PerpNumber = MathHelper.GetRandomInteger(100, 1700);

            return (PerpNumber.ToString("D4") + " " + (string)PerpAddress[PerpStreet]);
        }

        private string GetRandomOccupation()
        {
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

            int PerpJob = MathHelper.GetRandomInteger(PerpOccupation.Count);

            return PerpOccupation[PerpJob];
        }

        internal enum Type { NumberCheck, ShowCheck }
    }
}
