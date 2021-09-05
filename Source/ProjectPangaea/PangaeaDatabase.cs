using Verse;
using System.Linq;
using System.Collections.Generic;
using ProjectPangaea.Overrides;

namespace ProjectPangaea
{
    public class PangaeaThingEntry
    {
        public ThingDef ThingDef { get; }

        public ModExt_Extinct ExtinctExtension { get; }

        public OrganismCategory Category { get; }

        private DNA dna;
        public DNA DNA => dna;

        //TODO remove dna overriding and instead have dna splicing
        public bool IsDNAOverriden { get; private set; }

        public float dissectionYieldEfficiency = 1;

        public Fossil Fossil { get; }

        public List<ThingEfficiency> ExtraDNAExtractionProducts { get; } = new List<ThingEfficiency>();
        public bool overrideBaseDrops;

        public PangaeaThingEntry(ThingDef thingDef, AnimalType animalType = AnimalType.Unspecified)
        {
            ThingDef = thingDef;
            ExtinctExtension = thingDef.GetModExtension<ModExt_Extinct>();
            Category = GetCategory(thingDef, animalType);

            dna = new DNA(thingDef, Category);

            if (ExtinctExtension != null)
            {
                Fossil = new Fossil(thingDef, ExtinctExtension);
            }
        }

        public PangaeaResource GetResourceOfCategory(ThingCategoryDef category)
        {
            if (category == PangaeaThingCategoryDefOf.PangaeaDNA)
            {
                return DNA;
            }
            if (category == PangaeaThingCategoryDefOf.PangaeaFossils)
            {
                return Fossil;
            }
            return null;
        }

        public PangaeaResource GetResourceOfDef(ThingDef def)
        {
            if (def == PangaeaThingDefOf.Pangaea_DNABase)
            {
                return DNA;
            }
            if (def == PangaeaThingDefOf.Pangaea_FossilBase)
            {
                return Fossil;
            }
            //TODO embryo
            return null;
        }

        public void OverrideDNA(DNA newDNA)
        {
            dna = newDNA;
            IsDNAOverriden = true;
        }

        private static OrganismCategory GetCategory(ThingDef thingDef, AnimalType animalType)
        {
            if (thingDef.plant != null)
            {
                return new PlantCategory(thingDef);
            }
            return new AnimalCategory(thingDef, animalType);
        }
    }

    public static class PangaeaDatabase
    {
        private static Dictionary<string, PangaeaThingEntry> database = new Dictionary<string, PangaeaThingEntry>();
        private static Dictionary<string, PangaeaThingEntry> extinctOnlyDatabase = new Dictionary<string, PangaeaThingEntry>();

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

        public static PangaeaThingEntry AddEntry(ThingDef thingDef) => AddEntry(thingDef, CalculateAnimalType(thingDef));

        private static AnimalType CalculateAnimalType(ThingDef thingDef)
        {
            foreach (var categoryDef in DefDatabase<PangaeaAnimalCategorizationDef>.AllDefs.Reverse())
            {
                if (categoryDef.ContainsDef(thingDef, out var entry))
                {
                    return entry.animalType;
                }
            }

            ModExt_Extinct extinct = thingDef.GetModExtension<ModExt_Extinct>();
            if (extinct != null)
            {
                return extinct.animalType;
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

                if (thingDef.HasModExtension<ModExt_Extinct>())
                {
                    extinctOnlyDatabase.Add(thingDef.defName, entry);
                }
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

            List<ThingEfficiency> extraProducts = overrideDef.dnaExtractionExtraProducts;
            if (!extraProducts.NullOrEmpty())
            {
                entry.ExtraDNAExtractionProducts.AddRange(extraProducts);
            }

            ThingDef dnaDropOverrideOwner = overrideDef.dnaExtractionYieldOverride;
            if (dnaDropOverrideOwner != null)
            {
                if (TryGetEntry(dnaDropOverrideOwner, out PangaeaThingEntry overrideDNAEntry))
                {
                    entry.OverrideDNA(overrideDNAEntry.DNA);
                }
            }

            float dnaDropEfficiencyOverride = overrideDef.dnaExtractionYieldEfficiency;
            if (dnaDropEfficiencyOverride >= 0)
            {
                entry.dissectionYieldEfficiency = dnaDropEfficiencyOverride;
            }

            var dnaOverride = overrideDef.dnaOverride;
            if (dnaOverride != null)
            {
                if (!dnaOverride.label.NullOrEmpty())
                {
                    entry.DNA.overrideLabel = dnaOverride.label;
                }

                if (!dnaOverride.description.NullOrEmpty())
                {
                    entry.DNA.overrideDescription = dnaOverride.description;
                }
            }

            var fossilOverride = overrideDef.fossilOverride;
            if (fossilOverride != null && entry.Fossil != null)
            {
                if (!fossilOverride.label.NullOrEmpty())
                {
                    entry.Fossil.overrideLabel = fossilOverride.label;
                }

                if (!fossilOverride.description.NullOrEmpty())
                {
                    entry.Fossil.overrideDescription = fossilOverride.description;
                }
            }

            return entry;
        }

        public static PangaeaThingEntry RandomEntry() => database.RandomElement().Value;
        public static PangaeaThingEntry RandomExtinctEntry() => extinctOnlyDatabase.RandomElement().Value;

        public static IEnumerable<PangaeaThingEntry> AllEntries => database.Values;
        public static IEnumerable<PangaeaThingEntry> AllExtinctEntries => extinctOnlyDatabase.Values;

        public static bool HasDNA(this ThingDef thingDef, out PangaeaThingEntry e) => TryGetEntry(thingDef, out e) && e.DNA != null;
        public static bool HasDNA(this ThingDef thingDef) => thingDef.HasDNA(out _);
        public static bool IsExtinct(this ThingDef thingDef) => thingDef.IsExtinct(out _);
        public static bool IsExtinct(this ThingDef thingDef, out PangaeaThingEntry e) => TryGetEntry(thingDef, out e) && e.ExtinctExtension != null;

        public static int Count(this PangaeaResource resource, Map map)
        {
            var thingLister = map.listerThings;
            return thingLister.ThingsInGroup(ThingRequestGroup.HaulableEver)
                .Count(t => t is PangaeaResourceThingBase pt && pt.Resource == resource);
        }

        public static bool TryGetEntryFromThing(Thing thing, out PangaeaThingEntry entry) => TryGetEntryFromThing(thing, out entry, out _);
        /// <returns>Returns true if the returned entry is not null.</returns>
        /// <param name="shouldYieldEntry">Returns true if this should have yielded an entry.</param>
        public static bool TryGetEntryFromThing(Thing thing, out PangaeaThingEntry entry, out bool shouldYieldEntry)
        {
            if (thing is PangaeaResourceThingBase pangaeaThing)
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
