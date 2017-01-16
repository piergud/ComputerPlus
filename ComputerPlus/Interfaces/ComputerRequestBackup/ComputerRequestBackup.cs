using System;
using System.Drawing;
using Rage;
using Rage.Forms;
using Gwen.Control;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response;
using ComputerPlus.Interfaces.ComputerPedDB;

namespace ComputerPlus
{
    internal class ComputerRequestBackup : GwenForm
    {
        private Button btn_main, btn_request;
        private ListBox list_unit;
        private ComboBox dropdown_resp;
        private Label text_resp;

        public ComputerRequestBackup() : base(typeof(ComputerRequestBackupTemplate))
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.btn_request.Clicked += this.RequestButtonClickedHandler;
            this.dropdown_resp.ItemSelected += this.DropdownChangedHandler;
            if (Functions.GetActivePursuit() != null)
                this.dropdown_resp.AddItem("Pursuit", "Pursuit", EBackupResponseType.Pursuit);
            if (ComputerPedController.Instance.PedsCurrentlyStoppedByPlayer.Count > 0)
                this.dropdown_resp.AddItem("Suspect Transport", "Transport", EBackupResponseType.SuspectTransporter);
            this.dropdown_resp.AddItem("Code 2", "Code2", EBackupResponseType.Code2);
            this.dropdown_resp.AddItem("Code 3", "Code3", EBackupResponseType.Code3);
           
            
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
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
            if (e.SelectedItem != null && e.SelectedItem.UserData is EBackupUnitType)
            {
                var changedTo = (EBackupResponseType)e.SelectedItem.UserData;
                switch (changedTo)
                {
                    case EBackupResponseType.Code2: AddCode2Units(); break;
                    case EBackupResponseType.Code3: AddCode3Units(); break;
                    case EBackupResponseType.Pursuit: AddPursuitUnits(); break;
                    case EBackupResponseType.SuspectTransporter: AddTransportUnits(); break;
                }

            }
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
                    (EBackupResponseType)dropdown_resp.SelectedItem.UserData,
                    (EBackupUnitType)list_unit.SelectedRow.UserData);
                    Function.AddBackupRequestToRecents(resp, unit);
                });
                text_resp.Text = String.Format("A {0} has been dispatched to your location.", list_unit.SelectedRow.Text);
            }
        }       

        private void AddPursuitUnits()
        {
            list_unit.Clear();
            
            list_unit.AddRow("Local Patrol Unit", "localPatrol", EBackupUnitType.LocalUnit);
            list_unit.AddRow("State Patrol Unit", "statePatrol", EBackupUnitType.StateUnit);
            list_unit.AddRow("Local SWAT Team", "localSwat", EBackupUnitType.SwatTeam);
            list_unit.AddRow("NOOSE SWAT Team", "nooseSwat", EBackupUnitType.NooseTeam);
            list_unit.AddRow("Local Air Support Unit", "localAir", EBackupUnitType.AirUnit);
            list_unit.AddRow("NOOSE Air Support Unit", "nooseAir", EBackupUnitType.NooseAirUnit);
            list_unit.AddRow("FIB Air Support Unit", "fbiAir", EBackupUnitType.FIBAirUnit);
            list_unit.SelectRow(0);
        }

        private void AddCode3Units()
        {
            list_unit.Clear();
            list_unit.AddRow("Local Patrol Unit", "localPatrol", EBackupUnitType.LocalUnit);
            list_unit.AddRow("State Patrol Unit", "statePatrol", EBackupUnitType.StateUnit);
            list_unit.AddRow("Local SWAT Team", "localSwat", EBackupUnitType.SwatTeam);
            list_unit.AddRow("NOOSE SWAT Team", "nooseSwat", EBackupUnitType.NooseTeam);
            list_unit.AddRow("Local EMS Unit", "localEms", EBackupUnitType.Ambulance);
            list_unit.AddRow("Local Fire Unit", "fire", EBackupUnitType.Firetruck);
            list_unit.SelectRow(0);

        }

        private void AddCode2Units()
        {
            list_unit.Clear();
            list_unit.AddRow("Local Patrol Unit", "localPatrol", EBackupUnitType.LocalUnit);
            list_unit.AddRow("State Patrol Unit", "statePatrol", EBackupUnitType.StateUnit);
            list_unit.AddRow("Local EMS Unit", "localEms", EBackupUnitType.Ambulance);
            list_unit.AddRow("Local Fire Unit", "fire", EBackupUnitType.Firetruck);
            list_unit.SelectRow(0);
        }

        private void AddTransportUnits()
        {
            list_unit.Clear();
            list_unit.AddRow("Local Patrol Unit", "localPatrol", EBackupUnitType.LocalUnit);
            list_unit.AddRow("State Patrol Unit", "statePatrol", EBackupUnitType.StateUnit);
            list_unit.SelectRow(0);

        }
    }
}
