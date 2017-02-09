using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Gwen.ControlInternal;
using Rage.Forms;

namespace ComputerPlus.Interfaces.Common
{
    internal enum DialogAction { CLOSED = 0, POSITIVE = 1, NEGATIVE = -1 }
    delegate void DialogActionEvent(object sender, DialogAction action);
    class ActionDialog : GwenForm
    {
        protected String DialogMessage;
        protected String PositiveButtonText;
        protected String NegativeButtonText;
        protected RichLabel DialogLabel;
        protected Button PositiveButton;
        protected Button NegativeButton;

        public event DialogActionEvent OnDialogAction;

        public ActionDialog(String dialogTitle, String dialogText, String positiveButtonText = "Accept", String negativeButtonText = "Cancel") : base(dialogTitle, 700, 300)
        {
            DialogMessage = dialogText;
            PositiveButtonText = positiveButtonText;
            NegativeButtonText = negativeButtonText;
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Window.IsClosable = false;
            this.Position = this.GetLaunchPosition();
            try
            {
                DialogLabel = new RichLabel(this);
                PositiveButton = new Button(this);
                NegativeButton = new Button(this);
                PositiveButton.Clicked += ActionButtonClicked;
                NegativeButton.Clicked += ActionButtonClicked;

                if (!String.IsNullOrEmpty(PositiveButtonText))
                    PositiveButton.Text = PositiveButtonText;
                else
                {
                    PositiveButton.Hide();
                    PositiveButton.Disable();
                }

                if (!String.IsNullOrEmpty(NegativeButtonText))
                    NegativeButton.Text = NegativeButtonText;
                else
                {
                    NegativeButton.Hide();
                    NegativeButton.Disable();
                }

                if (!String.IsNullOrEmpty(DialogMessage))
                {
                    var font = DialogLabel.Skin.DefaultFont.Copy();
                    font.Size = 16;
                    DialogLabel.AddText(DialogMessage, System.Drawing.Color.Black, font);
                    DialogLabel.Margin = new Gwen.Margin(20, 75, 20, 75);
                    
                }

                
                DialogLabel.Dock = Gwen.Pos.Fill;
                if (NegativeButton.IsVisible && PositiveButton.IsVisible)
                {
                    NegativeButton.SetPosition(((this.Window.Width / 2 - NegativeButton.Width / 2) - NegativeButton.Width - 10f), (this.Window.Height - NegativeButton.Height / 2) - 50f);
                    PositiveButton.SetPosition(((this.Window.Width / 2 - PositiveButton.Width / 2) + PositiveButton.Width + 10f), (this.Window.Height - PositiveButton.Height / 2) - 50f);
                }
                else if (NegativeButton.IsVisible)
                {
                    NegativeButton.SetPosition(((this.Window.Width / 2 - NegativeButton.Width / 2)), (this.Window.Height - NegativeButton.Height / 2) - 50f);
                }
                else
                {
                    PositiveButton.SetPosition(((this.Window.Width / 2 - PositiveButton.Width / 2)), (this.Window.Height - PositiveButton.Height / 2) - 50f);
                }
            }
            catch(Exception e)
            {
                Function.Log(e.ToString());
            }

        }

        private void ActionButtonClicked(Base sender, ClickedEventArgs arguments)
        {
            if (OnDialogAction != null)
            {
                if (sender == PositiveButton)
                {
                    OnDialogAction(this, DialogAction.POSITIVE);
                }
                else if (sender == NegativeButton)
                {
                    OnDialogAction(this, DialogAction.NEGATIVE);
                }
            }
            this.Window.Close();

        }
    }
}
