using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage.Forms;

namespace ComputerPlus.Extensions.Gwen
{
    internal static class FormExtensions
    {
        internal static void Close(this GwenForm form)
        {
            try
            {
                if (form == null) return;
                if (form.Window.IsClosable)
                    form.Window.Close();
            }
            catch { }
        }
        internal static bool IsOnTop(this GwenForm form)
        {
            
            try
            {
                if (form == null) return false;
                return form.Window.IsOnTop;
            }
            catch
            {
                return false;
            }
        }

        internal static bool IsOpen(this GwenForm form)
        {            
            try
            {
                if (form == null || form.Window == null) return false;
                return (form.Window.IsVisible || form.Window.IsOnTop || form.Window.IsClosable);
            }
            catch
            {
                return false;
            }
        }
    }
}
