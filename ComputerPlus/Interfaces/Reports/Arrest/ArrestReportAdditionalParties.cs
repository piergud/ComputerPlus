using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage.Forms;
using Gwen.Control;
using ComputerPlus.Interfaces.Common;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    class ArrestReportAdditionalParties : GwenForm
    {
        Button btn_addPartyToReport;
        ComboBox cb_type;
        StateControlledTextbox tb_firstName;
        StateControlledTextbox tb_lastName;
        StateControlledTextbox tb_dob;
        ListBox lb_additional_parties;

        Label lbl_first_name;
        Label lbl_last_name;
        Label lbl_dob;
        Label lbl_party_type;

        Label[] labels;
        StateControlledTextbox[] textBoxes;

        ArrestReport Report;
        ArrestReportAdditionalParty SelectedAdditionalParty
        {
            get
            {
                if (lb_additional_parties == null) return null;
                return lb_additional_parties.SelectedRow.UserData as ArrestReportAdditionalParty;
            }
        }

        internal ArrestReportAdditionalParties(ArrestReport report) : base(typeof(ArrestReportAdditionalPartiesTemplate))
        {
            Report = report;
        }


        public override void InitializeLayout()
        {
            base.InitializeLayout();
            PopulatePartyTypeComboBox();
            tb_firstName = new StateControlledTextbox(this);
            tb_lastName = new StateControlledTextbox(this);
            tb_dob = new StateControlledTextbox(this);
            lb_additional_parties.Dock = Gwen.Pos.Right;

            tb_firstName.SetPosition(111, 94);
            tb_firstName.SetSize(166, 20);

            tb_lastName.SetPosition(111, 129);
            tb_lastName.SetSize(166, 20);

            tb_dob.SetPosition(111, 168);
            tb_dob.SetSize(100, 20);

            labels = new Label[] { lbl_dob, lbl_first_name, lbl_last_name, lbl_party_type };
            textBoxes = new StateControlledTextbox[] { tb_dob, tb_firstName, tb_lastName };

            //lbl_error.SetPosition(94, 223);
            //lbl_error.SetSize((lb_additional_parties.X - (btn_addPartyToReport.X + btn_addPartyToReport.Width)) / 2, (this.Window.Height - (btn_addPartyToReport.Y + btn_addPartyToReport.Height)) / 2);

            btn_addPartyToReport.Clicked += AddPartyToReportClicked;
            lb_additional_parties.Clicked += AdditionalPartyListItemClicked;
            PopulateListBoxFromReport();

        }

        private void AdditionalPartyListItemClicked(Base sender, ClickedEventArgs arguments)
        {
            PopulateFieldsFromParty(SelectedAdditionalParty);
        }

        public void ChangeReport(ArrestReport report)
        {            
            this.Report = report;
            if (report == null)  return;            
            if (this.Exists())
            {
                PopulateListBoxFromReport();                
            }
        }

        private void PopulateListBoxFromReport()
        {
            if (Report == null || Report.AdditionalParties == null)
            {
                return;
            }
            else if (lb_additional_parties == null)
            {
                Function.Log("Cannot populate lb that is null");
            }
            else
            {
                lb_additional_parties.Clear();
                var parties = Report.AdditionalParties.ToArray();
                if (parties.Length > 0)
                {
                    AddAdditionalPartyToListBox(parties);
                    lb_additional_parties.SelectRow(0, true);
                    PopulateFieldsFromParty(parties[0]);
                }
            }
        }

        private void AddAdditionalPartyToListBox(ArrestReportAdditionalParty[] parties)
        {            
            foreach(var party in parties)
            {
                AddAdditionalPartyToListBox(party);
            }
        }

        private void AddAdditionalPartyToListBox(ArrestReportAdditionalParty party)
        {
            lb_additional_parties.AddRow(party.FullName, party.FullName, party);
        }

        private void PopulateFieldsFromParty(ArrestReportAdditionalParty additionalParty)
        {
            if (additionalParty == null) return;
            tb_dob.Text = additionalParty.DOB;
            tb_firstName.Text = additionalParty.FirstName;
            tb_lastName.Text = additionalParty.LastName;
            cb_type.SelectByUserData(additionalParty.PartyType);
            LockControls();
        }

        private void ClearErrorState()
        {
            foreach (var label in labels) label.ClearError();
            foreach (var textbox in textBoxes) textbox.ClearError();
        }

        private ArrestReportAdditionalParty AddPartyToReport()
        {
            try {
                
                var party = new ArrestReportAdditionalParty(Report) {
                    DOB = tb_dob.Text.Trim(),
                    FirstName = tb_firstName.Text.Trim(),
                    LastName = tb_lastName.Text.Trim(),
                    PartyType = (ArrestReportAdditionalParty.PartyTypes)cb_type.SelectedItem.UserData
                };
                Dictionary<String, String> failReason;
                ClearErrorState();
                if (!party.Validate(out failReason))
                {                     
                    foreach(var kvp in failReason)
                    {
                        if (!String.IsNullOrEmpty(kvp.Key))
                        {
                            switch (kvp.Key)
                            {
                                case "First Name":
                                    tb_firstName.Error(kvp.Value);
                                    lbl_first_name.Error(kvp.Value);
                                    break;
                                case "Last Name":
                                    tb_lastName.Error(kvp.Value);
                                    lbl_last_name.Error(kvp.Value);
                                    break;
                                case "DOB":
                                    tb_dob.Error(kvp.Value);
                                    lbl_dob.Error(kvp.Value);
                                    break;
                                case "Type":
                                    lbl_party_type.Error(kvp.Value);
                                    break;
                            }
                        }
                    }
                    return null;
                }
                else
                {                                        
                    Report.AdditionalParties.Add(party);
                    return party;
                }
            }
            catch(Exception e)
            {
                Function.Log(e.ToString());
                //lbl_error.Error("Could not save.. check console or logs");
                return null;
            }

        }

        private void ResetForm()
        {
            tb_dob.Text = String.Empty;
            tb_firstName.Text = String.Empty;
            tb_lastName.Text = String.Empty;
            cb_type.SelectByUserData(ArrestReportAdditionalParty.PartyTypes.UNKNOWN);
            LockControls(false);
        }

        private void LockControls(bool disable = true)
        {
            if (Report == null) return;
            if (!Report.IsNew && disable)
            {
                tb_dob.Disable();
                tb_firstName.Disable();
                tb_lastName.Disable();
                cb_type.Disable();
                btn_addPartyToReport.Disable();
                btn_addPartyToReport.Hide();
            }
            else
            {
                tb_dob.Enable();
                tb_firstName.Enable();
                tb_lastName.Enable();
                cb_type.Enable();
            }
        }

        private void AddPartyToReportClicked(Base sender, ClickedEventArgs arguments)
        {
            var party = AddPartyToReport();
            if (party != null)
            {
                AddAdditionalPartyToListBox(party);
                ResetForm();
            }                
        }

        private void PopulatePartyTypeComboBox()
        {
            if (cb_type == null) return;
            cb_type.AddItem("Select One", "Select One", ArrestReportAdditionalParty.PartyTypes.UNKNOWN);
            cb_type.AddItem("Accomplice", "Accomplice", ArrestReportAdditionalParty.PartyTypes.ACCOMPLICE);
            cb_type.AddItem("Victim", "Victim", ArrestReportAdditionalParty.PartyTypes.VICTIM);
            cb_type.AddItem("Witness", "Witness", ArrestReportAdditionalParty.PartyTypes.WITNESS);
        }
    }
}
