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
    internal class CitationViolationCode : GwenForm
    {
        internal static SubmitCheck state;

        // Housekeeping
        private Label FiskeyLabel;
        private Label MDTLabel2;
        private Label MenuLabel1;
        private Label MenuLabel2;
        private Label MenuLabel3;
        private Label MenuLabel4;
        private Label MenuLabel5;
        private ProgressBar ProgressBar;
        private int gen;
        private Vector3 CurrentLocation;
        internal static Boolean SpeedCheckisChecked;
        internal static string vehplate;

        /// <summary>
        /// Citation Section
        /// </summary>
        // Labels
        private Label InfoLabel;
        private Label Defendent;
        private Label Operated;
        private Label VehInfo;
        private Label VehPlate;
        private Label CitationStreet;
        private Label CitationCity;
        private Label Conditions;
        private Label CommittedOffenses;
        private Label Ina;
        private Label Ina2;
        private Label Area;
        private Label Violations;
        private Label ExtraInfo;
        private Label CourtInfo;
        private Label Fail;
        private Label CurrentStreet;
        // Boxes
        private TextBox VehInfoBox;
        internal TextBox VehPlateBox;
        private TextBox StreetBox;
        private TextBox CityBox;
        private TextBox SpeedBox;
        private TextBox DateBox;
        private TextBox TimeBox;
        private TextBox InABox;
        private TextBox CurrentStreetBox;
        private MultilineTextBox CitationViolationBox;
        private MultilineTextBox CitationExtraInfoBox;
        // Checkboxes
        private CheckBox VehCheck1;
        private CheckBox VehCheck2;
        private CheckBox VehCheck3;
        private CheckBox VehCheck4;
        private CheckBox VehCheck5;
        private CheckBox InCommCheck;
        private CheckBox InConstCheck;
        private CheckBox AccidentCheck;
        internal CheckBox SpeedCheck;
        // Drop-Down Lists
        private ComboBox CitationAreaBox;
        private ComboBox CitationWeatherBox;
        private ComboBox CitationStreetConditionBox;
        private ComboBox CitationLightConditionBox;
        private ComboBox CitationTrafficConditionBox;
        private ComboBox CitationSpeedDeviceBox;
        // Button
        private Button CitationSubmitButton;
        private Button BackButton;

        public CitationViolationCode()
            : base(typeof(CitationViolationForm))
        {

        }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                Game.LogTrivial("Initializing Citation Violations");
                this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
                this.CitationSubmitButton.Clicked += this.OnCitationSubmitClick;
                this.SpeedCheck.Checked += this.SpeedCheckChecked;
                BackButton.Clicked += OnBackButtonClick;
                Ina2.Hide();
                InABox.Hide();
                CitationSpeedDeviceBox.Hide();
                SpeedBox.Hide();
                gen = Configs.RandomNumber.r.Next(1, 61);
                string date = DateTime.Now.AddDays(gen).ToShortDateString();
                DateBox.Text = date;
                DateTime start = DateTime.Today.AddHours(8);
                DateTime value = start.AddMinutes(Configs.RandomNumber.r.Next(481));
                string time = value.ToShortTimeString();
                TimeBox.Text = time;
                CurrentLocation = Game.LocalPlayer.Character.Position;
                string currentstreet = Rage.World.GetStreetName(CurrentLocation);
                StreetBox.Text = currentstreet;
                CurrentStreetBox.Text = currentstreet;
                VehCheck();
                state = SubmitCheck.inprogress;
                GameFiber.Yield();
            });
        }
        private void VehCheck()
        {
            if (Functions.IsPlayerPerformingPullover() == true)
            {
                Vehicle[] vehs = World.GetAllVehicles();
                for (int i = 0; i < vehs.Length; i++)
                {

                    VehPlateBox.Text = vehs[i].LicensePlate;
                }
            }
        }

        private void SpeedCheckChecked(Base sender, EventArgs arguments)
        {
            Ina2.Show();
            InABox.Show();
            CitationSpeedDeviceBox.Show();
            SpeedBox.Show();
            SpeedCheckisChecked = true;
        }

        private void OnCitationSubmitClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Game.LogTrivial("Citation page 2 submission begin...");
            using (StreamWriter Information = new StreamWriter("Plugins/LSPDFR/ComputerPlus/citations/completedcitations.txt", true))
            {
                Information.WriteLine("---VIOLATIONS---");
                Information.WriteLine("The above defendent operated a:");
                Information.WriteLine("Passenger: " + VehCheck1.IsChecked);
                Information.WriteLine("Commercial: " + VehCheck2.IsChecked);
                Information.WriteLine("Cycle: " + VehCheck3.IsChecked);
                Information.WriteLine("Bus: " + VehCheck4.IsChecked);
                Information.WriteLine("Other: " + VehCheck5.IsChecked);
                Information.WriteLine("Vehicle Make, Model, Color, Style: " + VehInfoBox.Text);
                Information.WriteLine("License Plate: " + VehPlateBox.Text);
                Information.WriteLine("Upon the public highway: " + StreetBox.Text + " in the city: " + CityBox.Text);
                Information.WriteLine("In the following conditions:");
                Information.WriteLine("Street Condition: " + CitationStreetConditionBox.Text);
                Information.WriteLine("Light Condition: " + CitationLightConditionBox.Text);
                Information.WriteLine("Traffic Condition: " + CitationTrafficConditionBox.Text);
                Information.WriteLine("And committed the following offenses:");
                Information.WriteLine("Accident: " + AccidentCheck.IsChecked);
                Information.WriteLine("Speed: " + SpeedCheck.IsChecked + " and was traveling " + SpeedBox.Text + " in a speed limit of " + InABox.Text);
                Information.WriteLine("Speed Device Used: " + CitationSpeedDeviceBox.Text);
                Information.WriteLine("In a:");
                Information.WriteLine("Commercial Vehicle: " + InCommCheck.IsChecked);
                Information.WriteLine("Construction Zone: " + InConstCheck.IsChecked);
                Information.WriteLine("Area: " + Area.Text);
                Information.WriteLine("Violations: " + Violations.Text);
                Information.WriteLine("Additional Information: " + ExtraInfo.Text);
                Information.WriteLine("And is summoned to appear in court on: " + DateBox.Text + " at " + TimeBox.Text);
                Information.WriteLine(" ");
                Information.WriteLine("Citation Submitted " + System.DateTime.Now.ToString());
                Information.WriteLine("---END CITATION---");
            }
            Game.LogTrivial("Citation page 2 submission success!");
            Game.DisplayNotification("Citation ~b~successfully~w~ submitted!");
            vehplate = VehPlateBox.Text.ToLower();
            CitationComplete();
            state = SubmitCheck.submitted;
            this.Window.Close();
            ComputerMain.form_report = new GameFiber(ComputerMain.OpenReportMenuForm);
            ComputerMain.form_report.Start();
        }
        internal static bool CitationComplete()
        {
            return true;
        }

        private void OnBackButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            this.Window.Close();
            ComputerMain.form_report = new GameFiber(ComputerMain.OpenReportMenuForm);
            ComputerMain.form_report.Start();
        }
        internal enum SubmitCheck
        {
            submitted,
            inprogress
        }
    }
}
