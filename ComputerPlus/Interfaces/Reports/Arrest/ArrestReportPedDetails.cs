using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Extensions.Gwen;
using Gwen.Control;
using Rage.Forms;
using System;
using ComputerPlus.Interfaces.Common;
using System.Collections.Generic;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    internal class ArrestReportPedDetails : GwenForm
    {
        StateControlledTextbox text_arrestee_dob;
        StateControlledTextbox text_arrestee_home_address;
        StateControlledTextbox text_arrestee_last_name;
        StateControlledTextbox text_arrestee_first_name;
        StateControlledTextbox text_arrest_street;
        StateControlledTextbox text_arrest_city;
        StateControlledTextbox text_arrest_time;
        StateControlledTextbox text_arrest_date;

        Label lbl_first_name;
        Label lbl_last_name;
        Label lbl_dob;

        Button btn_auto_location;

        Label[] labelsWithState;
        StateControlledTextbox[] textboxesWithState;

        internal ArrestReport Report;
        private IErrorNotifier ErrorNotifier;
        private IActionNotifier ActionNotifier;
       

        internal ArrestReportPedDetails(ArrestReport report) : base(typeof(ArrestReportPedDetailsTemplate))
        {
            Report = report;            
        }

        ~ ArrestReportPedDetails()
        {
            if (ErrorNotifier != null)
            ErrorNotifier.UnsubscribeToErrorEvents(OnValidationError);
        }

        private void OnValidationError(object sender, Dictionary<String, String> errors)
        {
            ClearErrorState();
            if (errors != null && errors.Count > 0) {
                foreach (KeyValuePair<String, String> kvp in errors)
                {
                    if (!String.IsNullOrEmpty(kvp.Key))
                    {
                        switch (kvp.Key)
                        {
                            case "First Name":
                                {
                                    text_arrestee_first_name.Error(kvp.Value);
                                    lbl_first_name.Error(kvp.Value);
                                    break;
                                }
                            case "Last Name":
                                {
                                    text_arrestee_last_name.Error(kvp.Value);
                                    lbl_last_name.Error(kvp.Value);
                                    break;
                                }
                            case "DOB":
                                {
                                    text_arrestee_dob.Error(kvp.Value);
                                    lbl_dob.Error(kvp.Value);
                                    break;
                                }
                        }
                    }
                }
            }
        }

        private void OnArrestReportSaveEvent(object sender, ArrestReportContainer.ArrestReportSaveResult result, ArrestReport report)
        {
            if (result == ArrestReportContainer.ArrestReportSaveResult.SAVE)
                ClearErrorState();
        }

        public void SetErrorSubscription(IErrorNotifier notifier)
        {
            if (ErrorNotifier != null) notifier.UnsubscribeToErrorEvents(OnValidationError);
            ErrorNotifier = notifier;
            ErrorNotifier.SubscribeToErrorEvents(OnValidationError);
        }

        public void SetActionSubscription(IActionNotifier notifier)
        {
            if (ActionNotifier != null) notifier.UnsubscribeToActionEvents(OnArrestReportSaveEvent);
            ActionNotifier = notifier;
            ActionNotifier.SubscribeToActionEvents(OnArrestReportSaveEvent);
        }


        internal void ChangeReport(ArrestReport report)
        {
            Report = report;
            if (this.Exists())
            {
                PopulateInputs(Report);
            }
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Window.IsClosable = false;
            
            text_arrestee_first_name = new StateControlledTextbox(this);
            text_arrestee_first_name.SetSize(166, 21);
            text_arrestee_first_name.SetPosition(25, 65);
            text_arrestee_first_name.TextChanged += TextInputChanged;

            text_arrestee_last_name = new StateControlledTextbox(this);
            text_arrestee_last_name.SetSize(166, 21);
            text_arrestee_last_name.SetPosition(215, 65);
            text_arrestee_last_name.TextChanged += TextInputChanged;

            text_arrestee_dob = new StateControlledTextbox(this);
            text_arrestee_dob.SetSize(100, 21);
            text_arrestee_dob.SetPosition(405, 65);
            text_arrestee_dob.TextChanged += TextInputChanged;

            text_arrestee_home_address = new StateControlledTextbox(this);
            text_arrestee_home_address.SetSize(355, 21);
            text_arrestee_home_address.SetPosition(25, 150);
            text_arrestee_home_address.TextChanged += TextInputChanged;

            text_arrest_street = new StateControlledTextbox(this);
            text_arrest_street.SetSize(355, 21);
            text_arrest_street.SetPosition(22, 273);
            text_arrest_street.TextChanged += TextInputChanged;

            text_arrest_city = new StateControlledTextbox(this);
            text_arrest_city.SetSize(166, 21);
            text_arrest_city.SetPosition(25, 325);
            text_arrest_city.TextChanged += TextInputChanged;

            text_arrest_time = new StateControlledTextbox(this);
            text_arrest_time.SetSize(100, 21);
            text_arrest_time.SetPosition(405, 316);
            text_arrest_time.TextChanged += TextInputChanged;

            text_arrest_date = new StateControlledTextbox(this);
            text_arrest_date.SetSize(100, 21);
            text_arrest_date.SetPosition(405, 274);
            text_arrest_date.TextChanged += TextInputChanged;

            labelsWithState = new Label[] { lbl_dob, lbl_first_name, lbl_last_name };
            textboxesWithState = new StateControlledTextbox[] { text_arrestee_dob, text_arrestee_first_name, text_arrestee_last_name };

            btn_auto_location.LocationIcon();
            btn_auto_location.Clicked += ButtonClicked;

            //lbl_error = new RichLabel(this);
            //lbl_error.SetPosition(text_arrestee_dob.X + text_arrestee_dob.Width + 10f, text_arrestee_dob.Y);
            //lbl_error.SetSize(((this.Window.Width - (text_arrestee_dob.X + text_arrestee_dob.Width)) / 2) + 50, ((this.Window.Height - (text_arrestee_dob.Y + text_arrestee_dob.Height)) / 2) + 50);

            PopulateInputs(Report);            
            LockControls();
            if (Report.ReadOnly)
            {
                LockPedDetails(true);
            }
        }

        private void ButtonClicked(Base sender, ClickedEventArgs arguments)
        {
            if(sender == btn_auto_location)
            {
                text_arrest_street.Text = Report.ArrestStreetAddress = Function.GetPedCurrentStreetName();
                text_arrest_city.Text = Report.ArrestCity = Function.GetPedCurrentZoneName();
            }
        }

        private void ClearErrorState()
        {
            foreach (var label in labelsWithState) label.ClearError();
            foreach (var textbox in textboxesWithState) textbox.ClearError();
        }

        private void PopulateInputs(ArrestReport report)
        {
            if (report == null) return;
            text_arrest_time.LocalDateTime(report.ArrestTimeDate, TextBoxExtensions.DateOutputPart.TIME);
            text_arrest_date.LocalDateTime(report.ArrestTimeDate, TextBoxExtensions.DateOutputPart.DATE);
            // text_arrestee_dob.LocalDateTime(report.DOB, TextBoxExtensions.DateOutputPart.DATE);
            text_arrestee_dob.SetText(report.DOB);
            text_arrestee_home_address.Text = report.HomeAddress;
            text_arrestee_first_name.Text = report.FirstName;
            text_arrestee_last_name.Text = report.LastName;
            if (report.IsNew)
            {
                text_arrest_street.Text = report.ArrestStreetAddress = Function.GetPedCurrentStreetName();
                text_arrest_city.Text = report.ArrestCity = Function.GetPedCurrentZoneName();
            }
            else
            {
                text_arrest_street.Text = report.ArrestStreetAddress;
                text_arrest_city.Text = report.ArrestCity;
            }
        }

        private void TextInputChanged(Base sender, System.EventArgs arguments)
        {
            if (Report == null) return;

            if (sender == text_arrestee_home_address)
                Report.HomeAddress = text_arrestee_home_address.Text.Trim();
            else if (sender == text_arrestee_dob)
                Report.DOB = text_arrestee_dob.Text.Trim();
            else if (sender == text_arrestee_first_name)
                Report.FirstName = text_arrestee_first_name.Text.Trim();
            else if (sender == text_arrestee_last_name)
                Report.LastName = text_arrestee_last_name.Text.Trim();
            else if (sender == text_arrest_street)
                Report.ArrestStreetAddress = text_arrest_street.Text.Trim();
            else if (sender == text_arrest_city)
                Report.ArrestCity = text_arrest_city.Text.Trim();
        }

        public void LockPedDetails(bool disable)
        {
            if (disable)
            {
                text_arrestee_dob.Disable();
                text_arrestee_home_address.Disable();
                text_arrestee_last_name.Disable();
                text_arrestee_first_name.Disable();
            }
            else
            {
                text_arrestee_dob.Enable();
                text_arrestee_home_address.Enable();
                text_arrestee_last_name.Enable();
                text_arrestee_first_name.Enable();
            }
        }

        private void LockControls()
        {
            if (Report == null) return;
            text_arrest_time.Disable();
            text_arrest_date.Disable();
            if (Report.ReadOnly)
            {                
                text_arrestee_dob.Disable();
                text_arrestee_home_address.Disable();
                text_arrestee_last_name.Disable();
                text_arrestee_first_name.Disable();
                text_arrest_street.Disable();
                text_arrest_city.Disable();
                text_arrest_time.Disable();
                text_arrest_date.Disable();
            }
        }
    }
}