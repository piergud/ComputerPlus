using Rage.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Controllers;
using Gwen.Control;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Interfaces.Reports.Models;

namespace ComputerPlus.Interfaces.Reports.Citation
{
    class TrafficCitationList : ListBox
    {
        public enum ListItemClickType { DOUBLE = 0, SINGLE = 1 }
        public ListItemClickType ListClickStyle;

        internal delegate void TrafficCitationSelected(object sender, TrafficCitation citation);
        internal delegate void TrafficCitationRowRenderer(TrafficCitation citation, ListBoxRow row);
        private readonly TrafficCitationSelected OnTrafficCitationSelected;
        private readonly TrafficCitationRowRenderer OnRenderRowText;
        ListBox listBox;
        List<TrafficCitation> Citations;
        public TrafficCitationList(Base parent, List<TrafficCitation> reports, TrafficCitationSelected onSelected, TrafficCitationRowRenderer rowRenderer) : base(parent)
        {
            Citations = reports;
            listBox = new ListBox(this);
            listBox.Dock = Gwen.Pos.Fill;
            OnTrafficCitationSelected = onSelected;
            if (rowRenderer == null) OnRenderRowText = DefaultRowRender;
            else OnRenderRowText = rowRenderer;

            AddCitationsToList();
        }

        public void ChangeCitations(List<TrafficCitation> reports)
        {
            lock (reports)
            {
                this.Citations = reports.OrderByDescending(o => o.CitationTimeDate).ToList();
                AddCitationsToList();
            }
        }

        private void RowClicked(Base sender, ClickedEventArgs arguments)
        {
            var row = sender as ListBoxRow;
            if (row == null || row.UserData == null) return;
            var citation = row.UserData as TrafficCitation;
            if (citation != null && OnTrafficCitationSelected != null)
                OnTrafficCitationSelected(this, citation);
        }

        internal static void DefaultRowRender(TrafficCitation citation, ListBoxRow row)
        {
            row.Text = String.Format("{0} {1} {2} {3}", citation.ShortId(), citation.CitationDate, citation.CitationTime, citation.FullName);
        }

        private void AddCitationsToList(bool clearPrevious = true, bool selectFirstElementInList = false)
        {
            if (Citations == null)
            {
                return;
            }
            if (clearPrevious) listBox.Clear();

            foreach (var citation in Citations)
            {

                try
                {
                    var row = listBox.AddRow(String.Empty, citation.Id(), citation);
                    OnRenderRowText(citation, row);
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
                listBox.SelectRow(0, true);
            }

        }

    }

    class TrafficCitationListContainer : GwenForm
    {

        List<TrafficCitation> reports;
        private TrafficCitationList list;

        internal TrafficCitationList.TrafficCitationSelected OnTrafficCitationSelected;


        internal bool SelectFirstElementInList;

        public TrafficCitationList.ListItemClickType ListClickStyle
        {
            set
            {
                if (list != null) list.ListClickStyle = value;
            }
        }

        internal TrafficCitationListContainer(List<TrafficCitation> reports) : this()
        {
            this.reports = reports;
        }

        internal TrafficCitationListContainer() : base("Citations", Configs.BaseFormWidth, Configs.BaseFormHeight)
        {

        }
       
        public void DefaultOnRowSelect(object sender, TrafficCitation selected)
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            list = new TrafficCitationList(this, this.reports, OpenReport, TrafficCitationList.DefaultRowRender);
            list.Dock = Gwen.Pos.Fill;
        }





        private void OpenReport(object sender, TrafficCitation citation)
        {
            if (citation != null)
            {
                if (OnTrafficCitationSelected == null)
                {
                    ComputerReportsController.ShowTrafficCitationView(citation);
                }
                else
                {
                    OnTrafficCitationSelected(this, citation);
                }
            }

        }

        private void ReportRowClicked(Base sender, ItemSelectedEventArgs arguments)
        {
            var citation = arguments.SelectedItem.UserData as TrafficCitation;
            if (citation != null)
                OpenReport(this, citation);
            //list.SelectedRow = null;
        }
    }

}
