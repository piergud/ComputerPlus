using Rage.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Controllers;
using ComputerPlus.Interfaces.Reports.Models;
using Gwen.Control;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    class ArrestReportList : GwenForm
    {
        ListBox list;
        internal ArrestReportList() : base("Arrest Reports", Configs.BaseFormWidth, Configs.BaseFormHeight)
        {
        }
        private List<ArrestReport> FetchData(int skip = 0, int limit = 10)
        {
            return ComputerReportsController.GetAllArrestReports(skip, limit);
        }
        public override void InitializeLayout()
        {
            base.InitializeLayout();
            var data = FetchData();
            list = new ListBox(this);
            list.Dock = Gwen.Pos.Fill;
            foreach (var report in data)
            {
                var row = list.AddRow(String.Format("{0} {1} | {2} {3}", report.ArrestDate, report.ArrestTime, report.FullName, report.DOB), report.Id(), report);
                row.DoubleClicked += RowDoubleClicked;
            }
            //list.RowSelected += ReportRowClicked;
            this.Position = this.GetLaunchPosition();
        }

        private void RowDoubleClicked(Base sender, ClickedEventArgs arguments)
        {
            Function.Log("Double click");
            var report = list.SelectedRow.UserData as ArrestReport;
            if (report != null)
                OpenReport(report);
        }

        private void OpenReport(ArrestReport report)
        {
            ComputerReportsController.ShowArrestReportView(report);
        }

        private void ReportRowClicked(Base sender, ItemSelectedEventArgs arguments)
        {
            var report = arguments.SelectedItem.UserData as ArrestReport;
            if (report != null)
                OpenReport(report);
            list.SelectedRow = null;
        }
    }
    
}
