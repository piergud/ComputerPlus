using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using ComputerPlus.Controllers.Models;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Interfaces.Reports.Arrest;
using ComputerPlus.Interfaces.Reports.Citation;
using GwenSkin = Gwen.Skin;
using Gwen;
using Rage.Forms;
using ComputerPlus.Controllers;
using ComputerPlus.Extensions;
using DateOutputPart = ComputerPlus.Extensions.Gwen.TextBoxExtensions.DateOutputPart;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus.Interfaces.ComputerPedDB
{
    class ComputerPedViewExtended : GwenForm
    {
        ComputerPlusEntity Entity;
        List<ArrestReport> Arrests;
        List<TrafficCitation> TrafficCitations;
        ComputerPedView pedView;
        ArrestReportList arrestReportList;
        ArrestReportView arrestReportView;
        TrafficCitationList trafficCitationList;
        TrafficCitationView trafficCitationView;
        TabControl tabcontrol_details;
        DockBase arrestsContainer, trafficCitationContainer;

        enum Page { PED_DETAILS, ARRESTS, TRAFFIC_CITATIONS };

        private static int DefaultWidth = Configs.BaseFormWidth;
        private static int DefaultHeight = Configs.BaseFormHeight * 2;

        private ComputerPedViewExtended(ComputerPlusEntity entity) : base(entity.FullName, DefaultWidth, DefaultHeight)
        {
            Entity = entity;
        }

        public ComputerPedViewExtended(PedReport report) : this(report.Entity)
        {
            this.Arrests = report.Arrests != null ? report.Arrests : new List<ArrestReport>();
            this.TrafficCitations = report.TrafficCitations != null ? report.TrafficCitations : new List<TrafficCitation>();
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            pedView = new ComputerPedView(this, Entity, PedViewQuickActionSelected);            
            pedView.Dock = Pos.Fill;
            tabcontrol_details = new TabControl(this);
            tabcontrol_details.Dock = Pos.Fill;

            arrestsContainer = new DockBase(this);
            arrestsContainer.Hide();

            trafficCitationContainer = new DockBase(this);
            trafficCitationContainer.Hide();

            var details = tabcontrol_details.AddPage("Details", pedView);
            details.UserData = Page.PED_DETAILS;
            details.Clicked += PageTabClicked;


            AddArrestReportsTab();
            AddTrafficCitationsTab();
        }

        private void PageTabClicked(Base sender, ClickedEventArgs arguments)
        {
            var page = (Page)sender.UserData;
            switch(page)
            {
                case Page.PED_DETAILS:
                    this.Window.Height = DefaultHeight;
                    this.Window.Width = DefaultWidth;
                    break;
                case Page.ARRESTS:
                    this.Window.Height = ArrestReportView.DefaultHeight;
                    this.Window.Width = DefaultWidth;
                    break;
                case Page.TRAFFIC_CITATIONS:
                    this.Window.Height = TrafficCitationView.DefaultHeight;
                    this.Window.Width = TrafficCitationView.DefaultWidth + 200; //200 is from  trafficCitationContainer.LeftDock.Width = 200; 
                    break;
            }
            this.Position = this.GetLaunchPosition();
            
        }

        private void AddArrestReportsTab()
        {
            if (Arrests == null) return;
            lock (Arrests)
            {
                if (Arrests.Count > 0)
                {
                    if (arrestsContainer.Children.Count == 0)
                    {
                        //Function.Log("AddArrestReportsTab with " + Arrests.Length.ToString());
                        
                        arrestsContainer.Dock = Pos.Fill;
                        arrestsContainer.LeftDock.Width = 200;
                        arrestsContainer.RightDock.Width = arrestsContainer.Width - arrestsContainer.LeftDock.Width;
                        arrestReportList = new ArrestReportList(arrestsContainer.LeftDock, Arrests, ChangeArrestReportDetailView, RenderArrestReportListBoxRow);
                        arrestReportView = new ArrestReportView(arrestsContainer, Arrests[0]);
                        arrestsContainer.Name = String.Empty;
                        arrestReportList.Dock = Pos.Fill;
                        arrestReportView.Dock = Pos.Fill;
                        //arrestReportView.SizeFull();                        
                        arrestsContainer.Show();
                        var page = tabcontrol_details.AddPage("Arrests", arrestsContainer);
                        page.UserData = Page.ARRESTS;
                        page.Clicked += PageTabClicked;

                    }
                    else
                    {
                        arrestReportList.ChangeReports(Arrests);
                        arrestReportView.ChangeReport(Arrests[0]);
                    }
                }
            }
        }

        private void AddTrafficCitationsTab()
        {
            if (TrafficCitations == null) return;
            lock (TrafficCitations)
            {
                if (TrafficCitations.Count > 0)
                {
                    if (trafficCitationContainer.Children.Count == 0)
                    {
                        //Function.Log("AddArrestReportsTab with " + Arrests.Length.ToString());

                        trafficCitationContainer.Dock = Pos.Fill;
                        trafficCitationContainer.LeftDock.Width = 200;
                        trafficCitationList = new TrafficCitationList(trafficCitationContainer.LeftDock, TrafficCitations, ChangeTrafficCitationDetailView, RenderTrafficCitationListBoxRow) { ListClickStyle = TrafficCitationList.ListItemClickType.DOUBLE };
                        trafficCitationView = new TrafficCitationView(trafficCitationContainer, TrafficCitations.FirstOrDefault(), TrafficCitationView.ViewTypes.VIEW);
                        trafficCitationContainer.Name = String.Empty;
                        trafficCitationList.Dock = Pos.Fill;
                        trafficCitationView.Dock = Pos.Fill;
                        trafficCitationContainer.Show();

                        var page = tabcontrol_details.AddPage("Traffic Citations", trafficCitationContainer);
                        page.UserData = Page.TRAFFIC_CITATIONS;
                        page.Clicked += PageTabClicked;

                    }
                    else
                    {
                        trafficCitationList.ChangeCitations(TrafficCitations);
                        trafficCitationView.ChangeCitation(TrafficCitations.FirstOrDefault());
                    }
                }
            }
        }



        private void ChangeArrestReportDetailView(ArrestReport report)
        {
            if (arrestReportView != null && report != null)
            {
                arrestReportView.ChangeReport(report);
            }
        }


        private void RenderArrestReportListBoxRow(ArrestReport report, ListBoxRow row)
        {
            var dateString = report.ArrestTimeDate.ToLocalTimeString(DateOutputPart.DATE);
            row.Text = String.Format("{0} Charges: {1}", dateString, report.Charges.Count);
            row.SetToolTipText(String.Format("Arrested {0}, {1} charges with {2} felony", dateString, report.Charges.Count, report.Charges.Count(x => x.IsFelony)));
        }

        private void ChangeTrafficCitationDetailView(object sender, TrafficCitation citation)
        {
            if (trafficCitationView != null && citation != null)
            {
                trafficCitationView.ChangeCitation(citation);
            }
        }


        private void RenderTrafficCitationListBoxRow(TrafficCitation citation, ListBoxRow row)
        {
            var dateString = citation.CitationTimeDate.ToLocalTimeString(DateOutputPart.DATE);
            row.Text = String.Format("{0}: {1}", dateString, citation.CitationReason);
            row.SetToolTipText(String.Format("Fine {0}", citation.CitationAmount));
        }



        private void PedViewQuickActionSelected(object sender, ComputerPedView.QuickActions action)
        {
            switch(action)
            {
                case ComputerPedView.QuickActions.CREATE_ARREST_REPORT:
                    {
                        ComputerReportsController.ShowArrestReportCreate(Entity, PedCreateArrestReportActions);
                        return;
                    }
                case ComputerPedView.QuickActions.CREATE_TRAFFIC_CITATION:
                    {
                        ComputerReportsController.ShowTrafficCitationCreate(null, Entity, PedCreateTrafficCitationActions);
                        return;
                    }
            }
        }

        private void PedCreateArrestReportActions(object sender, ArrestReportContainer.ArrestReportSaveResult action, ArrestReport report)
        {
            Arrests.Add(report);
            AddArrestReportsTab();

        }

        private void PedCreateTrafficCitationActions(object sender, TrafficCitationView.TrafficCitationSaveResult action, TrafficCitation citation)
        {
            TrafficCitations.Add(citation);
            AddTrafficCitationsTab();

        }

    }
}
