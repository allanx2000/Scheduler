using System.Collections.Generic;
using Scheduler.API.Models;

namespace Scheduler.API
{
    public class Analysis
    {
        public List<Item> RootItems { get; private set; }
        public List<Group> Groups { get; private set; }

        public Analysis(List<Item> rootItems, List<Group> groups)
        {
            this.RootItems = rootItems;
            this.Groups = groups;
        }
    }
}