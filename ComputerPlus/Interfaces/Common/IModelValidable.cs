using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.Common
{
    interface IModelValidable
    {
        bool Validate(out KeyValuePair<String, String> failReason);
        bool Validate(out Dictionary<String, String> failReasons);
        String ValidationFailureFormat(String prop, String message);
    }
}
