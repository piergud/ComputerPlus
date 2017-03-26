using ComputerPlus.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ComputerPlus.DB.Tables
{
    class SchemaTable : BaseTable<SchemaVersion>
    {
        public SchemaTable(SQLiteConnection connection) : base(connection)
        {

        }
        private static String[] mProjection = new String[]
        {
            "id"
        };
        private static readonly String mTable = "SchemaVersion";
        protected override string[] Projection()
        {
            return mProjection; 
        }

        protected override string TableName()
        {
            return mTable;
        }
    }
}
