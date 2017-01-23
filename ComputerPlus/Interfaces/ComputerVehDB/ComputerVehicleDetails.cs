using System;
using System.Collections.Generic;
using System.Linq;
using Rage.Forms;
using Gwen;
using Gwen.Control;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine.Scripting.Entities;
using ComputerPlus.Interfaces.ComputerPedDB;
using ComputerPlus.Controllers.Models;
using System.IO;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Extensions.Rage;

namespace ComputerPlus.Interfaces.ComputerVehDB
{
    class ComputerVehicleDetails : GwenForm
    {
        TextBox text_first_name, text_last_name,
                text_home_address, text_dob, text_license_status,
                text_wanted_status, text_times_stopped, text_age,
                text_vehicle_model, text_vehicle_license,
                text_vehicle_insurance_status, text_vehicle_registration_status;
        Label lbl_alert, lbl_vehicle_alert_label, lbl_vehicle_alert, lbl_insurance_status, lbl_registration_status;
        Button btn_ped_image_holder, btn_vehicle_image_holder;

        ComboBox cb_action;
        
        private ComputerPlusEntity CPEntity;
        private Ped Owner
        {
            get
            {
                return CPEntity.Ped;
            }
        }
        private Vehicle Vehicle
        {
            get
            {
                return CPEntity.Vehicle;
            }
        }
        private Persona OwnerPersona
        {
            get
            {
                return CPEntity.PedPersona;
            }
        }
        private VehiclePersona VehiclePersona
        {
            get
            {
                return CPEntity.VehiclePersona;
            }
        }

        internal ComputerVehicleDetails(Vehicle vehicle, Ped owner, Persona ownerPersona, VehiclePersona vehiclePersona) : base(typeof(ComputerVehicleDetailsTemplate))
        {
            CPEntity = new ComputerPlusEntity(owner, ownerPersona, vehicle, vehiclePersona);
        }
        internal ComputerVehicleDetails(ComputerPlusEntity entity) : base(typeof(ComputerVehicleDetailsTemplate))
        {
            CPEntity = entity;
        }
        public override void InitializeLayout()
        {
            base.InitializeLayout();
            Function.LogDebug("ComputerPedView InitializeLayout");
            if (Owner == null || !Owner.Exists() || Vehicle == null || !Vehicle.Exists()) return;
            this.Window.DisableResizing();
            this.Position = this.GetLaunchPosition();
            btn_ped_image_holder.SetImage(DetermineImagePath(Owner), true);
            btn_vehicle_image_holder.SetImage(DetermineImagePath(Vehicle), true);
            text_first_name.KeyboardInputEnabled = false;
            text_last_name.KeyboardInputEnabled = false;
            text_home_address.KeyboardInputEnabled = false;
            text_dob.KeyboardInputEnabled = false;
            text_license_status.KeyboardInputEnabled = false;
            text_wanted_status.KeyboardInputEnabled = false;
            text_times_stopped.KeyboardInputEnabled = false;
            text_vehicle_model.KeyboardInputEnabled = false;
            text_vehicle_license.KeyboardInputEnabled = false;
            text_vehicle_insurance_status.KeyboardInputEnabled = false;
            text_vehicle_registration_status.KeyboardInputEnabled = false;
            lbl_vehicle_alert_label.TextColor = System.Drawing.Color.Red;
            lbl_vehicle_alert_label.TextColor = System.Drawing.Color.Red;

            cb_action.ItemSelected += ActionSelected;
            BindData();
        }

        private void ActionSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            if (arguments.SelectedItem == null || arguments.SelectedItem.Name.Equals("Placeholder")) return;
            switch(arguments.SelectedItem.Name)
            {
                case "Blip":
                    Function.LogDebug("ActionSelected Blip Vehicle");
                    ComputerVehicleController.BlipVehicle(Vehicle, System.Drawing.Color.Yellow);
                    break;
            }
        }

        private String DetermineImagePath(Ped ped)
        {           
            try
            {
                if (ped == null || !ped.Exists()) return Function.DefaultPedImagePath;
                String modelName = ped.Model.Name;
                int headDrawableIndex, headDrawableTextureIndex;

                ped.GetVariation(0, out headDrawableIndex, out headDrawableTextureIndex);

                String _model = String.Format(@"{0}__0_{1}_{2}", modelName, headDrawableIndex, headDrawableTextureIndex).ToLower();
                var path = Function.GetPedImagePath(_model);
                Function.LogDebug(String.Format("Loading image for model from  {0}", path));
                return path;
            }
            catch
            {
                Function.LogDebug("DetermineImagePath Error");
                return Function.DefaultPedImagePath;
            }
        }

        private String DetermineImagePath(Vehicle vehicle)
        {            
            try
            {
                if (vehicle == null || !vehicle.Exists()) return Function.DefaultVehicleImagePath;
                String modelName = vehicle.Model.Name;
                var path = Function.GetVehicleImagePath(modelName);
                Function.LogDebug(String.Format("Loading image for vehicle from {0}", path));
                return path;
            }
            catch
            {
                Function.LogDebug("DetermineImagePath Error");
                return Function.DefaultVehicleImagePath;
            }
        }

        private void BindData()
        {

            switch (OwnerPersona.LicenseState)
            {
                case ELicenseState.Expired:
                    text_license_status.Warn("Expired");
                    break;
                case ELicenseState.None:
                    text_license_status.Text = "None";
                    break;
                case ELicenseState.Suspended:
                    text_license_status.Warn("Suspended");
                    break;
                case ELicenseState.Valid:
                    text_license_status.Text = "Valid";
                    break;
            }

            if (OwnerPersona.IsAgent)
            {
                text_license_status.Warn("Individual is a federal agent");
                lbl_alert.Show();
            }
            else if (OwnerPersona.IsCop)
            {
                text_license_status.Warn("Individual is a police officer");                
                lbl_alert.Show();
            }

            if (!String.IsNullOrWhiteSpace(VehiclePersona.Alert))
            {                
                lbl_vehicle_alert.Text = VehiclePersona.Alert;
                lbl_vehicle_alert.Show();
                lbl_vehicle_alert_label.Show();
            } else
            {
                lbl_vehicle_alert_label.Hide();
                lbl_vehicle_alert.Hide();
            }

            cb_action.AddItem("Select One", "Placeholder");
            cb_action.AddItem("Blip (30 sec)", "Blip");
            
            var age = (DateTime.Today - OwnerPersona.BirthDay).Days / 365.25m;
            text_age.Text = ((int)age).ToString();
            text_first_name.Text = OwnerPersona.Forename;
            text_last_name.Text = OwnerPersona.Surname;
            text_home_address.Text = Owner.GetHomeAddress();
            text_dob.Text = OwnerPersona.BirthDay.ToString("MM/dd/yyyy");
            text_dob.SetToolTipText("MM/dd/yyyy");
            if (OwnerPersona.Wanted) text_wanted_status.Warn("Wanted");
            else text_wanted_status.SetText("None");
            text_times_stopped.Text = OwnerPersona.TimesStopped.ToString();

            text_vehicle_model.Text = Vehicle.Model.Name;
            text_vehicle_license.Text = Vehicle.LicensePlate;
            if (Function.IsTrafficPolicerRunning())
            {
                if (TrafficPolicerFunction.GetVehicleInsuranceStatus(Vehicle) == EVehicleStatus.Valid) text_vehicle_insurance_status.SetText("Valid");
                else text_vehicle_insurance_status.Warn("Expired");

                if (TrafficPolicerFunction.GetVehicleRegistrationStatus(Vehicle) == EVehicleStatus.Valid) text_vehicle_registration_status.SetText("Valid");
                else text_vehicle_registration_status.Warn("Expired");
            } else
            {
                text_vehicle_insurance_status.Hide();
                text_vehicle_registration_status.Hide();
                lbl_insurance_status.Hide();
                lbl_registration_status.Hide();
            }
                
        }
    }
}
