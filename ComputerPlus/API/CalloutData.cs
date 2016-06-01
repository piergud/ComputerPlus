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
        private string _info;

        public CalloutData(Guid ID, string callName, string callNameAbbreviated, Vector3 location,
            EResponseType response, string info = "", ECallStatus status = ECallStatus.Created, 
            List<Ped> peds = null, List<Vehicle> vehicles = null)
        {
            this.ID = ID;
            this.CallName = callName;
            this.CallNameAbbreviation = callNameAbbreviated;
            this._info = String.Format("[{0:MM/dd/yy HH:mm:ss}] {1}", Function.GetMixedDateTime(), info);
            this.Location = location;
            this.ResponseType = response;
            this.Status = status;
            this.Peds = peds;
            this.Vehicles = vehicles;
        }

        public void UpdateInformation(string text)
        {
            this._info += String.Format("\n[{0:MM/dd/yy HH:mm:ss}] {1}", Function.GetMixedDateTime(), text);
        }

        #region Properties

        public Guid ID { get; }
        public string CallName { get; }
        public string CallNameAbbreviation { get; }
        public string Information { get { return _info; } }
        public Vector3 Location { get; set;}
        public EResponseType ResponseType { get; set; }
        public ECallStatus Status { get; set; }
        public List<Ped> Peds { get; set; }
        public List<Vehicle> Vehicles { get; set; }

        #endregion
    }
}