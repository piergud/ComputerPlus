using ComputerPlus.Interfaces.Reports.Models;
using Gwen.Control;
using Rage.Forms;
using System;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    internal class ArrestReportPedDetails : GwenForm
    {
        TextBox text_arrestee_dob;
        TextBox text_arrestee_home_address;
        TextBox text_arrestee_last_name;
        TextBox text_arrestee_first_name;
        TextBox text_arrest_street;
        TextBox text_arrest_city;
        TextBox text_arrest_time;
        internal ArrestReport Report;

        internal ArrestReportPedDetails(ArrestReport report) : base(typeof(ArrestReportPedDetailsTemplate))
        {
            Report = report;
        }

        internal void ChangeReport(ArrestReport report)
        {
            Report = report;
            PopulateInputs(Report);
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Window.IsClosable = false;

            text_arrest_time.TextChanged += TextInputChanged;
            text_arrestee_dob.TextChanged += TextInputChanged;
            text_arrestee_first_name.TextChanged += TextInputChanged;
            text_arrestee_last_name.TextChanged += TextInputChanged;
            text_arrestee_home_address.TextChanged += TextInputChanged;
            text_arrest_street.TextChanged += TextInputChanged;
            text_arrest_city.TextChanged += TextInputChanged;
            PopulateInputs(Report);            
            LockControls();
        }

        private void PopulateInputs(ArrestReport report)
        {
            if (report == null) return;
            text_arrest_time.Text = Report.ArrestTime;
            text_arrestee_dob.Text = Report.DOB;
            text_arrestee_home_address.Text = Report.HomeAddress;
            text_arrestee_first_name.Text = Report.FirstName;
            text_arrestee_last_name.Text = Report.LastName;
            text_arrest_street.Text = Report.ArrestStreetAddress;
            text_arrest_city.Text = Report.HomeAddress;
        }

        private void TextInputChanged(Base sender, System.EventArgs arguments)
        {
            if (Report == null) return;

            if (sender == text_arrestee_home_address)
                Report.HomeAddress = text_arrestee_home_address.Text;
            //else if (sender == text_arrest_time)
                //Report.ChangeArrestTime(= text_arrest_time.Text;
            else if (sender == text_arrestee_dob)
                Report.DOB = text_arrestee_dob.Text;
            else if (sender == text_arrestee_first_name)
                Report.FirstName = text_arrestee_first_name.Text;
            else if (sender == text_arrestee_last_name)
                Report.LastName = text_arrestee_last_name.Text;
            else if (sender == text_arrest_street)
                Report.ArrestStreetAddress = text_arrest_street.Text;
            else if (sender == text_arrest_city)
                Report.ArrestCity = text_arrest_city.Text;
        }

        private void LockControls()
        {
            if (Report == null) return;
            text_arrest_time.IsDisabled = true;
            if (!String.IsNullOrWhiteSpace(Report.DOB))
                text_arrestee_dob.IsDisabled = true;
        }
    }
}