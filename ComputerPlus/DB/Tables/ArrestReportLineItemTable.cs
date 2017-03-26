using ComputerPlus.Interfaces.Reports.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.DB.Tables
{
    class ArrestReportLineItemTable : BaseTable<ArrestChargeLineItem>
    {
        public static readonly String CHARGE = "Charge";
        public static readonly String FELONY_LEVEL = "FelonyLevel";
        public static readonly String NOTE = "Note";
        public static readonly String REPORT_ID = "arrestReportId";

        public ArrestReportLineItemTable(SQLiteConnection connection) : base(connection)
        {

        }
        private static String[] mProjection = new String[]
       {
            ID_KEY,
            CHARGE,
            FELONY_LEVEL,
            NOTE,
            REPORT_ID
       };
        private static readonly String mTable = "ArrestReportLineItem";
        protected override string[] Projection()
        {
            return mProjection;
        }

        protected override string TableName()
        {
            return mTable;
        }

        public List<ArrestChargeLineItem> PopulateArrestCharges(ArrestReport report)
        {
            var query = String.Format("SELECT * FROM [{0}] WHERE arrestReportId = '{1}'", TableName(), report.Id());
            Function.Log(query);
            using (var db = mConnection.OpenAndReturn())
            using (var reader = Where(db, query))
            {

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var row = ReaderToMap(reader);
                        var entry = new ArrestChargeLineItem();
                        entry.FromMap(row);
                        Function.Log("Adding row to list");
                        report.Charges.Add(entry);
                    }
                }
                return report.Charges;
            }
        }
    }
}
