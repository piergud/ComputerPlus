using ComputerPlus.Interfaces.Reports.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.DB.Tables
{
    class ArrestReportTable: BaseTable<ArrestReport>
    {
     
        public static readonly String ARREST_TIME = "ArrestTime";
        public static readonly String FIRST_NAME = "FirstName";
        public static readonly String LAST_NAME = "LastName";
        public static readonly String DOB = "DOB";
        public static readonly String HOME_ADDRESS = "HomeAddress";
        public static readonly String ARREST_STREET_ADDRESS = "ArrestStreetAddress";
        public static readonly String ARREST_CITY = "ArrestCity";

        public ArrestReportTable(SQLiteConnection connection) : base(connection)
        {

        }

        private static String[] mProjection = new String[]
       {
            ID_KEY,
            ARREST_TIME,
            FIRST_NAME,
            LAST_NAME,
            DOB,
            HOME_ADDRESS,
            ARREST_STREET_ADDRESS,
            ARREST_CITY
       };
        private static readonly String mTable = "ArrestReport";
        protected override string[] Projection()
        {
            return mProjection;
        }

        protected override string TableName()
        {
            return mTable;
        }

        public List<ArrestReport> GetArrestReportsForPed(String firstName, String lastName, String dob)
        {
            List<ArrestReport> result = new List<ArrestReport>();
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(firstName))
                sb.AppendFormat(" {0} = '{1}'", FIRST_NAME, firstName);
            if (!String.IsNullOrWhiteSpace(lastName))
                sb.AppendFormat(" {0} = '{1}'", LAST_NAME, lastName);
            if (!String.IsNullOrWhiteSpace(dob))
                sb.AppendFormat(" {0} = '{1}'", DOB, dob);

            using (var db = mConnection.OpenAndReturn())
            using (var reader = Where(db, sb.ToString()))
            {
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var row = ReaderToMap(reader);
                        var entry = new ArrestReport();
                        entry.FromMap(row);
                        result.Add(entry);
                    }
                }
                return result;
            }
        }

        
    }
}
