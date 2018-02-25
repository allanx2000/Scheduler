using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.API.Models
{
    public abstract class BaseItem
    {
       public string Name  { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public TimeSpan? StartTime { get; set; }
        
        public TimeSpan Duration { get; set; }

        //DaysOfWeek

        private static readonly TimeSpan DefaultTimespan = new TimeSpan(0, 1, 0);
        
        public BaseItem(string name)
        {
            this.Name = name;
            this.Duration = DefaultTimespan;
        }
    }
}
