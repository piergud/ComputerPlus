using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
//using System.Data.SQLite;
//using SQLite.Net.Attributes;

namespace ComputerPlus.DB.Models
{
    [Serializable()]
    [XmlRoot("SchemaUpgrades")]
    public class SchemaUpgrade
    {
        [XmlElement]
        public List<SchemaVersion> Versions;
    }

    public class SchemaVersion : PersistedModel
    {
        String mId;

//        [PrimaryKey]
        public String id
        {
            get { return mId;  }
            set
            {
                mId = value;
                Version = new Version(mId);
            }
                    
        }
//        [Ignore]
        public Version Version
        {
            get;
            private set;
        }

        [XmlAttribute(AttributeName = "plans")]
        public List<String> Plans;

        public static SchemaVersion Create(String id, List<String> plans)
        {
            return new SchemaVersion() { id = id, Plans = plans };
        }

        public static SchemaVersion Create()
        {
            return new SchemaVersion() { id = "1.0.0", Plans = new List<string>() { "initial" } };
        }
    }

}
