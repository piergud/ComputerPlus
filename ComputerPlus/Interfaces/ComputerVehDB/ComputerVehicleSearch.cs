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
using System.Runtime.ExceptionServices;

namespace ComputerPlus.Interfaces.ComputerVehDB
{
    sealed class ComputerVehicleSearch : GwenForm
    {
        ListBox list_collected_tags;
        ListBox list_manual_results;
        List<Vehicle> AlprDetectedVehicles = new List<Vehicle>();
        TextBox text_manual_name;

        internal ComputerVehicleSearch() : base(typeof(ComputerVehicleSearchTemplate))
        {

        }

        ~ComputerVehicleSearch()
        {
            list_manual_results.RowSelected -= onListItemSelected;
            list_collected_tags.RowSelected -= onListItemSelected;
            text_manual_name.SubmitPressed -= onSearchSubmit;
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            this.Window.DisableResizing();
            Function.LogDebug("Populating ALPR list");
            PopulateAnprList();
            list_collected_tags.AllowMultiSelect = false;
            list_manual_results.AllowMultiSelect = false;
            list_collected_tags.RowSelected += onListItemSelected;
            list_manual_results.RowSelected += onListItemSelected;
            text_manual_name.SubmitPressed += onSearchSubmit;
            Function.LogDebug("Checking currently pulled over");
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
            ComputerVehicleController.ShowVehicleDetails();
            this.Close();
        }

        private void onListItemSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            try
            { 
                if (arguments.SelectedItem.UserData is ComputerPlusEntity)
                {
                    ComputerVehicleController.LastSelected = arguments.SelectedItem.UserData as ComputerPlusEntity;
                    Function.AddVehicleToRecents(ComputerVehicleController.LastSelected.Vehicle);
                    ClearSelections();
                    this.ShowDetailsView();
                }
            }
            catch(Exception e)
            {
                Function.Log(e.ToString());
            }
        }
        

        private void PopulateAnprList()
        {
            try { 
                ComputerVehicleController.ALPR_Detected
                //.GroupBy(x => x.Vehicle)
                //.Select(x => x.Last())
                .Where(x => x.Vehicle)
                .GroupBy(x => x.Vehicle.LicensePlate)
                .Select(x => x.FirstOrDefault())
                .Select(x =>
                {
                    var data = ComputerVehicleController.LookupVehicle(x.Vehicle);
                
                    if (data == null)
                    {
                        Function.Log("ALPR integration issue.. data missing from LookupVehicle");
                        return null;
                    }
                    if (!String.IsNullOrWhiteSpace(x.Message))
                    {
                        //@TODO may have to come back to this
                        //vehiclePersona.Alert = x.Message;
                      //  data.VehiclePersona = vehiclePersona;
                    }
                
                    return data;
                })
                //.Where(x => x != null && x.Validate())
                .ToList()
                .ForEach(x =>
                {                
                    list_collected_tags.AddVehicle(x);
                });
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
            }

        }
       
    }
}
