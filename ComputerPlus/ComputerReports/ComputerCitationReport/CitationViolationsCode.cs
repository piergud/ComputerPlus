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
    internal class CitationViolationCode : GwenForm
    {
        // Housekeeping
        private Label FiskeyLabel, MDTLabel2, MenuLabel1, MenuLabel2, MenuLabel3, MenuLabel4, MenuLabel5;
        private ProgressBar ProgressBar;
        private int gen;
        private Vector3 CurrentLocation;
        internal static bool SpeedCheckisChecked;

        /// <summary>
        /// Citation Section
        /// </summary>
        // Labels
        private Label InfoLabel, Defendent, Operated, VehInfo, VehPlate, CitationStreet, CitationCity, 
            Conditions, CommittedOffenses, Ina, Ina2, speed_type_lbl, Area, Violations, ExtraInfo, CourtInfo, Fail, CurrentStreet;
        // Boxes
        private TextBox VehInfoBox, offense_other_box, veh_other_box, VehPlateBox, StreetBox, CityBox, _areaOtherBox, 
            SpeedBox, InABox;
        private MultilineTextBox CitationViolationBox, CitationExtraInfoBox, summons_box;
        // Checkboxes
        private CheckBox VehCheck1, VehCheck2, VehCheck3, VehCheck4, VehCheck5, AccidentCheck, SpeedCheck, offense_other_check;

        // Drop-Down Lists
        private ComboBox CitationAreaBox, CitationWeatherBox, CitationStreetConditionBox, CitationLightConditionBox, CitationTrafficConditionBox, CitationSpeedDeviceBox;

        private bool _BoxCheck1, _BoxCheck2, _BoxCheck3, _BoxCheck4, _BoxCheck5, _BoxCheck6, _BoxCheck7;

        // Button
        private Button CitationSubmitButton;

        public CitationViolationCode()
            : base(typeof(CitationViolationForm))
        {  }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();

                MethodStart();

                HideThings();

                FillinBoxes();

                VehCheck();

                AddToolTips();

                GameFiber.Yield();
            });
        }

        private void MethodStart()
        {
            Game.LogTrivial("Initializing Citation Violations");
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);

            this.CitationSubmitButton.Clicked += OnCitationSubmitClick;
            this.SpeedCheck.Checked += SpeedCheckChecked;
            this.SpeedCheck.UnChecked += SpeedCheckUnChecked;
            this.VehCheck5.Checked += VehCheck5_Checked;
            this.VehCheck5.UnChecked += VehCheck5_UnChecked;
            this.offense_other_check.Checked += Offense_other_check_Checked;
            this.offense_other_check.UnChecked += Offense_other_check_UnChecked;
            this.CitationWeatherBox.Clicked += ComboBoxClicked;
            this.CitationStreetConditionBox.Clicked += ComboBoxClicked;
            this.CitationLightConditionBox.Clicked += ComboBoxClicked;
            this.CitationTrafficConditionBox.Clicked += ComboBoxClicked;
            this.CitationAreaBox.ItemSelected += AreaBoxSelected;
            this.AccidentCheck.Checked += AccidentCheck_Checked;
            this.AccidentCheck.UnChecked += AccidentCheck_UnChecked;
        }
        
        private void FillinBoxes()
        {
            ProgressBar.Value = 0.45f;

            SummonsStuff();

            ExtensionMethods.IncreaseProgressBar(ProgressBar, 0.15f);

            LocationStuff();
        }

        private void SummonsStuff()
        {
            gen = MathHelper.GetRandomInteger(1, 61);
            string date = DateTime.Now.AddDays(gen).ToShortDateString();
            DateTime start = DateTime.Today.AddHours(8);
            DateTime value = start.AddMinutes(MathHelper.GetRandomInteger(481));
            string time = value.ToShortTimeString();
            summons_box.Text = date + "\n" + time;
        }

        private void LocationStuff()
        {
            Vector3 pos = Game.LocalPlayer.Character.Position;
            string currentstreet = Rage.World.GetStreetName(pos);
            StreetBox.Text = currentstreet;

            if (Rage.Native.NativeFunction.CallByName<uint>("GET_HASH_OF_MAP_AREA_AT_COORDS", pos.X, pos.Y, pos.Z) == Game.GetHashKey("city"))
            {
                CityBox.Text = "Los Santos";
            }
            else
            {
                CityBox.Text = "Blaine County";
            }
        }

        private void HideThings()
        {
            Ina2.Hide();
            InABox.Hide();
            speed_type_lbl.Hide();
            CitationSpeedDeviceBox.Hide();
            veh_other_box.Hide();
            offense_other_box.Hide();
            SpeedBox.Hide();
            _areaOtherBox.Hide();
        }

        private void AddToolTips()
        {
            VehCheck1.SetToolTipText("Suspect operated passenger car");
            VehCheck2.SetToolTipText("Suspect operated commercial vehicle");
            VehCheck3.SetToolTipText("Suspect operated motor/bicycle");
            VehCheck4.SetToolTipText("Suspect operated service vehicle");
            VehCheck5.SetToolTipText("Suspect operated other type of vehicle");

            CitationAreaBox.SetToolTipText("Type of area");

            SpeedBox.SetToolTipText("Suspect speed");
            InABox.SetToolTipText("Speed limit of area");
            CitationSpeedDeviceBox.SetToolTipText("Device used to measure speed");
        }

        #region Checkbox_Methods
        private void AccidentCheck_Checked(Base sender, EventArgs arguments)
        {
            ExtensionMethods.IncreaseProgressBar(ProgressBar);
        }
        private void AccidentCheck_UnChecked(Base sender, EventArgs arguments)
        {
            ExtensionMethods.DecreaseProgressBar(ProgressBar);
        }

        private void ComboBoxClicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender == CitationWeatherBox && !_BoxCheck1)
            {
                _BoxCheck1 = true;
                ExtensionMethods.IncreaseProgressBar(ProgressBar);
            }
            else if (sender == CitationStreetConditionBox && !_BoxCheck2)
            {
                _BoxCheck2 = true;
                ExtensionMethods.IncreaseProgressBar(ProgressBar);
            }
            else if (sender == CitationLightConditionBox && !_BoxCheck3)
            {
                _BoxCheck3 = true;
                ExtensionMethods.IncreaseProgressBar(ProgressBar);
            }
            else if (sender == CitationTrafficConditionBox && !_BoxCheck4)
            {
                _BoxCheck4 = true;
                ExtensionMethods.IncreaseProgressBar(ProgressBar);
            }
            else if (sender == CitationWeatherBox && !_BoxCheck5)
            {
                _BoxCheck5 = true;
                ExtensionMethods.IncreaseProgressBar(ProgressBar);
            }
            else if (sender == CitationWeatherBox && !_BoxCheck6)
            {
                _BoxCheck6 = true;
                ExtensionMethods.IncreaseProgressBar(ProgressBar);
            }
        }

        private void AreaBoxSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            Game.LogTrivial("Area box selected; text = " + CitationAreaBox.Text);
            if (!_BoxCheck7)
            {
                _BoxCheck7 = true;
                ExtensionMethods.IncreaseProgressBar(ProgressBar);
            }

            if (CitationAreaBox.Text.ToLower() == "other")
            {
                Game.LogTrivial("Text is equal -- showing");
                _areaOtherBox.Show();
            }
            else
            {
                Game.LogTrivial("Text is not equal -- hiding");
                _areaOtherBox.Hide();
            }
        }

        private void Offense_other_check_UnChecked(Base sender, EventArgs arguments)
        {
            offense_other_box.Hide();
            ExtensionMethods.DecreaseProgressBar(ProgressBar);
        }

        private void Offense_other_check_Checked(Base sender, EventArgs arguments)
        {
            offense_other_box.Show();
            ExtensionMethods.IncreaseProgressBar(ProgressBar, 0.10f);
        }

        private void VehCheck5_UnChecked(Base sender, EventArgs arguments)
        {
            veh_other_box.Hide();
            ExtensionMethods.DecreaseProgressBar(ProgressBar);
        }

        private void VehCheck5_Checked(Base sender, EventArgs arguments)
        {
            veh_other_box.Show();
            ExtensionMethods.IncreaseProgressBar(ProgressBar);
        }

        private void SpeedCheckUnChecked(Base sender, EventArgs arguments)
        {
            ExtensionMethods.DecreaseProgressBar(ProgressBar);
            Ina2.Hide();
            InABox.Hide();
            CitationSpeedDeviceBox.Hide();
            SpeedBox.Hide();
            speed_type_lbl.Hide();
            SpeedCheckisChecked = false;
        }

        private void SpeedCheckChecked(Base sender, EventArgs arguments)
        {
            ExtensionMethods.IncreaseProgressBar(ProgressBar);
            Ina2.Show();
            InABox.Show();
            CitationSpeedDeviceBox.Show();
            SpeedBox.Show();
            speed_type_lbl.Show();
            SpeedCheckisChecked = true;
        }
        #endregion

        private void VehCheck()
        {
            if (Functions.IsPlayerPerformingPullover() == true)
            {
                Vehicle[] vehs = World.GetAllVehicles();
                for (int i = 0; i < vehs.Length; i++)
                {
                    VehPlateBox.Text = vehs[i].LicensePlate;
                    VehInfoBox.Text = vehs[i].Model.Name.ToString();
                }
                ExtensionMethods.IncreaseProgressBar(ProgressBar);
            }
        }

        private void OnCitationSubmitClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            ProgressBar.Value = 1f;

            WriteInformation();

            CloseForm();
        }

        private void WriteInformation()
        {
            Game.LogTrivial("Citation page 2 submission begin...");
            using (StreamWriter Information = new StreamWriter("Plugins/LSPDFR/ComputerPlus/citations/completedcitations.txt", true))
            {
                Information.WriteLine("---VIOLATIONS---");
                Information.WriteLine("The above defendent operated a:");
                if (VehCheck1.IsChecked)
                    Information.WriteLine("Passenger Vehicle");
                if (VehCheck2.IsChecked)
                    Information.WriteLine("Commercial Vehicle");
                if (VehCheck3.IsChecked)
                    Information.WriteLine("Cycle");
                if (VehCheck4.IsChecked)
                    Information.WriteLine("Service Vehicle");
                if (VehCheck5.IsChecked)
                    Information.WriteLine(veh_other_box.Text);
                Information.WriteLine("Vehicle Make, Model, Color, Style: " + VehInfoBox.Text);
                Information.WriteLine("License Plate: " + VehPlateBox.Text);
                Information.WriteLine("Upon the public highway: " + StreetBox.Text + " in the city: " + CityBox.Text);
                Information.WriteLine("In a: " + Area.Text);
                Information.WriteLine("In the following conditions:");
                Information.WriteLine("Weather: " + CitationWeatherBox.Text);
                Information.WriteLine("Street: " + CitationStreetConditionBox.Text);
                Information.WriteLine("Light: " + CitationLightConditionBox.Text);
                Information.WriteLine("Traffic: " + CitationTrafficConditionBox.Text);
                Information.WriteLine("And committed the following offenses:");
                if (AccidentCheck.IsChecked)
                    Information.WriteLine("Accident: " + AccidentCheck.IsChecked);
                if (SpeedCheck.IsChecked)
                    Information.WriteLine("Speed: " + SpeedCheck.IsChecked + " and was traveling " + SpeedBox.Text + " in a speed limit of " + InABox.Text);
                if (offense_other_check.IsChecked)
                    Information.WriteLine("Other: " + offense_other_box.Text);
                Information.WriteLine("Violations: " + CitationViolationBox.Text);
                Information.WriteLine("Additional Information: " + CitationExtraInfoBox.Text);
                Information.WriteLine("And is summoned to appear in court on: " + summons_box.Text);
                Information.WriteLine(" ");
                Information.WriteLine("Citation Submitted " + System.DateTime.Now.ToString());
                Information.WriteLine("---END CITATION---");
            }
            Game.LogTrivial("Citation page 2 submission success!");
        }

        private void CloseForm()
        {
            this.Window.Close();
        }
        
        private void OnBackButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            this.Window.Close();
        }
    }
}
