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

            private Dictionary<ThingDef, ResourceGraphicDef> specificDict = new Dictionary<ThingDef, ResourceGraphicDef>();
            public List<ResourceGraphicDef> nonSpecific = new List<ResourceGraphicDef>();

            public ResourceGraphicDef GetFor(PangaeaThingEntry entry)
            {
                if (specificDict.TryGetValue(entry.ThingDef, out var graphic))
                {
                    return graphic;
                }
                for (int i = nonSpecific.Count - 1; i >= 0; i--)
                {
                    if (nonSpecific[i].filter.Allows(entry))
                    {
                        return nonSpecific[i];
                    }
                }
                return null;
            }

            public void Register(ResourceGraphicDef graphic)
            {
                if (graphic.resourceType != ResourceType)
                {
                    Log.Error("Tried registering " + graphic + " with resourceType different than " + ResourceType);
                    return;
                }

                if (!graphic.filter.directDefFilter.EnumerableNullOrEmpty())
                {
                    foreach (var thing in graphic.filter.directDefFilter)
                    {
                        specificDict.SetOrAdd(thing, graphic);
                    }
                    return;
                }
                nonSpecific.Add(graphic);
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
}
