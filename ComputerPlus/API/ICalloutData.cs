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
        string Name { get; }
        string ShortName { get; }
        string Description { get; }
        Vector3 Location { get; }
        DateTime TimeReceived { get; }
        DateTime? TimeConcluded { get; }
        EResponseType ResponseType { get; }
        ECallStatus Status { get; }
        List<Ped> Peds { get; }
        List<Vehicle> Vehicles { get; }
        DateTime LastUpdated { get; }
        List<CalloutUpdate> Updates { get; }
        bool IsPlayerAssigned { get; }
        string PrimaryUnit { get; }
    }
}