using Verse;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public static class PangaeaDatabase
    {
        private static Dictionary<string, PangaeaThingEntry> database = new Dictionary<string, PangaeaThingEntry>();
        public static IEnumerable<PangaeaThingEntry> AllEntries => database.Values;

        public static bool TryGetEntry(ThingDef thingDef, out PangaeaThingEntry entry)
        {
            if (thingDef == null)
            {
                entry = null;
                return false;
            }

            return database.TryGetValue(thingDef.defName, out entry);
        }

        public static PangaeaThingEntry GetOrAddEntry(ThingDef thingDef)
        {
            if (thingDef == null)
            {
                return null;
            }

            if (!database.TryGetValue(thingDef.defName, out PangaeaThingEntry entry))
            {
                entry = AddEntry(thingDef);
            }
            return entry;
        }
        
        public static PangaeaThingEntry GetOrNull(ThingDef thingDef)
        {
            if (thingDef == null)
            {
                return null;
            }

            if (!database.TryGetValue(thingDef.defName, out PangaeaThingEntry entry))
            {
                return null;
            }
            return entry;
        }

        public static PangaeaThingEntry AddEntry(ThingDef thingDef)
        {
            if (thingDef == null)
            {
                return null;
            }

            PangaeaThingEntry entry = null;
            if (!database.ContainsKey(thingDef.defName))
            {
                entry = new PangaeaThingEntry(thingDef);
                database.Add(thingDef.defName, entry);
            }
            return entry;
        }

        public static bool TryGetEntryFromThing(Thing thing, out PangaeaThingEntry entry) => TryGetEntryFromThing(thing, out entry, out _);
        /// <returns>Returns true if the returned entry is not null.</returns>
        /// <param name="shouldYieldEntry">Outs true if this should have yielded an entry.</param>
        public static bool TryGetEntryFromThing(Thing thing, out PangaeaThingEntry entry, out bool shouldYieldEntry)
        {
            if (thing is PangaeaThing pangaeaThing)
            {
                shouldYieldEntry = true;
                return TryGetEntry(pangaeaThing.Resource?.ThingDef, out entry);
            }

            if (thing is Corpse corpse)
            {
                shouldYieldEntry = true;
                return TryGetEntry(corpse.InnerPawn.def, out entry);
            }

            if (thing is Pawn pawn)
            {
                shouldYieldEntry = true;
                return TryGetEntry(pawn.def, out entry);
            }

            shouldYieldEntry = false;
            entry = null;
            return false;
        }
    }
}
