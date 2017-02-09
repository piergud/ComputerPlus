using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage.Forms;
using Gwen;
using Gwen.Control;
using ComputerPlus.Interfaces.Common;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Controllers;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    internal interface IErrorNotifier
    {
        void SubscribeToErrorEvents(ArrestReportContainer.ArrestReportValidationError subscriber);
        void UnsubscribeToErrorEvents(ArrestReportContainer.ArrestReportValidationError subscriber);
    }

    public class ArrestReportContainer : GwenForm, IErrorNotifier
    {
        ArrestReport Report;
        TabbableContainer tabContainer;
        ArrestReportPedDetails pedDetailsPage = new ArrestReportPedDetails(Globals.PendingArrestReport);
        ArrestReportChargeDetails chargeDetailsPage = new ArrestReportChargeDetails(Globals.PendingArrestReport);
        ArrestReportAdditionalParties additionalPartiesPage = new ArrestReportAdditionalParties(Globals.PendingArrestReport);
        ArrestReportDetails arrestDetails;
        Base testPage;
        Button btn_save;
        Button btn_clear;

        public delegate void ArrestReportValidationError(object sender, Dictionary<String, String> errors);
        internal event ArrestReportValidationError OnArrestReportValidationError;


        internal ArrestReportContainer() : this(Globals.PendingArrestReport)
        {

        }

        internal ArrestReportContainer(ArrestReport arrestReport) : base(typeof(ArrestReportContainerTemplate)) {
            Report = arrestReport;            
            pedDetailsPage.SetErrorSubscription(this);            
            
        }

        private void UpdateReportForChildren()
        {
            pedDetailsPage.ChangeReport(Report);
            chargeDetailsPage.ChangeReport(Report);
            additionalPartiesPage.ChangeReport(Report);
            arrestDetails.ChangeReport(Report);
        }

        private async void SaveClicked(Base sender, ClickedEventArgs arguments)
        {
            try
            {
                Dictionary<String, String> validationErrors;
                if (Report.Validate(out validationErrors))
                {
                    await ComputerReportsController.SaveArrestRecordAsync(Report);
                    if (Report == Globals.PendingArrestReport)
                    {
                        Function.Log("Resetting pending arrest report");
                        Globals.PendingArrestReport = new ArrestReport();
                    }
                    else
                    {
                        Function.Log("Saving report that isnt global pending");
                    }
                }
                if (OnArrestReportValidationError != null)
                    OnArrestReportValidationError(this, validationErrors);
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
            }
        }

        public override void InitializeLayout()
        {
           
            base.InitializeLayout();
            try {
                
                this.Position = this.GetLaunchPosition();
                this.Window.DisableResizing();
                pedDetailsPage.Window.IsClosable = false;
                chargeDetailsPage.Window.IsClosable = false;
                additionalPartiesPage.Window.IsClosable = false;
                arrestDetails = new ArrestReportDetails(this);

                UpdateReportForChildren();

                tabContainer = new TabbableContainer(this);
                tabContainer.Dock = Pos.Fill;
                tabContainer.AddPage("Arrestee", pedDetailsPage);
                tabContainer.AddPage("Charges", chargeDetailsPage);
                tabContainer.AddPage("Additional Parties", additionalPartiesPage);
                tabContainer.AddPage("Detailed Report", arrestDetails);

                if (Report.IsNew)
                {
                    btn_clear.Clicked += ClearClicked;
                    btn_save.Clicked += SaveClicked;
                    btn_clear.SetPosition(tabContainer.Right - btn_save.Width - 10, tabContainer.Y - 10);
                    btn_save.SetPosition(tabContainer.Right - btn_clear.Width - btn_save.Width - 10 - 10, tabContainer.Y - 10);
                    tabContainer.Margin = new Margin(0, 20, 0, 0);
                }
                else
                {
                    Function.Log("ArrestReportContainer view report");
                    btn_save.Disable();
                    btn_save.Hide();
                    btn_clear.Disable();
                    btn_clear.Hide();
                }
               
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
            }
        }

        private void ReportDetailsTextChanged(Base sender, EventArgs arguments)
        {
            if (Report == null) return;
            Report.Details = ((MultilineTextBox)sender).Text;
        }

        private void ClearClicked(Base sender, ClickedEventArgs arguments)
        {
            Report = Globals.PendingArrestReport = new ArrestReport();
            UpdateReportForChildren();
        }

        public void SubscribeToErrorEvents(ArrestReportValidationError subscriber)
        {
            OnArrestReportValidationError += subscriber;
        }

        public void UnsubscribeToErrorEvents(ArrestReportValidationError subscriber)
        {
            OnArrestReportValidationError -= subscriber;
        }
    }
}
