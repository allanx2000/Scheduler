using Scheduler.API;
using Scheduler.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            List<RawItem> list = new List<RawItem>();

            RawItem i;


            i = new RawItem("G");
            i.StartTime = new TimeSpan(1, 0, 0);
            list.Add(i);

            i = new RawItem("C");
            i.AddDependency("E");
            i.AddDependency("B");
            list.Add(i);

            i = new RawItem("D");
            i.AddDependency("A");
            list.Add(i);

            i = new RawItem("E");
            i.AddDependency("A");
            list.Add(i);

            i = new RawItem("F");
            list.Add(i);

            i = new RawItem("A");
            i.ParentName = "G";
            list.Add(i);

            i = new RawItem("B");
            list.Add(i);

            //A,B,D,E,C
            //F

            //TODO: Need StartTime, Duration to validate

            Analyzer analyzer = new Analyzer();

            Analysis result = analyzer.AnalyzeItems(list);

            var schedule = result.Groups.First().GetSchedule();
        }
    }
}
