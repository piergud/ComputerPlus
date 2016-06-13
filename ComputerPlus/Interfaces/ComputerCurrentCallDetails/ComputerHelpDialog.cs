using ComputerPlus.Interfaces.ComputerCurrentCallDetails;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Rage;
using Rage.Forms;
using Gwen.Control;

namespace ComputerPlus
{
    internal class ComputerHelpDialog : GwenForm
    {
        Button btn_ok;

        public ComputerHelpDialog() : base(typeof(ComputerHelpDialogTemplate))
        {
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.btn_ok.Clicked += this.OKButtonClickedHandler;
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            this.Window.MakeModal(true);
        }

        public void OKButtonClickedHandler(object sender, ClickedEventArgs e)
        {
            this.Window.Close();
        }
    }
}