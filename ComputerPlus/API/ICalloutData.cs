using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.API
{
    public interface ICalloutData
    {
        Guid ID { get; }
        string CallName { get; }
        string CallNameAbbreviation { get; }
        Vector3 Location { get; set; }
        EResponseType ResponseType { get; set; }
        ECallStatus Status { get; set; }
        List<Ped> Peds { get; set; }
        List<Vehicle> Vehicles { get; set; }
    }
}