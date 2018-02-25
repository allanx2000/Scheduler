using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.API.Models
{
    public class Item : BaseItem
    {
        
        public Item(string name) : base(name)
        {
            Dependencies = new List<Item>();
            Dependents = new List<Item>();
            Children = new List<Item>();
        }

        public Item(int id, RawItem raw) : this(raw.Name)
        {
            ID = id;
            this.Raw = raw;

            CopyValues(raw);
        }

        public int? GroupID { get; set; }

        private void CopyValues(RawItem raw)
        {
            this.Description = raw.Description;
            this.StartTime = raw.StartTime;
            this.Type = raw.Type;

        }

        public int ID { get; set; }
        public RawItem Raw { get; }
        
        /// <summary>
        /// Items this depends on before completing
        /// </summary>
        public List<Item> Dependencies { get; private set; }

        /// <summary>
        /// Items that depend on this item
        /// </summary>
        public List<Item> Dependents { get; private set; }
        
        public Item Parent { get; set; }
        public List<Item> Children { get; private set; }

        public override string ToString()
        {
            return Name; //TODO: Add othe values
        }
    }
}
