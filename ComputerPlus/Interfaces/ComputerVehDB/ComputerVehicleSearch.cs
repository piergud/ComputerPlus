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
            Function.Log("Populating ALPR list");
            PopulateAnprList();
            list_collected_tags.AllowMultiSelect = false;
            list_manual_results.AllowMultiSelect = false;
            list_collected_tags.RowSelected += onListItemSelected;
            list_manual_results.RowSelected += onListItemSelected;
            text_manual_name.SubmitPressed += onSearchSubmit;
            Function.Log("Checking currently pulled over");
            var currentPullover = ComputerVehicleController.CurrentlyPulledOver;
            Function.Log(String.Format("currently pulled over null is {0}", currentPullover == null));
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
            
            
            if (vehicle != null && vehicle.Validate())
            {
                text_manual_name.ClearError();
                list_manual_results.AddVehicle(vehicle);
                ComputerVehicleController.LastSelected = vehicle;                
                this.ShowDetailsView();
            }
            else if(vehicle != null)
            {
                text_manual_name.Error("The vehicle no longer exists");
            }
            else
            {                
                text_manual_name.Error("No vehicles found");
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
                ComputerVehicleController.LastSelected = arguments.SelectedItem.UserData as ComputerPlusEntity;
                ClearSelections();
                this.ShowDetailsView();
            }
        }
        

        private void PopulateAnprList()
        {
            var list = ComputerVehicleController.ALPR_Detected
                .ToList();
            list
            .GroupBy(x => x.Vehicle)
            .Select(x => x.Last())
            .Where(x => x.Vehicle != null && x.Vehicle.Exists())
            .Select(x =>
            {
                var data = ComputerVehicleController.LookupVehicle(x.Vehicle);
                
                if (data == null)
                {
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
            .Where(x => x != null && x.Validate())
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
                return;
            }
            var data = ComputerVehicleController.LookupVehicle(e.Vehicle);
            
            if (data == null)
            {
                return;
            }

            if (!data.IsPersistent)
            {
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
                }
                while(list_collected_tags.RowCount >= 6)
                    list_collected_tags.RemoveRow(0);
            }         
            list_collected_tags.AddVehicle(data);
        }
    }
}
