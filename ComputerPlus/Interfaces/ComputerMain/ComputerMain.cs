
using System.Drawing;
using System.Linq;
using Rage;
using Rage.Forms;
using Gwen.Control;
using System;
using ComputerPlus.Interfaces.ComputerPedDB;
using ComputerPlus.Interfaces.ComputerVehDB;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Controllers;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Interfaces.Common;

namespace ComputerPlus
{
    internal class ComputerMain : GwenForm
    {
        private Button btn_logout, btn_ped_db, btn_veh_db, btn_request, btn_activecalls, btn_notepad, btn_arrest_report, btn_browse_report;
        internal ListBox list_recent;
        private Label label_external_ui;
        private ComboBox list_external_ui;
        private CheckBox cb_toggle_pause, cb_toggle_background;
        MenuItem external_ui_default;
        internal static GameFiber external_ui_fiber = null;

        private bool ShouldShowExtraUIControls
        {
            get
            {
                return Globals.ExternalUI.Count > 0;
            }
        }

        public ComputerMain() : base(typeof(ComputerMainTemplate))
        {

        }

        ~ComputerMain()
        {
            this.btn_logout.Clicked -= this.LogoutButtonClickedHandler;
            this.btn_ped_db.Clicked -= this.PedDBButtonClickedHandler;
            this.btn_veh_db.Clicked -= this.VehDBButtonClickedHandler;
            this.btn_request.Clicked -= this.RequestBackupButtonClickedHandler;
            this.cb_toggle_background.CheckChanged -= checkbox_change;
            this.cb_toggle_pause.CheckChanged -= checkbox_change;
            this.btn_activecalls.Clicked -= this.ActiveCallsClickedHandler;
            if (ShouldShowExtraUIControls)
            {
                list_external_ui.ItemSelected -= ExternalUISelected;
            }
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Window.Skin.SetDefaultFont(Configs.FontName, Configs.FontSize);
            this.cb_toggle_background.IsChecked = Globals.ShowBackgroundWhenOpen;
            this.cb_toggle_pause.IsChecked = Globals.PauseGameWhenOpen;
            this.btn_logout.Clicked += this.LogoutButtonClickedHandler;
            this.btn_ped_db.Clicked += this.PedDBButtonClickedHandler;
            this.btn_veh_db.Clicked += this.VehDBButtonClickedHandler;
            this.btn_request.Clicked += this.RequestBackupButtonClickedHandler;
            this.btn_notepad.Clicked += OpenNotepadHandler;
            //this.btn_arrest_report.Clicked += this.ReportsClickedHandler;
            //this.btn_browse_report.Clicked += this.ReportsClickedHandler;

            this.btn_browse_report.Disable();
            this.btn_browse_report.Hide();
            this.btn_arrest_report.Disable();
            this.btn_arrest_report.Hide();

            this.cb_toggle_background.CheckChanged += checkbox_change;
            this.cb_toggle_pause.CheckChanged += checkbox_change;
            this.Window.KeyboardInputEnabled = true;
            
            GameFiber.StartNew(() =>
            {
                while(true)
                {
                    if(this.cb_toggle_pause.IsChecked != Globals.PauseGameWhenOpen)
                        this.cb_toggle_pause.IsChecked = Globals.PauseGameWhenOpen;
                    GameFiber.Yield();
                }
            });
            this.btn_activecalls.Clicked += this.ActiveCallsClickedHandler;
            this.Window.DisableResizing();
            foreach (string r in EntryPoint.recent_text)
            {
                list_recent.AddRow(r);
            }
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);

            if (ShouldShowExtraUIControls)
            {
                ControlExternalUISelectVisibility(ShouldShowExtraUIControls);
                external_ui_default = list_external_ui.AddItem("Select One", "placeholder");
                Globals.SortedExternalUI.ToList().ForEach(x => list_external_ui.AddItem(x.DisplayName, x.Identifier.ToString()));
                list_external_ui.ItemSelected += ExternalUISelected;
            }

        }

        private void OpenNotepadHandler(Base sender, ClickedEventArgs arguments)
        {
            EntryPoint.ShowNotepad(false);            
        }

        private void checkbox_change(Base sender, EventArgs arguments)
        {
            if (sender == cb_toggle_pause && cb_toggle_pause.IsChecked != Globals.PauseGameWhenOpen)
                EntryPoint.TogglePause();
            else if (sender == cb_toggle_background)
                EntryPoint.ToggleBackground();
        }

        private void LogoutButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            Globals.Navigation.Clear();
        }

        private void ReportsClickedHandler(Base sender, ClickedEventArgs e)
        {
            //if (sender == btn_browse_report)
            //    ComputerReportsController.ShowTrafficCitationList();
            //else if(sender == btn_arrest_report)
            //ComputerReportsController.ShowTrafficCitationCreate(null);

            if (sender == btn_browse_report)
                ComputerReportsController.ShowArrestReportList();
            else if (sender == btn_arrest_report)
                ComputerReportsController.ShowArrestReportCreate(null, null);
        }


        private void PedDBButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            ComputerPedController.ShowPedSearch();
        }

        private void VehDBButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            ComputerVehicleController.ShowVehicleSearch();
        }

        private void RequestBackupButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            OpenRequestBackupForm();
        }


        private void ActiveCallsClickedHandler(Base sender, ClickedEventArgs e)
        {
            OpenActiveCallsForm();
        }

        private void ExternalUISelected(Base sender, ItemSelectedEventArgs arguments)
        {
            if (String.IsNullOrWhiteSpace(arguments.SelectedItem.Name) || arguments.SelectedItem.Name.Equals("placeholder")) return;
            Function.LogDebug(String.Format("External UI Selected {0}", arguments.SelectedItem.Name));
            System.Guid guid = System.Guid.Parse(arguments.SelectedItem.Name);
            var match = Globals.ExternalUI.DefaultIfEmpty(null).FirstOrDefault(x => x.Identifier == guid);
            if (match == null) return;
            list_external_ui.SelectedItem = external_ui_default;
            try
            {
                var fiber = GameFiber.StartNew(delegate
                {
                    var form = match.Creator();
                    if (form == null)
                    {
                        Game.DisplayNotification(String.Format("Empty form provided for {0}", match.DisplayName));
                        return;
                    }
                    form.Show();
                    if (match.OnOpen != null)
                        match.OnOpen();
                    Globals.ActiveExternalUI_ID = match.Identifier;
                });
                while (fiber.IsAlive && !fiber.IsHibernating)
                {
                    GameFiber.Yield();
                }
                match.OnClose();
            }
            catch (Exception e)
            {
                Function.Log(String.Format("Error while initializing extra form {0}", match.DisplayName));
                Function.Log(e.Message);
            }
        }


        internal static void OpenRequestBackupForm()
        {
            Globals.Navigation.Push(new ComputerRequestBackup());
        }

        internal static void OpenActiveCallsForm()
        {
            Globals.Navigation.Push(new ComputerCurrentCallDetails());
        }

        private void ControlExternalUISelectVisibility(bool visible)
        {
            label_external_ui.IsHidden = !visible;
            list_external_ui.IsHidden = !visible;            
        }

        internal static void ShowMain()
        {
            Globals.Navigation.Push(new ComputerMain());
        }
    }
}