using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea.Production
{
    public class Pangaea_DissectCorpseRecipeExtension : Pangaea_YieldDNARecipeExtension
    {
        public float freshEfficiency;
        public float rottingEfficiency;
        public float dessicatedEfficiency;

        public override float ResolveEfficiencyFromIngredient(Thing ingredient)
        {
            if (!(ingredient is Corpse corpse))
            {
                return base.ResolveEfficiencyFromIngredient(ingredient);
            }

            float corpseEfficiency = RotEfficiencyForCorpse(corpse);
            float dnaAmount = corpse.InnerPawn.GetStatValue(PangaeaStatDefOf.Pangaea_DNAAmount);
            return corpseEfficiency * dnaAmount;
        }

        public float RotEfficiencyForCorpse(Corpse corpse)
        {
            RotStage rotStage = corpse.GetRotStage();
            switch (rotStage)
            {
                case RotStage.Fresh: return freshEfficiency;
                case RotStage.Rotting: return rottingEfficiency;
                case RotStage.Dessicated: return dessicatedEfficiency;
                default: return 1;
            }
        }

        public override PangaeaBillCounter GetBillCounter(RecipeDef recipe)
        {
            return new PangaeaBillCounter(recipe, ThingCategoryDefOf.CorpsesAnimal, specialFilterToDisallow: PangaeaSpecialThingFilterDefOf.Pangaea_CorpsesWithoutDNA);
        }

        public override List<PangaeaThingEntry> GetListerEntries(RecipeDef recipe)
        {
            return PangaeaDatabase.AllEntries.Where(e => e.ThingDef.race?.corpseDef != null).ToList();
        }
    }
}