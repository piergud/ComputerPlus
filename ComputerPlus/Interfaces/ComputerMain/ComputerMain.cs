
using System.Drawing;
using System.Linq;
using Rage;
using Rage.Forms;
using Gwen.Control;
using System;
using ComputerPlus.Interfaces.ComputerPedDB;

namespace ComputerPlus
{
    internal class ComputerMain : GwenForm
    {
        private Button btn_logout, btn_ped_db, btn_veh_db, btn_request, btn_activecalls;
        internal ListBox list_recent;
        private Label label_external_ui;
        private ComboBox list_external_ui;
        internal static GameFiber form_veh_db = new GameFiber(OpenVehDBForm);
        internal static GameFiber form_backup = new GameFiber(OpenRequestBackupForm);
        internal static GameFiber form_report = new GameFiber(OpenReportMenuForm);
        internal static GameFiber form_active_calls = new GameFiber(OpenActiveCallsForm);
        //private Button btn_ReportMain; // Fiskey111 Edit

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
            this.btn_logout.Clicked += this.LogoutButtonClickedHandler;
            this.btn_ped_db.Clicked += this.PedDBButtonClickedHandler;
            this.btn_veh_db.Clicked += this.VehDBButtonClickedHandler;
            this.btn_request.Clicked += this.RequestBackupButtonClickedHandler;
            //this.btn_ReportMain.Clicked += this.ReportMainClickedHandler;  // Fiskey111 Edit
            this.btn_activecalls.Clicked += this.ActiveCallsClickedHandler;
            this.Window.DisableResizing();
            foreach (string r in EntryPoint.recent_text)
            {
                list_recent.AddRow(r);
            }
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            if (!Function.IsBackgroundEnabled())
                Function.EnableBackground();

            if (ShouldShowExtraUIControls)
            {
                ControlExternalUISelectVisibility(ShouldShowExtraUIControls);
                list_external_ui.AddItem("Select One");
                Globals.SortedExternalUI.ToList().ForEach(x => list_external_ui.AddItem(x.DisplayName, x.Identifier.ToString()));
                list_external_ui.ItemSelected += ExternalUISelected;
            }
            
        }



        private void LogoutButtonClickedHandler(Base sender, ClickedEventArgs e) 
        {
            this.Window.Close();
        }


        private void PedDBButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
            var fiber = ComputerPedController.PedSearchGameFiber;
            if (fiber.IsHibernating) fiber.Wake();
            else if(!fiber.IsAlive) fiber.Start();
        }

        private void VehDBButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
            form_veh_db = new GameFiber(OpenVehDBForm);
            form_veh_db.Start();
        }

        private void RequestBackupButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
            form_backup = new GameFiber(OpenRequestBackupForm);
            form_backup.Start();
        }

        private void ReportMainClickedHandler(Base sender, ClickedEventArgs e)   // Fiskey111 Edit
        {
            this.Window.Close();
            form_report = new GameFiber(OpenReportMenuForm);
            form_report.Start();
        }

        private void ActiveCallsClickedHandler(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
            form_active_calls = new GameFiber(OpenActiveCallsForm);
            form_active_calls.Start();
        }

        private void ExternalUISelected(Base sender, ItemSelectedEventArgs arguments)
        {
            if (arguments.SelectedItem.Name == null) return;
            System.Guid guid = System.Guid.Parse(arguments.SelectedItem.Name);
            var match = Globals.ExternalUI.DefaultIfEmpty(null).FirstOrDefault(x => x.Identifier == guid);
            if (match == null) return;

            try
            {
                GameFiber.StartNew(delegate
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
            }
            catch(Exception e)
            {
                Game.LogVerbose(string.Format("Error while initializing extra form {0}", match.DisplayName));
                Game.LogVerbose(e.Message);
            }
        }


        internal static void OpenVehDBForm()
        {
            GwenForm veh_db = new ComputerVehDB();
            veh_db.Show();
            while (veh_db.Window.IsVisible)
                GameFiber.Yield();
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
            /*GwenForm reportmenu = new ReportMain();
            reportmenu.Show();
            while (reportmenu.Window.IsVisible)
                GameFiber.Yield();*/
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
            if(visible)
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
    }
}
