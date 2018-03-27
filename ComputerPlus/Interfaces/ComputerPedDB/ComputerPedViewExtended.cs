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
    class ComputerPedViewExtended : Base
    {

        internal static int DefaultWidth = Configs.BaseFormWidth;
        internal static int DefaultHeight = Configs.BaseFormHeight + 180;
        DetailedEntity DetailedEntity;
        internal enum Page { PED_DETAILS = 0, PED_ARRESTS, TRAFFIC_CITATIONS };

        ComputerPedView pedView;
        ArrestReportList arrestReportList;
        ArrestReportView arrestReportView;
        TrafficCitationList trafficCitationList;
        TrafficCitationView trafficCitationView;

        TabControl tabcontrol_details;
        DockBase arrestsContainer, trafficCitationContainer;

        internal delegate void PageTabChanged(object sender, Page page);
        internal event PageTabChanged OnPageTabChanged;

        internal ComputerPedViewExtended(Base parent, DetailedEntity entity, PageTabChanged onPageChangedCallback = null) : base(parent)
        {
            DetailedEntity = entity;
            if (onPageChangedCallback != null) OnPageTabChanged += onPageChangedCallback;
            InitializeLayout();
        }

        internal void InitializeLayout()
        {
            Function.LogDebug("Creating ComputerPedView");

            pedView = new ComputerPedView(this, DetailedEntity, PedViewQuickActionSelected);            
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
            int width, height;
            switch (page)
            {
                case Page.PED_DETAILS:
                    height = DefaultHeight;
                    width = DefaultWidth;
                    break;
                case Page.PED_ARRESTS:
                    height = ArrestReportView.DefaultHeight;
                    width = ArrestReportView.DefaultWidth + 300;
                    break;
                case Page.TRAFFIC_CITATIONS:
                    height = TrafficCitationView.DefaultHeight;
                    width = TrafficCitationView.DefaultWidth + 300; //300 is from  trafficCitationContainer.LeftDock.Width = 300; 
                    break;
                default: return;
            }
            this.SetSize(width, height);
            OnPageTabChanged(this, page);

        }

        private void AddArrestReportsTab()
        {
            if (DetailedEntity.Arrests == null) return;
            lock (DetailedEntity.Arrests)
            {
                if (DetailedEntity.Arrests.Count > 0)
                {
                    if (arrestsContainer.Children.Count == 0)
                    {
                        arrestsContainer.Dock = Pos.Fill;
                        arrestsContainer.LeftDock.Width = 300;
                        arrestReportList = new ArrestReportList(arrestsContainer.LeftDock, DetailedEntity.Arrests, ChangeArrestReportDetailView, RenderArrestReportListBoxRow) { ListClickStyle = ArrestReportList.ListItemClickType.DOUBLE };
                        arrestReportView = new ArrestReportView(arrestsContainer, DetailedEntity.Arrests[0]);
                        arrestsContainer.Name = String.Empty;
                        arrestReportList.Dock = Pos.Fill;
                        arrestReportView.Dock = Pos.Fill;
                        arrestsContainer.Show();
                        var page = tabcontrol_details.AddPage("Arrests", arrestsContainer);
                        page.UserData = Page.PED_ARRESTS;
                        page.Clicked += PageTabClicked;
                    }
                    else
                    {
                        arrestReportList.ChangeReports(DetailedEntity.Arrests);
                        arrestReportView.ChangeReport(DetailedEntity.Arrests[0]);
                    }
                }
            }
        }

        private void AddTrafficCitationsTab()
        {
            if (DetailedEntity.TrafficCitations == null) return;
            lock (DetailedEntity.TrafficCitations)
            {
                if (DetailedEntity.TrafficCitations.Count > 0)
                {
                    if (trafficCitationContainer.Children.Count == 0)
                    {
                        //Function.Log("AddArrestReportsTab with " + Arrests.Length.ToString());

                        trafficCitationContainer.Dock = Pos.Fill;
                        trafficCitationContainer.LeftDock.Width = 300;
                        trafficCitationList = new TrafficCitationList(trafficCitationContainer.LeftDock, DetailedEntity.TrafficCitations, ChangeTrafficCitationDetailView, RenderTrafficCitationListBoxRow) { ListClickStyle = TrafficCitationList.ListItemClickType.DOUBLE };
                        trafficCitationView = new TrafficCitationView(trafficCitationContainer, DetailedEntity.TrafficCitations.FirstOrDefault(), TrafficCitationView.ViewTypes.VIEW);
                        trafficCitationContainer.Name = String.Empty;
                        trafficCitationList.Dock = Pos.Fill;
                        trafficCitationView.Dock = Pos.Fill;
                        trafficCitationContainer.Show();

                        var page = tabcontrol_details.AddPage("Citations", trafficCitationContainer);
                        page.UserData = Page.TRAFFIC_CITATIONS;
                        page.Clicked += PageTabClicked;
                    }
                    else
                    {
                        trafficCitationList.ChangeCitations(DetailedEntity.TrafficCitations);
                        trafficCitationView.ChangeCitation(DetailedEntity.TrafficCitations.FirstOrDefault());
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
            row.UserData = report;
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
            row.UserData = citation;
        }



        private void PedViewQuickActionSelected(object sender, ComputerPedView.QuickActions action)
        {
            switch(action)
            {
                case ComputerPedView.QuickActions.CREATE_ARREST_REPORT:
                    {
                        ComputerReportsController.ShowArrestReportCreate(DetailedEntity.Entity, PedCreateArrestReportActions);
                        return;
                    }
                case ComputerPedView.QuickActions.CREATE_TRAFFIC_CITATION:
                    {
                        ComputerReportsController.ShowTrafficCitationCreate(Globals.PendingTrafficCitation, DetailedEntity.Entity, PedCreateTrafficCitationActions);
                        return;
                    }
            }
        }

        private void PedCreateArrestReportActions(object sender, ArrestReportContainer.ArrestReportSaveResult action, ArrestReport report)
        {
            if (action != ArrestReportContainer.ArrestReportSaveResult.SAVE) return;
            if (!DetailedEntity.Arrests.Contains(report)) DetailedEntity.Arrests.Add(report);
            AddArrestReportsTab();

        }

        private void PedCreateTrafficCitationActions(object sender, TrafficCitationView.TrafficCitationSaveResult action, TrafficCitation citation)
        {
            if (action != TrafficCitationView.TrafficCitationSaveResult.SAVE) return;
            Globals.AddTrafficCitationsInHandForPed(DetailedEntity.Entity.Ped, citation);
            if (!DetailedEntity.TrafficCitations.Contains(citation)) DetailedEntity.TrafficCitations.Add(citation);
            AddTrafficCitationsTab();

        }

    }

    class ComputerPedViewExtendedContainer : GwenForm
    {
        ComputerPedViewExtended PedView;
        DetailedEntity Entity;
        internal ComputerPedViewExtendedContainer(DetailedEntity entity) : base(entity.Entity.FullName, ComputerPedViewExtended.DefaultWidth, ComputerPedViewExtended.DefaultHeight)
        {
            Entity = entity;
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            Function.LogDebug("Creating ComputerPedViewExtended");
            PedView = new ComputerPedViewExtended(this, Entity, OnPageChanged);
            PedView.Dock = Pos.Fill;
        }

        private void OnPageChanged(object sender, ComputerPedViewExtended.Page page)
        {

            this.Window.Width = PedView.Width;
            this.Window.Height = PedView.Height;
            this.Position = this.GetLaunchPosition();
        }
    }
}
