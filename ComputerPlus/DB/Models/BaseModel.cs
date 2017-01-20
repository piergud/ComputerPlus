using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.DB.Models
{
    public abstract class BaseModel : IDBModel<BaseModel>
    {
        public String id
        {
            get;
            protected set;
        }
        
        public String Id() {
            return this.id;
        }
        protected internal void FromMap(Dictionary<String, dynamic> map)
        {
            if (map.ContainsKey("id")) this.id = map["id"];
        }
        protected internal Dictionary<String, dynamic> ToMap()
        {
            var map = new Dictionary<String, dynamic>()
            {
                { "id", this.Id() }
            };
            return map;
        }


        protected BaseModel(Dictionary<String, dynamic> map)
        {
            this.FromMap(map);
        }
        protected BaseModel()
        {

        }
    }
}
