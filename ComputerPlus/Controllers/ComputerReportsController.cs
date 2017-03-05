using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Interfaces.Reports.Arrest;
using ComputerPlus.Interfaces.Reports.Citation;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Interfaces.Reports.Models;
using SQLiteNetExtensions;
using SQLiteNetExtensionsAsync.Extensions;
using ComputerPlus.DB;
using CodeEngine.Framework.QueryBuilder;
using QueryEnum = CodeEngine.Framework.QueryBuilder.Enums;
using ComputerPlus.Controllers.Models;
using ComputerPlus.Extensions;
using SystemThreading = System.Threading.Tasks;

namespace ComputerPlus.Controllers
{
    class ComputerReportsController
    {     
        public static void ShowArrestReportCreate()
        {
            if (Globals.PendingArrestReport == null)
                Globals.PendingArrestReport = new ArrestReport();
            Globals.Navigation.Push(new ArrestReportContainer(Globals.PendingArrestReport));
        }

        public static void ShowArrestReportCreate(ComputerPlusEntity entity, ArrestReportContainer.ArrestReportActionEvent callbackDelegate)
        {
            ArrestReport report;
            var container = ArrestReportContainer.CreateForPed(entity, out report);
            if (callbackDelegate != null) container.OnArrestReportAction += callbackDelegate;
            Globals.PendingArrestReport = report;
            Globals.Navigation.Push(container);
        }

        public static async SystemThreading.Task ShowArrestReportView(ArrestReport report)
        {
            await PopulateArrestLineItems(report);
            //Globals.Navigation.Push(new ArrestReportContainer(report));
            Globals.Navigation.Push(new ArrestReportViewContainer(report));
        }

        public static async void ShowArrestReportList()
        {
            var reports = await ComputerReportsController.GetAllArrestReportsAsync(0, 0);
            if (reports == null) Function.Log("Reports is null");
            else if (Globals.Navigation == null) Function.Log("Global nav is null");
            else
            Globals.Navigation.Push(new ArrestReportListContainer(reports));
        }


        public static async Task<ArrestReport> SaveArrestRecordAsync(ArrestReport report)
        {
            try
            {
                if (report.IsNew)
                {
                    report.id = Guid.NewGuid();
                    await Globals.Store.Connection().InsertWithChildrenAsync(report, true);
                }
                else
                {
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
                var results = await Globals.Store.Connection().QueryAsync<ArrestReport>(query.BuildQuery());
                return results != null ? results : new List<ArrestReport>();
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return null;
            }
        }

        public static async Task<List<ArrestReport>> GetArrestReportsForPedAsync(ComputerPlusEntity entity)
        {
            return await GetArrestReportsForPedAsync(entity.FirstName, entity.LastName, entity.PedPersona.BirthDay.ToLocalTimeString(TextBoxExtensions.DateOutputPart.DATE));
        }

        public static async Task<List<ArrestReport>> GetArrestReportsForPedAsync(String firstName, String lastName, String dob)
        {
            try
            {
                firstName = firstName.Trim();
                lastName = lastName.Trim();
                dob = dob.Trim();
                //var query = new SelectQueryBuilder();
                //query.SelectAllColumns();
                //query.SelectFromTable(DB.Storage.Tables.Names.ArrestReport);
                //query.AddWhere(DB.Storage.Tables.ArrestReport.FIRST_NAME, QueryEnum.Comparison.Equals, firstName);
                //query.AddWhere(DB.Storage.Tables.ArrestReport.LAST_NAME, QueryEnum.Comparison.Equals, lastName);
                //query.AddWhere(DB.Storage.Tables.ArrestReport.DOB, QueryEnum.Comparison.Equals, dob);
                return await Globals.Store.Connection().GetAllWithChildrenAsync<ArrestReport>(report => report.FirstName == firstName && report.LastName == lastName && report.DOB == dob, true);
                
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
                await Globals.Store.Connection().GetChildrenAsync(report, true);

                return true;
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return false;
            }
        }

        /** Traffic Citations **/

        public static async void ShowTrafficCitationList()
        {
            var citations = await ComputerReportsController.GetAllTrafficCitationsAsync(0, 0);
            if (citations == null) Function.Log("Citations are null");
            else if (Globals.Navigation == null) Function.Log("Global nav is null");
            else
                Globals.Navigation.Push(new TrafficCitationListContainer(citations));
        }

        public static void ShowTrafficCitationView(TrafficCitation citation)
        {
            Globals.Navigation.Push(new TrafficCitationCreateContainer(citation, TrafficCitationView.ViewTypes.VIEW)); //Switch to view
        }

        public static void ShowTrafficCitationCreate(TrafficCitation citation, ComputerPlusEntity entity = null, TrafficCitationView.TrafficCitationActionEvent callbackDelegate = null)
        {
            TrafficCitation mCitation = null;
            if (citation == null && entity == null)
            {
                mCitation = Globals.PendingTrafficCitation != null ? Globals.PendingTrafficCitation : new TrafficCitation();
            }
            else if (citation == null && entity.Validate())
            {
                Function.Log("Creating Traffic Citation for ped in vehicle");
                mCitation = TrafficCitation.CreateForPedInVehicle(entity);
                Globals.PendingTrafficCitation = mCitation;
                Function.Log("Created Traffic Citation for ped  " + mCitation.FirstName);
            }
            else
            {
                mCitation = citation;
            }
            Globals.Navigation.Push(new TrafficCitationCreateContainer(mCitation, TrafficCitationView.ViewTypes.CREATE, callbackDelegate));
        }

        public static async Task<TrafficCitation> SaveTrafficCitationAsync(TrafficCitation citation)
        {
            try
            {
                if (citation.IsNew)
                {
                    citation.id = Guid.NewGuid();
                    await Globals.Store.Connection().InsertWithChildrenAsync(citation, true);                    

                }
                else
                {
                    await Globals.Store.Connection().UpdateWithChildrenAsync(citation);
                }
                if (Globals.PendingTrafficCitation != null && Globals.PendingTrafficCitation == citation) Globals.PendingTrafficCitation = null;
                return citation;
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                throw e;
            }
        }


        public static async Task<List<TrafficCitation>> GetAllTrafficCitationsAsync(int skip = 0, int limit = 20, String orderCol = "", String orderDir = "ASC")
        {
            try
            {
                orderCol = String.IsNullOrWhiteSpace(orderCol) ? DB.Storage.Tables.TrafficCitation.CITATION_TIME_DATE : orderCol;
                orderDir = String.IsNullOrWhiteSpace(orderDir) ? "ASC" : orderDir;
                var query = new SelectQueryBuilder();
                query.SelectAllColumns();
                query.SelectFromTable(DB.Storage.Tables.Names.TrafficCitation);
                query.AddOrderBy(orderCol, QueryEnum.Sorting.Descending);
                var results = await Globals.Store.Connection().QueryAsync<TrafficCitation>(query.BuildQuery());
                return results != null ? results : new List<TrafficCitation>();
            }
            catch (Exception e)
            {
                Function.Log("Non fatal catch: " + e.ToString());
                return new List<TrafficCitation>();
            }
        }

        public static async Task<List<TrafficCitation>> GetTrafficCitationsForPedAsync(ComputerPlusEntity entity)
        {
            return await GetTrafficCitationsForPedAsync(entity.FirstName, entity.LastName, entity.PedPersona.BirthDay.ToLocalTimeString(TextBoxExtensions.DateOutputPart.DATE));
        }

        public static async Task<List<TrafficCitation>> GetTrafficCitationsForPedAsync(String firstName, String lastName, String dob)
        {
            try
            {
                firstName = firstName.Trim();
                lastName = lastName.Trim();
                dob = dob.Trim();
                return await Globals.Store.Connection().GetAllWithChildrenAsync<TrafficCitation>(report => report.FirstName == firstName && report.LastName == lastName && report.DOB == dob, true);

            }
            catch (Exception e)
            {
                Function.Log("Non fatal catch: " + e.ToString());
                return new List<TrafficCitation>();
            }
        }
       

        public static async Task<DetailedEntity> GetAllReportsForPedAsync(ComputerPlusEntity entity)
        {
            var arrests = await GetArrestReportsForPedAsync(entity);
            var traffic = await GetTrafficCitationsForPedAsync(entity);
            return new DetailedEntity(entity, arrests, traffic);
        }
    }
}
