using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace ComputerPlus.API
{
    public sealed class CalloutData : ICalloutData
    {
        public CalloutData(string callName, string shortName, Vector3 location,
            EResponseType response, string description = "", ECallStatus status = ECallStatus.Created, 
            List<Ped> callPeds = null, List<Vehicle> callVehicles = null)
        {
            ID = Guid.NewGuid();
            Name = callName;
            ShortName = shortName;
            mDescription = description;
            Location = location;
            TimeReceived = DateTime.Now;
            mTimeConcluded = null;
            ResponseType = response;
            mStatus = status;
            mPeds = new List<Ped>();
            mVehicles = new List<Vehicle>();

            if (Peds != null)
                Peds.AddRange(callPeds);

            if (Vehicles != null)
                Vehicles.AddRange(callVehicles);

            mLastUpdated = DateTime.Now;
            mUpdates = new List<CalloutUpdate>();
        }

        internal void AddUpdate(string pText)
        {
            mUpdates.Add(new CalloutUpdate(pText));
            mLastUpdated = DateTime.Now;
        }

        internal void UpdateDescription(string pText)
        {
            mDescription = pText;
            mLastUpdated = DateTime.Now;
            AddUpdate("Description updated.");
        }

        internal void UpdateStatus(ECallStatus status)
        {
            if(status != mStatus)
                AddUpdate(String.Format("STATUS -- {0} => {1}", mStatus, status));

            mStatus = status;
        }

        internal void AddPed(Ped ped)
        {
            mPeds.Add(ped);
            mLastUpdated = DateTime.Now;
        }

        internal void AddVehicle(Vehicle veh)
        {
            mVehicles.Add(veh);
            mLastUpdated = DateTime.Now;
        }

        internal void ConcludeCallout()
        {
            mTimeConcluded = DateTime.Now;
            UpdateStatus(ECallStatus.Completed);
            AddUpdate("Code 4; all units return to patrol.");
        }

        #region Properties

        public Guid ID { get; }
        public string Name { get; }
        public string ShortName { get; }

        private string mDescription = "";
        public string Description { get { return mDescription; } }

        public Vector3 Location { get; }
        public DateTime TimeReceived { get; }

        private DateTime? mTimeConcluded = null;
        public DateTime? TimeConcluded { get { return mTimeConcluded; } }
        public EResponseType ResponseType { get; }

        private ECallStatus mStatus = ECallStatus.Created;
        public ECallStatus Status { get { return mStatus; } }

        private List<Ped> mPeds = new List<Ped>();
        public List<Ped> Peds { get { return mPeds; } }

        private List<Vehicle> mVehicles = new List<Vehicle>();
        public List<Vehicle> Vehicles { get { return mVehicles; } }

        private DateTime mLastUpdated;
        public DateTime LastUpdated { get { return mLastUpdated; } }

        private List<CalloutUpdate> mUpdates = new List<CalloutUpdate>();
        public List<CalloutUpdate> Updates { get { return mUpdates.OrderBy(x=> x.TimeAdded).ToList(); } }

        #endregion
    }
}