using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Rage.Forms;

namespace ComputerPlus.Interfaces.Reports.ArrestReport
{
    internal class ArrestReportChargeDetails : GwenForm
    {
        internal ArrestReportChargeDetails() : base(typeof(ArrestReportChargeDetailsTemplate))
        {
            
        }
        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Window.IsClosable = false;
        }
    }
}
