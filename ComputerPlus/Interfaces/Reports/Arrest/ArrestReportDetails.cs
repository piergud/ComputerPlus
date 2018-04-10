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
using ComputerPlus.Interfaces.Common;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    class ArrestReportDetails : DockBase
    {
        ArrestReport mReport;
        bool RebindNeeded = false;
        int LastKnownPartiesCount = 0;
        ArrestReport Report
        {
            get
            {
                if (mReport != null && LastKnownPartiesCount != mReport.AdditionalParties.Count) RebindNeeded = true;
                return mReport;
            }
            set
            {
                if (value != mReport)
                {
                    if (LastKnownPartiesCount != value.AdditionalParties.Count) RebindNeeded = true;
                    LastKnownPartiesCount = value.AdditionalParties.Count;
                    RebindNeeded = true;
                    mReport = value;
                }
            }
        }
        Button transferTextFromSimpleNotepad;
        StateControlledMultilineTextbox reportDetailsTextBox;
        ListBox lb_allParties;
        TabButton AllButton;
        TabButton VictimButton;
        TabButton WitnessButton;

        public ArrestReportDetails(Base parent) : base(parent)
        {
            //Top
            this.TopDock.Height = 75;
            Base instructionsContainer = new Base(this.TopDock);
            instructionsContainer.Dock = Pos.Fill;
            RichLabel instructions = new RichLabel(instructionsContainer);
            instructions.AddText("Please enter in the details for the arrest report below.", System.Drawing.Color.Black);
            instructions.AddLineBreak();
            instructions.AddText("Toggle the Party button. Double click a party in the right pane to quickly insert their name into the report", System.Drawing.Color.Black);
            instructions.SetSize(600, this.TopDock.Height);
            instructions.Position(Pos.Top, 25, 15);
            //this.TopDock.FitChildrenToSize();
           
            //Center/Fill
            
            reportDetailsTextBox = new StateControlledMultilineTextbox(this);            
            reportDetailsTextBox.Dock = Pos.Fill;
            reportDetailsTextBox.TextChanged += ReportDetailsTextChanged;
            reportDetailsTextBox.ForceWordWrap = false;

            //Bottom
            this.BottomDock.Height = 60;
            Base actionButtonContainer = new Base(this.BottomDock);
            actionButtonContainer.Dock = Pos.Fill;

            transferTextFromSimpleNotepad = new Button(actionButtonContainer);            
            transferTextFromSimpleNotepad.SetToolTipText("Transfer from SimpleNotepad");
            transferTextFromSimpleNotepad.CopyContentIcon();
            transferTextFromSimpleNotepad.Position(Pos.Top, 25, 15);
            transferTextFromSimpleNotepad.Clicked += ActionButtonClicked;

            //Right
            this.RightDock.Width = 200;
            lb_allParties = new ListBox(this);
            lb_allParties.IsTabable = true;
            lb_allParties.RowSelected += PartyListItemClicked;

            AllButton = this.RightDock.TabControl.AddPage("All", lb_allParties);
            AllButton.UserData = ArrestReportAdditionalParty.PartyTypes.UNKNOWN;
            WitnessButton = this.RightDock.TabControl.AddPage("Witnesses", lb_allParties);
            WitnessButton.UserData = ArrestReportAdditionalParty.PartyTypes.WITNESS;
            VictimButton = this.RightDock.TabControl.AddPage("Victims", lb_allParties);
            VictimButton.UserData = ArrestReportAdditionalParty.PartyTypes.VICTIM;

            //var accomplices = this.RightDock.TabControl.AddPage("Accomplices", lb_allParties);
            //accomplices.UserData = ArrestReportAdditionalParty.PartyTypes.ACCOMPLICE;
            //all.Press(); //Must trigger before we add the rest of the pressed handlers
            AllButton.Pressed += FilteredPartiesButtonPressed;
            WitnessButton.Pressed += FilteredPartiesButtonPressed;
            VictimButton.Pressed += FilteredPartiesButtonPressed;
            //accomplices.Pressed += FilteredPartiesButtonPressed;

        }

        private void FilteredPartiesButtonPressed(Base sender, EventArgs arguments)
        {
            ((Button)sender).Press(sender); //hack to update the list... without it the list would only update if you manually clicked twice
            UpdatePedsInListBox();
        }

        private void ActionButtonClicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender == transferTextFromSimpleNotepad)
            {
                reportDetailsTextBox.InsertAtCursor(Function.SimpleNotepadCut(), true);
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
            if (Report == null) return;
            
            var filter = (ArrestReportAdditionalParty.PartyTypes)this.RightDock.TabControl.CurrentButton.UserData;
            var parties = filter == ArrestReportAdditionalParty.PartyTypes.UNKNOWN ? Report.AdditionalParties.ToArray() : Report.AdditionalParties.Where(x => x.PartyType == filter).ToArray();
            if (filter == ArrestReportAdditionalParty.PartyTypes.UNKNOWN)
            {
                //Add the Arrestee to the "All" list
                if (!String.IsNullOrWhiteSpace(Report.FullName))
                    lb_allParties.AddRow(Report.FullName, Report.Id(), Report);
            }
            foreach (var party in parties)
                lb_allParties.AddRow(party.FullName, party.Id(), party);
        }

        private void PartyListItemClicked(Base sender, ItemSelectedEventArgs arguments)
        {
            
            if (lb_allParties.SelectedRow.UserData is ArrestReportAdditionalParty)
            {
                var party = lb_allParties.SelectedRow.UserData as ArrestReportAdditionalParty;
                if (party != null)
                {
                    reportDetailsTextBox.InsertAtCursor(party.FullName);
                    //reportDetailsTextBox.AppendText(party.FullName, false);
                }
            }
            else if (lb_allParties.SelectedRow.UserData is ArrestReport)
            {
                var party = lb_allParties.SelectedRow.UserData as ArrestReport;
                if (party != null)
                {
                    reportDetailsTextBox.InsertAtCursor(party.FullName);
                    // reportDetailsTextBox.AppendText(party.FullName, false);
                }
            }
            reportDetailsTextBox.Focus();
            lb_allParties.SelectedRow = null;
            
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
            else
                UpdatePedsInListBox();
        }
        private bool DidQuickSwitchHack = false;
        protected override void PostLayout(GwenSkin.Base skin)
        {
            base.PostLayout(skin);
            if (this.IsVisible && !DidQuickSwitchHack)
            {
                WitnessButton.Press();
                AllButton.Press();
               // UpdatePedsInListBox();
                DidQuickSwitchHack = true;
            }
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
