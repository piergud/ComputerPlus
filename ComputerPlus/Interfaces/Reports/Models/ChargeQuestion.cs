using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComputerPlus.Interfaces.Reports.Models
{
    [Serializable]
    public class ChargeQuestion
    {
        [XmlText]
        public String Question;
    }
}
