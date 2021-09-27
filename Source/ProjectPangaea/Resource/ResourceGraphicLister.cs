using Verse;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using System;

namespace ProjectPangaea
{
    public static class ResourceGraphicLister
    {
        private class ResourceTypeGraphicEntry
        {
            public ResourceTypeDef ResourceType { get; }

            public ResourceTypeGraphicEntry(ResourceTypeDef resourceType) => ResourceType = resourceType;

            private static Dictionary<ThingDef, ResourceGraphicDef> specificDict = new Dictionary<ThingDef, ResourceGraphicDef>();
            private static List<ResourceGraphicDef> nonSpecificList = new List<ResourceGraphicDef>();

            public ResourceGraphicDef GetFor(PangaeaThingEntry entry)
            {
                if (specificDict.TryGetValue(entry.ThingDef, out var graphicDef))
                {
                    return graphicDef;
                }
                for (int i = 0; i < nonSpecificList.Count; i++)
                {
                    if (nonSpecificList[i].filter.Allows(entry))
                    {
                        graphicDef = nonSpecificList[i];
                    }
                }
                return graphicDef;
            }

            public void Register(ResourceGraphicDef graphicDef)
            {
                if (graphicDef.resourceType != ResourceType) return;

                if (graphicDef.filter.directDefFilter != null)
                {
                    foreach (var thing in graphicDef.filter.directDefFilter)
                    {
                        specificDict.SetOrAdd(thing, graphicDef);
                    }
                    return;
                }
                nonSpecificList.Add(graphicDef);
            }
        }

        private static Dictionary<ResourceTypeDef, ResourceTypeGraphicEntry> graphics = new Dictionary<ResourceTypeDef, ResourceTypeGraphicEntry>();
        private static ResourceTypeGraphicEntry GetGraphicEntry(ResourceTypeDef resourceType)
        {
            if (!graphics.TryGetValue(resourceType, out var entry))
            {
                entry = new ResourceTypeGraphicEntry(resourceType);
                graphics.Add(resourceType, entry);
            }
            return entry;
        }

        public static void Init()
        {
            foreach (var def in DefDatabase<ResourceGraphicDef>.AllDefs)
            {
                GetGraphicEntry(def.resourceType).Register(def);
            }
        }

        public static ResourceGraphicDef GetFor(PangaeaThingEntry entry, ResourceTypeDef resourceType)
        {
            return GetGraphicEntry(resourceType).GetFor(entry);
        }
    }


    /*public static class ResourceGraphicLister
    {
        private static Dictionary<Tuple<PangaeaDiet, AnimalType, ExtinctionStatus>, ResourceGraphicDef> dict = new Dictionary<Tuple<PangaeaDiet, AnimalType, ExtinctionStatus>, ResourceGraphicDef>();

        private static ResourceGraphicDef plantDNAGraph;

        private static Tuple<PangaeaDiet, AnimalType, ExtinctionStatus> Tuple(PangaeaDiet diet, AnimalType type, ExtinctionStatus extinction)
            => System.Tuple.Create(diet, type, extinction);


        public static ResourceGraphicDef GetDNAGraphicType(OrganismCategory category)
        {
            if (category is AnimalCategory animalCategory)
            {
                return GetDNAGraphicType(animalCategory);
            }
            return plantDNAGraph;
        }

        public static ResourceGraphicDef GetDNAGraphicType(AnimalCategory category)
        {
            var tuple = Tuple(category.Diet, category.Type, category.ExtinctionStatus);
            dict.TryGetValue(tuple, out ResourceGraphicDef result);
            return result;
        }

        public static void Init()
        {
            foreach (var dnaGraphic in DefDatabase<ResourceGraphicDef>.AllDefs)
            {
                if (dnaGraphic.isPlant)
                {
                    plantDNAGraph = dnaGraphic;
                    continue;
                }

                var tuple = Tuple(dnaGraphic.diet, dnaGraphic.animalType, dnaGraphic.extinctionStatus);
                if (!dict.ContainsKey(tuple))
                {
                    dict.Add(tuple, dnaGraphic);
                }
                else dict[tuple] = dnaGraphic;
            }
        }
    }*/
}
