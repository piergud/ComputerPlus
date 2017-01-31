using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.DB.Models
{
    public abstract class PersistedModel : IDBModel<PersistedModel>
    {
        public Guid id
        {
            get;
            protected set;
        }
        
        public String Id() {
            return this.id.ToString(); ;
        }
        protected virtual internal void FromMap(Dictionary<String, dynamic> map)
        {
            if (map.ContainsKey("id"))
                this.id = Guid.Parse(map["id"]);
        }
        protected virtual internal Dictionary<String, dynamic> ToMap()
        {
            var map = new Dictionary<String, dynamic>()
            {
                { "id", this.Id() }
            };
            return map;
        }


        protected PersistedModel(Dictionary<String, dynamic> map) : this()
        {
            this.FromMap(map);
        }
        protected PersistedModel() : this(Guid.NewGuid())
        {
            
        }

        protected PersistedModel(Guid id)
        {
            this.id = id;
        }
    }
}
