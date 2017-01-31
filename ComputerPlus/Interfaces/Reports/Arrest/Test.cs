using Rage.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    class Test : GwenForm
    {
        internal Test() : base(typeof(TestTemplate))
        {
        }
        public override void InitializeLayout()
        {
            base.InitializeLayout();
        }
    }
}
