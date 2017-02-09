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
        List<ArrestReport> reports = new List<ArrestReport>();
        internal ArrestReportList(List<ArrestReport> reports) : base("Arrest Reports", Configs.BaseFormWidth, Configs.BaseFormHeight)
        {
            this.reports.AddRange(reports);
        }


        

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();            
            list = new ListBox(this);           
            list.Dock = Gwen.Pos.Fill;
            foreach (var report in reports)
            {
                try
                {
                    String label = String.Format("{0}| {1} {2} {3}", report.Id().Substring(30), report.ArrestDate, report.ArrestTime, report.FullName);
                    var row = list.AddRow(label, report.Id(), report);
                    row.DoubleClicked += RowDoubleClicked;
                    
                }
                catch (Exception e)
                {
                    Function.Log("Lower caught");
                    Function.Log(e.ToString());
                    throw e;
                }
            }
            
            //list.RowSelected += ReportRowClicked;

        }

        private void RowDoubleClicked(Base sender, ClickedEventArgs arguments)
        {
            Function.Log("Double click");
            var report = list.SelectedRow.UserData as ArrestReport;
            if (report != null)
                OpenReport(report);
        }

        private async void OpenReport(ArrestReport report)
        {
            if (report != null)
                await ComputerReportsController.ShowArrestReportView(report);
        }

        private void ReportRowClicked(Base sender, ItemSelectedEventArgs arguments)
        {
            var report = arguments.SelectedItem.UserData as ArrestReport;
            if (report != null)
                OpenReport(report);
            //list.SelectedRow = null;
        }
    }
    
}
