
using System.Drawing;
using System.Linq;
using Rage;
using Rage.Forms;
using Gwen.Control;
using System;
using ComputerPlus.Interfaces.ComputerPedDB;
using ComputerPlus.Interfaces.ComputerVehDB;

namespace ComputerPlus
{
    internal class ComputerMain : GwenForm
    {
        private Button btn_logout, btn_ped_db, btn_veh_db, btn_request, btn_activecalls;
        internal ListBox list_recent;
        private Label label_external_ui;
        private ComboBox list_external_ui;
        private CheckBox cb_toggle_pause, cb_toggle_background;
        MenuItem external_ui_default;
        internal static GameFiber ComputerMainGameFiber = new GameFiber(ShowMain);
        internal static GameFiber form_backup = new GameFiber(OpenRequestBackupForm);
        internal static GameFiber form_report = new GameFiber(OpenReportMenuForm);
        internal static GameFiber form_active_calls = new GameFiber(OpenActiveCallsForm);
        internal static GameFiber external_ui_fiber = null;
        private Button report_button;

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

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.cb_toggle_background.IsChecked = Globals.ShowBackgroundWhenOpen;
            this.cb_toggle_pause.IsChecked = Globals.PauseGameWhenOpen;
            this.btn_logout.Clicked += this.LogoutButtonClickedHandler;
            this.btn_ped_db.Clicked += this.PedDBButtonClickedHandler;
            this.btn_veh_db.Clicked += this.VehDBButtonClickedHandler;
            this.btn_request.Clicked += this.RequestBackupButtonClickedHandler;
            this.cb_toggle_background.CheckChanged += checkbox_change;
            this.cb_toggle_pause.CheckChanged += checkbox_change;
            this.report_button.Clicked += this.ReportMainClickedHandler;  // Fiskey111 Edit
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

        private void checkbox_change(Base sender, EventArgs arguments)
        {
            Game.LogVerboseDebug("ALERT checkbox_change");
            if (sender == cb_toggle_pause)
                EntryPoint.TogglePause();
            else if (sender == cb_toggle_background)
                EntryPoint.ToggleBackground();
        }

        private void LogoutButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
        }


        private void PedDBButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            //this.Window.Close();
            var fiber = ComputerPedController.PedSearchGameFiber;
            if (fiber.IsHibernating) fiber.Wake();
            else if (!fiber.IsAlive) fiber.Start();
        }

        private void VehDBButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            //this.Window.Close();
            var fiber = ComputerVehicleController.VehicleSearchGameFiber;
            if (fiber.IsHibernating) fiber.Wake();
            else if (!fiber.IsAlive) fiber.Start();
        }

        private void RequestBackupButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            form_backup = new GameFiber(OpenRequestBackupForm);
            form_backup.Start();
        }

        private void ReportMainClickedHandler(Base sender, ClickedEventArgs e)   // Fiskey111 Edit
        {
            form_report = new GameFiber(OpenReportMenuForm);
            form_report.Start();
        }

        private void ActiveCallsClickedHandler(Base sender, ClickedEventArgs e)
        {
            form_active_calls = new GameFiber(OpenActiveCallsForm);
            form_active_calls.Start();
        }

        private void ExternalUISelected(Base sender, ItemSelectedEventArgs arguments)
        {
            if (String.IsNullOrWhiteSpace(arguments.SelectedItem.Name) || arguments.SelectedItem.Name.Equals("placeholder")) return;
            Game.LogVerboseDebug(String.Format("External UI Selected {0}", arguments.SelectedItem.Name));
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
                        Game.DisplayNotification(string.Format("Empty form provided for {0}", match.DisplayName));
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
                Game.LogVerbose(string.Format("Error while initializing extra form {0}", match.DisplayName));
                Game.LogVerbose(e.Message);
            }
        }


        internal static void OpenRequestBackupForm()
        {
            GwenForm backup = new ComputerRequestBackup();
            backup.Show();
            while (backup.Window.IsVisible)
                GameFiber.Yield();
        }

        internal static void OpenReportMenuForm()
        {
            GwenForm reportmenu = new ReportMain();
            reportmenu.Show();
            while (reportmenu.Window.IsVisible)
                GameFiber.Yield();
        }

        internal static void OpenActiveCallsForm()
        {
            GwenForm active_calls = new ComputerCurrentCallDetails();
            active_calls.Show();
            while (active_calls.Window.IsVisible)
                GameFiber.Yield();
        }

        private void ControlExternalUISelectVisibility(bool visible)
        {
            if (visible)
            {
                label_external_ui.Show();
                list_external_ui.Show();
            }
            else
            {
                label_external_ui.Hide();
                label_external_ui.Hide();
            }
        }

        internal static void ShowMain()
        {
            while (true)
            {
                var form = new ComputerMain();
                form.Show();
                Game.LogVerboseDebug("Init new ComputerMain");
                do
                {
                    GameFiber.Yield();
                }
                while (form.IsOpen());
                Game.LogVerboseDebug(String.Format("Close ComputerMain? {0}", form.IsOpen()));
                Game.LogVerboseDebug("ComputerMain Hibernating");
                GameFiber.Hibernate();
            }
        }
    }
}