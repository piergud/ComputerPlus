using ComputerPlus.DB.Models;
using ComputerPlus.DB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.Reports.Models
{
    public class ArrestChargeLineItem : PersistedModel
    {
    
        public Charge Charge
        {
            get;
            internal set;
        }
        public String Note
        {
            get;
            internal set;
        } = String.Empty;

        public ArrestReport ContainingReport;

        public ArrestChargeLineItem() : this(null, String.Empty)
        {

        }
        public ArrestChargeLineItem(Charge charge, String note) : this(Guid.NewGuid(), charge, note)
        {

        }

        public ArrestChargeLineItem(Guid id, Charge charge, String note) : base(id)
        {
            Charge = charge;
            Note = note != null ? note : String.Empty;
        }

        protected override internal void FromMap(Dictionary<String, dynamic> map)
        {
            base.FromMap(map);
            if (map.ContainsKey(ArrestReportLineItemTable.CHARGE) && map.ContainsKey(ArrestReportLineItemTable.FELONY_LEVEL))
                this.Charge = new Charge(map[ArrestReportLineItemTable.CHARGE], map[ArrestReportLineItemTable.FELONY_LEVEL] == "true");
            if (map.ContainsKey(ArrestReportLineItemTable.NOTE))
                this.Note = map[ArrestReportLineItemTable.NOTE];
        }
        protected override internal Dictionary<String, dynamic> ToMap()
        {
            var map = base.ToMap();
            map.Add(ArrestReportLineItemTable.CHARGE, this.Charge.Name);
            map.Add(ArrestReportLineItemTable.FELONY_LEVEL, this.Charge.IsFelony);
            map.Add(ArrestReportLineItemTable.NOTE, this.Note);
            if (ContainingReport != null)
            {
                map.Add(ArrestReportLineItemTable.REPORT_ID, ContainingReport.Id());
            }
            return map;
        }

    }
}
