using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Rage.Forms;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    internal class ArrestReportChargeDetails : GwenForm
    {
        TreeControl chargesTree;
        ListBox lb_charges;
        MultilineTextBox tb_notes;
        Button btnAddCharge;
        Button btnRemoveSelectedCharge;
        Label lbl_addedCharges;
        Label lbl_availableCharges;
        Label lbl_notes;
        bool AddChargeEnabled = false;
        Charge SelectedCharge = null;
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
                chargesTree.SetSize(314, 307);
                chargesTree.SetPosition(12, 72);
                chargesTree.Dock = Gwen.Pos.Left;
                chargesTree.Margin = new Gwen.Margin(0, 30, 0, 0);
                lb_charges.Dock = Gwen.Pos.Right;
                lb_charges.Margin = new Gwen.Margin(0, 30, 0, 100);
                tb_notes.SetPosition(chargesTree.X + chargesTree.Width + 5, lb_charges.Y - 10);
                tb_notes.SetSize((lb_charges.X - (chargesTree.X + chargesTree.Width)), tb_notes.Height);
                lbl_notes.SetPosition(tb_notes.X, lbl_notes.Y);
                lbl_notes.SetSize(tb_notes.Width, lbl_notes.Height);

                lbl_addedCharges.SetPosition((lb_charges.X + 30), lbl_addedCharges.Y);

                btnAddCharge.SetPosition(tb_notes.X, (tb_notes.Y + tb_notes.Height + 5));
                btnAddCharge.SetSize(tb_notes.Width, btnAddCharge.Height);
                PopulateChargesTree(Globals.ChargeCategoryList);
                btnAddCharge.Clicked += ChangeChargesClicked;
                btnRemoveSelectedCharge.Clicked += ChangeChargesClicked;
                btnRemoveSelectedCharge.DeleteIcon();
                btnRemoveSelectedCharge.SetPosition(lb_charges.X + btnRemoveSelectedCharge.Width /  2 + 10, (lb_charges.Y + lb_charges.Height) + btnRemoveSelectedCharge.Height / 2  + 50);
            }
            else
            {
                HideButtons();
                HideLabels();
                AddReportCharges();
                tb_notes.Dock = Gwen.Pos.Fill;
                lb_charges.Dock = Gwen.Pos.Right;
                if (Report.Charges.Count > 0)
                {
                    lb_charges.SelectRow(0, true);
                    SetNotesByCharge(Report.Charges[0]);
                }
            }
            
            lb_charges.RowSelected += ChargeRowSelected;
            LockControls();

        }

        internal void HideLabels()
        {
            var labels = new Label[] { lbl_addedCharges, lbl_availableCharges, lbl_notes };
            foreach(var label in labels)
                label.Hide();
        }

        internal void ShowLabels()
        {
            var labels = new Label[] { lbl_addedCharges, lbl_availableCharges, lbl_notes };
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
                lb_charges.Clear();
                Function.Log(String.Format("ChangeReport adding charge {0}", Report.Charges.Count));
                AddReportCharges();
            }
        }

        private void AddReportCharges()
        {
            var charges = Report.Charges.ToArray();
            foreach(var charge in charges)
                AddChargeToReportListbox(charge);
        }


        private void ChargeRowSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            if (sender == null)
            {
                tb_notes.Text = String.Empty;                
            }            
            else
            {
                var charge = arguments.SelectedItem.UserData as ArrestChargeLineItem;
                SetNotesByCharge(charge);
            }
                
        }

        private void SetNotesByCharge(ArrestChargeLineItem item)
        {
            if (item != null)
                tb_notes.Text = item.Note;
        }

        private void TransverseCharges(TreeNode parent, Charge charge)
        {
            if (charge.IsContainer)
            {
                var container = parent.AddNode(charge.Name);
                charge.Children.ForEach(childCharge =>
                TransverseCharges(container, childCharge));
            }
            else {
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
                x.Charges.ForEach(charge => TransverseCharges(parent, charge));
            });
        }

        private void ChargeTreeItemSelected(Base sender, EventArgs arguments)
        {
            if (sender == null)
            {
                SelectedCharge = null;
                chargesTree.IsSelected = false;
            }
            else if (sender.UserData is Charge)
            {
                SelectedCharge = sender.UserData as Charge;
                lb_charges.SelectedRow = null;
                AddChargeEnabled = true;
                UpdateButtonState();
            }

        }

        private void UpdateButtonState()
        {
            btnAddCharge.IsDisabled = !AddChargeEnabled;
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
              );
        }
        private void RemoveChargeFromReport(ArrestReport report, ArrestChargeLineItem lineItem)
        {
            if (report == null || lineItem == null) return;
            report.Charges.Remove(lb_charges.SelectedRow.UserData as ArrestChargeLineItem);
            lb_charges.RemoveRow(lb_charges.SelectedRowIndex);
        }

        private void ChangeChargesClicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender == btnAddCharge)
            {
                if (SelectedCharge == null || !AddChargeEnabled) return;
                var lineItem = new ArrestChargeLineItem(SelectedCharge, tb_notes.Text);
                AddChargeToReport(Report, lineItem);
                tb_notes.Text = String.Empty;
                SelectedCharge = null;
                AddChargeEnabled = false;
            }
            else if (sender == btnRemoveSelectedCharge)
            {
                if (lb_charges.SelectedRow == null) return;
                RemoveChargeFromReport(Report, lb_charges.SelectedRow.UserData as ArrestChargeLineItem);
                tb_notes.Text = String.Empty;
                SelectedCharge = null;
                AddChargeEnabled = false;               
            }
            UpdateButtonState();
        }    
    }
}
