using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Data.SQLite;

namespace ComputerPlus.DB.Models
{
    [Serializable()]
    [XmlRoot("SchemaUpgrades")]
    public class SchemaUpgrade
    {
        [XmlElement]
        public List<SchemaVersion> Versions;
    }

    public class SchemaVersion : BaseModel
    {
        [XmlAttribute(AttributeName = "plans")]
        public List<String> Plans;

        public static SchemaVersion Create(String id)
        {
            return new SchemaVersion() { id = id };
        }
    }

}
