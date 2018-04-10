using System;
using Gwen;
using Gwen.Control;
using Rage;

using ComputerPlus.Controllers.Models;

using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Extensions.Rage;
using ComputerPlus.Controllers;
using ComputerPlus.Interfaces.Common;
using ComputerPlus.Interfaces.Reports.Models;
using GwenSkin = Gwen.Skin;
using SystemDrawing = System.Drawing;


namespace ComputerPlus.Interfaces.ComputerVehDB
{
    class ComputerVehicleDetails : Base
    {
        public enum QuickActions { PLACEHOLDER = 0, BLIP_VEHICLE = 1, CREATE_TRAFFIC_CITATION = 2, CREATE_ARREST_REPORT_FOR_DRIVER = 3 };

        LabeledComponent<StateControlledTextbox> labeled_first_name, labeled_last_name,
            labeled_home_address, labeled_dob, labeled_license_status,
            labeled_wanted_status, labeled_times_stopped, labeled_age,
            labeled_vehicle_model, labeled_vehicle_license,
            labeled_vehicle_insurance_status, labeled_vehicle_registration_status, labeled_vehicle_stolen_status,
            labeled_vehicle_extra_1, labeled_vehicle_extra_2, labeled_vehicle_extra_3,
            labeled_ped_extra_1;

        LabeledComponent<Label> labeled_alpr, labeled_owner_alert;

        ImagePanel image_ped_image_holder = null;
        ImagePanel image_vehicle_image_holder = null;

        ComboBox cb_action;
        
        private DetailedEntity DetailedEntity;
        private Ped Owner
        {
            get
            {
                return DetailedEntity.Entity.Ped;
            }
        }
        private Vehicle Vehicle
        {
            get
            {
                return DetailedEntity.Entity.Vehicle;
            }
        }
 

        internal static int DefaultHeight = 630;
        internal static int DefaultWidth = 730;

        FormSection registrationInformation, ownerInformation;
        Base registrationContent, ownerContent;

        SystemDrawing.Color labelColor = SystemDrawing.Color.Black;
        Font labelFont;

        bool BindNeeded;

        internal event ComputerVehicleQuickActionSelected OnQuickActionSelected;
        internal delegate void ComputerVehicleQuickActionSelected(object sender, QuickActions action);

        internal ComputerVehicleDetails(Base parent, DetailedEntity pedReport, ComputerVehicleQuickActionSelected quickActionCallback = null) : base(parent)
        {
            DetailedEntity = pedReport;
            InitializeLayout();
            BindNeeded = true;
            if (quickActionCallback != null) OnQuickActionSelected += quickActionCallback;
            else OnQuickActionSelected += OnQuickAction;
        }

        public void InitializeLayout()
        {
            Function.Log("inside ComputerVehicleDetails.InitializeLayout()");

            labelFont = this.Skin.DefaultFont.Copy();
            labelFont.Size = 14;
            labelFont.Smooth = true;

            cb_action = new ComboBox(this);

            registrationInformation = new FormSection(this, "Registration Information");
            registrationContent = new Base(this);

            labeled_vehicle_model = LabeledComponent.StatefulTextbox(registrationContent, "Model", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_vehicle_license = LabeledComponent.StatefulTextbox(registrationContent, "License Plate", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_vehicle_insurance_status = LabeledComponent.StatefulTextbox(registrationContent, "Insurance Status", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            //BPS does not have registration
            if (ComputerPlusEntity.PersonaType == PersonaTypes.BPS)
            {
                labeled_vehicle_registration_status = LabeledComponent.StatefulTextbox(registrationContent, "Taxed Status", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
                labeled_vehicle_extra_1 = LabeledComponent.StatefulTextbox(registrationContent, "MOT Status", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
                labeled_vehicle_extra_2 = LabeledComponent.StatefulTextbox(registrationContent, "SORN Status", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            }
            else labeled_vehicle_registration_status = LabeledComponent.StatefulTextbox(registrationContent, "Registration Status", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_vehicle_stolen_status = LabeledComponent.StatefulTextbox(registrationContent, "Stolen Vehicle", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);

            if (Configs.DisplayVehicleImage)
            {
                image_vehicle_image_holder = new ImagePanel(registrationContent);
                image_vehicle_image_holder.ShouldCacheToTexture = false;
                image_vehicle_image_holder.SetSize(400, 160);
            }

            labeled_alpr = new LabeledComponent<Label>(registrationContent, "ALPR", new Label(registrationContent), RelationalPosition.LEFT, RelationalSize.MEDIUM, Configs.BaseFormControlSpacingDouble, labelFont, System.Drawing.Color.Red);
            labeled_owner_alert = new LabeledComponent<Label>(registrationContent, "Alert", new Label(registrationContent),  RelationalPosition.LEFT, RelationalSize.MEDIUM, Configs.BaseFormControlSpacingDouble, labelFont, System.Drawing.Color.Red);

            ownerInformation = new FormSection(this, "Owner Information");
            ownerContent = new Base(this);
            labeled_first_name = LabeledComponent.StatefulTextbox(ownerContent, "First Name", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_last_name = LabeledComponent.StatefulTextbox(ownerContent, "Last Name", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_age = LabeledComponent.StatefulTextbox(ownerContent, "Age", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);

            labeled_home_address = LabeledComponent.StatefulTextbox(ownerContent, "Home Address", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_dob = LabeledComponent.StatefulTextbox(ownerContent, "DOB", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);

            labeled_license_status = LabeledComponent.StatefulTextbox(ownerContent, "License Status", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_wanted_status = LabeledComponent.StatefulTextbox(ownerContent, "Wanted Status", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_times_stopped = LabeledComponent.StatefulTextbox(ownerContent, "Times Stopped", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);

            if (ComputerPlusEntity.PersonaType == PersonaTypes.BPS)
            {
                labeled_ped_extra_1 = LabeledComponent.StatefulTextbox(ownerContent, "Insured To Drive", RelationalPosition.LEFT, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            }

            if (Configs.DisplayPedImage)
            {
                image_ped_image_holder = new ImagePanel(ownerContent);
                image_ped_image_holder.ShouldCacheToTexture = false;
                image_ped_image_holder.SetSize(155, 217);
            }

            labeled_first_name.Component.Disable();
            labeled_last_name.Component.Disable();
            labeled_age.Component.Disable();
            labeled_home_address.Component.Disable();
            labeled_dob.Component.Disable();
            labeled_license_status.Component.Disable();
            labeled_wanted_status.Component.Disable();
            labeled_times_stopped.Component.Disable();
            labeled_vehicle_model.Component.Disable();
            labeled_vehicle_license.Component.Disable();
            labeled_vehicle_insurance_status.Component.Disable();
            labeled_vehicle_registration_status.Component.Disable();
            labeled_vehicle_stolen_status.Component.Disable();
            if (ComputerPlusEntity.PersonaType == PersonaTypes.BPS)
            {
                labeled_vehicle_extra_1.Component.Disable();
                labeled_vehicle_extra_2.Component.Disable();
                labeled_ped_extra_1.Component.Disable();
            }
            if (Configs.DisplayPedImage && Owner)
            {
                image_ped_image_holder.ImageName = DetermineImagePath(Owner);
                // image_ped_image_holder.ShouldCacheToTexture = true;
            }
            if (Configs.DisplayVehicleImage && Vehicle)
            {
                image_vehicle_image_holder.ImageName = DetermineImagePath(Vehicle);
                // image_vehicle_image_holder.ShouldCacheToTexture = true;
            }
            cb_action.ItemSelected += ActionSelected;

        }

        protected override void Layout(GwenSkin.Base skin)
        {
            base.Layout(skin);
            
            BindData();
            cb_action.SetSize(200, cb_action.Height);
            cb_action.PlaceLeftOf();
            cb_action.LogPositionAndSize();

            registrationInformation
             .AddContentChild(registrationContent)
             .PlaceBelowOf(cb_action)
             .AlignLeftWith()
             .SizeWidthWith();

            labeled_vehicle_license.Component.SmallSize();
            labeled_vehicle_model.Component.SmallSize();
            labeled_vehicle_insurance_status.Component.SmallSize();
            labeled_vehicle_registration_status.Component.SmallSize();
            labeled_vehicle_stolen_status.Component.SmallSize();

            labeled_vehicle_license
                .PlaceRightOf(labeled_vehicle_model, Configs.BaseFormControlSpacingTriple)
                .AlignTopWith(labeled_vehicle_model);
            
            labeled_vehicle_insurance_status
                .PlaceBelowOf(labeled_vehicle_model)
                .AlignLeftWith(labeled_vehicle_model);

            labeled_vehicle_registration_status
                .PlaceBelowOf(labeled_vehicle_insurance_status)
                .AlignLeftWith(labeled_vehicle_insurance_status);

            labeled_vehicle_stolen_status
                .PlaceBelowOf(labeled_vehicle_registration_status)
                .AlignLeftWith(labeled_vehicle_registration_status);

            labeled_vehicle_insurance_status.Component.AlignLeftWith(labeled_vehicle_license);
            labeled_vehicle_registration_status.Component.AlignLeftWith(labeled_vehicle_insurance_status.Component);
            labeled_vehicle_stolen_status.Component.AlignLeftWith(labeled_vehicle_registration_status.Component);

            if (ComputerPlusEntity.PersonaType == PersonaTypes.BPS)
            {
                labeled_vehicle_extra_1.Component.SmallSize();
                labeled_vehicle_extra_2.Component.SmallSize();
                labeled_vehicle_extra_1.PlaceBelowOf(labeled_vehicle_stolen_status);
                labeled_vehicle_extra_1.Component.AlignLeftWith(labeled_vehicle_stolen_status);
                labeled_vehicle_extra_2.PlaceBelowOf(labeled_vehicle_extra_1);
                labeled_vehicle_extra_2.Component.AlignLeftWith(labeled_vehicle_extra_1);
            }

            if (Configs.DisplayVehicleImage)
            {
                image_vehicle_image_holder
                    .PlaceLeftOf();

                labeled_alpr
                    .PlaceBelowOf(image_vehicle_image_holder)
                    .AlignLeftWith(image_vehicle_image_holder)
                    .SizeWidthWith(image_vehicle_image_holder);
            }
            else
            {
                labeled_alpr
                    .PlaceBelowOf(labeled_vehicle_stolen_status)
                    .AlignLeftWith(labeled_vehicle_stolen_status)
                    .SizeWidthWith(labeled_vehicle_stolen_status);
            }

            labeled_owner_alert
                .Align(labeled_vehicle_stolen_status, labeled_alpr)
                .SizeWidthWith(labeled_vehicle_stolen_status);

            registrationContent.SizeToChildrenBlock();
            registrationInformation.SizeToChildrenBlock();


            ownerInformation
             .AddContentChild(ownerContent)
             .PlaceBelowOf(registrationInformation)
             .AlignLeftWith(registrationInformation)
             .SizeWidthWith(registrationInformation);

            labeled_first_name.Component.MediumSize();
            labeled_last_name.Component.MediumSize();
            labeled_times_stopped.Component.SmallSize();
            labeled_wanted_status.Component.SmallSize();
            labeled_license_status.Component.SmallSize();
            labeled_age.Component.SmallSize();
            labeled_dob.Component.SmallSize();

            labeled_last_name
                .PlaceRightOf(labeled_first_name, Configs.BaseFormControlSpacingDouble)
                .AlignTopWith(labeled_first_name);

            labeled_age
                .PlaceRightOf(labeled_last_name, Configs.BaseFormControlSpacingDouble)
                .AlignTopWith(labeled_last_name);            

            labeled_home_address
                .PlaceBelowOf(labeled_first_name, Configs.BaseFormControlSpacingDouble)
                .AlignLeftWith(labeled_first_name);

            labeled_dob
                .Align(labeled_age, labeled_home_address);            

            labeled_license_status
                .PlaceBelowOf(labeled_home_address, Configs.BaseFormControlSpacingDouble)
                .AlignLeftWith(labeled_home_address);

            labeled_times_stopped
                .AlignRightWith(labeled_dob)
                .AlignTopWith(labeled_license_status);

            if (ComputerPlusEntity.PersonaType == PersonaTypes.BPS)
            {
                labeled_ped_extra_1.Component.SmallSize();
                labeled_ped_extra_1.PlaceBelowOf(labeled_times_stopped)
                .AlignLeftWith(labeled_times_stopped);

                //Adjust left for labeled_times_stopped
                labeled_times_stopped.Component.AlignLeftWith(labeled_ped_extra_1.Component);
            }


            labeled_wanted_status
                .PlaceBelowOf(labeled_license_status)
                .AlignLeftWith(labeled_license_status);

            if (Configs.DisplayPedImage)
            {
                image_ped_image_holder
                   .PlaceLeftOf();
            }

            ownerInformation.SizeToChildrenBlock();
            ownerContent.SizeToChildrenBlock();
        }

        private void ActionSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            if (arguments.SelectedItem == null || (QuickActions)arguments.SelectedItem.UserData == QuickActions.PLACEHOLDER || arguments.SelectedItem.Name.Equals("Placeholder")) return;
            OnQuickActionSelected(this, (QuickActions)arguments.SelectedItem.UserData);
            cb_action.SelectByUserData(QuickActions.PLACEHOLDER);
        }

        private void OnQuickAction(object sender, QuickActions action)
        {
            switch (action)
            {
                case QuickActions.BLIP_VEHICLE:
                    Function.LogDebug("ActionSelected Blip Vehicle");
                    ComputerVehicleController.BlipVehicle(Vehicle, System.Drawing.Color.Yellow);
                    break;
                case QuickActions.CREATE_TRAFFIC_CITATION:
                    ComputerReportsController.ShowTrafficCitationCreate(null, DetailedEntity.Entity);
                    break;
                case QuickActions.CREATE_ARREST_REPORT_FOR_DRIVER:
                    ComputerReportsController.ShowArrestReportCreate(DetailedEntity.Entity, null);
                    break;
            }
        }

        private String DetermineImagePath(Ped ped)
        {
            try
            {
                // if (ped == null || !ped.Exists()) return Function.DefaultPedImagePath;
                String modelName = String.Empty;
                if (ped != null && ped.IsValid()) modelName = ped.Model.Name;

                int headDrawableIndex = 0, headDrawableTextureIndex = 0;
                if (ped != null && ped.IsValid()) ped.GetVariation(0, out headDrawableIndex, out headDrawableTextureIndex);

                //String _model = String.Format(@"{0}__0_{1}_{2}", modelName, headDrawableIndex, headDrawableTextureIndex).ToLower();
                var path = Function.GetPedImagePath(modelName, headDrawableIndex, headDrawableTextureIndex);
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
                // if (vehicle == null || !vehicle.Exists()) return Function.DefaultVehicleImagePath;
                String modelName = String.Empty;
                if (vehicle != null && vehicle.IsValid()) modelName = vehicle.Model.Name;

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
            if (DetailedEntity.Entity.Ped == null || !DetailedEntity.Entity.Ped.IsValid()) return;
            if (!BindNeeded) return;
            BindNeeded = false;


            if (DetailedEntity.Entity == null) return;

            lock(DetailedEntity.Entity)
            {
                if (DetailedEntity.Entity.IsLicenseValid)
                {
                    labeled_license_status.Component.Valid("Valid");
                }
                else
                {
                    labeled_license_status.Component.Warn(DetailedEntity.Entity.LicenseStateString);
                }

                if (DetailedEntity.Entity.IsAgent)
                {
                    labeled_owner_alert.Component.Warn("Individual is a federal agent");
                    labeled_owner_alert.Show();
                }
                else if (DetailedEntity.Entity.IsCop)
                {
                    labeled_owner_alert.Component.Warn("Individual is a police officer");
                    labeled_owner_alert.Show();
                }
                else
                {
                    labeled_owner_alert.Hide();
                }

                if (!String.IsNullOrWhiteSpace(DetailedEntity.Entity.VehicleAlert))
                {
                    labeled_alpr.Component.SetText(DetailedEntity.Entity.VehicleAlert);
                    labeled_alpr.Show();
                }
                else
                {
                    labeled_alpr.Hide();
                }

                cb_action.AddItem("Select One", "Placeholder", QuickActions.PLACEHOLDER);
                cb_action.AddItem("Blip (30 sec)", "Blip", QuickActions.BLIP_VEHICLE);
                cb_action.AddItem("Create Citation", "TrafficCitation", QuickActions.CREATE_TRAFFIC_CITATION);
                cb_action.AddItem("Create Arrest Report", "ArrestReport", QuickActions.CREATE_ARREST_REPORT_FOR_DRIVER);

                labeled_age.Component.Text = DetailedEntity.Entity.AgeString;
                labeled_first_name.Component.Text = DetailedEntity.Entity.FirstName;
                labeled_last_name.Component.Text = DetailedEntity.Entity.LastName;
                labeled_home_address.Component.Text = Owner.GetHomeAddress();
                labeled_dob.Component.Text = DetailedEntity.Entity.DOBString;
                if (DetailedEntity.Entity.IsWanted)
                    labeled_wanted_status.Component.Warn("Wanted");
                else
                    labeled_wanted_status.Component.Valid("None");
                labeled_times_stopped.Component.Text = DetailedEntity.Entity.TimesStopped.ToString();

                labeled_vehicle_model.Component.Text = Vehicle.Model.Name;
                labeled_vehicle_license.Component.Text = Vehicle.LicensePlate;
                if (ComputerPlusEntity.PersonaType == PersonaTypes.BPS)
                {
                    if (BritishPolicingFunctions.IsVehicleRegistered(DetailedEntity.Entity.RawVehiclePersona))
                        labeled_vehicle_registration_status.Component.Valid("Yes");
                    else
                        labeled_vehicle_registration_status.Component.Warn("No");
                    Function.LogDebug("set labeled_vehicle_registration_status");

                    if (BritishPolicingFunctions.IsVehicleInsured(DetailedEntity.Entity.RawVehiclePersona))
                        labeled_vehicle_insurance_status.Component.Valid("Yes");
                    else
                        labeled_vehicle_insurance_status.Component.Warn("No");
                    Function.LogDebug("set labeled_vehicle_insurance_status");

                    if (BritishPolicingFunctions.DoesVehicleHaveMOT(DetailedEntity.Entity.RawVehiclePersona))
                        labeled_vehicle_extra_1.Component.Valid("Yes");
                    else
                        labeled_vehicle_extra_1.Component.Warn("No");
                    Function.LogDebug("set labeled_vehicle_extra_1");

                    if (BritishPolicingFunctions.DoesVehicleHaveSORN(DetailedEntity.Entity.RawVehiclePersona))
                        labeled_vehicle_extra_2.Component.Valid("Yes");
                    else
                        labeled_vehicle_extra_2.Component.Warn("No");
                    Function.LogDebug("set labeled_vehicle_extra_2");

                    if (BritishPolicingFunctions.IsPedInsuredToDriveVehicle(DetailedEntity.Entity.RawPedPersona, DetailedEntity.Entity.RawVehiclePersona))
                        labeled_ped_extra_1.Component.Valid("Yes");
                    else
                        labeled_ped_extra_1.Component.Warn("No");

                    Function.LogDebug("set labeled_ped_extra_1");
                }
                else if (Function.IsTrafficPolicerRunning())
                {
                    var insuranceStatus = TrafficPolicerFunction.GetVehicleInsuranceStatus(Vehicle);
                    if (insuranceStatus == EVehicleStatus.Valid)
                        labeled_vehicle_insurance_status.Component.Valid("Valid");
                    else if (insuranceStatus == EVehicleStatus.Expired)
                        labeled_vehicle_insurance_status.Component.Warn("Expired");
                    else
                        labeled_vehicle_insurance_status.Component.Warn("None");

                    var registrationStatus = TrafficPolicerFunction.GetVehicleRegistrationStatus(Vehicle);
                    if (registrationStatus == EVehicleStatus.Valid)
                        labeled_vehicle_registration_status.Component.Valid("Valid");
                    else if (registrationStatus == EVehicleStatus.Expired)
                        labeled_vehicle_registration_status.Component.Warn("Expired");
                    else
                        labeled_vehicle_registration_status.Component.Warn("None");
                }
                else
                {
                    labeled_vehicle_insurance_status.Hide();
                    labeled_vehicle_registration_status.Hide();
                }

                if (Vehicle.IsStolen)
                    labeled_vehicle_stolen_status.Component.Warn("Yes");
                else
                    labeled_vehicle_stolen_status.Component.Valid("No");

            }
        }
    }
}
