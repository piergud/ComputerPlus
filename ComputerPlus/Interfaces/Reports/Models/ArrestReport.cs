using ComputerPlus.Controllers;
using ComputerPlus.Controllers.Models;
using ComputerPlus.DB.Models;
using ComputerPlus.Extensions;
using ComputerPlus.Interfaces.Common;
using LSPD_First_Response.Engine.Scripting.Entities;
using Rage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.Reports.Models
{
    public class ArrestReport : IModelValidable
    {

        [PrimaryKey]
        [Column("id")]
        public Guid id
        {
            get;
            set;
        }

        [Ignore]
        public bool IsNew
        {
            get
            {                
                return id == null || id == Guid.Empty;
            }
        }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ArrestChargeLineItem> Charges
        {
            get;
            set;
        } = new List<ArrestChargeLineItem>();

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ArrestReportAdditionalParty> AdditionalParties
        {
            get;
            set;
        } = new List<ArrestReportAdditionalParty>();

        [Column("ArrestTime")]
        public DateTime ArrestTimeDate
        {
            get;
            set;
        }

        [Ignore]
        public String ArrestTime
        {
            get { return ArrestTimeDate.ToShortTimeString();  }
        }

        [Ignore]
        public String ArrestDate
        {
            get { return ArrestTimeDate.ToShortDateString(); }
        }

        [Column("FirstName")]
        [Indexed(Name ="arrest_report_ped")]
        public String FirstName
        {
            get;
            internal set;
        } = String.Empty;

        [Column("LastName")]
        [Indexed(Name = "arrest_report_ped")]
        public String LastName
        {
            get;
            internal set;
        } = String.Empty;

        [Ignore]
        public String FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Column("DOB")]
        [Indexed(Name = "arrest_report_ped")]
        public String DOB
        {
            get;
            internal set;
        } = String.Empty;

        [Column("HomeAddress")]
        public String HomeAddress
        {
            get;
            internal set;
        } = String.Empty;

        [Column("ArrestStreetAddress")]
        public String ArrestStreetAddress
        {
            get;
            internal set;
        } = String.Empty;

        [Column("ArrestCity")]
        public String ArrestCity
        {
            get;
            internal set;
        } = String.Empty;

        [Column("Details")]
        public String Details
        {
            get;
            internal set;
        } = String.Empty;

        public String Id()
        {
            return this.id.ToString(); ;
        }

        public String ShortId()
        {
            return this.Id().Substring(30);
        }

        public ArrestReport(List<ArrestChargeLineItem> charges, List<ArrestReportAdditionalParty> parties, DateTime arrestTime, String notes) : this()
        {
            Function.Log("ArrestReport overload");
            if (charges != null)
            {
                lock (charges)
                    Charges.AddRange(charges);
            }
            if (parties != null)
            {
                lock (parties)
                    AdditionalParties.AddRange(parties);
            }
            ArrestTimeDate = arrestTime != null ? arrestTime : DateTime.Now;
        }

        public ArrestReport(List<ArrestChargeLineItem> charges, DateTime arrestTime, String notes) : this(charges, null, arrestTime, notes)
        {
        }

        public ArrestReport () 
        {            
            ArrestTimeDate = DateTime.Now;
        }

        public void ChangeArrestTime(DateTime time)
        {            
            ArrestTimeDate = time;
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
            failReason = failReasons.DefaultIfEmpty(new KeyValuePair<string, string>(null, null)).FirstOrDefault();
            return failReasons.Count == 0;
        }

        public static implicit operator bool(ArrestReport report)
        {
            return report != null;
        }

        public static ArrestReport CreateForPed(ComputerPlusEntity entity)
        {
            return new ArrestReport()
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DOB = entity.PedPersona.BirthDay.ToLocalTimeString(Extensions.Gwen.TextBoxExtensions.DateOutputPart.DATE),
                HomeAddress = entity.Address,
            };

        }
    }
}
