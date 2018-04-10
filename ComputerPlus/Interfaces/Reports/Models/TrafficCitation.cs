using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComputerPlus.Controllers;
using ComputerPlus.Controllers.Models;
using ComputerPlus.DB.Models;
using ComputerPlus.Interfaces.Common;
using ComputerPlus.Extensions.Rage;
using Rage;
using System.Globalization;
using static ComputerPlus.Extensions.Gwen.TextBoxExtensions;

namespace ComputerPlus.Interfaces.Reports.Models
{
//    [Table("TrafficCitation")]
    class TrafficCitation : IModelValidable
    {
        private static TrafficCitation mEmpty = new TrafficCitation();
        public static TrafficCitation Empty
        {
            get { return mEmpty; }
        }

        //        [PrimaryKey]
        //        [Column("id")]
        public Guid id
        {
            get;
            set;
        }


        //[Ignore]
        public bool IsNew
        {
            get
            {
                return id == null || id == Guid.Empty;
            }
        }

        //[Ignore]
        public bool CreateCourtCase
        {
            get;
            internal set;
        } = false;

        //[Column("CitationTimeDate")]
        public DateTime CitationTimeDate
        {
            get;
            set;
        }

        //[Ignore]
        public String CitationTime
        {
            get { return CitationTimeDate.ToShortTimeString(); }
        }

        //[Ignore]
        public String CitationDate
        {
            get { return CitationTimeDate.ToShortDateString(); }
        }

        //[Column("FirstName")]
        //[Indexed(Name = "citation_ped")]
        public String FirstName
        {
            get;
            internal set;
        } = String.Empty;

        //[Column("LastName")]
        //[Indexed(Name = "citation_ped")]
        public String LastName
        {
            get;
            internal set;
        } = String.Empty;

        //[Ignore]
        public String FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        //[Column("DOB")]
        //[Indexed(Name = "citation_ped")]
        public String DOB
        {
            get;
            internal set;
        } = String.Empty;

        //[Column("HomeAddress")]
        public String HomeAddress
        {
            get;
            internal set;
        } = String.Empty;

        //[Column("CitationStreetAddress")]
        public String CitationStreetAddress
        {
            get;
            internal set;
        } = String.Empty;

        //[Column("CitationCity")]
        public String CitationCity
        {
            get;
            internal set;
        } = String.Empty;



        //[Ignore]
        public Vector3 CitationPos
        {
            get
            {
                if (!CitationPosX.HasValue || !CitationPosY.HasValue || !CitationPosZ.HasValue)
                    return Vector3.Zero;
                return new Vector3(CitationPosX.Value, CitationPosY.Value, CitationPosZ.Value);
            }
            internal set
            {
                if (value != Vector3.Zero)
                {
                    //Function.Log("Setting CitationPOS");
                    CitationPosX = value.X;
                    CitationPosY = value.Y;
                    CitationPosZ = value.Z;
                }
            }
        }

        //[Column("CitationPosX")]
        public float? CitationPosX
        {
            get;
            private set;
        }

        //[Column("CitationPosY")]
        public float? CitationPosY
        {
            get;
            private set;
        }

        //[Column("CitationPosZ")]
        public float? CitationPosZ
        {
            get;
            private set;
        }

        //[Column("VehicleType")]
        public String VehicleType
        {
            get;
            internal set;
        } = String.Empty;

        //[Column("VehicleModel")]
        public String VehicleModel
        {
            get;
            internal set;
        } = String.Empty;

        // [Column("VehicleTag")]
        public String VehicleTag
        {
            get;
            internal set;
        } = String.Empty;

        //[Column("VehicleColor")]
        public String VehicleColor
        {
            get;
            internal set;
        } = String.Empty;

        // [Column("CitationReason")]
        public String CitationReason
        {
            get;
            internal set;
        } = String.Empty;

        //[Column("CitationAmount")]
        public double CitationAmount
        {
            get;
            internal set;
        } = 0;

        //[Column("Details")]
        public String Details
        {
            get;
            internal set;
        } = String.Empty;

        // [Column("IsArrestable")]
        public bool IsArrestable
        {
            get;
            internal set;
        }


        private CitationDefinition mCitation;
        // [Ignore]
        public CitationDefinition Citation
        {
            get
            {
                return mCitation;
            }
            set
            {
                mCitation = value;
                if (mCitation.IsContainer) throw new ArgumentException("Citation cannot be a container item");
                CitationReason = mCitation.Name;
                CitationAmount = mCitation.FineAmount;
                IsArrestable = mCitation.IsArrestable;
            }
        }


        public String Id()
        {
            return this.id.ToString(); ;
        }

        public String ShortId()
        {
            return this.Id().Substring(30);
        }

        private Dictionary<String, String> Validate()
        {
            var failReasons = new Dictionary<String, String>();
            var notAllowedEmpty = new Dictionary<String, String>() {
               { "First Name", FirstName },
               { "Last Name", LastName },
               { "DOB", DOB },
               { "Home Address", DOB },
               { "Vehicle Model", VehicleModel },
               { "Vehicle Tag", VehicleTag },
               { "Vehicle Color", VehicleColor },
               { "Vehicle Type", VehicleType },               
               { "Citation Street Address", CitationStreetAddress },
               { "Citation City", CitationCity },
               { "Citation Date", CitationDate },
               { "Citation Time", CitationTime },
               { "Citation Reason", CitationReason },
           };

            foreach (var kvp in notAllowedEmpty)
            {
                if (String.IsNullOrWhiteSpace(kvp.Value))
                {
                    failReasons.Add(kvp.Key, ValidationFailureFormat(kvp.Key, "Cannot be empty"));
                }

            }
            var dobPattern = Function.DateFormatForPart(DateOutputPart.DATE);
            DateTime parsedDob;
            if (!failReasons.ContainsKey("DOB") && !DateTime.TryParseExact(DOB, dobPattern, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsedDob))
            {
                failReasons.Add("DOB", ValidationFailureFormat("DOB", String.Format("Needs to be in the format {0}", dobPattern)));
            }
            
            if (CitationAmount <= 0) failReasons.Add("Citation Amount", "Amount must be greater than 0");
            return failReasons;
        }

        public bool Validate(out Dictionary<string, string> failReasons)
        {
            failReasons = Validate();
            return failReasons.Count == 0;
        }

        public bool Validate(out KeyValuePair<string, string> failReason)
        {
            var failReasons = Validate();
            failReason = failReasons.DefaultIfEmpty(new KeyValuePair<string, string>(null, null)).FirstOrDefault();
            return failReasons.Count == 0;
        }

        public string ValidationFailureFormat(string prop, string message)
        {
            return String.Format("{0}: {1}", prop, message);
        }

        public static implicit operator bool(TrafficCitation citation)
        {
            return citation != null;
        }

        public static TrafficCitation CreateForPedInVehicle(ComputerPlusEntity entity)
        {
            var citation = new TrafficCitation()
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DOB = entity.DOBString,
                HomeAddress = entity.Address,
                CitationTimeDate = DateTime.Now.ToUniversalTime()
            };
            if (entity.Vehicle && !entity.Vehicle.IsPoliceVehicle)
            {
                citation.VehicleModel = entity.Vehicle.Model.Name;
                citation.VehicleTag = entity.VehicleTag;
                citation.VehicleColor = entity.Vehicle.GetVehicleColorName();
            }
            else if(entity.Ped && entity.Ped.LastVehicle && !entity.Ped.LastVehicle.IsPoliceVehicle)
            {
                citation.VehicleModel = entity.Ped.LastVehicle.Model.Name;
                citation.VehicleTag = entity.Ped.LastVehicle.LicensePlate;
                citation.VehicleColor = entity.Ped.LastVehicle.GetVehicleColorName();
            }
            else
            {
                citation.VehicleModel = "N/A";
                citation.VehicleTag = "N/A";
                citation.VehicleColor = "N/A";
            }
            return citation;
        }

        public static TrafficCitation CloneFromCitation(TrafficCitation citation)
        {
            return new TrafficCitation()
            {
                FirstName = citation.FirstName,
                LastName = citation.LastName,
                DOB = citation.DOB,
                HomeAddress = citation.HomeAddress,
                CitationCity = citation.CitationCity,
                CitationStreetAddress = citation.CitationStreetAddress,
                VehicleColor = citation.VehicleColor,
                VehicleTag = citation.VehicleTag,
                VehicleModel = citation.VehicleModel,
                VehicleType = citation.VehicleType,
            };
        }

    }
}
