
using System.Drawing;
using System.Linq;
using Rage;
using Rage.Forms;
using Gwen.Control;


namespace ExampleExternalUI
{
    class ExampleForm : GwenForm
    {
        private Button button1;

        public ExampleForm() : base(typeof(ExampleFormTemplate))
        {

        }

        public override void InitializeLayout()
        {
            button1.Clicked += Button1_Clicked;
            //Show in the middle of the screen
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
        }

        private void Button1_Clicked(Base sender, ClickedEventArgs arguments)
        {
            Game.DisplayHelp("Hey you clicked me");
        }
    }
}
