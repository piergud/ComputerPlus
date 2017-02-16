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

        private ComputerPedViewExtended(ComputerPlusEntity entity) : base(entity.FullName, Configs.BaseFormWidth, Configs.BaseFormHeight * 2)
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
            
            
            tabcontrol_details.AddPage("Details", pedView);
            AddArrestReportsTab();
        }

        private void AddArrestReportsTab()
        {
            if (Arrests.Length > 0)
            {
                if (arrestReportList == null)
                {
                    //Function.Log("AddArrestReportsTab with " + Arrests.Length.ToString());
                    var arrestsContainer = new DockBase(this);
                    arrestsContainer.Dock = Pos.Fill;
                    arrestsContainer.LeftDock.Width = 200;
                    arrestsContainer.RightDock.Width = this.Window.Width - arrestsContainer.LeftDock.Width;
                    arrestReportList = new ArrestReportList(arrestsContainer.LeftDock, Arrests, ChangeArrestReportDetailView, RenderArrestReportListBoxRow);
                    arrestReportView = new ArrestReportView(arrestsContainer, Arrests[0]);
                    arrestReportList.Dock = Pos.Fill;
                    arrestReportView.Dock = Pos.Fill;
                    tabcontrol_details.AddPage("Arrests", arrestsContainer);
                }
                else
                {
                    arrestReportList.ChangeReports(Arrests);
                    arrestReportView.ChangeReport(Arrests[0]);
                }
            }
        }


        private void ChangeArrestReportDetailView(ArrestReport report)
        {
            Function.Log("ChangeArrestReportDetailView start");
            if (arrestReportView != null && report != null)
            {
                Function.Log("ChangeArrestReportDetailView valid");
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
                    Function.Log("PedCreateArrestReportActions with " + report.Charges.Count.ToString());
                    var nArrests = new ArrestReport[Arrests.Length + 1];
                    Arrests.CopyTo(nArrests, 0);
                    nArrests[Arrests.Length] = report;
                    Arrests = nArrests;
                }
                else
                {
                    Function.Log("Update PedCreateArrestReportActions with " + report.Charges.Count.ToString());
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
