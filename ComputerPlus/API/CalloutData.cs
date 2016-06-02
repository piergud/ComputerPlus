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
        /// <summary>
        /// Constructs a new CalloutData object.
        /// </summary>
        /// <param name="callName">The name of the callout.</param>
        /// <param name="shortName">A short name for the callout; this could be anything, such as a radio/penal code, or abbreviation.</param>
        /// <param name="location">The location of the call.</param>
        /// <param name="response">Whether the call requires a Code 2 or Code 3 response.</param>
        /// <param name="description">The description of the call, as received by 911.</param>
        /// <param name="status">The status of the call. Set this to Created or Dispatched, and then update it to UnitResponding when the player accepts the call.</param>
        /// <param name="callPeds">The list of peds to be added to the call. Can be null to not add any peds; peds can always be added later on (for example, you can add a victim ped now, and then add a suspect ped after the player meets him/her).</param>
        /// <param name="callVehicles">The list of vehicles to be added to the call. Can be null to not add any vehicles; vehicles can always be added later on, for example, after the player encounters a suspect vehicle).</param>
        public CalloutData(string callName, string shortName, Vector3 location,
            EResponseType response, string description = "", ECallStatus status = ECallStatus.Created, 
            List<Ped> callPeds = null, List<Vehicle> callVehicles = null)
        {
            ID = Guid.NewGuid();
            Name = callName;
            ShortName = shortName;
            mDescription = description;
            Location = location;
            TimeReceived = DateTime.UtcNow;
            mTimeConcluded = null;
            ResponseType = response;
            mStatus = status;
            mPeds = new List<Ped>();
            mVehicles = new List<Vehicle>();

            if (Peds != null)
                Peds.AddRange(callPeds);

            if (Vehicles != null)
                Vehicles.AddRange(callVehicles);

            mLastUpdated = DateTime.UtcNow;
            mUpdates = new List<CalloutUpdate>();

            mIsPlayerAssigned = true;
            mPrimaryUnit = Configs.UnitNumber;
        }

        internal void AddUpdate(string pText)
        {
            mUpdates.Add(new CalloutUpdate(pText));
            mLastUpdated = DateTime.UtcNow;
        }

        internal void UpdateDescription(string pText)
        {
            mDescription = pText;
            mLastUpdated = DateTime.UtcNow;
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
            mLastUpdated = DateTime.UtcNow;
        }

        internal void AddVehicle(Vehicle veh)
        {
            mVehicles.Add(veh);
            mLastUpdated = DateTime.UtcNow;
        }

        internal void ConcludeCallout()
        {
            if (mStatus == ECallStatus.Completed || mStatus == ECallStatus.Cancelled)
                return;

            mTimeConcluded = DateTime.UtcNow;
            UpdateStatus(ECallStatus.Completed);
            AddUpdate("Code 4; all units return to patrol.");
        }

        internal void CancelCallout()
        {
            if (mStatus == ECallStatus.Completed || mStatus == ECallStatus.Cancelled)
                return;

            mTimeConcluded = DateTime.UtcNow;
            UpdateStatus(ECallStatus.Cancelled);
            AddUpdate("Call is unfounded. Code 4; all units return to patrol.");
        }

        internal void AssignCallToAIUnit()
        {
            mPrimaryUnit = Function.GetRandomAIUnit();
            UpdateStatus(ECallStatus.Unit_Responding);
            mIsPlayerAssigned = false;
        }

        #region Properties

        public Guid ID { get; }
        public string Name { get; }
        public string ShortName { get; }

        private string mDescription = "";
        public string Description { get { return mDescription; } }

        public Vector3 Location { get; }

        /// <summary>
        /// The time the call was dispatched (in GMT/UTC)
        /// </summary>
        public DateTime TimeReceived { get; }

        private DateTime? mTimeConcluded = null;
        /// <summary>
        /// The time the call was concluded (in GMT/UTC)
        /// </summary>
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

        private bool mIsPlayerAssigned = true;
        public bool IsPlayerAssigned { get { return mIsPlayerAssigned; } }

        private string mPrimaryUnit = "";
        public string PrimaryUnit { get { return mPrimaryUnit; } }

        #endregion
    }
}