using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using ComputerPlus.Controllers.Models;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Interfaces.Reports.Arrest;
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
        ArrestReport[] Arrests;
        ComputerPedView pedView;
        ArrestReportList arrestReportList;
        ArrestReportView arrestReportView;
        TabControl tabcontrol_details;
        DockBase arrestsContainer;

        enum Page { PED_DETAILS, ARRESTS, TICKETS };

        private static int DefaultWidth = Configs.BaseFormWidth;
        private static int DefaultHeight = Configs.BaseFormHeight * 2;

        private ComputerPedViewExtended(ComputerPlusEntity entity) : base(entity.FullName, DefaultWidth, DefaultHeight)
        {
            Entity = entity;
        }

        public ComputerPedViewExtended(ComputerPlusEntity entity, ArrestReport[] arrests) : this(entity)
        {
            this.Arrests = arrests;
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

            var details = tabcontrol_details.AddPage("Details", pedView);
            details.UserData = Page.PED_DETAILS;
            details.Clicked += PageTabClicked;


            AddArrestReportsTab();
        }

        private void PageTabClicked(Base sender, ClickedEventArgs arguments)
        {
            var page = (Page)sender.UserData;
            switch(page)
            {
                case Page.PED_DETAILS:
                    this.Window.Height = DefaultHeight;
                    break;
                case Page.ARRESTS:
                    this.Window.Height = ArrestReportView.DefaultHeight;
                    break;
            }
            this.Position = this.GetLaunchPosition();
            
        }

        private void AddArrestReportsTab()
        {
            lock (Arrests)
            {
                if (Arrests.Length > 0)
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

     

        private void ChangeArrestReportDetailView(ArrestReport report)
        {
            if (arrestReportView != null && report != null)
            {
                arrestReportView.ChangeReport(report);
            }
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
            }
        }

        private void PedCreateArrestReportActions(object sender, ArrestReportContainer.ArrestReportSaveResult action, ArrestReport report)
        {
            if (action == ArrestReportContainer.ArrestReportSaveResult.SAVE)
            {
                if (!Arrests.Contains(report))
                {
                    var nArrests = new ArrestReport[Arrests.Length + 1];
                    Arrests.CopyTo(nArrests, 0);
                    nArrests[Arrests.Length] = report;
                    Arrests = nArrests;
                }
                else
                {
                    Arrests = Arrests.Select(x => x.id == report.id ? report : x).ToArray();
                }
                AddArrestReportsTab();
            }
        }

        private void RenderArrestReportListBoxRow(ArrestReport report, ListBoxRow row)
        {
            var dateString = report.ArrestTimeDate.ToLocalTimeString(DateOutputPart.DATE);
            row.Text = String.Format("{0} Charges: {1}", dateString, report.Charges.Count);
            row.SetToolTipText(String.Format("Arrested {0}, {1} charges with {2} felony", dateString, report.Charges.Count, report.Charges.Count(x => x.IsFelony)));
        }


    }
}
