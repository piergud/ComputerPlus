using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace ComputerPlus.API
{
    public class CalloutData : ICalloutData
    {
        #region Properties

        public Guid ID { get; set; }
        public string FullName { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public Vector3 Location { get; set; }
        public string Address { get; set; }
        public DateTime CallReceived { get; set; }
        public DateTime UnitDispatched { get; set; }
        public DateTime CallConcluded { get; set; }
        public EResponseType ResponseType { get; set; }
        public ECallStatus State { get; set; }
        public List<Ped> Peds { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<CalloutUpdate> Updates { get; set; }

        #endregion
    }
}