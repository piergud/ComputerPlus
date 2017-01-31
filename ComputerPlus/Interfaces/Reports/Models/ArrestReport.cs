using ComputerPlus.Controllers;
using ComputerPlus.Controllers.Models;
using ComputerPlus.DB.Models;
using ComputerPlus.DB.Tables;
using LSPD_First_Response.Engine.Scripting.Entities;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.Reports.Models
{
    public class ArrestReport : PersistedModel
    {

        private readonly Dictionary<String, Boolean> Fetched = new Dictionary<string, bool>();
        private readonly List<ArrestChargeLineItem> _Charges = new List<ArrestChargeLineItem>();
        public List<ArrestChargeLineItem> Charges
        {
            get
            {
                if (Fetched.ContainsKey("Charges") && Fetched["Charges"]) return _Charges;
                Fetched.Add("Charges", true);
                ComputerReportsController.PopulateArrestLineItems(this);
                return _Charges;
            }
        }
        public DateTime ArrestTimeDate
        {
            get;
            internal set;
        }
        public String ArrestTime
        {
            get { return ArrestTimeDate.ToShortTimeString();  }
        }
        public String ArrestDate
        {
            get { return ArrestTimeDate.ToShortDateString(); }
        }
        public String FirstName
        {
            get;
            internal set;
        } = String.Empty;

        public String LastName
        {
            get;
            internal set;
        } = String.Empty;

        public String FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public String DOB
        {
            get;
            internal set;
        } = String.Empty;

        public String HomeAddress
        {
            get;
            internal set;
        } = String.Empty;

        public String ArrestStreetAddress
        {
            get;
            internal set;
        } = String.Empty;

        public String ArrestCity
        {
            get;
            internal set;
        } = String.Empty;

        public ArrestReport(List<ArrestChargeLineItem> charges, DateTime arrestTime, String notes)
        {
            Charges.AddRange(charges);
            ArrestTimeDate = arrestTime != null ? arrestTime : DateTime.Now;
        }

        public ArrestReport () 
        {
            ArrestTimeDate = DateTime.Now;
        }

        public void ChangeArrestTime(DateTime time)
        {
            ArrestTimeDate = time;
        }

        protected override internal void FromMap(Dictionary<String, dynamic> map)
        {
            base.FromMap(map);
            if (map.ContainsKey(ArrestReportTable.ARREST_TIME))
                this.ChangeArrestTime(DateTime.Parse(map[ArrestReportTable.ARREST_TIME]));
            if (map.ContainsKey(ArrestReportTable.FIRST_NAME))
                this.FirstName = map[ArrestReportTable.FIRST_NAME];
            if (map.ContainsKey(ArrestReportTable.LAST_NAME))
                this.LastName = map[ArrestReportTable.LAST_NAME];
            if (map.ContainsKey(ArrestReportTable.DOB))
                this.DOB = map[ArrestReportTable.DOB];
            if (map.ContainsKey(ArrestReportTable.HOME_ADDRESS))
                this.HomeAddress = map[ArrestReportTable.HOME_ADDRESS];
            if (map.ContainsKey(ArrestReportTable.ARREST_STREET_ADDRESS))
                this.ArrestStreetAddress = map[ArrestReportTable.ARREST_STREET_ADDRESS];
            if (map.ContainsKey(ArrestReportTable.ARREST_CITY))
                this.ArrestCity = map[ArrestReportTable.ARREST_CITY];
        }
        protected override internal Dictionary<String, dynamic> ToMap()
        {
            var map = base.ToMap();
            map.Add("ArrestTime", this.ArrestTimeDate.ToString("s"));
            map.Add("FirstName", this.FirstName);
            map.Add("LastName", this.LastName);
            map.Add("DOB", this.DOB);
            map.Add("HomeAddress", this.HomeAddress);
            map.Add("ArrestStreetAddress", this.ArrestStreetAddress);
            map.Add("ArrestCity", this.ArrestCity);
            return map;
        }


    }
}
