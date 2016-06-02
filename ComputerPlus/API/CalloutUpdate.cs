using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.API
{
    public class CalloutUpdate
    {
        internal CalloutUpdate(string pText)
        {
            TimeAdded = Function.GetMixedDateTime();
            Text = pText;
        }

        public DateTime TimeAdded { get; }
        public string Text { get; }
    }
}