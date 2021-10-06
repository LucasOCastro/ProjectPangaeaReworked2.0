using Verse;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public static class PangaeaDatabase
    {
        public static bool Initiated { get; private set; }

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

        public static void Init()
        {
            //Add extinct entries to DB
            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                if (thingDef.IsExtinct())
                {
                    AddEntry(thingDef);
                }
            }
            OverrideHelper.DoOverrides();

            Initiated = true;
        }

        public static void AssertInitiated(string before = "")
        {
            if (!Initiated)
            {
                throw new System.Exception("Should initiate database" + (before.NullOrEmpty() ? "!" : (" before " + before + "!")));
            }
        }
    }
}
