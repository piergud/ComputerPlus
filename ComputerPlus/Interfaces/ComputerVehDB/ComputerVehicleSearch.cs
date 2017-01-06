using System;
using System.Collections.Generic;
using System.Linq;
using Rage.Forms;
using Gwen;
using Gwen.Control;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;
using ComputerPlus.Controllers.Models;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus.Interfaces.ComputerVehDB
{
    sealed class ComputerVehicleSearch : GwenForm
    {
        ListBox list_collected_tags;
        ListBox list_manual_results;
        List<Vehicle> AlprDetectedVehicles = new List<Vehicle>();
        TextBox text_manual_name;
        bool IsFloatingWindow = false;

        internal ComputerVehicleSearch() : base(typeof(ComputerVehicleSearchTemplate))
        {

        }

        ~ComputerVehicleSearch()
        {
            ComputerVehicleController.OnAlprVanillaMessage -= OnAlprVanillaMessage;
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            this.Window.DisableResizing();
            ComputerVehicleController.OnAlprVanillaMessage += OnAlprVanillaMessage;
            PopulateAnprList();
            list_collected_tags.AllowMultiSelect = false;
            list_manual_results.AllowMultiSelect = false;
            list_collected_tags.RowSelected += onListItemSelected;
            list_manual_results.RowSelected += onListItemSelected;
            text_manual_name.SubmitPressed += onSearchSubmit;
            var currentPullover = ComputerVehicleController.CurrentlyPulledOver;
            if (currentPullover != null) list_collected_tags.AddVehicle(currentPullover);
        }

        private void ClearSelections()
        {
            list_collected_tags.UnselectAll();
            list_manual_results.UnselectAll();
        }

        private void onSearchSubmit(Base sender, EventArgs arguments)
        {
            String tag = text_manual_name.Text.ToUpper();
            if (String.IsNullOrWhiteSpace(tag)) return;
            var vehicle = ComputerVehicleController.LookupVehicle(tag);
            
            if (vehicle != null)
            {
                text_manual_name.TextColorOverride = System.Drawing.Color.Green;
                text_manual_name.UpdateColors();
                text_manual_name.SetToolTipText(String.Empty);
                list_manual_results.AddVehicle(vehicle);
                ComputerVehicleController.LastSelected = vehicle;                
                this.ShowDetailsView();
            }
            else
            {                
                text_manual_name.TextColorOverride = System.Drawing.Color.Red;
                text_manual_name.UpdateColors();
                text_manual_name.SetToolTipText("No vehicles found");
            }
        }

        private void ShowDetailsView()
        {
            if (ComputerVehicleController.LastSelected == null) return;
            var fiber = ComputerVehicleController.VehicleDetailsGameFiber;
            if (fiber.IsHibernating) fiber.Wake();
            else if (!fiber.IsAlive) fiber.Start();
        }

        private void onListItemSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            if (arguments.SelectedItem.UserData is ComputerPlusEntity)
            {
                Game.LogVerboseDebug(String.Format(" arguments.SelectedItem.UserData ? null {0}", arguments.SelectedItem.UserData == null));
                ComputerVehicleController.LastSelected = arguments.SelectedItem.UserData as ComputerPlusEntity;
                ClearSelections();
                Game.LogVerboseDebug(String.Format("ComputerVehicleController.LastSelected ? null {0}", ComputerVehicleController.LastSelected == null));
                this.ShowDetailsView();
                Game.LogVerboseDebug("ComputerVehicleSearch.onListItemSelected successful");
            }
            else
            {
                Game.LogVerboseDebug("ComputerVehicleSearch.onListItemSelected arguments were not valid");
            }
        }
        

        private void PopulateAnprList()
        {
            var list = ComputerVehicleController.ALPR_Detected
                .ToList();
            Game.LogVerboseDebug(String.Format("PopulateAnprList size {0}", list.Count));
            list
            .GroupBy(x => x.Vehicle)
            .Select(x => x.Last())
            .Where(x => x.Vehicle != null && x.Vehicle.Exists())
            .Select(x =>
            {
                Game.LogVerboseDebug("PopulateAnprList Tuple");
                var data = ComputerVehicleController.LookupVehicle(x.Vehicle);
                
                if (data == null)
                {
                    Game.LogVerboseDebug("PopulateAnprList Tuple data was null");
                    return null;
                }
                VehiclePersona vehiclePersona = data.VehiclePersona;
                if (!String.IsNullOrWhiteSpace(x.Message))
                {
                    vehiclePersona.Alert = x.Message;
                    data.VehiclePersona = vehiclePersona;
                }
                
                return data;
            })
            .Where(x => x.Validate())
            .ToList()
            .ForEach(x =>
            {
                list_collected_tags.AddVehicle(x);
            });
            
        }

        private void OnAlprVanillaMessage(object sender, ALPR_Arguments e)
        {
            if (e.Vehicle == null || !e.Vehicle.Exists())
            {
                Game.LogVerboseDebug("e.Item1 OnAlprVanillaMessage null or invalid");
                return;
            }
            var data = ComputerVehicleController.LookupVehicle(e.Vehicle);
            
            if (data == null)
            {
                Game.LogVerboseDebug("data OnAlprVanillaMessage null or invalid");
                return;
            }

            if (!data.IsPersistent)
            {
                Game.LogVerboseDebug(String.Format("OnAlprVanillaMessage Adding persistance flag for {0}", data.PedPersona.FullName));
                data.IsPersistent = true;
            }
            
            var vehiclePersona = data.VehiclePersona;
            vehiclePersona.Alert = e.Message;
            data.VehiclePersona = vehiclePersona;
            if (list_collected_tags.RowCount >= 6)
            {
                var entry = list_collected_tags[0];
                var first = entry.UserData as ComputerPlusEntity;
                if (first != null && first.Validate() && first.IsPersistent) {
                    first.IsPersistent = false;
                    Game.LogVerboseDebug(String.Format("OnAlprVanillaMessage Removed persistance flag for {0}", first.PedPersona.FullName));
                }
                while(list_collected_tags.RowCount >= 6)
                    list_collected_tags.RemoveRow(0);
            }         
            list_collected_tags.AddVehicle(data);
        }
    }
}
