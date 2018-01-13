using ComputerPlus.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace ComputerPlus.Interfaces.Reports.Models
{
//    [Table("ArrestReportAdditionalParty")]
    public class ArrestReportAdditionalParty : IModelValidable
    {
        public enum PartyTypes { UNKNOWN = 0, ACCOMPLICE = 1, VICTIM = 2, WITNESS = 4 }
        //@TODO should probably make ArrestReport have a .AddNew(ArrestReportAdditionalParty) and assign ID that way
        public ArrestReportAdditionalParty(Guid id)
        {
            this.id = id;
        }

        public ArrestReportAdditionalParty()
        {
            this.id = Guid.NewGuid();
        }

        public ArrestReportAdditionalParty(ArrestReport report) : this()
        {
            ReportId = report.id;
        }

//        [PrimaryKey]
//        [Column("id")]
        public Guid id
        {
            get;
            set;
        }

//        [Ignore]
        public bool IsNew
        {
            get
            {
                return id == null || id == Guid.Empty;
            }
        }

//        [Column("PartyType")]
        public PartyTypes PartyType
        {
            get;
            internal set;
        } = PartyTypes.UNKNOWN;

//        [Column("FirstName")]
//        [Indexed(Name = "arrest_report_additional_party_name")]
        public String FirstName
        {
            get;
            internal set;
        } = String.Empty;

//        [Column("LastName")]
//        [Indexed(Name = "arrest_report_additional_party_name")]
        public String LastName
        {
            get;
            internal set;
        } = String.Empty;

//        [Ignore]
        public String FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

//        [Column("DOB")]
//        [Indexed(Name = "arrest_report_additional_party_name")]
        public String DOB
        {
            get;
            internal set;
        } = String.Empty;

//        [Indexed(Name = "arrest_report_additional_party_report_id")]
//        [ForeignKey(typeof(ArrestReport), Name = "arrestReportId")]
//        [Column("arrestReportId")]
        public Guid ReportId
        {
            get;
            set;
        }

        public String Id()
        {
            return this.id.ToString();
        }

        public String ValidationFailureFormat(String prop, String message)
        {
            return String.Format("{0}: {1}", prop, message);
        }

        private Dictionary<String, String> Validate()
        {
            var failReasons = new Dictionary<String, String>();
            var notAllowedEmpty = new Dictionary<String, String>() {
               { "First Name", FirstName },
               { "Last Name", LastName },
               { "DOB", DOB }
           };
            foreach (var kvp in notAllowedEmpty)
            {
                if (String.IsNullOrWhiteSpace(kvp.Value))
                {
                    failReasons.Add(kvp.Key, ValidationFailureFormat(kvp.Key, "Cannot be empty"));
                }

            }
            var dobPattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            DateTime parsedDob;
            if(PartyType == PartyTypes.UNKNOWN)
            {
                failReasons.Add("Type", "Please select one");
            }
            if (!failReasons.ContainsKey("DOB") && !DateTime.TryParseExact(DOB, dobPattern, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsedDob))
            {
                failReasons.Add("DOB", ValidationFailureFormat("DOB", String.Format("Needs to be in the format {0}", dobPattern)));
            }
            return failReasons;
        }

        public bool Validate(out Dictionary<String, String> failReasons)
        {
            failReasons = Validate();
            return failReasons.Count == 0;
        }

        public bool Validate(out KeyValuePair<String, String> failReason)
        {
            var failReasons = Validate();
            failReason = failReasons.DefaultIfEmpty(new KeyValuePair<String, String>(null, null)).FirstOrDefault();
            return failReasons.Count == 0;
        }
    }
}
