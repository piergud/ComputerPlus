using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using ComputerPlus.DB.Models;

namespace ComputerPlus.DB.Tables
{
    interface ISQLiteTable<M>
    {
        M FindById(String id);
    }
    abstract class BaseTable<M> : ISQLiteTable<M> where M : PersistedModel, new()
    {
        protected readonly SQLiteConnection mConnection;
        protected abstract String TableName();
        protected abstract String[] Projection();
        public static readonly String ID_KEY = "id";

   

        public Dictionary<String, dynamic> ReaderToMap(SQLiteDataReader reader)
        {
            if (reader.HasRows == false) return null;
            var projection = Projection();
            var map = new Dictionary<String, dynamic>();
            for (int i = 0; i < projection.Length; i++) map.Add(projection[i], reader.GetValue(i));
            return map;
        }

        public M FindById(String id) {
            using (var db = mConnection.OpenAndReturn())
            using (var reader = Where(db, String.Format("{0} = '{1}'", ID_KEY, id)))
            {
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        var row = ReaderToMap(reader);
                        var entry = new M();
                        entry.FromMap(row);
                        return entry;
                    }
                }
            }
            return null;
        }

        public List<M> Browse(String order = "", String limit = "")
        {
            using (var db = mConnection.OpenAndReturn())
            using (var reader = Where(db, "*", String.Empty, order, limit))
            {
                List<M> result = new List<M>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var row = ReaderToMap(reader);
                        var entry = new M();
                        entry.FromMap(row);
                        result.Add(entry);
                    }
                }
                return result;
            }
        }

        //@TODO build custom query bulder class with methods like .Select, .As, .LeftJoin, etc
        private SQLiteCommand QueryBuilder(String projection = "*", String where = "", String order = "", String limit = "", String join = "")
        {
            StringBuilder sb = new StringBuilder(String.Format("SELECT {0} FROM [{1}]", projection, this.TableName()));
            if (!String.IsNullOrWhiteSpace(join))
                sb.AppendFormat(" {0}", join);
            if (!String.IsNullOrWhiteSpace(where))
                sb.AppendFormat(" WHERE {0}", where);
            if (!String.IsNullOrWhiteSpace(order))
                sb.AppendFormat(" ORDER BY {0}", order);
            if (!String.IsNullOrWhiteSpace(limit))
                sb.AppendFormat(" LIMIT {0}", limit);          

            return new SQLiteCommand(sb.ToString());
        }

        private SQLiteCommand QueryBuilder(SQLiteConnection connection, String projection = "*", String where = "", String order = "", String limit = "", String join = "")
        {
            var query = QueryBuilder(projection, where, order, limit);
            query.Connection = connection;
            return query;
        }

        protected SQLiteDataReader Where (SQLiteConnection db, String projection = "*", String queryWhere = "", String order = "", String limit = "", String join = "")
        {
          
            var query = QueryBuilder(db, projection, queryWhere, order, limit);
            return query.ExecuteReader();
               
        }

        protected internal SQLiteDataReader Where(SQLiteConnection db, String sqlStatement)
        {
            var query = new SQLiteCommand(sqlStatement, db);
            return query.ExecuteReader();
        }


        bool InsertSecure(SQLiteConnection cnn, String tableName, Dictionary<String, dynamic> data)
        {
            // table name can not contains space
            if (tableName.Contains(' ')) { return false; }
            String columns = "";
            String values = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, dynamic> val in data)
            {
                columns += String.Format(" '{0}',", val.Key.ToString());
                // all values as parameters
                values += String.Format(" @{0},", val.Key.ToString());
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                // setup your connection here
                // (connection is probably set in your original ExecuteNonQuery)
               
                SQLiteCommand cmd = cnn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                // prepare insert command based on data
                cmd.CommandText = String.Format("insert into {0} ({1}) values ({2})",
                    tableName, columns, values);

                // now your command looks like:
                // insert into table_name (columnA, columnB) values (@columnA, @columnB)
                // next we can set values for any numbers of columns
                // over parameters to prevent SQL injection
                foreach (KeyValuePair<String, dynamic> val in data)
                {
                    // safe way to add parameter
                    cmd.Parameters.Add(
                        new SQLiteParameter("@" + val.Key.ToString(), val.Value));
                    // you just added for @columnA parameter value valueA
                    // and so for @columnB in this foreach loop
                }
                Function.Log(cmd.CommandText);
                Function.Log(cmd.ToString());
                // execute new insert with parameters
                cmd.ExecuteNonQuery();
                // close connection and set return code to true
                returnCode = true;
            }
            catch (Exception fail)
            {
                Function.Log(fail.ToString());
                returnCode = false;
            }
            return returnCode;
        }

        public int Insert(M entry)
        {
            var map = entry.ToMap();
            Function.Log("entry to map");
            using (var db = mConnection.OpenAndReturn())
            //using (var adapter = new SQLiteDataAdapter())
            //using (var insert = new SQLiteCommandBuilder(adapter).GetInsertCommand(true))
            {
                return InsertSecure(db, this.TableName(), map) ? 1 : 0;
                //try
                //{
                //    Function.Log("building insert.Parameters");
                //    foreach (KeyValuePair<String, dynamic> kp in map)
                //    {
                //        insert.Parameters[kp.Key].Value = kp.Value;
                //    }
                //    Function.Log("preparing sqlitecommand");
                //    Function.Log(insert.CommandText);
                //    SQLiteCommand command = new SQLiteCommand(insert.CommandText, db);                    
                //    Function.Log("doing execute");
                //    return insert.ExecuteNonQuery();
                //}
                //catch(Exception e)
                //{
                //    Function.LogDebug(e.ToString());
                //    return 0;
                //}
            }
            

        }

        protected BaseTable(SQLiteConnection connection)
        {
            mConnection = connection;
        }
    }
}
