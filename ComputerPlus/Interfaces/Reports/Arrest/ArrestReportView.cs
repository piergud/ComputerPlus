using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Gwen.Control.Layout;
using Rage.Forms;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    class ArrestReportView : GwenForm
    {
        ArrestReport Report;
        Label value_arrest_report_id;
        Label value_first_name;
        Label value_last_name;
        Label value_dob;
        Label value_home_address;
        Label value_arrest_street_address;
        Label value_arrest_city;
        Label value_arrest_time;
        Label value_arrest_date;
        Label lbl_arrest_report_id;
        Label lbl_first_name;
        Label lbl_last_name;
        Label lbl_dob;
        Label lbl_home_address;
        Label lbl_arrest_street_address;
        Label lbl_arrest_city;
        Label lbl_arrest_time;
        Label lbl_arrest_date;

        ListBox lb_charges;
        ListBox lb_additional_parties;
        MultilineTextBox tb_report_details;

        public ArrestReportView(ArrestReport report) : base(typeof(ArrestReportViewTemplate))
        {
            Report = report;
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            StyleLabels();
            BindDataFromReport();
            
        }

        private void StyleLabels()
        {
            var labelFont = lbl_arrest_city.Skin.DefaultFont.Copy();
            labelFont.FaceName = "Times New Roman Bold";
            Label[] labels = new Label[]
            {
                lbl_first_name,
                lbl_last_name,
                lbl_dob,
                lbl_home_address,
                lbl_arrest_street_address,
                lbl_arrest_city,
                lbl_arrest_time,
                lbl_arrest_date
            };
            foreach(var label in labels)
            {
                label.MakeColorBright();
                label.TextColorOverride = System.Drawing.Color.Gray;
            }
        }

        private void BindDataFromReport()
        {
            if (Report == null) return;
            AddChargesFromReport();
            AddAdditionalPartiesFromReport();
            value_arrest_report_id.SetText(Report.Id().Substring(30));
            value_first_name.SetText(Report.FirstName);
            value_last_name.SetText(Report.LastName);            
            value_dob.LocalDateTime(Report.DOB, TextBoxExtensions.DateOutputPart.DATE);
            value_home_address.SetText(Report.HomeAddress);
            value_arrest_street_address.SetText(Report.ArrestStreetAddress);
            value_arrest_city.SetText(Report.ArrestCity);
            value_arrest_date.LocalDateTime(Report.ArrestTimeDate, TextBoxExtensions.DateOutputPart.DATE);
            value_arrest_time.LocalDateTime(Report.ArrestTimeDate, TextBoxExtensions.DateOutputPart.TIME);
            tb_report_details.SetText(Report.Details);
        }

        private void AddChargesFromReport()
        {
            Report.Charges.ForEach(AddChargeToListBox);
        }

        private void AddAdditionalPartiesFromReport()
        {
            Report.AdditionalParties.ForEach(AddAdditionalPartyToListBox);
        }

        private void AddChargeToListBox(ArrestChargeLineItem charge)
        {
            if (charge == null || lb_charges == null) return;
            var row = lb_charges.AddRow(
                String.Format("{0}. {1}", lb_charges.RowCount + 1, charge.Charge),
                charge.id.ToString(),
                charge
             );
            if (!String.IsNullOrWhiteSpace(charge.Note))
            {
                row.SetToolTipText(charge.Note);
                row.SetCellText(0, "* " + row.Text);
            }
            if (charge.IsFelony)
            {
                row.SetCellText(0, row.Text + " (Felony)");
            }

        }

        private void AddAdditionalPartyToListBox(ArrestReportAdditionalParty party)
        {
            if (party == null || lb_additional_parties == null) return;
            var row = lb_additional_parties.AddRow(
                String.Format("({0}) {1}", party.PartyType.ToString().Substring(0, 1), party.FullName),
                party.Id(),
                party
             );

        }
    }
}
