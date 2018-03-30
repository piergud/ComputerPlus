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
using ComputerPlus.Controllers.Models;
using ComputerPlus.Extensions;
namespace ComputerPlus.Interfaces.Reports.Arrest
{
    internal interface IErrorNotifier
    {
        void SubscribeToErrorEvents(ArrestReportContainer.ArrestReportValidationError subscriber);
        void UnsubscribeToErrorEvents(ArrestReportContainer.ArrestReportValidationError subscriber);
    }

    internal interface IActionNotifier
    {
        void SubscribeToActionEvents(ArrestReportContainer.ArrestReportActionEvent subscriber);
        void UnsubscribeToActionEvents(ArrestReportContainer.ArrestReportActionEvent subscriber);
    }

    public class ArrestReportContainer : GwenForm, IErrorNotifier, IActionNotifier
    {
        ArrestReport Report;
        ComputerPlusEntity reportEntity;
        TabbableContainer tabContainer;
        ArrestReportPedDetails pedDetailsPage = new ArrestReportPedDetails(Globals.PendingArrestReport);
        ArrestReportChargeDetails chargeDetailsPage = new ArrestReportChargeDetails(Globals.PendingArrestReport);
        ArrestReportAdditionalParties additionalPartiesPage = new ArrestReportAdditionalParties(Globals.PendingArrestReport);
        ArrestReportDetails arrestDetails;
        Base testPage;
        Button btn_save;
        Button btn_clear;

        public bool LockArresteePedDetails
        {
            set
            {
                Report.ReadOnly = true;
            }
        }

        public delegate void ArrestReportValidationError(object sender, Dictionary<String, String> errors);
        internal event ArrestReportValidationError OnArrestReportValidationError;
        public enum ArrestReportSaveResult { SAVE, SAVE_FAILED, SAVE_ERROR, CLEAR }
        public delegate void ArrestReportActionEvent(object sender, ArrestReportSaveResult action, ArrestReport report);
        internal event ArrestReportActionEvent OnArrestReportAction;

        internal ArrestReportContainer() : this(Globals.PendingArrestReport, null)
        {

        }        

        internal ArrestReportContainer(ArrestReport arrestReport, ComputerPlusEntity entity) : base(typeof(ArrestReportContainerTemplate)) {
            Report = arrestReport;
            reportEntity = entity;            
            pedDetailsPage.SetErrorSubscription(this);
            pedDetailsPage.SetActionSubscription(this);
        }
        

        private void UpdateReportForChildren()
        {
            pedDetailsPage.ChangeReport(Report);
            chargeDetailsPage.ChangeReport(Report);
            additionalPartiesPage.ChangeReport(Report);
            arrestDetails.ChangeReport(Report);
        }

        private void SaveClicked(Base sender, ClickedEventArgs arguments)
        {
            var result = SaveReport();
            if (result == ArrestReportSaveResult.SAVE_ERROR)
            {
                var message = (Report.Charges.Count == 0) ? "Report has no charges" : "The Report is missing required information";
                new MessageBox(this, message);
            }
            else
            {
                this.Window.Close();
            }
        }

        private ArrestReportSaveResult SaveReport()
        {
            try
            {
                Dictionary<String, String> validationErrors;
                if (Report.Validate(out validationErrors))
                {
                    ComputerReportsController.SaveArrestRecord(Report);
                    if (Report == Globals.PendingArrestReport)
                    {
                        Globals.PendingArrestReport = new ArrestReport();
                    }                
                    NotifyForEvent(ArrestReportSaveResult.SAVE);

                    // create LSPDFR+ court case
                    ComputerReportsController.createCourtCaseForArrest(Report, reportEntity);

                    return ArrestReportSaveResult.SAVE;
                }
                else
                {
                    if (OnArrestReportValidationError != null) OnArrestReportValidationError(this, validationErrors);                    
                    return ArrestReportSaveResult.SAVE_ERROR;
                }
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return ArrestReportSaveResult.SAVE_FAILED;
            }
        }

        private void NotifyForEvent(ArrestReportSaveResult action)
        {
            if (OnArrestReportAction != null)
            {
                OnArrestReportAction(this, action, Report);
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
                tabContainer.AddPage("Arrestee", pedDetailsPage).Clicked += ContainerTabButtonClicked;
                tabContainer.AddPage("Charges", chargeDetailsPage).Clicked += ContainerTabButtonClicked;
                tabContainer.AddPage("Additional Parties", additionalPartiesPage).Clicked += ContainerTabButtonClicked;
                tabContainer.AddPage("Detailed Report", arrestDetails).Clicked += ContainerTabButtonClicked;

                if (Report.IsNew)
                {
                    btn_clear.Clicked += ClearClicked;
                    btn_save.Clicked += SaveClicked;
                    btn_clear.SetPosition(tabContainer.Right - btn_save.Width - btn_clear.Width - 10 - 10, tabContainer.Y - 10);
                    btn_save.SetPosition(tabContainer.Right - btn_save.Width - 10, tabContainer.Y - 10);
                    tabContainer.Margin = new Margin(0, 20, 0, 0);
                }
                else
                {
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

        private void ContainerTabButtonClicked(Base sender, ClickedEventArgs arguments)
        {
            // commented this since we will save the report when player presses the save button anyway
            // SaveReport();
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
            NotifyForEvent(ArrestReportSaveResult.CLEAR);
        }

        public void SubscribeToErrorEvents(ArrestReportValidationError subscriber)
        {
            OnArrestReportValidationError += subscriber;
        }

        public void UnsubscribeToErrorEvents(ArrestReportValidationError subscriber)
        {
            OnArrestReportValidationError -= subscriber;
        }

        public void SubscribeToActionEvents(ArrestReportActionEvent subscriber)
        {
            OnArrestReportAction += subscriber;
        }

        public void UnsubscribeToActionEvents(ArrestReportActionEvent subscriber)
        {
            OnArrestReportAction -= subscriber;
        }

        internal static ArrestReportContainer CreateForPed(ComputerPlusEntity entity, out ArrestReport report)
        {
            report = ArrestReport.CreateForPed(entity);
            return new ArrestReportContainer(report, entity) { LockArresteePedDetails = true };
        }
    }
}
