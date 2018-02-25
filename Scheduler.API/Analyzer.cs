using Scheduler.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler.API
{
    public class Analyzer
    {
        public Analysis AnalyzeItems(List<RawItem> items)
        {
            int id = 1;

            Dictionary<string, Item> all = new Dictionary<string, Item>();
            foreach (var i in items)
            {
                all[i.Name] = new Item(id++, i);
            }

            List<Item> rootItems = LinkChildren(all);
            List<Group> groups = LinkDependencies(all);

            Analysis analysis = new Analysis(rootItems, groups);
            
            return analysis;
        }

        private List<Group> LinkDependencies(Dictionary<string, Item> all)
        {
            int groupId = 0;
            foreach (var i in all.Values)
            {
                SetGroup(i, all, ref groupId);
            }
            
            //Group the Items
            Dictionary<int, Group> groups = new Dictionary<int, Group>();

            foreach (var i in all.Values)
            {
                int g = i.GroupID.Value;

                if (!groups.ContainsKey(g))
                    groups[g] = new Group();

                groups[g].Add(i);
            }

            return groups.Values.ToList();
        }

        private void SetGroup(Item i, Dictionary<string, Item> all, ref int groupId)
        {
            if (i.GroupID.HasValue)
                return;

            SortedSet<int> groupIds = new SortedSet<int>();


            if (i.Raw.ParentName != null)
            {
                SetGroup(i.Parent, all, ref groupId);
                groupIds.Add(i.Parent.GroupID.Value);
            }

            if (i.Raw.HasDependencies)
            {
                foreach (string dName in i.Raw.DependencyNames)
                {
                    var d = all[dName];

                    SetGroup(d, all, ref groupId);

                    groupIds.Add(d.GroupID.Value);

                    i.Dependencies.Add(d);
                    d.Dependents.Add(i);
                }

            }

            if (groupIds.Count > 0)
            {
                //Union join Multiple groups
                int minId = groupIds.First();
                i.GroupID = minId;

                if (groupIds.Count > 1)
                {
                    var items = from x in all.Values
                                where (
                                    x.GroupID.HasValue
                                    //    && x.GroupID != minId
                                    && groupIds.Contains(x.GroupID.Value)
                                )
                                select x;

                    foreach (var x in items)
                    {
                        x.GroupID = minId;
                    }
                }
            }
            
            if (!i.GroupID.HasValue)
                i.GroupID = groupId++;
        }

        private List<Item> LinkChildren(Dictionary<string, Item> all)
        {
            List<Item> roots = new List<Item>();

            foreach (var i in all.Values)
            {
                var pn = i.Raw.ParentName;
                if (string.IsNullOrEmpty(pn))
                    roots.Add(i);
                else
                {
                    var parent = all[pn];

                    i.Parent = parent;
                    parent.Children.Add(i);
                }
            }

            return roots;
        }
    }
}
