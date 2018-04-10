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

        public LiteCollection<TrafficCitationDoc> citationCollection { get; set; }
        public LiteCollection<ArrestReportDoc> arrestReportCollection { get; set; }
        public LiteCollection<ArrestReportChargeDoc> arrestReportChargeCollection { get; set; }
        public LiteCollection<ArrestReportPartyDoc> arrestReportPartyCollection { get; set; }

        public Storage()
        {
            Function.Log(String.Format("Attempting to load LiteDB database from {0}", DB_FILE_NAME));
            //db = new LiteDatabase(@String.Format("Filename={0};Cache Size=16;Flush=true", DB_FILE_NAME));
            db = new LiteDatabase(@String.Format("Filename={0};Cache Size=0;Flush=true", DB_FILE_NAME));
        }

        public void initDB()
        {
            citationCollection = db.GetCollection<TrafficCitationDoc>("citations");
            citationCollection.EnsureIndex(x => x.DOB, false);
            citationCollection.EnsureIndex(x => x.CitationTimeDate, false);

            arrestReportCollection = db.GetCollection<ArrestReportDoc>("arrestreports");
            arrestReportCollection.EnsureIndex(x => x.DOB, false);
            arrestReportCollection.EnsureIndex(x => x.ArrestTimeDate, false);

            arrestReportChargeCollection = db.GetCollection<ArrestReportChargeDoc>("arrestreportcharges");
            arrestReportChargeCollection.EnsureIndex(x => x.ArrestReportId, false);

            arrestReportPartyCollection = db.GetCollection<ArrestReportPartyDoc>("arrestreportparties");
            arrestReportPartyCollection.EnsureIndex(x => x.ArrestReportId, false);
        }
    }
}
