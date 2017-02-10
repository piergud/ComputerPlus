using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Interfaces.Reports.Arrest;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Interfaces.Reports.Models;
using SQLiteNetExtensions;
using SQLiteNetExtensionsAsync.Extensions;
using ComputerPlus.DB;
using CodeEngine.Framework.QueryBuilder;
using QueryEnum = CodeEngine.Framework.QueryBuilder.Enums;

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

        public static async System.Threading.Tasks.Task ShowArrestReportView(ArrestReport report)
        {
            await PopulateArrestLineItems(report);
            //Globals.Navigation.Push(new ArrestReportContainer(report));
            Globals.Navigation.Push(new ArrestReportView(report));
        }

        public static async void ShowArrestReportList()
        {
            var reports = await ComputerReportsController.GetAllArrestReportsAsync(0, 0);
            Globals.Navigation.Push(new ArrestReportList(reports));
        }


        public static async Task<ArrestReport> SaveArrestRecordAsync(ArrestReport report)
        {
            try
            {
                if (report.IsNew)
                {
                    report.id = Guid.NewGuid();
                    Function.Log("Creating new arrest report");                    
                    await Globals.Store.Connection().InsertWithChildrenAsync(report, true);
                }
                else
                {
                    Function.Log(String.Format("Updating arrest report {0}", report.Id()));
                    await Globals.Store.Connection().InsertOrReplaceWithChildrenAsync(report, true);
                }
                    
                return report;
            }
            catch(Exception e)
            {
                Function.Log(e.ToString());
                return null;
            }
        }

        
        public static async Task<List<ArrestReport>> GetAllArrestReportsAsync(int skip = 0, int limit = 20, String orderCol = "", String orderDir = "ASC")
        {
            try
            {
                orderCol = String.IsNullOrWhiteSpace(orderCol) ? DB.Storage.Tables.ArrestReport.ARREST_TIME : orderCol;
                orderDir = String.IsNullOrWhiteSpace(orderDir) ? "ASC" : orderDir;
                var query = new SelectQueryBuilder();
                query.SelectAllColumns();
                query.SelectFromTable(DB.Storage.Tables.Names.ArrestReport);
                query.AddOrderBy(orderCol, QueryEnum.Sorting.Descending);
                Function.Log("query is " + query.BuildQuery());
                return await Globals.Store.Connection().QueryAsync<ArrestReport>(query.BuildQuery());
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return null;
            }
        }

        public static async Task<List<ArrestReport>> GetArrestReportsForPedAsync(String firstName, String lastName, String dob)
        {
            try
            {
                firstName = firstName.Trim();
                lastName = lastName.Trim();
                dob = dob.Trim();
                var query = new SelectQueryBuilder();
                query.SelectAllColumns();
                query.SelectFromTable(DB.Storage.Tables.Names.ArrestReport);
                query.AddWhere(DB.Storage.Tables.ArrestReport.FIRST_NAME, QueryEnum.Comparison.Equals, firstName);
                query.AddWhere(DB.Storage.Tables.ArrestReport.LAST_NAME, QueryEnum.Comparison.Equals, lastName);
                query.AddWhere(DB.Storage.Tables.ArrestReport.DOB, QueryEnum.Comparison.Equals, dob);
                return await Globals.Store.Connection().QueryAsync<ArrestReport>(query.BuildQuery());
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return null;
            }
        }

        public static async Task<bool> PopulateArrestLineItems(ArrestReport report)
        {
            try
            {
                Function.Log(String.Format("PopulateArrestLineItems charges {0}", report.Charges.Count));
                Function.Log(String.Format("PopulateArrestLineItems additional parties {0}", report.AdditionalParties.Count));
                await Globals.Store.Connection().GetChildrenAsync(report, true);
                Function.Log(String.Format("PopulateArrestLineItems after charges {0}", report.Charges.Count));
                Function.Log(String.Format("PopulateArrestLineItems after additional parties {0}", report.AdditionalParties.Count));

                return true;
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return false;
            }
        }


    }
}
