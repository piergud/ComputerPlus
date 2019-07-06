using System;
using System.Collections.Generic;

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
    }

    public class ArrestReportPartyDoc
    {
        public Guid Id { get; set; }
        public int PartyType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
    }

    public class Storage
    {
        public Dictionary<string, List<TrafficCitationDoc>> trafficCitationDict;
        public Dictionary<string, List<ArrestReportDoc>> arrestReportDict;
        public Dictionary<Guid, List<ArrestReportChargeDoc>> arrestReportChargeDict;
        public Dictionary<Guid, List<ArrestReportPartyDoc>> arrestReportPartyDict;

        public Storage()
        {
            Function.Log(String.Format("Attempting to initialize the in-memory DB"));
        }

        public void initDB()
        {
            trafficCitationDict = new Dictionary<string, List<TrafficCitationDoc>>();
            arrestReportDict = new Dictionary<string, List<ArrestReportDoc>>();
            arrestReportChargeDict = new Dictionary<Guid, List<ArrestReportChargeDoc>>();
            arrestReportPartyDict = new Dictionary<Guid, List<ArrestReportPartyDoc>>();
        }
    }
}
