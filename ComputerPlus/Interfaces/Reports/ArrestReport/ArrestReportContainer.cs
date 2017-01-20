using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage.Forms;
using Gwen;
using Gwen.Control;
using ComputerPlus.Interfaces.Common;


namespace ComputerPlus.Interfaces.Reports.ArrestReport
{

    public class ArrestReportContainer : GwenForm
    {
        TabbableContainer tabContainer;
        ArrestReportPedDetails pedDetailsPage = new ArrestReportPedDetails();
        ArrestReportChargeDetails chargeDetailsPage = new ArrestReportChargeDetails();
        internal ArrestReportContainer() : base(typeof(ArrestReportContainerTemplate)) {

        }
        public override void InitializeLayout()
        {
            base.InitializeLayout();
            
            this.Position = this.GetLaunchPosition();
            tabContainer = new TabbableContainer(this);
            tabContainer.AddPage("Arrestee", pedDetailsPage);
            tabContainer.AddPage("Charges", chargeDetailsPage);
        }
    }
}
