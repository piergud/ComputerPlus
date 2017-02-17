using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SQLite;
using System.IO;
using ComputerPlus.DB.Models;
using SQLite.Net.Async;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System.Threading;

namespace ComputerPlus.DB
{
    public enum UpgradeStatus { MISSING_SCHEMAS, COMPLETED, BAD_CONNECTION, FAILED };
    class Storage
    {
        

        private static readonly String DB_FILE_NAME = Function.GetAssetPath("reports.sqlite");
        private static readonly String CONNECTION_STRING = String.Format("Data Source={0};Version=3;FKSupport=True;", DB_FILE_NAME);
        private SQLiteConnectionWithLock mConnectionLock;

        Storage()
        {
            mConnectionLock = new SQLiteConnectionWithLock(new SQLitePlatformWin32(Function.GetAssetPath(@"..\..\..\")), new SQLiteConnectionString(DB_FILE_NAME, false));
        }

        private void SqlConnectionDisposed(object sender, EventArgs e)
        {
        }

        ~Storage()
        {
        }
       
        public SQLiteAsyncConnection Connection()
        {
            return new SQLiteAsyncConnection(() => mConnectionLock);
        }

        public void Close()
        {
            //if (m_dbOpened)
            //{
            //    dbConnection.Close();
            //}
            try
            {
                mConnectionLock.Close();
            }
            catch { }

        }

        internal async Task<UpgradeStatus> UpgradeSchemaFactory(SchemaVersion toVersion)
        {
            try {
                await Connection().RunInTransactionAsync(new Action<SQLiteConnection>((conn) =>
                {
                    conn.BeginTransaction();
                    foreach (var exec in toVersion.Plans.Select(x => File.ReadAllText(Function.GetAssetPath(String.Format(@"Schemas\{0}.sql", x)))))
                    {
                        conn.Execute(exec);
                    }

                    conn.Commit();

                }));
                return UpgradeStatus.COMPLETED;
            } catch
            {
                return UpgradeStatus.FAILED;
            }

        }

        internal async Task<UpgradeStatus> Upgrade(SchemaVersion toVersion)
        {
            var connection = this.Connection();

            if (!toVersion.Plans.All(x => File.Exists(Function.GetAssetPath(String.Format(@"Schemas\{0}.sql", x))))) return UpgradeStatus.MISSING_SCHEMAS;
            var transactionProcess = await UpgradeSchemaFactory(toVersion);
            return transactionProcess;
            
        }

        public static Storage Open()
        {
            return new Storage();
        }

        public async static Task<Storage> ReadOrInit()
        {
            if (File.Exists(DB_FILE_NAME))
                return new Storage();
            else {
                return await Storage.InitNew();
            }
        }

        public async static Task<Storage> InitNew()
        {
            
            var store = new Storage();
            var entry = SchemaVersion.Create(Globals.SchemaVersion);
            await store.Connection().InsertAsync(entry);
            return store;
        }

        public static class Tables
        {
            public static class Names
            {
                public static readonly String ArrestReport = "ArrestReport";
                public static readonly String ArrestReportCharges = "ArrestReportCharges";
                public static readonly String SchemaVersion = "SchemaVersion";
            }
            public static readonly String ID_KEY = "id";
            public static class ArrestReport
            {
                public static readonly String ARREST_TIME = "ArrestTime";
                public static readonly String FIRST_NAME = "FirstName";
                public static readonly String LAST_NAME = "LastName";
                public static readonly String DOB = "DOB";
                public static readonly String HOME_ADDRESS = "HomeAddress";
                public static readonly String ARREST_STREET_ADDRESS = "ArrestStreetAddress";
                public static readonly String ARREST_CITY = "ArrestCity";
                public static readonly String[] PROJECTION = {
                    ID_KEY,
                    ARREST_TIME,
                    FIRST_NAME,
                    LAST_NAME,
                    DOB,
                    HOME_ADDRESS,
                    ARREST_STREET_ADDRESS,
                    ARREST_CITY
               };
            }
        }
    }
}
