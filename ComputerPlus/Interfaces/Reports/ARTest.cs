using ComputerPlus.Interfaces.Common;
using ComputerPlus.Interfaces.Reports.Arrest;
using Gwen.Control;
using Rage.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.Reports
{
    class ARTest : GwenForm
    {
        TabbableContainer container;
        public ARTest() : base(typeof(TestTemplate))
        {
            container = new TabbableContainer(this);
        }
        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.GetLaunchPosition();
            TabControl tc = new TabControl(this);
            tc.AddPage("test", new Reports.Arrest.Test());
            tc.Dock = Gwen.Pos.Fill;
        }
    }
    
}
