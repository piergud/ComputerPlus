using ComputerPlus.DB.Models;
using System;


namespace ComputerPlus.Interfaces.Reports.Models
{
//    [Table("ArrestReportLineItem")]
    public class ArrestChargeLineItem : PersistedModel
    {

//        [PrimaryKey]
        public Guid id
        {
            get;
            set;
        }

        public String Charge
        {
            get;
            internal set;
        } = String.Empty;

//        [Column("FelonyLevel")]
        public bool IsFelony
        {
            get;
            internal set;
        } = false;

        public bool IsTraffic
        {
            get;
            internal set;
        } = false;

        public String Note
        {
            get;
            internal set;
        } = String.Empty;


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

        public ArrestChargeLineItem() : this(null, String.Empty)
        {

        }
        public ArrestChargeLineItem(Charge charge, String note) : this(Guid.NewGuid(), charge, note)
        {
        }

        public ArrestChargeLineItem(Guid id, Charge charge, String note)
        {
            this.id = id;
            Charge = charge != null ? charge.Name : String.Empty;
            Note = note != null ? note : String.Empty;
            IsFelony = charge != null ? charge.IsFelony : false;
            IsTraffic = charge != null ? charge.IsTraffic : false;
        }

    }
}
