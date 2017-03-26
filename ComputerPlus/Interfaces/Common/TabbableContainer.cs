using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;

namespace ComputerPlus.Interfaces.Common
{
    internal class TabbableContainer : DockedTabControl
    {
        internal TabbableContainer(Base parent) : base(parent)
        {
            this.SetPosition(13, 12);
            this.SetSize(692, 564);
            this.TitleBarVisible = false;
        }

        internal TabbableContainer(Base parent, int posX, int posY) : base(parent)
        {
            this.SetPosition(posX, posY);
            this.SetSize(692 - posX, 564 - posY);
        }

    }
}
