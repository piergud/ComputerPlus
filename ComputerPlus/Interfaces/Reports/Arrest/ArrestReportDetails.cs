using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using GwenSkin = Gwen.Skin;
using Gwen;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    class ArrestReportDetails : DockBase
    {
        ArrestReport mReport;
        bool RebindNeeded = false;
        ArrestReport Report
        {
            get
            {
                return mReport;
            }
            set
            {
                if (value != mReport)
                {
                    RebindNeeded = true;
                    mReport = value;
                }
            }
        }
        Button transferTextFromSimpleNotepad;
        MultilineTextBox reportDetailsTextBox;
        ListBox lb_allParties;

        public ArrestReportDetails(Base parent) : base(parent)
        {

            //Top
            this.TopDock.Height = 60;
            Base instructionsContainer = new Base(this.TopDock);
            instructionsContainer.Dock = Pos.Fill;
            RichLabel instructions = new RichLabel(instructionsContainer);
            instructions.AddText("Please enter in the details for the arrest report below.", System.Drawing.Color.Black);
            instructions.AddLineBreak();
            instructions.AddText("Double click a party in the right pane to quickly insert their name into the report", System.Drawing.Color.Black);
            instructions.SetSize(600, this.TopDock.Height);
            instructions.Position(Pos.Top, 25, 15);
            //this.TopDock.FitChildrenToSize();
           
            //Center/Fill
            
            reportDetailsTextBox = new MultilineTextBox(this);            
            reportDetailsTextBox.Dock = Pos.Fill;
            reportDetailsTextBox.TextChanged += ReportDetailsTextChanged;

            //Bottom
            this.BottomDock.Height = 60;
            Base actionButtonContainer = new Base(this.BottomDock);
            actionButtonContainer.Dock = Pos.Fill;

            transferTextFromSimpleNotepad = new Button(actionButtonContainer);            
            transferTextFromSimpleNotepad.SetToolTipText("Transfer from SimpleNotepad");
            transferTextFromSimpleNotepad.CopyContentIcon();
            transferTextFromSimpleNotepad.Position(Pos.Top, 15, 15);
            transferTextFromSimpleNotepad.Clicked += ActionButtonClicked;


            //Right
            this.RightDock.Width = 200;
            lb_allParties = new ListBox(this.RightDock);
            this.RightDock.TabControl.AddPage("All", lb_allParties).UserData = ArrestReportAdditionalParty.PartyTypes.UNKNOWN;
            this.RightDock.TabControl.AddPage("Witnesses", lb_allParties).UserData = ArrestReportAdditionalParty.PartyTypes.WITNESS;
            //this.RightDock.FitChildrenToSize();
        }

        private void ActionButtonClicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender == transferTextFromSimpleNotepad)
            {
                reportDetailsTextBox.AppendText(Function.SimpleNotepadCut());
            }
        }

        internal void ChangeReport(ArrestReport report)
        {
            Report = report;
            BindDataFromReport();            
        }

        private void UpdatePedsInListBox()
        {
            lb_allParties.Clear();
            Function.Log("UpdatePedsInListBox called");
            if (Report == null) return;            
            var filter = (ArrestReportAdditionalParty.PartyTypes)this.RightDock.TabControl.CurrentButton.UserData;
            Function.Log("UpdatePedsInListBox filter created");
            var parties = filter == ArrestReportAdditionalParty.PartyTypes.UNKNOWN ? Report.AdditionalParties.ToArray() : Report.AdditionalParties.Where(x => x.PartyType == filter).ToArray();
            Function.Log("UpdatePedsInListBox filtered list");
            foreach (var party in parties)
                lb_allParties.AddRow(party.FullName, party.Id(), party).DoubleClicked += PartyListItemDoubleClicked; ;
            Function.Log("UpdatePedsInListBox done");
        }

        private void PartyListItemDoubleClicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender == lb_allParties)
            {
                var party = lb_allParties.SelectedRow.UserData as ArrestReportAdditionalParty;
                if (party != null)
                {
                    reportDetailsTextBox.AppendText(party.FullName, false);
                }
            }
        }

        private void BindDataFromReport()
        {
            if (Report == null) return;
            reportDetailsTextBox.Text = Report.Details;
            UpdatePedsInListBox();
            RebindNeeded = false;
        }

        private void CheckButtonState()
        {
            if (Report == null) return;
            var isDisabled = transferTextFromSimpleNotepad.IsDisabled;
            var notepadEmpty = String.IsNullOrWhiteSpace(Globals.SimpleNotepadText);
            if (notepadEmpty && !transferTextFromSimpleNotepad.IsDisabled)
            {
                transferTextFromSimpleNotepad.Disable();
            }
            else if (!notepadEmpty && transferTextFromSimpleNotepad.IsDisabled)
            {
                transferTextFromSimpleNotepad.Enable();
            }
        }

        protected override void Layout(GwenSkin.Base skin)
        {
            base.Layout(skin);
            if (RebindNeeded)
                BindDataFromReport();
        }

        protected override void PostLayout(GwenSkin.Base skin)
        {
            base.PostLayout(skin);
            //Function.Log("Post layout called");
        }

        protected override void Render(GwenSkin.Base skin)
        {
            base.Render(skin);
            CheckButtonState();
        }

        private void ReportDetailsTextChanged(Base sender, EventArgs arguments)
        {
            if (Report != null && sender is MultilineTextBox)
            {
                Report.Details = ((MultilineTextBox)sender).Text;
            }
        }
    }
}
