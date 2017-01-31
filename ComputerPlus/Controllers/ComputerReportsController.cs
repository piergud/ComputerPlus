using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Interfaces.Reports.Arrest;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.DB.Tables;

namespace ComputerPlus.Controllers
{
    class ComputerReportsController
    {     
        public static void ShowArrestReportCreate()
        {
            if (Globals.PendingArrestReport == null)
                Globals.PendingArrestReport = new Interfaces.Reports.Models.ArrestReport();
            Globals.Navigation.Push(new ArrestReportContainer(Globals.PendingArrestReport));
        }

        public static void ShowArrestReportView(ArrestReport report)
        {
            var charges = report.Charges;
            Function.Log(String.Format("report charge count {0}", charges.Count));
            Globals.Navigation.Push(new ArrestReportContainer(report));
        }

        public static void ShowArrestReportList()
        {
            Globals.Navigation.Push(new ArrestReportList());
        }

        public static ArrestReport CreateArrestRecord(ArrestReport report)
        {
            try
            {
                using (var connection = Globals.Store.Connection())
                {
                    ArrestReportTable arrestReportDb = new ArrestReportTable(connection);
                    arrestReportDb.Insert(report);
                    return report;
                }
            }
            catch(Exception e)
            {
                Function.Log(e.ToString());
                return null;
            }
        }

        public static List<ArrestReport> GetAllArrestReports(int skip = 0, int limit = 20, String order = "")
        {
            try
            {
                using (var connection = Globals.Store.Connection())
                {
                    ArrestReportTable arrestReportDb = new ArrestReportTable(connection);
                    order = String.IsNullOrWhiteSpace(order) ? String.Format("{0} DESC", ArrestReportTable.ARREST_TIME) : "";
                    return arrestReportDb.Browse(order, String.Format("{0}, {1}", skip, limit));
                }
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return null;
            }
        }

        public static List<ArrestReport> GetArrestReportsForPed(String firstName, String lastName, String dob)
        {
            try
            {
                using (var connection = Globals.Store.Connection())
                {
                    ArrestReportTable arrestReportDb = new ArrestReportTable(connection);
                    return arrestReportDb.GetArrestReportsForPed(firstName, lastName, dob);
                }
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return null;
            }
        }

        public static bool PopulateArrestLineItems(ArrestReport report)
        {
            try
            {
                using (var connection = Globals.Store.Connection())
                {
                    ArrestReportLineItemTable arrestReportDb = new ArrestReportLineItemTable(connection);
                    arrestReportDb.PopulateArrestCharges(report);
                    return true;
                }
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return false;
            }
        }


    }
}
