using Scheduler.API.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Scheduler.API
{
    public class Group : List<Item>
    {
        private List<Item> topo;

        public IEnumerable<Item> TopologicalOrder
        {
            get
            {
                if (topo == null)
                    GenerateTopological();

                return topo;
            }
        }


        public List<SortedItem> GetSchedule()
        {
            return GetSchedule(DateTime.Today);
        }

        public class SortedItem : IComparable
        {
            public SortedItem(DateTime st, Item i)
            {
                this.StartTime = st;
                this.Item = i;
            }

            public DateTime StartTime { get; set; }
            public Item Item { get; set; }

            public override string ToString()
            {
                return StartTime.ToString("yyyy-mm-dd HH:mm") + ": " + Item.Name;
            }

            public int CompareTo(object obj)
            {
                SortedItem other = obj as SortedItem;

                if (obj == null)
                    return 1;
                else
                    return StartTime.CompareTo(other.StartTime);
            }
            
        }

        public List<SortedItem> GetSchedule(DateTime startTime)
        {

            List<SortedItem> sorted = new List<SortedItem>();

            Dictionary<String, DateTime> endTimes = new Dictionary<string, DateTime>();

            foreach (var i in TopologicalOrder)
            {
                List<DateTime> dst = new List<DateTime>();

                dst.Add(ToDate(startTime, i.StartTime));

                foreach (var d in i.Dependencies)
                {
                    dst.Add(endTimes[d.Name]);
                }

                var st = dst.Max();
                var et = ToDate(st, i.StartTime) + i.Duration;

                sorted.Add(new SortedItem(st, i));
                endTimes.Add(i.Name, et);
            }

            sorted.Sort();

            return sorted;
        }

        private DateTime ToDate(DateTime startTime, TimeSpan? hhMM)
        {
            if (!hhMM.HasValue)
                return startTime;

            var val = hhMM.Value;

            DateTime dt = new DateTime(startTime.Year, startTime.Month, startTime.Day
                , val.Hours, val.Minutes, 0);

            if (startTime.Hour > val.Hours && startTime.Minute > val.Minutes)
                dt = dt.AddDays(1);

            return dt;
        }

        private void GenerateTopological()
        {
            List<Item> order = new List<Item>();

            foreach (var i in this)
            {
                RecursiveAdd(order, i);
            }

            topo = order;
        }

        private void RecursiveAdd(List<Item> order, Item i)
        {
            if (i.Parent != null)
                RecursiveAdd(order, i.Parent);

            foreach (var d in i.Dependencies)
            {
                RecursiveAdd(order, d);
            }

            if (!order.Contains(i))
                order.Add(i);
        }

    }
}