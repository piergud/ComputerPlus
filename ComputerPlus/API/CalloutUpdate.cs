using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.API
{
    internal class CalloutUpdate
    {
        internal CalloutUpdate(string pText)
        {
            TimeAdded = DateTime.UtcNow;
            Text = pText;
        }

        internal DateTime TimeAdded { get; }
        internal string Text { get; }
    }
}