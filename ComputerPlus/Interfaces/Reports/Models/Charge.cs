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
        [XmlAttribute("felony")]
        public bool IsFelony
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Question")]
        public List<ChargeQuestion> Questions
        {
            get;
            set;
        }


    }
}
