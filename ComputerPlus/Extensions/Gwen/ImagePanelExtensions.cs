using Gwen.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Extensions.Gwen
{
    static internal class ImagePanelExtensions
    {
        static internal void RegularSizeVertical(this ImagePanel panel)
        {
            panel.SetSize(155, 217);
        }
    }
}
