using System;
using LiteDB;

namespace ComputerPlus.DB
{
    public enum UpgradeStatus { MISSING_SCHEMAS, COMPLETED, BAD_CONNECTION, FAILED };

    public class TrafficCitationDoc
    {
        public Guid Id { get; set; }
        public string CitationTimeDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string HomeAddress { get; set; }
        public string CitationStreetAddress { get; set; }
        public string CitationCity { get; set; }
        public float CitationPosX { get; set; }
        public float CitationPosY { get; set; }
        public float CitationPosZ { get; set; }
        public string VehicleType { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleTag { get; set; }
        public string VehicleColor { get; set; }
        public string CitationReason { get; set; }
        public double CitationAmount { get; set; }
        public string Details { get; set; }
        public bool IsArrestable { get; set; }
    }

    public class ArrestReportDoc
    {
        public Guid Id { get; set; }
        public string ArrestTimeDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string HomeAddress { get; set; }
        public string ArrestStreetAddress { get; set; }
        public string ArrestCity { get; set; }
        public string Details { get; set; }
    }

    public class ArrestReportChargeDoc
    {
        public Guid Id { get; set; }
        public string Charge { get; set; }
        public bool IsFelony { get; set; }
        public string Note { get; set; }
        public Guid ArrestReportId { get; set; }
    }

    public class ArrestReportPartyDoc
    {
        public Guid Id { get; set; }
        public int PartyType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public Guid ArrestReportId { get; set; }
    }

    public class Storage
    {
        private static readonly String DB_FILE_NAME = Function.GetAssetPath("reports.db", true);

        private LiteDatabase db;

        public Storage()
        {
            Function.Log(String.Format("Attempting to load LiteDB database from {0}", DB_FILE_NAME));
            db = new LiteDatabase(DB_FILE_NAME);
        }

        public LiteDatabase getDB()
        {
            return db;
        }

        public void initDB()
        {
            var citationDb = db.GetCollection<TrafficCitationDoc>("citations");
            citationDb.EnsureIndex(x => x.DOB, false);
            citationDb.EnsureIndex(x => x.CitationTimeDate, false);

            var arrestReportDb = db.GetCollection<ArrestReportDoc>("arrestreports");
            arrestReportDb.EnsureIndex(x => x.DOB, false);
            arrestReportDb.EnsureIndex(x => x.ArrestTimeDate, false);

            var arrestReportChargeDb = db.GetCollection<ArrestReportChargeDoc>("arrestreportcharges");
            arrestReportChargeDb.EnsureIndex(x => x.ArrestReportId, false);

            var arrestReportPartyDb = db.GetCollection<ArrestReportPartyDoc>("arrestreportparties");
            arrestReportPartyDb.EnsureIndex(x => x.ArrestReportId, false);
        }
    }
}
