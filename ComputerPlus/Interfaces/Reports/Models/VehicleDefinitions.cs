using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComputerPlus.Interfaces.Reports.Models
{
    [XmlRoot("VehicleDefinitions")]
    [Serializable]
    public class VehicleDefinitions
    {
        [XmlElement(ElementName = "VehicleCategories")]
        public VehicleCategories Types
        {
            get;
            set;
        }

    }

    [Serializable]
    public class VehicleCategories
    {
        [XmlElement(ElementName = "VehicleCategory")]
        public List<VehicleCategory> Categories
        {
            get;
            set;
        }
    }

    [Serializable]
    public class VehicleCategory
    {
        [XmlText]
        public String Value
        {
            get;
            set;
        }
    }
}
