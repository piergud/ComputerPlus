using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;

namespace ComputerPlus.Interfaces.Common
{
    internal class TabbableContainer : TabControl
    {
        internal TabbableContainer(Base parent) : base(parent)
        {
            this.SetPosition(13, 12);
            this.SetSize(692, 564);
        }
        
    }
}
