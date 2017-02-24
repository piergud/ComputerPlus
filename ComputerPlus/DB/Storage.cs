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

using CodeEngine.Framework.QueryBuilder;
using QueryEnum = CodeEngine.Framework.QueryBuilder.Enums;

namespace ComputerPlus.DB
{
    public enum UpgradeStatus { MISSING_SCHEMAS, COMPLETED, BAD_CONNECTION, FAILED };
    class Storage
    {
        

        private static readonly String DB_FILE_NAME = Function.GetAssetPath("reports.sqlite", true);
        private static readonly String CONNECTION_STRING = String.Format("Data Source={0};Version=3;FKSupport=True;", DB_FILE_NAME);
        private SQLiteConnectionWithLock mConnectionLock;

        Storage()
        {
            var interopPath = Function.GetAssetPath(String.Empty, true);
            Function.Log(String.Format("Attempting to load SQL interop from {0}", interopPath));
            Function.Log(String.Format("Attempting to load SQL db from {0}", DB_FILE_NAME));
            
            mConnectionLock = new SQLiteConnectionWithLock(new SQLitePlatformWin32(interopPath), new SQLiteConnectionString(DB_FILE_NAME, false));
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
                    //conn.BeginTransaction();
                    foreach (var lines in toVersion.Plans.Select(x => File.ReadLines(Function.GetAssetPath(String.Format(@"Schemas\{0}.sql", x)))))
                    {
                        foreach (var exec in lines)
                        {
                            Function.Log(String.Format("q: {0}", exec));
                            conn.Execute(exec);
                        }
                    }                   
                    conn.Commit();                    
                }));
                
                return UpgradeStatus.COMPLETED;
            } catch(Exception e)
            {
                Function.Log(e.ToString());
                return UpgradeStatus.FAILED;
            }

        }

        internal async Task<UpgradeStatus> Upgrade(SchemaVersion toVersion)
        {
            var connection = this.Connection();

            if (!toVersion.Plans.All(x => File.Exists(Function.GetAssetPath(String.Format(@"Schemas\{0}.sql", x))))) return UpgradeStatus.MISSING_SCHEMAS;
            var transactionProcess = await UpgradeSchemaFactory(toVersion);
            if (transactionProcess == UpgradeStatus.COMPLETED)
            {
                Function.Log(String.Format("Bumping schema version table to {0}", toVersion.id));
                await connection.InsertAsync(toVersion);
            }
            return transactionProcess;
            
        }

        

        public static List<SchemaVersion> DiscoverAvailableUpgrades(SchemaVersion minSchema)
        {
            var path = Function.GetAssetPath("Schemas", true);
            Function.Log("Path Length " + path.Length);
            return Directory
                .EnumerateFiles(path, "*.sql", SearchOption.TopDirectoryOnly)
                .Select(x => x.Substring(path.Length + 1).Replace(".sql", String.Empty)) //remove dir and .sql from name
                .Where(x => x != "initial")
                .Select(x => SchemaVersion.Create(x, new List<string>() { x }))               
                .Where(x => x.Version.CompareTo(minSchema.Version) > 0 && x.Version.CompareTo(Globals.SchemaVersion) <= 0) //only versions less than the global.schema
                .ToList();
        }

        public async static Task<Storage> ReadOrInit()
        {
            if (File.Exists(DB_FILE_NAME))
            {

                var store = new Storage();
                var latestStoredSchema = await store.Connection().Table<SchemaVersion>().OrderByDescending(x => x.id).FirstOrDefaultAsync();
                Function.Log("Store schema version check");
                if (latestStoredSchema != null)
                {
                    Function.Log(String.Format("Store schema version check returned a stored version {0} and global is {1}.. upgrade {2}", latestStoredSchema.id, Globals.SchemaVersion, latestStoredSchema.Version.CompareTo(Globals.SchemaVersion)));
                    if (latestStoredSchema.Version.CompareTo(Globals.SchemaVersion) >= 0)
                    {
                        Function.Log("Store is at the latest version");
                        return store;
                    }
                    else
                    {
                        Function.Log("Store is not at the latest version");
                        //Upgrade is required..
                        var availableUpdates = DiscoverAvailableUpgrades(latestStoredSchema).ToList();
                        Function.Log("DiscoverAvailableUpgrades completed");
                        Function.Log(String.Format("Store has {0} updates available", availableUpdates.Count));
                        availableUpdates.ForEach(async x => {
                            Function.Log(String.Format("Upgrading to {0}", x.id));
                            await store.UpgradeProcess(x);
                        });
                        
                        
                        return store;
                    }
                }
            }
            
            return await Storage.InitNew();
            
        }

        private async Task<UpgradeStatus> UpgradeProcess(SchemaVersion schema)
        {
            return await Upgrade(schema);
            
        }

        private async static Task<Storage> InitNew()
        {
            
            var store = new Storage();
            var result = await store.UpgradeProcess(SchemaVersion.Create());
            Function.Log("InitStore performing version upgrade");
          
            if (result == UpgradeStatus.COMPLETED)
            {
                Function.Log("SQL is ready");
            }
            else if (result == UpgradeStatus.MISSING_SCHEMAS)
            {
                Function.Log("Missing schema");
            }
            else
            {
                Function.Log(String.Format("Schema upgrade failed as {0}", result));
            }
            Function.Log("InitStore completed version upgrade");

            
            return store;
        }

        public static class Tables
        {
            public static class Names
            {
                public static readonly String ArrestReport = "ArrestReport";
                public static readonly String ArrestReportCharges = "ArrestReportCharges";
                public static readonly String TrafficCitation = "TrafficCitation";
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

            public static class TrafficCitation
            {
                public static readonly String CITATION_TIME_DATE = "CitationTimeDate";
                public static readonly String FIRST_NAME = "FirstName";
                public static readonly String LAST_NAME = "LastName";
                public static readonly String DOB = "DOB";
                public static readonly String HOME_ADDRESS = "HomeAddress";
                public static readonly String CITATION_STREET_ADDRESS = "CitationStreetAddress";
                public static readonly String CITATION_CITY = "CitationCity";
                public static readonly String VEHICLE_MODEL = "VehicleModel";
                public static readonly String VEHICLE_TAG = "VehicleTag";
                public static readonly String VEHICLE_COLOR = "VehicleColor";
                public static readonly String CITATION = "CitationReason";
                public static readonly String AMOUNT = "CitationAmount";
                public static readonly String DETAILS = "Details";
                public static readonly String IS_ARRESTABLE = "IsArrestable";
                public static readonly String[] PROJECTION = {
                    ID_KEY,
                    FIRST_NAME,
                    LAST_NAME,
                    DOB,
                    HOME_ADDRESS,
                    CITATION_STREET_ADDRESS,
                    CITATION_CITY,
                    VEHICLE_MODEL,
                    VEHICLE_TAG,
                    VEHICLE_COLOR,
                    CITATION,
                    AMOUNT,
                    DETAILS,
                    IS_ARRESTABLE,
                };
            }
        }
    }
}
