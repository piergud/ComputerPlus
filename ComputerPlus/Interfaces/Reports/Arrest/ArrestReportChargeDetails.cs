using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Rage.Forms;
using ComputerPlus.Interfaces.Reports.Models;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    internal class ArrestReportChargeDetails : GwenForm
    {
        TreeControl chargesTree;
        ListBox lb_charges;
        MultilineTextBox tb_notes;
        Button btnAddCharge;
        Button btnRemoveSelectedCharge;
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
            chargesTree = new TreeControl(this);
            btnAddCharge.Clicked += ChangeChargesClicked;
            btnRemoveSelectedCharge.Clicked += ChangeChargesClicked;
            PositionAndSizeComponents();
            PopulateChargesTree(Globals.ChargeCategoryList);
            lb_charges.RowSelected += ChargeRowSelected;

        }

        private void ChargeRowSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            var charge = arguments.SelectedItem.UserData as ArrestChargeLineItem;
            if (charge != null)
                tb_notes.Text = charge.Note;
        }

     

        internal void ChangeReport(ArrestReport report)
        {
            Report = report;
            lb_charges.Clear();
            Report.Charges.ForEach(x => AddChargeToListBox(x));
            //PopulateInputs(Report);
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
                var child = parent.AddNode(charge.Name);
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
            if (sender.UserData is Charge)
            {
                SelectedCharge = sender.UserData as Charge;
                AddChargeEnabled = true;
                UpdateButtonState();
            }

        }

        private void UpdateButtonState()
        {
            btnAddCharge.IsDisabled = !AddChargeEnabled;
        }

        private void AddChargeToListBox(ArrestChargeLineItem lineItem)
        {
            lb_charges.AddRow(
                    String.Format("({0}) {1}", lineItem.Charge.IsFelony ? "F" : "M", lineItem.Charge.Name)
                    , lineItem.Charge.Name,
                    lineItem
              );
        }

        private void ChangeChargesClicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender == btnAddCharge)
            {
                if (SelectedCharge == null || !AddChargeEnabled) return;
                var lineItem = new ArrestChargeLineItem(SelectedCharge, tb_notes.Text);
                AddChargeToListBox(lineItem);
                Report.Charges.Add(lineItem);
                SelectedCharge = null;
                AddChargeEnabled = false;
                UpdateButtonState();
            }
            else if (sender == btnRemoveSelectedCharge)
            {
                if (lb_charges.SelectedRow == null) return;
                Report.Charges.Remove(lb_charges.SelectedRow.UserData as ArrestChargeLineItem);
            }
        }

        private void PositionAndSizeComponents()
        {
            chargesTree.SetSize(314, 307);
            chargesTree.SetPosition(12, 72);
            //chargesTree.Dock = Gwen.Pos.Left;
            Function.Log(String.Format("Position {0} {1}", this.Position.X, this.Position.Y));
            Function.Log(String.Format("Size {0} {1}", this.Size.Height, this.Size.Width));
        }
    }
}
