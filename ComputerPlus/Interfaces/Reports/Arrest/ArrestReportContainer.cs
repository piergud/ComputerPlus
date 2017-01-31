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
using ComputerPlus.DB.Tables;

namespace ComputerPlus.Interfaces.Reports.Arrest
{

    public class ArrestReportContainer : GwenForm
    {
        ArrestReport Report;
        TabbableContainer tabContainer;
        ArrestReportPedDetails pedDetailsPage = new ArrestReportPedDetails(Globals.PendingArrestReport);
        ArrestReportChargeDetails chargeDetailsPage = new ArrestReportChargeDetails(Globals.PendingArrestReport);
        Base testPage;
        Button btn_save;
        ArrestReportTable arrestReportDb = new ArrestReportTable(Globals.Store.Connection());
        

        internal ArrestReportContainer() : this(Globals.PendingArrestReport)
        {
        }

        internal ArrestReportContainer(ArrestReport arrestReport) : base(typeof(ArrestReportContainerTemplate)) {
            Report = arrestReport;
        }

        private void SaveClicked(Base sender, ClickedEventArgs arguments)
        {

            try {
                
                Function.Log(String.Format("Saving report for {0} with {1} charges", Report.FirstName, Report.Charges.Count));
                arrestReportDb.Insert(Report);
                Report.Charges.ForEach(charge => {
                    ArrestReportLineItemTable arrestReportChargesDb = new ArrestReportLineItemTable(Globals.Store.Connection());
                    charge.ContainingReport = Report;
                    arrestReportChargesDb.Insert(charge);
                });
            }
            catch(Exception e)
            {
                Function.Log(e.ToString());
            }
        }

        public override void InitializeLayout()
        {
           
            base.InitializeLayout();
            try {
                
                this.Position = this.GetLaunchPosition();
                btn_save.Clicked += SaveClicked;
                btn_save.Dock = Pos.Top;
                pedDetailsPage.Window.IsClosable = false;
                chargeDetailsPage.Window.IsClosable = false;
                pedDetailsPage.ChangeReport(Report);
                chargeDetailsPage.ChangeReport(Report);
                tabContainer = new TabbableContainer(this);
                tabContainer.Dock = Pos.Fill;
                tabContainer.Margin = new Margin(0, 20, 0, 0);
                tabContainer.AddPage("Arrestee", pedDetailsPage);
                tabContainer.AddPage("Charges", chargeDetailsPage);
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
            }
        }
    }
}
