using System;
using System.Drawing;
using Rage;
using Rage.Forms;
using Gwen.Control;
using ComputerPlus.Interfaces.ComputerPedDB;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus
{
    internal class ComputerLogin : GwenForm
    {
        private Button btn_login;
        private TextBox input_user, input_pass;
        private TextBoxPassword pass;
        private Label label_invalid;
        private ImagePanel panel_invalid_user, panel_invalid_pass;
        internal static GameFiber ComputerLoginGameFiber = new GameFiber(ShowLogin);

        public ComputerLogin() : base(typeof(ComputerLoginTemplate))
        {
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            pass = new TextBoxPassword(input_pass);
            pass.SetSize(260, 20);
            this.btn_login.Clicked += this.LoginButtonClickedHandler;
            this.input_user.SubmitPressed += this.OnFieldSubmit;
            this.pass.SubmitPressed += this.OnFieldSubmit;
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            this.label_invalid.Hide();
            this.input_user.Text = Configs.Username;
            this.pass.Text = Configs.Password;
        }
        

        private void LoginButtonClickedHandler(Base sender, ClickedEventArgs e) 
        {
            Login();
        }

        private void OnFieldSubmit(Base sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            if (this.input_user.Text == Configs.Username 
                && this.pass.Text == Configs.Password)
            {
                this.Window.Close();
                EntryPoint.OpenMain();
            }
            else
            {
                this.label_invalid.Show();
            }
        }

        internal static void ShowLogin()
        {
            while (true)
            {
                var form = new ComputerLogin();
                Function.LogDebug("Init new ComputerLogin");
                form.Show();
                while (form.IsOpen() && !Globals.CloseRequested)
                    GameFiber.Yield();
                Function.LogDebug("Close ComputerLogin");
                form.Close();
                Function.LogDebug("ComputerLogin Hibernating");
                GameFiber.Hibernate();
            }
        }
    }
}
