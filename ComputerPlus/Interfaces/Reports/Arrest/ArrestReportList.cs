using Rage.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Controllers;
using ComputerPlus.Interfaces.Reports.Models;
using Gwen.Control;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus.Interfaces.Reports.Arrest
{
    class ArrestReportList : ListBox
    {
        public enum ListItemClickType { DOUBLE = 0, SINGLE = 1 }
        public ListItemClickType ListClickStyle;
        internal delegate void ArrestReportSelected(ArrestReport report);
        internal delegate void ArrestReportRowRenderer(ArrestReport report, ListBoxRow row);
        private readonly ArrestReportSelected OnArrestReportSelected;
        private readonly ArrestReportRowRenderer OnRenderRowText;
        ListBox list;
        List<ArrestReport> Reports;
        public ArrestReportList(Base parent, List<ArrestReport> reports, ArrestReportSelected onSelected, ArrestReportRowRenderer rowRenderer) : base(parent)
        {
            Reports = reports;
            list = new ListBox(this);
            list.Dock = Gwen.Pos.Fill;
            OnArrestReportSelected = onSelected;
            if (rowRenderer == null) OnRenderRowText = DefaultRowRender;
            else OnRenderRowText = rowRenderer;

            AddReportsToList(); 
        }

        public void ChangeReports(List<ArrestReport> reports)
        {
            lock(reports)
            {
                this.Reports = reports.OrderByDescending(o => o.ArrestTimeDate).ToList();
                AddReportsToList();
            }
        }

        private void RowClicked(Base sender, ClickedEventArgs arguments)
        {
            var report = list.SelectedRow.UserData as ArrestReport;
            if (report != null && OnArrestReportSelected != null)
                OnArrestReportSelected(report);
        }

        internal static void DefaultRowRender(ArrestReport report, ListBoxRow row)
        {
            row.Text = String.Format("{0} {1} {2} {3}", report.Id().Substring(30), report.ArrestDate, report.ArrestTime, report.FullName);
        }

        private void AddReportsToList(bool clearPrevious = true, bool selectFirstElementInList = false)
        {
            if (Reports == null)
            {
                return;
            }
            if (clearPrevious) list.Clear();

            foreach (var report in Reports)
            {

                try
                {
                    var row = list.AddRow(String.Empty, report.Id(), report);
                    OnRenderRowText(report, row);
                    if (ListClickStyle == ListItemClickType.DOUBLE)
                        row.DoubleClicked += RowClicked;
                    else
                        row.Clicked += RowClicked;

                }
                catch (Exception e)
                {
                    Function.Log(e.ToString());
                    throw e;
                }
            }
            if (selectFirstElementInList)
            {
                list.SelectRow(0, true);
            }

        }

    }

    class ArrestReportListContainer : GwenForm
    {

        List<ArrestReport> reports;
        private ArrestReportList list;

        internal ArrestReportList.ArrestReportSelected OnArrestReportSelected;


        internal bool SelectFirstElementInList;

        public ArrestReportList.ListItemClickType ListClickStyle
        {
            set
            {
                if (list != null) list.ListClickStyle = value;
            }
        }

        internal ArrestReportListContainer(List<ArrestReport> reports) : this()
        {
            this.reports = reports;
        }

        internal ArrestReportListContainer() : base("Arrest Reports", Configs.BaseFormWidth, Configs.BaseFormHeight)
        {

        }
        

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            list = new ArrestReportList(this, this.reports, OpenReport, ArrestReportList.DefaultRowRender);           
            list.Dock = Gwen.Pos.Fill;            
        }


        private void OpenReport(ArrestReport report)
        {
            if (report != null)
            {
                if (OnArrestReportSelected == null)
                {
                    ComputerReportsController.ShowArrestReportView(report);
                }
                else
                {
                    OnArrestReportSelected(report);
                }
            }
                
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
