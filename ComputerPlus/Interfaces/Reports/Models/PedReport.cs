using ComputerPlus.Controllers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ComputerPlus.Interfaces.Reports.Models
{
    internal struct DetailedEntity
    {
        public List<ArrestReport> Arrests;
        public List<TrafficCitation> TrafficCitations;
        public ComputerPlusEntity Entity;
        bool HasTrafficCitations
        {
            get
            {
                return TrafficCitations != null && TrafficCitations.Any();
            }
        }

        bool HasArrests
        {
            get
            {
                return Arrests != null && Arrests.Any();
            }
        }

        internal DetailedEntity(ComputerPlusEntity entity, List<ArrestReport> arrests = null, List<TrafficCitation> citations = null)
        {
            Entity = entity;
            Arrests = arrests != null ? arrests : new List<ArrestReport>();
            TrafficCitations = citations != null ? citations : new List<TrafficCitation>();
        }
    }
}
