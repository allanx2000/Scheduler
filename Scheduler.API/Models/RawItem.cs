using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.API.Models
{
    public class RawItem : BaseItem
    {
        public RawItem(string name) : base(name)
        {
        }

        public string ParentName { get; set; }
        public List<string> DependencyNames { get; private set; }

        public void AddDependency(string name)
        {
            if (DependencyNames == null)
                DependencyNames = new List<string>();

            DependencyNames.Add(name);
        }

        public bool HasDependencies
        {
            get { return DependencyNames != null; }
        }

    }
}
