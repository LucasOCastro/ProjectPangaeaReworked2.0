using Verse;
using System.Linq;
using System.Collections.Generic;
using ProjectPangaea.Overrides;

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
                entry = AddEntry(thingDef, CalculateAnimalType(thingDef));
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

        private static AnimalType CalculateAnimalType(ThingDef thingDef)
        {
            ModExt_Extinct extinct = thingDef.GetModExtension<ModExt_Extinct>();
            if (extinct != null)
            {
                return extinct.animalType;
            }

            foreach (var categoryDef in DefDatabase<PangaeaAnimalCategorizationDef>.AllDefs.Reverse())
            {
                if (categoryDef.ContainsDef(thingDef, out var entry))
                {
                    return entry.animalType;
                }
            }

            return AnimalType.Unspecified;
        }

        public static PangaeaThingEntry AddEntry(ThingDef thingDef, AnimalType animalType)
        {
            if (thingDef == null)
            {
                return null;
            }

            PangaeaThingEntry entry = null;
            if (!database.ContainsKey(thingDef.defName))
            {
                entry = new PangaeaThingEntry(thingDef, animalType);
                database.Add(thingDef.defName, entry);
            }
            return entry;
        }

        public static PangaeaThingEntry AddOrUpdateFromOverrideDef(PangaeaOverrideDef overrideDef)
        {
            ThingDef thingDef = overrideDef.overridenThingDef;
            if (thingDef == null)
            {
                return null;
            }

            PangaeaThingEntry entry = GetOrAddEntry(thingDef);

            var dnaOverride = overrideDef.dnaOverride;
            if (dnaOverride != null && entry.DNA != null)
            {
                dnaOverride.Override(entry.DNA);
            }

            var fossilOverride = overrideDef.fossilOverride;
            if (fossilOverride != null && entry.Fossil != null)
            {
                fossilOverride.Override(entry.Fossil);
            }

            return entry;
        }

        public static bool HasDNA(this ThingDef thingDef, out PangaeaThingEntry e) => TryGetEntry(thingDef, out e) && e.DNA != null;
        public static bool HasDNA(this ThingDef thingDef) => thingDef.HasDNA(out _);
        public static bool IsExtinct(this ThingDef thingDef) => thingDef.IsExtinct(out _);
        public static bool IsExtinct(this ThingDef thingDef, out PangaeaThingEntry e) => TryGetEntry(thingDef, out e) && e.ExtinctExtension != null;

        public static int Count(this PangaeaResource resource, Map map)
        {
            var thingLister = map.listerThings;
            return thingLister.ThingsInGroup(ThingRequestGroup.HaulableEver)
                .Count(t => t is PangaeaThing pt && pt.Resource == resource);
        }

        public static bool TryGetEntryFromThing(Thing thing, out PangaeaThingEntry entry) => TryGetEntryFromThing(thing, out entry, out _);
        /// <returns>Returns true if the returned entry is not null.</returns>
        /// <param name="shouldYieldEntry">Outs true if this should have yielded an entry.</param>
        public static bool TryGetEntryFromThing(Thing thing, out PangaeaThingEntry entry, out bool shouldYieldEntry)
        {
            if (thing is PangaeaThing pangaeaThing)
            {
                shouldYieldEntry = true;
                return TryGetEntry(pangaeaThing.Resource?.ParentThingDef, out entry);
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
