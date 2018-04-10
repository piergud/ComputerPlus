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
using GwenSkin = Gwen.Skin;
using ComputerPlus.Interfaces.Common;
using Gwen;
using SystemDrawing = System.Drawing;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    class ArrestReportView : Base
    {
        internal static int DefaultWidth = 539;
        internal static int DefaultHeight = 800;

        ArrestReport Report;
        SystemDrawing.Color labelColor = SystemDrawing.Color.Black;

        Font labelFont, valueFont;

        ListBox lb_charges, lb_additional_parties;
        LabeledComponent<Label> tb_report_details;

        LabeledComponent<Label> labeled_arrest_report_id, labeled_first_name, labeled_last_name,
            labeled_dob, labeled_home_address, labeled_arrest_street_address,
            labeled_arrest_city, labeled_arrest_time, labeled_arrest_date;

        LabeledComponent<ListBox> labeledCharges, labeledAdditionalParties;

        Base headerSection, arrestInformationContent, arrestLocationContent;
        FormSection arresteeInformationSection, arrestLocationSection;

        bool BindNeeded;

        public ArrestReportView(Base parent, ArrestReport report) : base(parent)
        {
            Report = report;
            InitializeLayout();            
        }

        public void ChangeReport(ArrestReport report)
        {
            Report = report;
            BindNeeded = true;
            BindDataFromReport();
        }

        public void InitializeLayout()
        {
            labelFont = this.Skin.DefaultFont.Copy();
            labelFont.Size = 14;
            labelFont.Smooth = true;

            headerSection = new Base(this);
            labeled_arrest_report_id = LabeledComponent.Label(headerSection, "Arrest Report");

            arresteeInformationSection = new FormSection(this, "Arrestee Information");
            arrestInformationContent = (new Base(this) { });            

            labeled_first_name = LabeledComponent.Label(arrestInformationContent, "First Name", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_last_name = LabeledComponent.Label(arrestInformationContent, "Last Name", new Label(arrestInformationContent), RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_dob = LabeledComponent.Label(arrestInformationContent, "DOB", new Label(arrestInformationContent), RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_home_address = LabeledComponent.Label(arrestInformationContent, "Home Address", new Label(arrestInformationContent), RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);

            arrestLocationSection = new FormSection(this, "Arrest Location");
            arrestLocationContent = (new Base(this) { });

            labeled_arrest_street_address = LabeledComponent.Label(arrestLocationContent, "Street Address", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_arrest_city = LabeledComponent.Label(arrestLocationContent, "City", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_arrest_date = LabeledComponent.Label(arrestLocationContent, "Date", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_arrest_time = LabeledComponent.Label(arrestLocationContent, "Time", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);

            lb_charges = new ListBox(this);
            lb_charges.Height = 100;

            labeledCharges = new LabeledComponent<ListBox>(this, "Charges", lb_charges, RelationalPosition.TOP, RelationalSize.NONE, Configs.BaseFormControlSpacingHalf, labelFont, labelColor);

            lb_additional_parties = new ListBox(this);
            lb_additional_parties.Height = 100;

            labeledAdditionalParties = new LabeledComponent<ListBox>(this, "Additional Parties", lb_additional_parties, RelationalPosition.TOP, RelationalSize.NONE, Configs.BaseFormControlSpacingHalf, labelFont, labelColor);

            tb_report_details = LabeledComponent.Label(this, "Details", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            BindNeeded = true;
        }

        protected override void Layout(GwenSkin.Base skin)
        {
            base.Layout(skin);            
            if (this.Parent == null && headerSection == null)
            {
                return;
            }
            BindDataFromReport();
            headerSection.SizeWidthWith().AlignTopWith().AlignLeftWith().SizeToChildrenBlock();

            arresteeInformationSection
             .AddContentChild(arrestInformationContent)
             .PlaceBelowOf(headerSection)
             .SizeWidthWith();
            

            labeled_last_name.PlaceRightOf(labeled_first_name);
            labeled_dob.PlaceRightOf(labeled_last_name);
            labeled_home_address.PlaceBelowOf(labeled_first_name);

            arrestInformationContent.SizeToChildren(false, true);
            arresteeInformationSection.SizeToChildren(false, true);

            arrestLocationSection
             .AddContentChild(arrestLocationContent)
             .PlaceBelowOf(arresteeInformationSection)
             .SizeWidthWith();

            labeled_arrest_date.PlaceRightOf(labeled_arrest_street_address, Configs.BaseFormControlSpacingDouble);
            labeled_arrest_city.PlaceBelowOf(labeled_arrest_street_address, Configs.BaseFormControlSpacingDouble);
            labeled_arrest_time.Align(labeled_arrest_date, labeled_arrest_city);
            
            arrestLocationContent.SizeToChildren(false, true);
            arrestLocationSection.SizeToChildren(false, true);
            
            labeledCharges.PlaceBelowOf(arrestLocationSection).SizeWidthWith().SizeChildrenWidth();
            labeledAdditionalParties.PlaceBelowOf(labeledCharges).SizeWidthWith().SizeChildrenWidth();
            tb_report_details.PlaceBelowOf(labeledAdditionalParties).SizeFull().SizeChildrenWidth().SizeChildrenHeight();
            tb_report_details.SizeToChildrenBlock();
        }      

        private void BindDataFromReport()
        {
            if (Report == null) return;
            if (!BindNeeded) return;
            BindNeeded = false;

            lock (Report)
            {
                AddChargesFromReport();
                AddAdditionalPartiesFromReport();

                labeled_arrest_report_id.SetValueText(Report.ShortId());
                labeled_first_name.SetValueText(Report.FirstName);
                labeled_last_name.SetValueText(Report.LastName);
                labeled_dob.SetValueText(Report.DOB);
                labeled_home_address.SetValueText(Report.HomeAddress);

                labeled_arrest_street_address.SetValueText(Report.ArrestStreetAddress);
                labeled_arrest_city.SetValueText(Report.ArrestCity);
                labeled_arrest_date.SetValueText(Function.ToLocalDateString(Report.ArrestTimeDate, TextBoxExtensions.DateOutputPart.DATE));
                labeled_arrest_time.SetValueText(Function.ToLocalDateString(Report.ArrestTimeDate, TextBoxExtensions.DateOutputPart.TIME));
                tb_report_details.SetValueText(Report.Details);
            }
        }

        private void AddChargesFromReport()
        {
            if (lb_charges == null) return;
            lb_charges.Clear();
            Report.Charges.ForEach(AddChargeToListBox);
        }

        private void AddAdditionalPartiesFromReport()
        {
            if (lb_additional_parties == null) return;
            lb_additional_parties.Clear();
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

    class ArrestReportViewContainer : GwenForm
    {
        ArrestReportView arrestReportView;
        ArrestReport Report;
        internal ArrestReportViewContainer(ArrestReport report) : base("Arrest Report", ArrestReportView.DefaultWidth, ArrestReportView.DefaultHeight)
        {
            Report = report;
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            arrestReportView = new ArrestReportView(this, Report);
            arrestReportView.Dock = Gwen.Pos.Fill;
        }

        public void ChangeReport(ArrestReport report)
        {
            Report = report;
            arrestReportView.ChangeReport(Report);
        }
    }
}
