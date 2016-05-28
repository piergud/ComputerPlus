using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Rage;
using Rage.Forms;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine.Scripting.Entities;
using Gwen.Control;

namespace ComputerPlus
{
    public class ComputerMain : GwenForm
    {
        private Button btn_logout, btn_ped_db, btn_veh_db, btn_request;
        public ListBox list_recent;
        public static GameFiber form_ped_db = new GameFiber(OpenPedDBForm);
        public static GameFiber form_veh_db = new GameFiber(OpenVehDBForm);
        public static GameFiber form_backup = new GameFiber(OpenRequestBackupForm);
        public static GameFiber form_report = new GameFiber(OpenReportMenuForm);
        private Button btn_ReportMain; // Fiskey111 Edit

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
            this.btn_ReportMain.Clicked += this.ReportMainClickedHandler;  // Fiskey111 Edit
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

        public static void OpenPedDBForm()
        {
            GwenForm ped_db = new ComputerPedDB();
            ped_db.Show();
            while (ped_db.Window.IsVisible)
                GameFiber.Yield();
        }

        public static void OpenVehDBForm()
        {
            GwenForm veh_db = new ComputerVehDB();
            veh_db.Show();
            while (veh_db.Window.IsVisible)
                GameFiber.Yield();
        }

        public static void OpenRequestBackupForm()
        {
            GwenForm backup = new ComputerRequestBackup();
            backup.Show();
            while (backup.Window.IsVisible)
                GameFiber.Yield();
        }

        public static void OpenReportMenuForm()
        {
            /*GwenForm reportmenu = new ReportMain();
            reportmenu.Show();
            while (reportmenu.Window.IsVisible)
                GameFiber.Yield();*/
        }
    }
}
