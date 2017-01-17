using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.ComputerReports
{
    [Serializable]
    class ArrestData
    {
        public string Name { get; set; }
        public Type DataType { get; set; }
        public List<string> DataList { get; set; } = new List<string>();
        public string BoxText { get; set; }

        public ArrestData() { }

        public ArrestData(string name, Type type, List<string> data, string boxText)
        {
            Name = name;
            DataType = type;
            DataList = data;
            BoxText = boxText;
        }
        
        public enum Type { Suspect, Victim, Vehicle, ReportInfo }
    }
}
