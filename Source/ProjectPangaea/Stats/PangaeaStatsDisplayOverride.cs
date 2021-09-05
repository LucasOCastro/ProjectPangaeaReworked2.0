using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace ProjectPangaea
{
    [HarmonyPatch(typeof(RaceProperties), "SpecialDisplayStats")]
    public static class PangaeaStatsDisplayOverride
    {
        public static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> originalResult, ThingDef parentDef, StatRequest req)
        {
            foreach (StatDrawEntry result in originalResult)
            {
                yield return result;
            }

            if (!PangaeaDatabase.TryGetEntry(parentDef, out PangaeaThingEntry entry))
            {
                yield break;
            }

            var category = PangaeaStatCategoryDefOf.PangaeaStats;

            string typeLabel = "Pangaea_AnimalType".Translate();
            string typeValue = ((AnimalCategory)entry.Category).Type.Translate();
            string typeDescription = GenDescriptionForAnimalTypeStat();
            yield return new StatDrawEntry(category, typeLabel, typeValue, typeDescription, 0);

            string extinctionLabel = "Pangaea_ExtinctionStatus".Translate();
            string extinctionValue = entry.Category.ExtinctionStatus.Translate();
            string extinctionDescription = "Pangaea_ExtinctionStatusExplanation".Translate();
            yield return new StatDrawEntry(category, extinctionLabel, extinctionValue, extinctionDescription, 0);
        }

        private static string GenDescriptionForAnimalTypeStat()
        {
            string description = "Pangaea_AnimalTypeExplanation".Translate();
            description += "\n\n" + "Pangaea_PossibleValues".Translate() + ":";
            description += "\n" + PangaeaEnumExtension.AllEnumValuesTranslated<AnimalType>(",\n") + ".";
            return description;
        }
    }
}
