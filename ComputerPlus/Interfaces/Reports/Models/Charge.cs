using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ComputerPlus.Interfaces.Reports.Models
{
    [Serializable]
    public class Charge
    {

        // [PrimaryKey]
        public Guid id
        {
            get;
            set;
        }

        public Charge()
        {

        }

        public Charge(String name, bool isFelony)
        {
            this.Name = name;
            this.IsFelony = IsFelony;
        }

        [XmlAttribute("name")]
        public String Name
        {
            get;
            set;
        }

        [XmlAttribute("felony")]
        public bool IsFelony
        {
            get;
            set;
        }

        public bool IsTraffic
        {
            get;
            set;
        } = false;

        [XmlElement(ElementName = "Charge")]
        public List<Charge> Children
        {
            get;
            set;
        }


        public bool IsContainer
        {
            get
            {
                return Children != null && Children.Count > 0;
            }
        }
       
    }
}
