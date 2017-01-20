using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using ComputerPlus.DB.Models;

namespace ComputerPlus.DB
{
    public enum UpgradeStatus { MISSING_SCHEMAS, COMPLETED, BAD_CONNECTION, FAILED };
    class Storage
    {
        private static readonly String DB_FILE_NAME = Function.GetAssetPath("reports.sqlite");
        private static readonly String CONNECTION_STRING = String.Format("Data Source={0};Version=3;FKSupport=True;", DB_FILE_NAME);

        Storage()
        {
        }

        private void SqlConnectionDisposed(object sender, EventArgs e)
        {
        }

        ~Storage()
        {
        }

        public SQLiteConnection Connection()
        {
            return new SQLiteConnection(CONNECTION_STRING);
        }

        public void Close()
        {
            //if (m_dbOpened)
            //{
            //    dbConnection.Close();
            //}
        }

        internal UpgradeStatus Upgrade(SchemaVersion toVersion)
        {
            using (var db = this.Connection().OpenAndReturn())
            {                
                if (!toVersion.Plans.All(x => File.Exists(Function.GetAssetPath(String.Format(@"Schemas\{0}.sql", x))))) return UpgradeStatus.MISSING_SCHEMAS;
                using (SQLiteCommand cmd = new SQLiteCommand(db))
                using (SQLiteTransaction transaction = db.BeginTransaction())
                {
                    try
                    {
                        foreach (var exec in toVersion.Plans.Select(x => File.ReadAllText(Function.GetAssetPath(String.Format(@"Schemas\{0}.sql", x)))))
                        {
                            cmd.CommandText = exec;
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        return UpgradeStatus.COMPLETED;
                    }
                    catch (SQLiteException e)
                    {
                        Function.Log(String.Format("Failed upgrade schema:{0}{1}", Environment.NewLine, e.Message));
                        return UpgradeStatus.FAILED;
                    }
                }
            }
            
        }

        public static Storage ReadOrInit()
        {
            return File.Exists(DB_FILE_NAME) ? new Storage() : Storage.InitNew();
        }

        public static Storage InitNew()
        {
            SQLiteConnection.CreateFile(DB_FILE_NAME);
            return new Storage();
        }
    }
}
