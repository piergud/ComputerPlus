using System;
using System.Drawing;
using Rage;
using Rage.Forms;
using Gwen.Control;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response;

namespace ComputerPlus
{
    internal class ComputerRequestBackup : GwenForm
    {
        private Button btn_main, btn_request;
        private ListBox list_unit;
        private ComboBox dropdown_resp;
        private Label text_resp;
        internal static GameFiber form_main = new GameFiber(OpenMainMenuForm);

        public ComputerRequestBackup() : base(typeof(ComputerRequestBackupTemplate))
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.btn_main.Clicked += this.MainButtonClickedHandler;
            this.btn_request.Clicked += this.RequestButtonClickedHandler;
            this.dropdown_resp.ItemSelected += this.DropdownChangedHandler;
            this.list_unit.RowSelected += this.ListSelectedHandler;
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            this.btn_request.Disable();
            this.text_resp.Alignment = Gwen.Pos.Center;
            if (Functions.GetActivePursuit() != null)
            {
                AddPursuitUnits();
            }
            else
            {
                AddCode3Units();
            }
        }

        private void DropdownChangedHandler(Base sender, ItemSelectedEventArgs e)
        {
            if (e.SelectedItem.Name.Contains("Pursuit"))
            {
                AddPursuitUnits();
            }
            else if (e.SelectedItem.Name.Contains("Code 3"))
            {
                AddCode3Units();
            }
            else if (e.SelectedItem.Name.Contains("Code 2"))
            {
                AddCode2Units();
            }
            this.btn_request.Disable();
        }

        private void ListSelectedHandler(Base sender, ItemSelectedEventArgs e)
        {
            this.btn_request.Enable();
        }


        private void RequestButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            if (!btn_request.IsDisabled)
            {
                GameFiber.StartNew(delegate
                {
                    string resp = dropdown_resp.SelectedItem.Text.Trim();
                    string unit = list_unit.SelectedRow.Text;
                    Functions.RequestBackup(Game.LocalPlayer.Character.CurrentVehicle.Position,
                    ConvertStringToEBackupResponseType(resp),
                    ConvertStringToEBackupUnitType(unit));
                    Function.AddBackupRequestToRecents(resp, unit);
                });
                text_resp.Text = String.Format("A {0} has been dispatched to your location.", list_unit.SelectedRow.Text);
            }
        }

        private EBackupUnitType ConvertStringToEBackupUnitType(string unit)
        {
            switch (unit)
            {
                case "Local Patrol Unit":
                    return EBackupUnitType.LocalUnit;
                case "State Patrol Unit":
                    return EBackupUnitType.StateUnit;
                case "Local Transport Unit":
                    return EBackupUnitType.PrisonerTransport;
                case "Local EMS Unit":
                    return EBackupUnitType.Ambulance;
                case "Local SWAT Team":
                    return EBackupUnitType.SwatTeam;
                case "NOOSE SWAT Team":
                    return EBackupUnitType.NooseTeam;
                case "Local Air Support Unit":
                    return EBackupUnitType.AirUnit;
                case "NOOSE Air Support Unit":
                    return EBackupUnitType.FIBAirUnit;
                case "Local Fire Unit":
                    return EBackupUnitType.Firetruck;
                case "FIB Air Support Unit":
                    return EBackupUnitType.FIBAirUnit;
                default:
                    return EBackupUnitType.LocalUnit;
            }
        }

        private EBackupResponseType ConvertStringToEBackupResponseType(string resp)
        {
            switch (resp)
            {
                case "Pursuit":
                    return EBackupResponseType.Pursuit;
                case "Code 3":
                    return EBackupResponseType.Code3;
                case "Code 2":
                    return EBackupResponseType.Code2;
                default:
                    return EBackupResponseType.Code3;
            }
        }

        private void MainButtonClickedHandler(Base sender, ClickedEventArgs e) 
        {
            this.Window.Close();
            form_main = new GameFiber(OpenMainMenuForm);
            form_main.Start();
        }

        internal static void OpenMainMenuForm()
        {
            GwenForm main = new ComputerMain();
            main.Show();
            while (main.Window.IsVisible)
                GameFiber.Yield();
        }

        private void AddPursuitUnits()
        {
            list_unit.Clear();
            list_unit.AddRow("Local Patrol Unit");
            list_unit.AddRow("State Patrol Unit");
            list_unit.AddRow("Local SWAT Team");
            list_unit.AddRow("NOOSE SWAT Team");
            list_unit.AddRow("Local Air Support Unit");
            list_unit.AddRow("NOOSE Air Support Unit");
            list_unit.AddRow("FIB Air Support Unit");
        }

        private void AddCode3Units()
        {
            list_unit.Clear();
            list_unit.AddRow("Local Patrol Unit");
            list_unit.AddRow("State Patrol Unit");
            list_unit.AddRow("Local SWAT Team");
            list_unit.AddRow("NOOSE SWAT Team");
            list_unit.AddRow("Local EMS Unit");
            list_unit.AddRow("Local Fire Unit");
        }

        private void AddCode2Units()
        {
            list_unit.Clear();
            list_unit.AddRow("Local Patrol Unit");
            list_unit.AddRow("State Patrol Unit");
            list_unit.AddRow("Local Transport Unit");
        }
    }
}
