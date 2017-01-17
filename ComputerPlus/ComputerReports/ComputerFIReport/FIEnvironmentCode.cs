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
    internal class FIEnvironmentCode : GwenForm
    {
        DateTime BDay;
        Persona PedPersona, PedPersona1, PedPersona2, PedPersona3, PedPersona4;
        string PedName1, PedName2, PedName3, PedName4, PedName5;
        public string FirstName, LastName;
        private Vector3 CurrentLocation;

        private bool ComboBox = false;

        private PedData _susData;
        
        public static GameFiber form_fipersonal = new GameFiber(OpenFIPersonalForm);
        
        private Label FIInfo, FIEnvironment, FIDate, FITime, FILocation, FICounty,
            FIOfficerNumber, FIOfficerName, SuspectLast, SuspectFirst;

        private TextBox FIDateBox, FITimeBox, FILocationBox, FICountyBox, FIOfficerNumberBox, FIOfficerNameBox, first_box, last_box;

        private Button FIContinueButton, FIBackButton;
        private ComboBox combo_last, combo_first;

        private bool isPullover = false;

        public FIEnvironmentCode()
            : base(typeof(FIEnvironmentForm))
        {  }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                Game.LogTrivial("Initializing FI Environment");
                this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);

                MethodStart();

                HideStuff();

                FillBoxes();
                
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
            FIContinueButton.Clicked += OnFIContinueButtonClick;
            FIBackButton.Clicked += OnFIBackButtonClick;
            combo_last.ItemSelected += UpdateCombo;
            combo_first.ItemSelected += UpdateCombo;
        }

        private void HideStuff()
        {
            combo_first.Hide();
            combo_last.Hide();
            first_box.Hide();
            last_box.Hide();
        }

        private void FillBoxes()
        {
            FIDateBox.Text = DateTime.Now.ToShortDateString();
            FITimeBox.Text = DateTime.Now.ToShortTimeString();
            FIOfficerNameBox.Text = Configs.Username;
            FIOfficerNumberBox.Text = Configs.UnitNumber;
            CurrentLocation = Game.LocalPlayer.Character.Position;
            string currentstreet = Rage.World.GetStreetName(CurrentLocation);
            FILocationBox.Text = currentstreet;
            FICountyBox.Text = GetCounty();
        }

        private string GetCounty()
        {
            string county = "";

            Vector3 pos = Game.LocalPlayer.Character.Position;

            if (Rage.Native.NativeFunction.CallByName<uint>("GET_HASH_OF_MAP_AREA_AT_COORDS", pos.X, pos.Y, pos.Z) == Game.GetHashKey("city"))
            {
                county = "Los Santos County";
            }
            else
            {
                county = "Blaine County";
            }
            return county;
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
                    _susData = new PedData(pulloverped);
                    combo_first.AddItem(_susData.Sus_Persona.Forename.ToString(), _susData.Sus_Persona.Forename.ToString());
                    combo_last.AddItem(_susData.Sus_Persona.Surname.ToString(), _susData.Sus_Persona.Surname.ToString());
                    combo_first.Show();
                    combo_last.Show();
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
                            PedPersona1 = pers;
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

        #region Button_Methods
        private void UpdateCombo(Base sender, ItemSelectedEventArgs e)
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
                            break;
                        }
                    }
                    else
                    {
                        if (pers.Forename.ToLower() == combo_first.SelectedItem.Text.ToLower())
                        {
                            Game.LogTrivial("Match found -- changing last name");
                            combo_last.Text = pers.Surname;
                            break;
                        }
                    }
                }
            }
        }

        private void OnFIBackButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            this.Window.Close();
        }

        private void OnFIContinueButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            WriteData();

            if (!isPullover)
            {
                SusDataCheck();
            }

            Carryover.CarryOverData(_susData);

            this.Window.Close();
            form_fipersonal = new GameFiber(OpenFIPersonalForm);
            form_fipersonal.Start();
        }
        #endregion

        private void WriteData()
        {
            using (StreamWriter Information = new StreamWriter("Plugins/LSPDFR/ComputerPlus/field interviews/" + combo_last.Text.ToLower() + combo_first.Text.ToLower() + ".txt", true))
            {
                // 8 Lines
                Information.WriteLine(" ");
                Information.WriteLine(" ");
                Information.WriteLine("---ENVIRONMENT---");
                Information.WriteLine("Date: " + FIDateBox.Text);
                Information.WriteLine("Time: " + FITimeBox.Text);
                Information.WriteLine("Location: " + FILocationBox.Text);
                Information.WriteLine("Officer Name and Number: " + FIOfficerNameBox.Text + " " + FIOfficerNumberBox.Text);
                if (!ComboBox)
                {
                    Information.WriteLine("Individual Full Name: " + first_box.Text + " " + last_box.Text);
                }
                else
                {
                    Information.WriteLine("Individual Full Name: " + combo_first.Text + " " + combo_last.Text);
                }
            }
            Game.LogTrivial("Successfully written to .txt");
        }

        private bool susFound = false;

        private void SusDataCheck()
        {
            foreach (Ped ped in World.GetAllPeds())
            {
                if (ped.Exists())
                {
                    Persona pers = Functions.GetPersonaForPed(ped);
                    if (ComboBox)
                    {
                        if (pers.Forename.ToLower() == combo_first.Text.ToLower() && pers.Surname.ToLower() == combo_last.Text.ToLower())
                        {
                            Game.LogTrivial("ComboBox ped matched");
                            _susData = new PedData(ped);
                            susFound = true;
                            break;
                        }
                    }
                    else
                    {
                        if (pers.FullName.ToLower() == first_box.Text.ToLower() + " " + last_box.Text.ToLower())
                        {
                            Game.LogTrivial("Textbox ped matched");
                            _susData = new PedData(ped);
                            susFound = true;
                            break;
                        }
                    }
                }
            }
            if (!susFound)
            {
                Game.LogTrivial("No ped matched");
                _susData = new PedData();
                _susData.FirstName = first_box.Text;
                _susData.LastName = last_box.Text;
            }
        }

        private static void OpenFIPersonalForm()
        {
            GwenForm FIPersonal = new FIPersonalInfoCode();
            FIPersonal.Show();
            while (FIPersonal.Window.IsVisible)
                GameFiber.Yield();
        }
    }
}
