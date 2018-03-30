using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen;
using GwenSkin = Gwen.Skin;
using Gwen.Control;
using Rage.Forms;
using ComputerPlus.Controllers.Models;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Interfaces.Reports.Arrest;
using ComputerPlus.Interfaces.Reports.Citation;
using ComputerPlus.Extensions;
using ComputerPlus.Extensions.Gwen;
using static ComputerPlus.Extensions.Gwen.TextBoxExtensions;
using ComputerPlus.Controllers;

namespace ComputerPlus.Interfaces.ComputerVehDB
{
    class ComputerVehicleViewExtended : Base
    {
        internal static int DefaultWidth = Configs.BaseFormWidth;
        internal static int DefaultHeight = Configs.BaseFormHeight * 2;
        DetailedEntity DetailedEntity;
        internal enum Page { VEHICLE_DETAILS = 0, PED_ARRESTS, PED_TRAFFIC_CITATIONS };

        ComputerVehicleDetails VehicleDetails;
        ArrestReportList arrestReportList;
        ArrestReportView arrestReportView;
        TrafficCitationList trafficCitationList;
        TrafficCitationView trafficCitationView;

        TabControl tabcontrol_details;
        DockBase arrestsContainer, trafficCitationContainer;

        internal delegate void PageTabChanged(object sender, Page page);
        internal event PageTabChanged OnPageTabChanged;

        internal ComputerVehicleViewExtended(Base parent, DetailedEntity entity, PageTabChanged onPageChangedCallback = null) : base(parent)
        {
            DetailedEntity = entity;
            if (onPageChangedCallback != null) OnPageTabChanged += onPageChangedCallback;
            InitializeLayout();            
        }

        internal void InitializeLayout()
        {
            VehicleDetails = new ComputerVehicleDetails(this, DetailedEntity, VehicleViewQuickActionSelected);
            VehicleDetails.Dock = Pos.Fill;
            tabcontrol_details = new TabControl(this);
            tabcontrol_details.Dock = Pos.Fill;

            arrestsContainer = new DockBase(this);
            arrestsContainer.Hide();

            trafficCitationContainer = new DockBase(this);
            trafficCitationContainer.Hide();

            var details = tabcontrol_details.AddPage("Details", VehicleDetails);
            details.UserData = Page.VEHICLE_DETAILS;
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
                case Page.VEHICLE_DETAILS:
                    height = DefaultHeight;
                    width = DefaultWidth;
                    break;
                case Page.PED_ARRESTS:
                    height = ArrestReportView.DefaultHeight;
                    width = ArrestReportView.DefaultWidth + 300;
                    break;
                case Page.PED_TRAFFIC_CITATIONS:
                    height =TrafficCitationView.DefaultHeight;
                    width = TrafficCitationView.DefaultWidth + 300; //200 is from  trafficCitationContainer.LeftDock.Width = 200; 
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
                        //Function.Log("AddArrestReportsTab with " + Arrests.Length.ToString());

                        arrestsContainer.Dock = Pos.Fill;
                        arrestsContainer.LeftDock.Width = 300;
                        arrestReportList = new ArrestReportList(arrestsContainer.LeftDock, DetailedEntity.Arrests, ChangeArrestReportDetailView, RenderArrestReportListBoxRow) { ListClickStyle = ArrestReportList.ListItemClickType.DOUBLE };
                        arrestReportView = new ArrestReportView(arrestsContainer, DetailedEntity.Arrests[0]);
                        arrestsContainer.Name = String.Empty;
                        arrestReportList.Dock = Pos.Fill;
                        arrestReportView.Dock = Pos.Fill;
                        //arrestReportView.SizeFull();                        
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
                        page.UserData = Page.PED_TRAFFIC_CITATIONS;
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



        private void VehicleViewQuickActionSelected(object sender, ComputerVehicleDetails.QuickActions action)
        {
            switch (action)
            {
                case ComputerVehicleDetails.QuickActions.BLIP_VEHICLE:
                    {
                        ComputerVehicleController.BlipVehicle(DetailedEntity.Entity.Vehicle, System.Drawing.Color.Yellow);
                        return;
                    }
                case ComputerVehicleDetails.QuickActions.CREATE_TRAFFIC_CITATION:
                    {                        
                        ComputerReportsController.ShowTrafficCitationCreate(Globals.PendingTrafficCitation, DetailedEntity.Entity, PedCreateTrafficCitationActions);
                        return;
                    }
                case ComputerVehicleDetails.QuickActions.CREATE_ARREST_REPORT_FOR_DRIVER:
                    {
                        ComputerReportsController.ShowArrestReportCreate(DetailedEntity.Entity, PedCreateArrestReportActions);
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
            if (!DetailedEntity.TrafficCitations.Contains(citation)) DetailedEntity.TrafficCitations.Add(citation);
            Globals.AddTrafficCitationsInHandForPed(DetailedEntity.Entity.Ped, citation);
            AddTrafficCitationsTab();

        }
    }

    class ComputerVehicleViewExtendedContainer : GwenForm
    {
        ComputerVehicleViewExtended VehicleView;
        DetailedEntity Entity;
        internal ComputerVehicleViewExtendedContainer(DetailedEntity entity) : base("Vehicle Details", ComputerVehicleViewExtended.DefaultWidth, ComputerVehicleViewExtended.DefaultHeight)
        {
            Entity = entity;
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            VehicleView = new ComputerVehicleViewExtended(this, Entity, OnPageChanged);
            VehicleView.Dock = Pos.Fill;

        }

        private void OnPageChanged(object sender, ComputerVehicleViewExtended.Page page)
        {

            this.Window.Width = VehicleView.Width;
            this.Window.Height = VehicleView.Height;
            this.Position = this.GetLaunchPosition();
        }
    }
}
