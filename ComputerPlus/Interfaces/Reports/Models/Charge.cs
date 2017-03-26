using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComputerPlus.Interfaces.Reports.Models
{
    [Serializable]
    public class Charge
    {

        [PrimaryKey]
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
