using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Rage.Forms;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Interfaces.Common;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    internal class ArrestReportChargeDetails : GwenForm
    {
        TreeControl chargesTree;
        ListBox lb_charges;
        LabeledComponent<StateControlledMultilineTextbox> tb_notes;
        Button btnAddCharge;
        Button btnRemoveSelectedCharge;
        Label lbl_addedCharges;
        Label lbl_availableCharges;
        Charge SelectedAvailableCharge = null;
        ArrestReport Report;
        internal ArrestReportChargeDetails(ArrestReport arrestReport) : base(typeof(ArrestReportChargeDetailsTemplate))
        {
            Report = arrestReport;
        }
        public override void InitializeLayout()
        {
            base.InitializeLayout();
            if (Report.IsNew)
            {
                ShowLabels();
                ShowButtons();
                
                chargesTree = new TreeControl(this);
                chargesTree.SelectionChanged += TreeNodeSelected;
                chargesTree.SetSize(314, 307);
                chargesTree.SetPosition(12, 72);
                chargesTree.Dock = Gwen.Pos.Left;
                chargesTree.Margin = new Gwen.Margin(0, 30, 0, 0);
                lb_charges.Dock = Gwen.Pos.Right;
                lb_charges.Margin = new Gwen.Margin(0, 30, 0, 100);

                tb_notes = new LabeledComponent<StateControlledMultilineTextbox>(this, "Notes for Charge", new StateControlledMultilineTextbox(this), RelationalPosition.TOP, RelationalSize.NONE, Configs.BaseFormControlSpacingHalf, lbl_addedCharges.Font, lbl_addedCharges.TextColor);
                tb_notes.Component.ForceWordWrap = false;
                tb_notes.Component.SetSize((lb_charges.X - chargesTree.Right), 250);
                tb_notes.Component.AlignTopWith(lb_charges);
                tb_notes.PlaceRightOf(chargesTree, 5).AlignTopWith(lbl_addedCharges);
                tb_notes.Component.TextChanged += OnTextChanged;


                lbl_addedCharges.SetPosition((lb_charges.X + 30), lbl_addedCharges.Y);
                btnAddCharge.AddIcon();
                btnAddCharge.Disable();
                btnRemoveSelectedCharge.Disable();
                PopulateChargesTree(Globals.ChargeDefinitions);
                btnAddCharge.Clicked += ButtonClicked;
                btnRemoveSelectedCharge.Clicked += ButtonClicked;
                btnRemoveSelectedCharge.DeleteIcon();
                btnRemoveSelectedCharge.SetPosition(lb_charges.X + btnRemoveSelectedCharge.Width /  2 + 10, (lb_charges.Y + lb_charges.Height) + btnRemoveSelectedCharge.Height / 2  + 50);
            }
            else
            {
                HideButtons();
                HideLabels();
                AddReportCharges();
                lb_charges.Dock = Gwen.Pos.Right;
                if (Report.Charges.Count > 0)
                {
                    lb_charges.SelectRow(0, true);
                    SetNotesByCharge(Report.Charges[0]);
                }
            }
            
            LockControls();

        }

        private void TreeNodeSelected(Base sender, EventArgs arguments)
        {
            if (chargesTree.SelectedChildren.All(x => x.UserData is Charge && !((Charge)x.UserData).IsContainer)) {
                ToggleComponentState(ComponentSender.AVAILABLE_CHARGE);
            }
            else
            {
                SelectedAvailableCharge = null;
                btnAddCharge.Disable();
            }
        }
        private void OnTextChanged(Base sender, EventArgs arguments)
        {
            if (sender == tb_notes && lb_charges.SelectedRow != null)
            {
                ((ArrestChargeLineItem)lb_charges.SelectedRow.UserData).Note = tb_notes.Component.Text;
            }
        }

        internal void HideLabels()
        {
            var labels = new Label[] { lbl_addedCharges, lbl_availableCharges };
            foreach(var label in labels)
                label.Hide();
        }

        internal void ShowLabels()
        {
            var labels = new Label[] { lbl_addedCharges, lbl_availableCharges };
            foreach (var label in labels)
                label.Show();
        }

        internal void HideButtons()
        {
            var buttons = new Button[] { btnAddCharge, btnRemoveSelectedCharge };
            foreach (var button in buttons)
                button.Hide();
        }

        internal void ShowButtons()
        {
            var buttons = new Button[] { btnAddCharge, btnRemoveSelectedCharge };
            foreach (var button in buttons)
                button.Show();
        }

        internal void LockControls()
        {
            if (!Report.IsNew)
            {
                btnAddCharge.IsDisabled = true;
                btnRemoveSelectedCharge.IsDisabled = true;

            }
        }
     

        internal void ChangeReport(ArrestReport report)
        {
            Report = report;
            if (this.Exists())
            {
                lb_charges.UnselectAll();
                lb_charges.Clear();
                chargesTree.UnselectAll();
                tb_notes.Component.SetText(String.Empty);
                AddReportCharges();
            }
        }

        private void AddReportCharges()
        {
            var charges = Report.Charges.ToArray();
            foreach(var charge in charges)
                AddChargeToReportListbox(charge);
        }
        enum ComponentSender { AVAILABLE_CHARGE, ADDED_CHARGE };
        private void ToggleComponentState(ComponentSender t)
        {
            if (t == ComponentSender.ADDED_CHARGE)
            {
                //if (chargesTree.IsSelected)
                chargesTree.UnselectAll();
                btnAddCharge.Disable();
                btnRemoveSelectedCharge.Enable();
            }
            else
            {
                if (lb_charges.SelectedRow != null)
                {
                    lb_charges.UnselectAll();
                    tb_notes.Component.SetText(String.Empty);
                }
                btnAddCharge.Enable();
                btnRemoveSelectedCharge.Disable();
            }
        }


        private void ChargeRowSelected(Base sender, ClickedEventArgs arguments)
        {
            if (sender == null)
            {
                tb_notes.Component.SetText(String.Empty);
            }            
            else
            {
                var charge = sender.UserData as ArrestChargeLineItem;
                SetNotesByCharge(charge);
                ToggleComponentState(ComponentSender.ADDED_CHARGE);
            }
                
        }

        private void SetNotesByCharge(ArrestChargeLineItem item)
        {
            if (item != null)
                tb_notes.Component.Text = item.Note;
        }

        private void TransverseCharges(TreeNode parent, Charge charge, string categoryName)
        {
            if (charge.IsContainer)
            {
                var container = parent.AddNode(charge.Name);
                container.IsSelectable = false;
                charge.Children.ForEach(childCharge =>
                TransverseCharges(container, childCharge, categoryName));
            }
            else {
                if (categoryName.Equals("Traffic")) charge.IsTraffic = true;
                var child = parent.AddNode(String.Format("{0}{1}", charge.Name, charge.IsFelony ? " (F)" : String.Empty, charge));
                child.UserData = charge;
                child.LabelPressed += ChargeTreeItemSelected;
            }
        }

        private void PopulateChargesTree(ChargeCategories categories)
        {
            if (categories == null) return;
            categories.Categories.ForEach(x =>
            {
                var parent = chargesTree.AddNode(x.Name);
                var categoryName = x.Name;
                x.Charges.ForEach(charge => TransverseCharges(parent, charge, categoryName));
            });
        }

        private void ChargeTreeItemSelected(Base sender, EventArgs arguments)
        {
            if (sender == null)
            {
                SelectedAvailableCharge = null;
                chargesTree.IsSelected = false;
            }
            else if (sender.UserData is Charge)
            {
                var selectedCharge = sender.UserData as Charge;
                if (selectedCharge.IsContainer) return;
                SelectedAvailableCharge = selectedCharge;
                
            }

        }


        private void AddChargeToReport(ArrestReport report, ArrestChargeLineItem lineItem)
        {
            if (report == null || lineItem == null) return;
            report.Charges.Add(lineItem);
            AddChargeToReportListbox(lineItem);
        }
        private void AddChargeToReportListbox(ArrestChargeLineItem lineItem)
        {
            lb_charges.AddRow(
                    String.Format("({0}) {1}", lineItem.IsFelony ? "F" : "M", lineItem.Charge)
                    , lineItem.Charge,
                    lineItem
              ).Clicked += ChargeRowSelected;
        }      

        private void RemoveChargeFromReport(ArrestReport report, ArrestChargeLineItem lineItem)
        {
            if (report == null || lineItem == null) return;
            report.Charges.Remove(lb_charges.SelectedRow.UserData as ArrestChargeLineItem);
            lb_charges.RemoveRow(lb_charges.SelectedRowIndex);
        }

        private void ButtonClicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender == btnAddCharge)
            {
                if (SelectedAvailableCharge == null || SelectedAvailableCharge.IsContainer) return;
                var lineItem = new ArrestChargeLineItem(SelectedAvailableCharge, tb_notes.Component.Text);
                AddChargeToReport(Report, lineItem);
                
                tb_notes.Component.ClearText();
                SelectedAvailableCharge = null;
            }
            else if (sender == btnRemoveSelectedCharge)
            {
                if (lb_charges.SelectedRow == null) return;
                RemoveChargeFromReport(Report, lb_charges.SelectedRow.UserData as ArrestChargeLineItem);
                tb_notes.Component.CursorPosition = new System.Drawing.Point(0, 0);
                tb_notes.Component.ClearText();
                SelectedAvailableCharge = null;
            }
        }    
    }
}
