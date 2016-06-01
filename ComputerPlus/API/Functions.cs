using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.API
{
    public static class Functions
    {
        /// <summary>
        /// Generates a new callout ID.
        /// </summary>
        /// <returns>The ID number.</returns>
        public static Guid GenerateNewCallID()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Sets the active callout on the computer.
        /// </summary>
        /// <param name="data">The CalloutData object to set as active.</param>
        public static void SetActiveCallout(CalloutData data)
        {
            EntryPoint.gActiveCallout = data;
        }

        /// <summary>
        /// Registers the callout with the computer.
        /// </summary>
        /// <param name="data">The CalloutData object to register.</param>
        public static void CreateCallout(CalloutData data)
        {
            throw new NotImplementedException();
        }
    }
}