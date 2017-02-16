using System;
using System.Collections.Generic;
using System.Linq;
using Rage.Forms;
using Gwen;
using Gwen.Control;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine.Scripting.Entities;
using ComputerPlus.Controllers.Models;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Extensions.Rage;
using ComputerPlus.Controllers;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Interfaces.Reports.Arrest;
using ComputerPlus.Interfaces.Common;

namespace ComputerPlus.Interfaces.ComputerPedDB
{
    public class ComputerPedView : Base
    {
        public enum QuickActions { PLACEHOLDER = 0, CREATE_ARREST_REPORT = 1 };
        public delegate void QuickActionSelected(object sender, QuickActions action);

        QuickActionSelected OnQuickActionSelected;
        ComputerPlusEntity mEntity;
        public ComputerPlusEntity Entity
        {
            get { return mEntity; }
            set
            {

                if (mEntity != value)
                {
                    mEntity = value;
                    if (this.IsVisible)
                        BindData();                    
                }
                else
                    mEntity = value;
            }
        }

        StateControlledTextbox text_first_name, text_last_name,
               text_home_address, text_dob, text_license_status,
               text_wanted_status, text_times_stopped, text_age;

        Label lbl_alert, lbl_first_name, lbl_last_name,
               lbl_home_address, lbl_dob, lbl_license_status,
               lbl_wanted_status, lbl_times_stopped, lbl_age;
        ComboBox cb_action;
        ImagePanel ped_image_holder;
        //Button btn_ped_image_holder; //TODO try image panel

        public ComputerPedView(Base parent, ComputerPlusEntity entity) : base (parent)
        {
            Entity = entity;
            InitializeLayout();
            BindData();
        }

        public ComputerPedView(Base parent, ComputerPlusEntity entity, QuickActionSelected quickActionSelectedCallback) : this(parent, entity)
        {
            OnQuickActionSelected = quickActionSelectedCallback;
        }

        public void ChangeEntity(ComputerPlusEntity entity)
        {
            this.Entity = entity;
            BindData();

        }

        private void InitializeLayout()
        {
            lbl_first_name = new Label(this) { Text = "First Name" };
            lbl_first_name.SetPosition(30, 30);
            lbl_first_name.SizeToContents();
            text_first_name = new StateControlledTextbox(this) { IsDisabled = true };
            text_first_name.SetPosition(this.X + Configs.BaseFormControlSpacing, this.Y + Configs.BaseFormControlSpacing);
            text_first_name.NormalSize();
            Gwen.Align.PlaceDownLeft(lbl_first_name, text_first_name);

            lbl_last_name = new Label(this) { Text = "Last Name" };
            lbl_last_name.SizeToContents();
            text_last_name = new StateControlledTextbox(this) { IsDisabled = true };
            text_last_name.NormalSize();
            Gwen.Align.PlaceRightBottom(text_last_name, text_first_name, Configs.BaseFormControlSpacing); //Place textbox last name to the right of first name
            lbl_last_name.SetPosition(text_last_name.X, lbl_first_name.Y); //Line up last name label with the last name text box (x) and the first name label (y)

            lbl_age = new Label(this) { Text = "Age" };
            lbl_age.SizeToContents();
            text_age = new StateControlledTextbox(this) { IsDisabled = true };
            text_age.SmallSize();
            Gwen.Align.PlaceRightBottom(text_age, text_last_name, Configs.BaseFormControlSpacing);
            Gwen.Align.PlaceDownLeft(lbl_age, text_age);
            //lbl_age.SetPosition(text_age.X, lbl_last_name.Y);

            lbl_home_address = new Label(this) { Text = "Home Address" };
            Gwen.Align.PlaceDownLeft(lbl_home_address, text_first_name, Configs.BaseFormControlSpacingDouble); //Place Home Address label below first name, align left with first name text
            text_home_address = new StateControlledTextbox(this) { IsDisabled = true };
            text_home_address.LongSize();
            Gwen.Align.PlaceDownLeft(text_home_address, lbl_home_address);

            lbl_dob = new Label(this) { Text = "DOB" };
            lbl_dob.SizeToContents();
            text_dob = new StateControlledTextbox(this) { IsDisabled = true };
            text_dob.SmallSize();
            text_dob.SetPosition(text_age.X, text_home_address.Y);
            //Gwen.Align.PlaceRightBottom(text_dob, text_home_address, Configs.BaseFormControlSpacing);
            lbl_dob.SetPosition(text_dob.X, lbl_home_address.Y);

            lbl_license_status = new Label(this) { Text = "License Status" };
            Gwen.Align.PlaceDownLeft(lbl_license_status, text_home_address, Configs.BaseFormControlSpacingTriple);
            text_license_status = new StateControlledTextbox(this) { IsDisabled = true };
            text_license_status.SmallSize();
            Gwen.Align.PlaceRightBottom(text_license_status, lbl_license_status, Configs.BaseFormControlSpacing);

            lbl_wanted_status = new Label(this) { Text = "Wanted Status" };
            Gwen.Align.PlaceDownLeft(lbl_wanted_status, lbl_license_status, Configs.BaseFormControlSpacing);
            text_wanted_status = new StateControlledTextbox(this) { IsDisabled = true };
            text_wanted_status.SmallSize();
            Gwen.Align.PlaceRightBottom(text_wanted_status, lbl_wanted_status, Configs.BaseFormControlSpacing);

            lbl_times_stopped = new Label(this) { Text = "Times Stopped" };
            Gwen.Align.PlaceDownLeft(lbl_times_stopped, lbl_wanted_status, Configs.BaseFormControlSpacing);
            text_times_stopped = new StateControlledTextbox(this) { IsDisabled = true };
            text_times_stopped.SmallSize();
            text_times_stopped.SetPosition(text_wanted_status.X, lbl_times_stopped.Y);


            ped_image_holder = new ImagePanel(this);
            ped_image_holder.RegularSizeVertical();
            ped_image_holder.ImageName = Function.DetermineImagePath(Entity.Ped);
            ped_image_holder.SetPosition(text_age.Right + Configs.BaseFormControlSpacingDouble, text_age.Y + Configs.BaseFormControlSpacingDouble);


            cb_action = new ComboBox(this);
            cb_action.AddItem("Select One", "PlaceHolder", QuickActions.PLACEHOLDER);
            cb_action.AddItem("Create Arrest Report", "ArrestReport", QuickActions.CREATE_ARREST_REPORT);
            cb_action.ItemSelected += ActionComboBoxItemSelected;
            cb_action.SetSize(ped_image_holder.Width, cb_action.Height);
            cb_action.SetPosition(ped_image_holder.X, text_last_name.Y);
        }



        private void BindData()
        {
            if (Entity == null) return;
            lock (Entity)
            {
                switch (Entity.PedPersona.LicenseState)
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

                if (Entity.PedPersona.IsAgent)
                {
                    lbl_alert.SetText("Individual is a federal agent");
                }
                else if (Entity.PedPersona.IsCop)
                {
                    lbl_alert.SetText("Individual is a police officer");
                }

                var age = (DateTime.Today - Entity.PedPersona.BirthDay).Days / 365.25m;
                text_age.Text = ((int)age).ToString();
                text_first_name.Text = Entity.PedPersona.Forename;
                text_last_name.Text = Entity.PedPersona.Surname;
                text_home_address.Text = Entity.Ped.GetHomeAddress();
                text_dob.Text = Entity.PedPersona.BirthDay.ToString("MM/dd/yyyy");
                text_dob.SetToolTipText("MM/dd/yyyy");
                if (Entity.PedPersona.Wanted) text_wanted_status.Warn("Wanted");
                else text_wanted_status.SetText("None");
                text_times_stopped.Text = Entity.PedPersona.TimesStopped.ToString();
            }
        }

        private void ActionComboBoxItemSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            var action = (QuickActions)arguments.SelectedItem.UserData;
            if (OnQuickActionSelected != null)
            {
                OnQuickActionSelected(this, action);
                return;
            }
            switch (action)
            {
                case QuickActions.CREATE_ARREST_REPORT:
                    {
                        ComputerReportsController.ShowArrestReportCreate(this.Entity, null);
                        return;
                    }
            }
        }

    }
    sealed class ComputerPedViewContainer : GwenForm
    {
        ComputerPlusEntity Entity;
        ComputerPedView pedView;

        internal ComputerPedViewContainer(ComputerPlusEntity entity) : this()
        {
            this.Entity = entity;
        }

        internal ComputerPedViewContainer() : base(typeof(ComputerPedViewTemplate))
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            Function.LogDebug("ComputerPedView InitializeLayout");            
            
            this.Position = this.GetLaunchPosition();
            this.Window.DisableResizing();
            pedView = new ComputerPedView(this, Entity);
            
        }

        public void ChangeEntity(ComputerPlusEntity entity)
        {
            Entity = pedView.Entity = entity;
        }       
    }
}
