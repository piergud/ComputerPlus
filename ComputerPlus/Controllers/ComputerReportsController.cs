using System;
using System.Collections.Generic;
using ComputerPlus.Interfaces.Reports.Arrest;
using ComputerPlus.Interfaces.Reports.Citation;
using ComputerPlus.Interfaces.Reports.Models;
using CodeEngine.Framework.QueryBuilder;
using QueryEnum = CodeEngine.Framework.QueryBuilder.Enums;
using ComputerPlus.Controllers.Models;
using SQLiteNetExtensions.Extensions;
using LSPD_First_Response.Mod.API;
using ComputerPlus.Interfaces.ComputerPedDB;
using System.Linq;

namespace ComputerPlus.Controllers
{
    class ComputerReportsController
    {
        private static int SECONDS_IN_A_DAY = 60 * 60 * 24;

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

        public static void ShowArrestReportView(ArrestReport report)
        {
            PopulateArrestLineItems(report);
            //Globals.Navigation.Push(new ArrestReportContainer(report));
            Globals.Navigation.Push(new ArrestReportViewContainer(report));
        }

        public static void ShowArrestReportList()
        {
            var reports = ComputerReportsController.GetAllArrestReportsAsync(0, 0);
            if (reports == null) Function.Log("Reports is null");
            else if (Globals.Navigation == null) Function.Log("Global nav is null");
            else
            Globals.Navigation.Push(new ArrestReportListContainer(reports));
        }


        public static ArrestReport SaveArrestRecordAsync(ArrestReport report)
        {
            try
            {
                if (report.IsNew)
                {
                    report.id = Guid.NewGuid();
                    Globals.Store.Connection().InsertWithChildren(report, true);
                }
                else
                {
                    Globals.Store.Connection().InsertOrReplaceWithChildren(report, true);
                }
                    
                return report;
            }
            catch(Exception e)
            {
                Function.LogCatch(e.ToString());
                return null;
            }
        }

        
        public static List<ArrestReport> GetAllArrestReportsAsync(int skip = 0, int limit = 20, String orderCol = "", String orderDir = "ASC")
        {
            try
            {
                orderCol = String.IsNullOrWhiteSpace(orderCol) ? DB.Storage.Tables.ArrestReport.ARREST_TIME : orderCol;
                orderDir = String.IsNullOrWhiteSpace(orderDir) ? "ASC" : orderDir;
                var query = new SelectQueryBuilder();
                query.SelectAllColumns();
                query.SelectFromTable(DB.Storage.Tables.Names.ArrestReport);
                query.AddOrderBy(orderCol, QueryEnum.Sorting.Descending);
                var results = Globals.Store.Connection().Query<ArrestReport>(query.BuildQuery());
                return results != null ? results : new List<ArrestReport>();
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
                return null;
            }
        }

        public static List<ArrestReport> GetArrestReportsForPedAsync(ComputerPlusEntity entity)
        {
            return GetArrestReportsForPedAsync(entity.FirstName, entity.LastName, entity.DOBString);
        }

        public static List<ArrestReport> GetArrestReportsForPedAsync(String firstName = "", String lastName = "", String dob = "")
        {
            try
            {
                firstName = firstName.Trim();
                lastName = lastName.Trim();
                dob = dob.Trim();
                return Globals.Store.Connection()
                    .GetAllWithChildren<ArrestReport>(report => report.FirstName.Equals(firstName) 
                        && report.LastName.Equals(lastName) && report.DOB.Equals(dob), true)
                        .OrderByDescending(o => o.ArrestTimeDate).ToList(); ;
            }
            catch (Exception e)
            {
                //Function.LogCatch(e.ToString());
                return new List<ArrestReport>();
            }
        }

        public static bool PopulateArrestLineItems(ArrestReport report)
        {
            try
            {
                Globals.Store.Connection().GetChildren(report, true);

                return true;
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
                return false;
            }
        }

        /** Traffic Citations **/

        public static void ShowTrafficCitationList()
        {
            var citations = GetAllTrafficCitationsAsync(0, 0);
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
            else if (citation != null && citation == Globals.PendingTrafficCitation && !citation.FullName.Equals(entity.FullName))
            {
                mCitation = new TrafficCitation();
                Globals.PendingTrafficCitation = mCitation;
            }
            else
            {
                mCitation = citation;
            }
            Globals.Navigation.Push(new TrafficCitationCreateContainer(mCitation, TrafficCitationView.ViewTypes.CREATE, callbackDelegate));
        }

        public static TrafficCitation SaveTrafficCitationAsync(TrafficCitation citation)
        {
            try
            {
                if (citation.IsNew)
                {
                    citation.id = Guid.NewGuid();
                    Globals.Store.Connection().InsertWithChildren(citation, true);                    

                }
                else
                {
                    Globals.Store.Connection().UpdateWithChildren(citation);
                }
                if (Globals.PendingTrafficCitation != null && Globals.PendingTrafficCitation == citation) Globals.PendingTrafficCitation = null;
                return citation;
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
                throw e;
            }
        }


        public static List<TrafficCitation> GetAllTrafficCitationsAsync(int skip = 0, int limit = 20, String orderCol = "", String orderDir = "ASC")
        {
            try
            {
                orderCol = String.IsNullOrWhiteSpace(orderCol) ? DB.Storage.Tables.TrafficCitation.CITATION_TIME_DATE : orderCol;
                orderDir = String.IsNullOrWhiteSpace(orderDir) ? "ASC" : orderDir;
                var query = new SelectQueryBuilder();
                query.SelectAllColumns();
                query.SelectFromTable(DB.Storage.Tables.Names.TrafficCitation);
                query.AddOrderBy(orderCol, QueryEnum.Sorting.Descending);
                var results = Globals.Store.Connection().Query<TrafficCitation>(query.BuildQuery());
                return results != null ? results : new List<TrafficCitation>();
            }
            catch (Exception e)
            {
                //Function.LogCatch(e.ToString());
                return new List<TrafficCitation>();
            }
        }

        private static TrafficCitation generateRandomCitation(ComputerPlusEntity entity)
        {
            TrafficCitation newCitation = TrafficCitation.CreateForPedInVehicle(entity);
            int randomSeconds = Globals.Random.Next(SECONDS_IN_A_DAY * 7, SECONDS_IN_A_DAY * 700) * -1;
            newCitation.CitationTimeDate = DateTime.Now.AddSeconds(randomSeconds);
            newCitation.CitationPos = Rage.World.GetRandomPositionOnStreet();
            newCitation.CitationStreetAddress = Rage.World.GetStreetName(newCitation.CitationPos);
            newCitation.CitationCity = Functions.GetZoneAtPosition(newCitation.CitationPos).RealAreaName;
            newCitation.Citation = ComputerPedController.GetRandomCitation();
            return SaveTrafficCitationAsync(newCitation);
        }

        public static List<TrafficCitation> GetTrafficCitationsForPedAsync(ComputerPlusEntity entity)
        {
            // check if ped has past citations
            List<TrafficCitation> pastCitationFromDB = GetTrafficCitationsForPedAsync(entity.FirstName, entity.LastName, entity.DOBString);
            if (entity.PedPersona.Citations > 0 && pastCitationFromDB.Count == 0)
            {
                // generate pastCitation
                for (var i = 0; i < entity.PedPersona.Citations; i++)
                {
                    pastCitationFromDB.Add(generateRandomCitation(entity));
                }
            }
            return pastCitationFromDB.OrderByDescending(o => o.CitationTimeDate).ToList();
        }

        public static List<TrafficCitation> GetTrafficCitationsForPedAsync(String firstName, String lastName, String dob)
        {
            try
            {
                firstName = firstName.Trim();
                lastName = lastName.Trim();
                dob = dob.Trim();
                return Globals.Store.Connection()
                    .GetAllWithChildren<TrafficCitation>(citation => citation.FirstName.Equals(firstName)
                        && citation.LastName.Equals(lastName) && citation.DOB.Equals(dob), true);
            }
            catch (Exception e)
            {
                //Function.LogCatch(e.ToString());
                return new List<TrafficCitation>();
            }
        }
       
        public static DetailedEntity GetAllReportsForPedAsync(ComputerPlusEntity entity)
        {            
            var arrests = GetArrestReportsForPedAsync(entity);
            var traffic = GetTrafficCitationsForPedAsync(entity);
            return new DetailedEntity(entity, arrests, traffic);
        }
    }
}
