using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.API
{
    public static class Functions
    {
        public static Guid GenerateNewCallID()
        {
            return Guid.NewGuid();
        }

        public static void SetActiveCallout(CalloutData data)
        {
            EntryPoint.gActiveCallout = data;
        }
    }
}