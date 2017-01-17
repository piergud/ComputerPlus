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
    internal class ArrestReportDataCode : GwenForm
    {
        private Label DataTypeLbl, InfoLbl, DataLbl1, DataLbl2, DataLbl3, DataLbl4, DataLbl5, DataLbl6, DataLbl7, DataLbl8;

        private TextBox ValueBox1, ValueBox2, ValueBox3, ValueBox4, ValueBox5, ValueBox6, ValueBox7, ValueBox8;

        private MultilineTextBox InfoBox;

        private Button FinishButton;

        public static ArrestData _data;
        
        public ArrestReportDataCode()
            : base(typeof(ArrestReportDataForm))
        {  }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();

                _data = new ArrestData();
                _data.DataType = ArrestDataCode.TypeofData;
                Game.LogTrivial("Initializing Arrest Data Creator");
                this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
                
                MethodStart();

                ShowLabels();

                GameFiber.Yield();
            });
        }

        private void MethodStart()
        {
            FinishButton.Clicked += OnFinishButtonClicked;
            if (_data.DataType == ArrestData.Type.Suspect || _data.DataType == ArrestData.Type.Victim)
            {
                ValueBox1.TextChanged += AutoFillPed;
                ValueBox2.TextChanged += AutoFillPed;
            }
            else if (_data.DataType == ArrestData.Type.ReportInfo)
            {
                AutoFillReport();
            }
            else if (_data.DataType == ArrestData.Type.Vehicle)
            {
                AutoFillVehicle();
            }
        }

        private void AutoFillPed(Base sender, EventArgs args)
        {
            foreach (Ped ped in World.GetAllPeds())
            {
                if (ped)
                {
                    Persona pers = Functions.GetPersonaForPed(ped);
                    if (ValueBox1.Text.ToLower() == pers.Forename.ToLower())
                    {
                        Game.LogTrivialDebug("First names match");
                        ValueBox2.Text = pers.Surname;
                        FillInPedData(pers);
                    }
                    else if (ValueBox2.Text.ToLower() == pers.Surname.ToLower())
                    {
                        Game.LogTrivialDebug("Last names match");
                        ValueBox1.Text = pers.Forename;
                        FillInPedData(pers);
                    }
                }
            }
        }

        private void FillInPedData(Persona persona)
        {
            ValueBox3.Text = persona.BirthDay.ToShortDateString();
            ValueBox4.Text = GetRandomSSN();
            ValueBox5.Text = GetRandomAddress();
            ValueBox7.Text = persona.LicenseState.ToString();
            ValueBox8.Text = persona.Gender.ToString();
        }

        private void AutoFillReport()
        {
            if (Configs.Username.Contains(" "))
            {
                string[] oName = Configs.Username.Split(' ');
                ValueBox1.Text = oName[0];
                ValueBox2.Text = oName[1];
            }
            ValueBox3.Text = Configs.UnitNumber;
            ValueBox4.Text = MathHelper.GetRandomInteger(200, 3000).ToString();

            Vector3 loc = Game.LocalPlayer.Character.Position;
            ValueBox5.Text = Rage.World.GetStreetName(loc);
            ValueBox7.Text = World.DateTime.Date.ToShortDateString();
            ValueBox8.Text = World.DateTime.TimeOfDay.ToString();
        }

        private void AutoFillVehicle()
        {
            if (Functions.IsPlayerPerformingPullover() == true)
            {
                Vehicle[] vehs = World.GetAllVehicles();
                for (int i = 0; i < vehs.Length; i++)
                {
                    ValueBox2.Text = vehs[i].Model.Name.ToString();
                    ValueBox3.Text = vehs[i].PrimaryColor.ToString();
                    ValueBox4.Text = vehs[i].LicensePlate;
                    ValueBox5.Text = TrafficPolicerFunction.GetVehicleRegistrationStatus(vehs[i]).ToString();
                    ValueBox6.Text = MathHelper.GetRandomInteger(300000, 950000).ToString();
                    ValueBox7.Text = Functions.GetVehicleOwnerName(vehs[i]);
                    ValueBox8.Text = "Coming soon";
                }
            }
        }

        private void ShowLabels()
        {
            if (_data.DataType == ArrestData.Type.Suspect || _data.DataType == ArrestData.Type.Victim)
            {
                if (_data.DataType == ArrestData.Type.Suspect)
                {
                    DataTypeLbl.Text = "Suspect Data";
                    DataLbl6.Text = "Weapon";
                    InfoLbl.Text = "Arrest Information";
                }
                else
                {
                    DataTypeLbl.Text = "Victim Data";
                    DataLbl6.Text = "Linked to Suspect";
                    InfoLbl.Text = "Victim Information";
                }
                DataLbl1.Text = "First Name";
                DataLbl2.Text = "Last Name";
                DataLbl3.Text = "DOB";
                DataLbl4.Text = "SSN";
                DataLbl5.Text = "Address";
                DataLbl7.Text = "License Status";
                DataLbl8.Text = "Gender";
            }
            else if (_data.DataType == ArrestData.Type.ReportInfo)
            {
                DataTypeLbl.Text = "Report Information";
                DataLbl1.Text = "Officer First Name";
                DataLbl2.Text = "Officer Last Name";
                DataLbl3.Text = "Officer Number";
                DataLbl4.Text = "Report Number";
                DataLbl5.Text = "Street";
                DataLbl6.Text = "City";
                DataLbl7.Text = "Date";
                DataLbl8.Text = "Time";
                InfoLbl.Text = "Officer Narrative";
            }
            else if (_data.DataType == ArrestData.Type.Vehicle)
            {
                DataTypeLbl.Text = "Vehicle Information";
                DataLbl1.Text = "Make";
                DataLbl2.Text = "Model";
                DataLbl3.Text = "Color";
                DataLbl4.Text = "Plate";
                DataLbl5.Text = "Registration";
                DataLbl6.Text = "VIN";
                DataLbl7.Text = "Owner";
                DataLbl8.Text = "Tags";
                InfoLbl.Text = "Other Vehicle Information";
            }
        }

        internal void OnFinishButtonClicked(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            TransferData();

            this.Window.Close();
        }

        private void TransferData()
        {
            Game.LogTrivial("Transferring Data...");

            AddData();

            ArrestDataCode._arrestData.Add(_data);

            ArrestDataCode.UpdateTable(_data);

            Game.LogTrivial("Data transfer completed");
        }

        private void AddData()
        {
            _data.DataList.Add(ValueBox1.Text);
            _data.DataList.Add(ValueBox2.Text);
            _data.DataList.Add(ValueBox3.Text);
            _data.DataList.Add(ValueBox4.Text);
            _data.DataList.Add(ValueBox5.Text);
            _data.DataList.Add(ValueBox6.Text);
            _data.DataList.Add(ValueBox7.Text);
            _data.DataList.Add(ValueBox8.Text);

            _data.BoxText = InfoBox.Text;

            _data.Name = ValueBox1.Text + ", " + ValueBox2.Text;
            Game.LogTrivial("Name: " + _data.Name);
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
            }
            int PerpStreet = MathHelper.GetRandomInteger(PerpAddress.Count);
            int PerpNumber = MathHelper.GetRandomInteger(100, 1700);

            return (PerpNumber.ToString("D4") + " " + (string)PerpAddress[PerpStreet]);
        }
    }
}
