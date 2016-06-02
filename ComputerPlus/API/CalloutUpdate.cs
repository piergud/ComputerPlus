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
            TimeAdded = DateTime.UtcNow;
            Text = pText;
        }

        public DateTime TimeAdded { get; }
        public string Text { get; }
    }
}