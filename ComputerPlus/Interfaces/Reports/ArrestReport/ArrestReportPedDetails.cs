using Rage.Forms;

namespace ComputerPlus.Interfaces.Reports.ArrestReport
{
    internal class ArrestReportPedDetails : GwenForm
    {
        internal ArrestReportPedDetails() : base(typeof(ArrestReportPedDetailsTemplate)) {

        }
        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Window.IsClosable = false;
        }
    }
}