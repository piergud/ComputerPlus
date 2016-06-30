
using System.Drawing;
using System.Linq;
using Rage;
using Rage.Forms;
using Gwen.Control;


namespace ExampleExternalUI
{
    class ExampleTwoForm : GwenForm
    {

        public ExampleTwoForm() : base(typeof(ExampleFormTwoTemplate))
        {

        }

        public override void InitializeLayout()
        {
        }        
    }
}
