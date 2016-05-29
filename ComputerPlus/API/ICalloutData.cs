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
        Guid ID { get; set; }
        string FullName { get; set; }
        string Abbreviation { get; set; }
        string Description { get; set; }
        Vector3 Location { get; set; }
        string Address { get; set; }
        DateTime CallReceived { get; set; }
        DateTime UnitDispatched { get; set; }
        DateTime CallConcluded { get; set; }
        EResponseType ResponseType { get; set; }
        ECallStatus State { get; set; }
        List<Ped> Peds { get; set; }
        List<Vehicle> Vehicles { get; set; }
        List<CalloutUpdate> Updates { get; set; }
    }
}