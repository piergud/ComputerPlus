using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Interfaces.Reports.ArrestReport;
using ComputerPlus.Extensions.Gwen;

namespace ComputerPlus.Controllers
{
    class ComputerReportsController
    {
        private static GameFiber CreateArrestReportContainer = new GameFiber(RunArrestReportCreate);

        private static void RunArrestReportCreate()
        {
            while (true)
            {
                var form = new ArrestReportContainer();
                Function.Log("ShowArrestReportCreate Show");
                form.Show();
                while (form.IsOpen())
                    GameFiber.Yield();
                form.Close();
                Function.Log("ShowArrestReportCreate Hibernating");
                GameFiber.Hibernate();
            }
        }

        public static void ShowArrestReportCreate()
        {
            if (IsCreateArrestReportContainerRunning) return;
            if (CreateArrestReportContainer.IsHibernating) CreateArrestReportContainer.Wake();
            CreateArrestReportContainer.Start();
        }

        internal static bool IsCreateArrestReportContainerRunning
        {
            get { return CreateArrestReportContainer.IsAlive && !CreateArrestReportContainer.IsHibernating; }
        }
    }
}
