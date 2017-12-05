using System;
using System.Collections.Generic;
using ComputerPlus.Interfaces.Reports.Arrest;
using ComputerPlus.Interfaces.Reports.Citation;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Controllers.Models;
using LSPD_First_Response.Mod.API;
using ComputerPlus.Interfaces.ComputerPedDB;
using System.Linq;
using System.Data.SQLite;
using Rage;
using ComputerPlus.DB;
using static ComputerPlus.Interfaces.Reports.Models.ArrestReportAdditionalParty;

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
            var reports = GetAllArrestReports(0, 100);
            if (reports == null) Function.Log("Reports is null");
            else if (Globals.Navigation == null) Function.Log("Global nav is null");
            else
            Globals.Navigation.Push(new ArrestReportListContainer(reports));
        }

        private static void insertArrestReport(ArrestReport report)
        {
            string sql = String.Format("insert into ArrestReport ("
                + "id, ArrestTime, FirstName, LastName, DOB, HomeAddress, ArrestStreetAddress, ArrestCity, Details)"
                + " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                report.id, report.ArrestTimeDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'"),
                report.FirstName, report.LastName, report.DOB, report.HomeAddress,
                report.ArrestStreetAddress, report.ArrestCity, report.Details);
            Globals.Store.ExecuteNonQuery(sql);
        }

        private static void updateArrestReport(ArrestReport report)
        {
            string sql = String.Format("update ArrestReport set "
                + "ArrestTime='{0}', FirstName='{1}', LastName='{2}', DOB='{3}', "
                + "HomeAddress='{4}', ArrestStreetAddress='{5}', ArrestCity='{6}', Details='{7}' "
                + "where id='{8}'",
                report.ArrestTimeDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'"),
                report.FirstName, report.LastName, report.DOB, report.HomeAddress,
                report.ArrestStreetAddress, report.ArrestCity, report.Details, report.id);
            Globals.Store.ExecuteNonQuery(sql);
        }

        private static void insertArrestReportLineItem(ArrestChargeLineItem charge, string arrestReportId)
        {
            string sql = String.Format("insert into ArrestReportLineItem ("
                + "id, Charge, FelonyLevel, Note, arrestReportId)"
                + " values ('{0}','{1}','{2}','{3}','{4}')",
                charge.id, charge.Charge, (charge.IsFelony ? '1' : '0'), charge.Note, arrestReportId);
            Globals.Store.ExecuteNonQuery(sql);
        }

        private static void insertArrestReportParties(ArrestReportAdditionalParty party, string arrestReportId)
        {
            string sql = String.Format("insert into ArrestReportAdditionalParty ("
                + "id, PartyType, FirstName, LastName, DOB, arrestReportId)"
                + " values ('{0}',{1},'{2}','{3}','{4}','{5}')",
                party.id, (int)party.PartyType, party.FirstName, party.LastName, party.DOB, arrestReportId);
            Globals.Store.ExecuteNonQuery(sql);
        }

        private static void clearArrestReportLineItem(string arrestReportId)
        {
            string sql = String.Format("delete from ArrestReportLineItem where arrestReportId='{0}'", arrestReportId);
            Globals.Store.ExecuteNonQuery(sql);
        }

        private static void clearArrestReportParties(string arrestReportId)
        {
            string sql = String.Format("delete from ArrestReportAdditionalParty where arrestReportId='{0}'", arrestReportId);
            Globals.Store.ExecuteNonQuery(sql);
        }

        public static ArrestReport SaveArrestRecord(ArrestReport report)
        {
            try
            {
                if (report.IsNew)
                {
                    report.id = Guid.NewGuid();

                    insertArrestReport(report);
                    if (report.Charges != null && report.Charges.Count > 0)
                    {
                        foreach (var charge in report.Charges)
                        {
                            insertArrestReportLineItem(charge, report.id.ToString());
                        }
                    }
                    if (report.AdditionalParties != null && report.AdditionalParties.Count > 0)
                    {
                        foreach (var party in report.AdditionalParties)
                        {
                            insertArrestReportParties(party, report.id.ToString());
                        }
                    }
                }
                else
                {
                    updateArrestReport(report);
                    if (report.Charges != null && report.Charges.Count > 0)
                    {
                        clearArrestReportLineItem(report.id.ToString());
                        foreach (var charge in report.Charges)
                        {
                            insertArrestReportLineItem(charge, report.id.ToString());
                        }
                    }
                    if (report.AdditionalParties != null && report.AdditionalParties.Count > 0)
                    {
                        clearArrestReportParties(report.id.ToString());
                        foreach (var party in report.AdditionalParties)
                        {
                            insertArrestReportParties(party, report.id.ToString());
                        }
                    }

                }
                return report;
            }
            catch(Exception e)
            {
                Function.LogCatch(e.ToString());
                return null;
            }
        }

        private static List<ArrestReport> fetchArrestReports(String sql)
        {
            var arrestReports = new List<ArrestReport>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(Storage.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var arrestReport = new ArrestReport();
                                arrestReport.id = new Guid((string)reader["id"]);
                                arrestReport.ChangeArrestTime(DateTime.Parse((string)reader["ArrestTime"]));
                                arrestReport.FirstName = (string)reader["FirstName"];
                                arrestReport.LastName = (string)reader["LastName"];
                                arrestReport.DOB = (string)reader["DOB"];
                                arrestReport.HomeAddress = (string)reader["HomeAddress"];
                                arrestReport.ArrestStreetAddress = (string)reader["ArrestStreetAddress"];
                                arrestReport.ArrestCity = (string)reader["ArrestCity"];
                                arrestReport.Details = (string)reader["Details"];

                                PopulateArrestReportCharges(arrestReport);
                                PopulateArrestParties(arrestReport);

                                arrestReports.Add(arrestReport);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
            return arrestReports;
        }

        private static void PopulateArrestReportCharges(ArrestReport arrestReport)
        {
            try
            {
                string sql = String.Format("select * from ArrestReportLineItem where arrestReportId = '{0}'", arrestReport.id);
                using (SQLiteConnection conn = new SQLiteConnection(Storage.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var charge = new ArrestChargeLineItem();
                                charge.id = new Guid((string)reader["id"]);
                                charge.Charge = (string)reader["Charge"];
                                charge.IsFelony = ((string)reader["FelonyLevel"]).Equals("1") ? true : false;
                                charge.Note = (string)reader["Note"];
                                charge.ReportId = new Guid((string)reader["arrestReportId"]);
                                arrestReport.Charges.Add(charge);
                            }
                        }
                    }
                    conn.Close();
                }
             }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
        }

        private static void PopulateArrestParties(ArrestReport arrestReport)
        {
            try
            {
                string sql = String.Format("select * from ArrestReportAdditionalParty where arrestReportId = '{0}'", arrestReport.id);
                using (SQLiteConnection conn = new SQLiteConnection(Storage.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var party = new ArrestReportAdditionalParty();
                                party.id = new Guid((string)reader["id"]);
                                party.PartyType = (PartyTypes) reader.GetInt32(1);
                                party.FirstName = (string)reader["FirstName"];
                                party.LastName = (string)reader["LastName"];
                                party.DOB = (string)reader["DOB"];
                                party.ReportId = new Guid((string)reader["arrestReportId"]);
                                arrestReport.AdditionalParties.Add(party);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
        }

        public static List<ArrestReport> GetAllArrestReports(int skip = 0, int limit = 100, String orderCol = "", String orderDir = "ASC")
        {
            string sql = String.Format("select * from ArrestReport order by ArrestTime desc limit {0}", limit);
            return fetchArrestReports(sql); ;
        }

        public static List<ArrestReport> GetArrestReportsForPed(ComputerPlusEntity entity)
        {
            return GetArrestReportsForPed(entity.FirstName, entity.LastName, entity.DOBString);
        }

        public static List<ArrestReport> GetArrestReportsForPed(String firstName = "", String lastName = "", String dob = "")
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            dob = dob.Trim();
            string sql = String.Format("select * from ArrestReport where FirstName = '{0}' and LastName = '{1}' "
                + "and DOB = '{2}' order by ArrestTime desc", firstName, lastName, dob);
            return fetchArrestReports(sql);
        }

        public static bool PopulateArrestLineItems(ArrestReport report)
        {
            PopulateArrestReportCharges(report);
            return true;
        }

        /** Traffic Citations **/

        public static void ShowTrafficCitationList()
        {
            var citations = GetAllTrafficCitations(0, 100);
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

        private static void insertTrafficCitation(TrafficCitation citation)
        {
            string sql = String.Format("insert into TrafficCitation ("
               + "id, CitationTimeDate, FirstName, LastName, DOB, HomeAddress, CitationStreetAddress, CitationCity, "
               + "CitationPosX, CitationPosY, CitationPosZ, VehicleType, VehicleModel, VehicleTag, VehicleColor, "
               + "CitationReason, CitationAmount, Details, IsArrestable)"
               + " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},{10},'{11}',"
               + "'{12}','{13}','{14}','{15}',{16},'{17}','{18}')",
               citation.id, citation.CitationTimeDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'"),
               citation.FirstName, citation.LastName, citation.DOB,
               citation.HomeAddress, citation.CitationStreetAddress, citation.CitationCity,
               citation.CitationPosX, citation.CitationPosY, citation.CitationPosZ,
               citation.VehicleType, citation.VehicleModel, citation.VehicleTag, citation.VehicleColor,
               citation.CitationReason, citation.CitationAmount, citation.Details, (citation.IsArrestable ? "1" : "0"));
            Globals.Store.ExecuteNonQuery(sql);
        }

        private static void updateTrafficCitation(TrafficCitation citation)
        {
            string sql = String.Format("update TrafficCitation "
                + "set CitationTimeDate='{0}', FirstName='{1}', LastName='{2}', DOB='{3}', HomeAddress='{4}', "
                + "CitationStreetAddress='{5}', CitationCity ='{6}', CitationPosX='{7}', CitationPosY='{8}', "
                + "CitationPosZ='{9}', VehicleType='{10}', VehicleModel='{11}', VehicleTag='{12}', VehicleColor='{13}',"
                + "CitationReason='{14}', CitationAmount='{15}', Details='{16}', IsArrestable='{17}' "
                + "where id = '{18}'",
                citation.CitationTimeDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'"), citation.FirstName,
                citation.LastName, citation.DOB, citation.HomeAddress, citation.CitationStreetAddress, citation.CitationCity,
                citation.CitationPosX, citation.CitationPosY, citation.CitationPosZ, citation.VehicleType,
                citation.VehicleModel, citation.VehicleTag, citation.VehicleColor, citation.CitationReason,
                citation.CitationAmount, citation.Details, (citation.IsArrestable ? "1" : "0"), citation.id);
            Globals.Store.ExecuteNonQuery(sql);
        }


        public static TrafficCitation SaveTrafficCitation(TrafficCitation citation)
        {
            try
            {
                if (citation.IsNew)
                {
                    citation.id = Guid.NewGuid();
                    insertTrafficCitation(citation);
                }
                else
                {
                    updateTrafficCitation(citation);
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
            if (Globals.PendingTrafficCitation != null && Globals.PendingTrafficCitation == citation) Globals.PendingTrafficCitation = null;
            return citation;
        }

        private static List<TrafficCitation> fetchTrafficCitations(String sql)
        {
            var citations = new List<TrafficCitation>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(Storage.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var citation = new TrafficCitation();
                                citation.id = new Guid((string)reader["id"]);
                                citation.CitationTimeDate = DateTime.Parse((string)reader["CitationTimeDate"]);
                                citation.FirstName = (string)reader["FirstName"];
                                citation.LastName = (string)reader["LastName"];
                                citation.DOB = (string)reader["DOB"];
                                citation.HomeAddress = (string)reader["HomeAddress"];
                                citation.CitationStreetAddress = (string)reader["CitationStreetAddress"];
                                citation.CitationCity = (string)reader["CitationCity"];
                                var posX = Convert.ToSingle((double)reader["CitationPosX"]);
                                var posY = Convert.ToSingle((double)reader["CitationPosY"]);
                                var posZ = Convert.ToSingle((double)reader["CitationPosZ"]);
                                citation.CitationPos = new Vector3(posX, posY, posZ);
                                citation.VehicleType = (string)reader["VehicleType"];
                                citation.VehicleModel = (string)reader["VehicleModel"];
                                citation.VehicleTag = (string)reader["VehicleTag"];
                                citation.VehicleColor = (string)reader["VehicleColor"];
                                citation.CitationReason = (string)reader["CitationReason"];
                                citation.CitationAmount = (double)reader["CitationAmount"];
                                citation.Details = (string)reader["Details"];
                                citation.IsArrestable = ((string)reader["IsArrestable"]).Equals("1") ? true : false;

                                citations.Add(citation);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
            return citations;
        }

        public static List<TrafficCitation> GetAllTrafficCitations(int skip = 0, int limit = 100, String orderCol = "", String orderDir = "ASC")
        {
            string sql = String.Format("select * from TrafficCitation order by CitationTimeDate desc limit {0}", limit);
            return fetchTrafficCitations(sql);
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
            return SaveTrafficCitation(newCitation);
        }

        public static List<TrafficCitation> GetTrafficCitationsForPed(ComputerPlusEntity entity)
        {
            // check if ped has past citations
            List<TrafficCitation> pastCitationFromDB = GetTrafficCitationsForPed(entity.FirstName, entity.LastName, entity.DOBString);
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

        public static List<TrafficCitation> GetTrafficCitationsForPed(String firstName, String lastName, String dob)
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            dob = dob.Trim();
            string sql = String.Format("select * from TrafficCitation where FirstName = '{0}' and LastName = '{1}' "
                + "and DOB = '{2}' order by CitationTimeDate desc", firstName, lastName, dob);
            return fetchTrafficCitations(sql);
        }
       
        public static DetailedEntity GetAllReportsForPed(ComputerPlusEntity entity)
        {            
            var arrests = GetArrestReportsForPed(entity);
            var traffic = GetTrafficCitationsForPed(entity);
            return new DetailedEntity(entity, arrests, traffic);
        }
    }
}
