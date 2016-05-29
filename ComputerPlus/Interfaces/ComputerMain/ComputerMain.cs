
using System.Drawing;
using Rage;
using Rage.Forms;
using Gwen.Control;

namespace ComputerPlus
{
    internal class ComputerMain : GwenForm
    {
        private Button btn_logout, btn_ped_db, btn_veh_db, btn_request;
        internal ListBox list_recent;
        internal static GameFiber form_ped_db = new GameFiber(OpenPedDBForm);
        internal static GameFiber form_veh_db = new GameFiber(OpenVehDBForm);
        internal static GameFiber form_backup = new GameFiber(OpenRequestBackupForm);
        internal static GameFiber form_report = new GameFiber(OpenReportMenuForm);
        internal static GameFiber form_active_calls = new GameFiber(OpenActiveCallsForm);
        //private Button btn_ReportMain; // Fiskey111 Edit

        internal ComputerMain() : base(typeof(ComputerMainTemplate))
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
            this.Window.DisableResizing();
            foreach (string r in EntryPoint.recent_text)
            {
                list_recent.AddRow(r);
            }
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
        }

        private void LogoutButtonClickedHandler(Base sender, ClickedEventArgs e) 
        {
            this.Window.Close();
        }

        private void PedDBButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
            form_ped_db = new GameFiber(OpenPedDBForm);
            form_ped_db.Start();
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

        internal static void OpenPedDBForm()
        {
            GwenForm ped_db = new ComputerPedDB();
            ped_db.Show();
            while (ped_db.Window.IsVisible)
                GameFiber.Yield();
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
            /*
            GwenForm active_calls = new ComputerActiveCalls();
            active_calls.Show();
            while (active_calls.Window.IsVisible)
                GameFiber.Yield();
            */
        }
    }
}
