using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public static class PangaeaBillIngFinder
    {
        private static Dictionary<PangaeaResource, int> availableResourceCountDict = new Dictionary<PangaeaResource, int>();
        private static List<PangaeaResourceThingBase> availableResourceThings = new List<PangaeaResourceThingBase>();
        private static PangaeaResource lastResource = null;

        public static void Clear()
        {
            availableResourceCountDict.Clear();
            availableResourceThings.Clear();
            lastResource = null;
        }

        public static void RegisterThing(PangaeaResourceThingBase pangaeaThing)
        {
            if (pangaeaThing == null || pangaeaThing.Resource == null)
            {
                return;
            }

            availableResourceThings.Add(pangaeaThing);
            if (!availableResourceCountDict.ContainsKey(pangaeaThing.Resource))
            {
                availableResourceCountDict.Add(pangaeaThing.Resource, pangaeaThing.stackCount);
            }
            else
            {
                availableResourceCountDict[pangaeaThing.Resource] += pangaeaThing.stackCount;
            }
        }

        public static bool ShouldSkipThing(Thing thing, int required)
        {
            if (!(thing is PangaeaResourceThingBase pt))
            {
                return false;
            }

            if (pt == null || !HasRequiredCount(pt.Resource, required) || !IsSameResourceAsLast(pt.Resource))
            {
                return true;
            }

            lastResource = pt.Resource;
            return false;
        }

        private static bool HasRequiredCount(PangaeaResource resource, int required)
        {
            if (resource == null)
            {
                return false;
            }

            return availableResourceCountDict.TryGetValue(resource, out int count) &&  count >= required;
        }

        private static bool IsSameResourceAsLast(PangaeaResource resource)
        {
            if (resource == null)
            {
                return false;
            }

            return (lastResource == null || resource == lastResource);
        }
    }
}