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
    internal class ArrestDataCode : GwenForm
    {        
        public static GameFiber form_dataadd;

        public static ArrestData.Type TypeofData;

        private Label InfoLbl, Data1Lbl, Data2Lbl, Data3Lbl, Data4Lbl, Data5Lbl, Data6Lbl, Data7Lbl, Data8Lbl,
            Value1, Value2, Value3, Value4, Value5, Value6, Value7, Value8,
            DataType, NewDataLbl, DataLbl1,
            Help1, Help2, Help3, Help4, Help5;
        //Help 1-3 show at beginning
        //Help 4 show after first data entry
        //Help 5 is for information

        private static ListBox DataList;

        private MultilineTextBox InfoBox;

        private ComboBox NewDataBox;

        private Button NewDataButton, BackButton, SubmitButton;

        public static List<ArrestData> _arrestData = new List<ArrestData>();

        public ArrestDataCode()
            : base(typeof(ArrestDataForm))
        {  }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                Game.LogTrivial("Initializing Arrest Report; FirstReport = " + EntryPoint._arrestFirstLogin.ToString());
                this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);

                CreateWindow();

                MethodStart();

                HideStuff();
                
                GameFiber.Yield();
            });
        }

        private void CreateWindow()
        {
            Game.LogTrivial("Creating Window");
            Base base_arrest = new Base(this);
            base_arrest.SetPosition(0, 0);
            base_arrest.SetSize(800, 600);

            DataList = new ListBox(base_arrest);
            DataList.SetPosition(12, 31);
            DataList.SetSize(204, 229);
            DataList.SetToolTipText("Arrest report section list");

            DataLbl1 = new Label(base_arrest);
            DataLbl1.SetPosition(9, 13);
            DataLbl1.Text = "Current Data:";

            DataType = new Label(base_arrest);
            DataType.SetPosition(241, 31);

            Data1Lbl = new Label(base_arrest);
            Data1Lbl.SetPosition(241, 64);

            Data2Lbl = new Label(base_arrest);
            Data2Lbl.SetPosition(362, 64);

            Data3Lbl = new Label(base_arrest);
            Data3Lbl.SetPosition(487, 64);

            Data4Lbl = new Label(base_arrest);
            Data4Lbl.SetPosition(615, 64);

            Data5Lbl = new Label(base_arrest);
            Data5Lbl.SetPosition(241, 118);

            Data6Lbl = new Label(base_arrest);
            Data6Lbl.SetPosition(362, 118);

            Data7Lbl = new Label(base_arrest);
            Data7Lbl.SetPosition(487, 118);

            Data8Lbl = new Label(base_arrest);
            Data8Lbl.SetPosition(615, 118);

            Value1 = new Label(base_arrest);
            Value1.SetPosition(241, 79);

            Value2 = new Label(base_arrest);
            Value2.SetPosition(362, 79);

            Value3 = new Label(base_arrest);
            Value3.SetPosition(487, 79);

            Value4 = new Label(base_arrest);
            Value4.SetPosition(615, 79);

            Value5 = new Label(base_arrest);
            Value5.SetPosition(241, 133);

            Value6 = new Label(base_arrest);
            Value6.SetPosition(362, 133);

            Value7 = new Label(base_arrest);
            Value7.SetPosition(487, 133);

            Value8 = new Label(base_arrest);
            Value8.SetPosition(615, 133);

            InfoLbl = new Label(base_arrest);
            InfoLbl.SetPosition(241, 174);

            InfoBox = new MultilineTextBox(base_arrest);
            InfoBox.SetPosition(244, 193);
            InfoBox.SetSize(436, 67);

            NewDataLbl = new Label(base_arrest);
            NewDataLbl.SetPosition(9, 263);
            NewDataLbl.Text = "Add a new data value below:";
            
            NewDataButton = new Button(base_arrest);
            NewDataButton.SetPosition(141, 281);
            NewDataButton.SetSize(75, 23);
            NewDataButton.Text = "Add";
            NewDataButton.SetToolTipText("Add a new section");

            BackButton = new Button(base_arrest);
            BackButton.SetPosition(643, 9);
            BackButton.SetSize(130, 35);
            BackButton.Text = "Back to Previous";

            SubmitButton = new Button(base_arrest);
            SubmitButton.SetPosition(643, 517);
            SubmitButton.SetSize(130, 35);
            SubmitButton.Text = "Submit Form";

            NewDataBox = new ComboBox(base_arrest);
            NewDataBox.SetPosition(12, 281);
            NewDataBox.SetSize(123, 23);
            NewDataBox.AddItem("Report Information");
            NewDataBox.AddItem("Suspect");
            NewDataBox.AddItem("Victim");
            NewDataBox.AddItem("Vehicle");
            NewDataBox.SetToolTipText("Select a data type");

            Help1 = new Label(base_arrest);
            Help1.SetPosition(281, 40);
            Help1.Text = "Welcome to the Arrest Report! \n\n Use these tips to help you write a good report!";
            Help1.Font.Size = 14;
            Help1.TextColor = Color.Green;
            
            Help2 = new Label(base_arrest);
            Help2.SetPosition(241, 89);
            Help2.Text = "\n\n<-- This box will contain the different parts of your arrest report!";
            Help2.MaximumSize = new Point(264, 40);
            Help2.Font.Size = 12;
            Help2.TextColor = Color.Green;
            
            Help3 = new Label(base_arrest);
            Help3.SetPosition(241, 250);
            Help3.Text = "<-- Add a new section of your arrest report by selecting an option \nfrom the dropdown menu and pressing add. \n\n\nThis will add a new section regarding your choice in the dropdown menu. \n\nYou can have multiple of one section \n(e.g. 2 vehicles, 3 suspects, etc). \n\nAll of these sections will be used to build your completed arrest report.";
            Help3.MaximumSize = new Point(380, 260);
            Help3.Font.Size = 12;
            Help3.TextColor = Color.Green;
            
            Help4 = new Label(base_arrest);
            Help4.SetPosition(241, 69);
            Help4.Text = "<-- Select the newly added row in this box to view the information";
            Help4.Font.Size = 12;
            Help4.TextColor = Color.Green;
            
            Help5 = new Label(base_arrest);
            Help5.SetPosition(281, 301);
            Help5.Text = "Here you can see the information you entered.";
            Help5.Font.Size = 12;
            Help5.TextColor = Color.Green;
        }

        private void MethodStart()
        {
            Game.LogTrivial("Starting Methods");
            SubmitButton.Clicked += SubmitButton_Clicked;
            BackButton.Clicked += OnArrestBackButtonClick;
            NewDataButton.Clicked += AddData;
            DataList.RowSelected += DataList_RowSelected;
        }
        
        private void HideStuff()
        {
            InfoLbl.Hide();
            Data1Lbl.Hide();
            Data2Lbl.Hide();
            Data3Lbl.Hide();
            Data4Lbl.Hide();
            Data5Lbl.Hide();
            Data6Lbl.Hide();
            Data7Lbl.Hide();
            Data8Lbl.Hide();
            Value1.Hide();
            Value2.Hide();
            Value3.Hide();
            Value4.Hide();
            Value5.Hide();
            Value6.Hide();
            Value7.Hide();
            Value8.Hide();
            InfoBox.Hide();
            DataType.Hide();

            if (!EntryPoint._arrestFirstLogin)
            {
                Help1.Hide();
                Help2.Hide();
                Help3.Hide();
            }
            Help4.Hide();
            Help5.Hide();
        }

        private void DataList_RowSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            foreach (ArrestData data in _arrestData)
            {
                Game.LogTrivial("Checking arrest data: data.Name=" + data.Name + "; row=" + DataList.SelectedRow.Text);
                if ((data.DataType.ToString() + ", " + data.Name) == DataList.SelectedRow.Text)
                {
                    Game.LogTrivial("Arrest data matched; data.ValueBox4 = " + data.DataList[3]);
                    if (data.DataType == ArrestData.Type.Suspect || data.DataType == ArrestData.Type.Victim)
                        FillInSusVicValues(data);
                    else if (data.DataType == ArrestData.Type.ReportInfo)
                        FillInReportValues(data);
                    else if (data.DataType == ArrestData.Type.Vehicle)
                        FillInVehicleValues(data);

                    ShowStuff();

                    if (!EntryPoint._arrestFirstLogin)
                    {
                        Help5.Hide();
                    }
                }
            }
        }
        
        private void FillInSusVicValues(ArrestData data)
        {
            if (data.DataType == ArrestData.Type.Suspect)
            {
                DataType.Text = "Suspect Data";
                Data6Lbl.Text = "Weapon";
                InfoLbl.Text = "Arrest Information";
            }
            else
            {
                DataType.Text = "Victim Data";
                Data6Lbl.Text = "Linked to Suspect";
                InfoLbl.Text = "Victim Information";
            }
            Data1Lbl.Text = "First Name";
            Data2Lbl.Text = "Last Name";
            Data3Lbl.Text = "DOB";
            Data4Lbl.Text = "SSN";
            Data5Lbl.Text = "Address";
            Data7Lbl.Text = "License Status";
            Data8Lbl.Text = "Gender";

            Value1.Text = data.DataList[0];
            Value2.Text = data.DataList[1];
            Value3.Text = data.DataList[2];
            Value4.Text = data.DataList[3];
            Value5.Text = data.DataList[4];
            Value6.Text = data.DataList[5];
            Value7.Text = data.DataList[6];
            Value8.Text = data.DataList[7];

            InfoBox.Text = data.BoxText;
        }

        private void FillInReportValues(ArrestData data)
        {
            DataType.Text = "Report Information";
            Data1Lbl.Text = "Officer First Name";
            Data2Lbl.Text = "Officer Last Name";
            Data3Lbl.Text = "Officer Number";
            Data4Lbl.Text = "Report Number";
            Data5Lbl.Text = "Street";
            Data6Lbl.Text = "City";
            Data7Lbl.Text = "Date";
            Data8Lbl.Text = "Time";
            InfoLbl.Text = "Officer Narrative";

            Value1.Text = data.DataList[0];
            Value2.Text = data.DataList[1];
            Value3.Text = data.DataList[2];
            Value4.Text = data.DataList[3];
            Value5.Text = data.DataList[4];
            Value6.Text = data.DataList[5];
            Value7.Text = data.DataList[6];
            Value8.Text = data.DataList[7];

            InfoBox.Text = data.BoxText;
        }

        private void FillInVehicleValues(ArrestData data)
        {
            DataType.Text = "Vehicle Information";
            Data1Lbl.Text = "Make";
            Data2Lbl.Text = "Model";
            Data3Lbl.Text = "Color";
            Data4Lbl.Text = "Plate";
            Data5Lbl.Text = "Registration";
            Data6Lbl.Text = "VIN";
            Data7Lbl.Text = "Owner";
            Data8Lbl.Text = "Tags";
            InfoLbl.Text = "Other Vehicle Information";

            Value1.Text = data.DataList[0];
            Value2.Text = data.DataList[1];
            Value3.Text = data.DataList[2];
            Value4.Text = data.DataList[3];
            Value5.Text = data.DataList[4];
            Value6.Text = data.DataList[5];
            Value7.Text = data.DataList[6];
            Value8.Text = data.DataList[7];

            InfoBox.Text = data.BoxText;
        }

        private void ShowStuff()
        {
            InfoLbl.Show();
            Data1Lbl.Show();
            Data2Lbl.Show();
            Data3Lbl.Show();
            Data4Lbl.Show();
            Data5Lbl.Show();
            Data6Lbl.Show();
            Data7Lbl.Show();
            Data8Lbl.Show();
            Value1.Show();
            Value2.Show();
            Value3.Show();
            Value4.Show();
            Value5.Show();
            Value6.Show();
            Value7.Show();
            Value8.Show();
            InfoBox.Show();
            DataType.Show();

            if (EntryPoint._arrestFirstLogin)
            {
                Help4.Hide();
                Help5.Show();
            }
            else
            {
                Help4.Hide();
                Help5.Hide();
                EntryPoint._arrestFirstLogin = false;
            }
        }

        private void WriteData()
        {
            EntryPoint._arrestFirstLogin = false;
            using (StreamWriter Information = new StreamWriter(@"Plugins/LSPDFR/ComputerPlus/arrest reports/" + DateTime.Now.ToString("yyyymmdd_hhmm") + ".txt", true))
            {
                int i = 0;
                foreach (ArrestData data in _arrestData)
                {
                    i++;
                    Game.LogTrivial("Data Type: " + data.DataType.ToString() + " int: " + i);
                    Information.WriteLine("Section type: " + data.DataType.ToString());
                    Information.WriteLine("");
                    if (data.DataType == ArrestData.Type.ReportInfo)
                    {
                        Information.WriteLine("Reporting Officer: " + data.DataList[0] + " " + data.DataList[1]);
                        Information.WriteLine("Officer Number: " + data.DataList[2]);
                        Information.WriteLine("Report Number: " + data.DataList[3]);
                        Information.WriteLine("Location: " + data.DataList[4] + " " + data.DataList[5] + ", San Andreas");
                        Information.WriteLine("On: " + data.DataList[6] + " at " + data.DataList[7]);
                        Information.WriteLine("Officer Narrative: " + data.BoxText);
                    }
                    else if (data.DataType == ArrestData.Type.Suspect)
                    {
                        Information.WriteLine("Full Name: " + data.DataList[0] + " " + data.DataList[1]);
                        Information.WriteLine("DOB: " + data.DataList[2]);
                        Information.WriteLine("SSN: " + data.DataList[3]);
                        Information.WriteLine("Address: " + data.DataList[4]);
                        Information.WriteLine("Weapon: " + data.DataList[5]);
                        Information.WriteLine("License Status: " + data.DataList[6]);
                        Information.WriteLine("Gender: " + data.DataList[7]);
                        Information.WriteLine("Arrest Information: " + data.BoxText);
                    }
                    else if (data.DataType == ArrestData.Type.Victim)
                    {
                        Information.WriteLine("Full Name: " + data.DataList[0] + " " + data.DataList[1]);
                        Information.WriteLine("DOB: " + data.DataList[2]);
                        Information.WriteLine("SSN: " + data.DataList[3]);
                        Information.WriteLine("Address: " + data.DataList[4]);
                        Information.WriteLine("License Status: " + data.DataList[5]);
                        Information.WriteLine("Linked to Suspect: " + data.DataList[6]);
                        Information.WriteLine("Gender: " + data.DataList[7]);
                        Information.WriteLine("Victim Information: " + data.BoxText);
                    }
                    else if (data.DataType == ArrestData.Type.Vehicle)
                    {
                        Information.WriteLine("Make and Model: " + data.DataList[0] + " " + data.DataList[1]);
                        Information.WriteLine("Color: " + data.DataList[2]);
                        Information.WriteLine("Plate: " + data.DataList[3]);
                        Information.WriteLine("Registration Status: " + data.DataList[4]);
                        Information.WriteLine("VIN: " + data.DataList[5]);
                        Information.WriteLine("Owner: " + data.DataList[6]);
                        Information.WriteLine("Tags: " + data.DataList[7]);
                        Information.WriteLine("Other Information: " + data.BoxText);
                    }
                    Information.WriteLine("");
                }
            }
            Game.LogTrivial("Successfully written report for " + DateTime.Now.ToString("yyyymmdd_hhmm") + ".txt");
        }

        private void AddData(Base sender, ClickedEventArgs e)
        {
            if (EntryPoint._arrestFirstLogin)
            {
                Help1.Hide();
                Help2.Hide();
                Help3.Hide();
                Help4.Show();
            }

            Game.LogTrivial("Starting add menu; DataBox = " + NewDataBox.Text);
            if (NewDataBox.Text == "Suspect")
                TypeofData = ArrestData.Type.Suspect;

            if (NewDataBox.Text == "Victim")
                TypeofData = ArrestData.Type.Victim;

            if (NewDataBox.Text == "Vehicle")
                TypeofData = ArrestData.Type.Vehicle;

            if (NewDataBox.Text == "Report Information")
                TypeofData = ArrestData.Type.ReportInfo;
            
            form_dataadd = new GameFiber(OpenDataAdd);
            form_dataadd.Start();
        }
        
        internal static void OpenDataAdd()
        {
            GwenForm ArrestData = new ArrestReportDataCode();
            ArrestData.Show();
            while (ArrestData.Window.IsVisible)
                GameFiber.Yield();
        }

        internal void OnArrestBackButtonClick(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
        }

        private void SubmitButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            WriteData();
            
            this.Window.Close();
        }

        public static void UpdateTable(ArrestData data)
        {
            Game.LogTrivial("Updating table; " + _arrestData.Count.ToString());
            DataList.AddRow(data.DataType.ToString() + ", " + data.Name);
            Game.LogTrivial("Row " + data.DataType.ToString() + ", " + data.Name + " added");
        }
    }
}
