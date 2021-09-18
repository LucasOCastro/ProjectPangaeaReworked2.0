using Verse;
using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using ProjectPangaea.Production.Splicing;


namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
    public class RecipeProductsOverride
    {
        public static IEnumerable<Thing> Postfix(IEnumerable<Thing> originalResult, RecipeDef recipeDef, List<Thing> ingredients)
        {
            var spliceExtension = recipeDef.GetModExtension<Pangaea_DNASplicingRecipeExtension>();
            if (spliceExtension != null)
            {
                foreach (var result in SplicingProductsOverride.CurrentBill.MakeResults())
                    yield return result;
                yield break;
            }

            var yieldDNAExtension = recipeDef.GetModExtension<Pangaea_YieldDNARecipeExtension>();
            var createLifeExtension = recipeDef.GetModExtension<Pangaea_CreateLifeRecipeExtension>();
            if (yieldDNAExtension == null && createLifeExtension == null)
            {
                foreach (Thing result in originalResult)
                {
                    yield return result;
                }
                yield break;
            }

            Thing entryIngredient = null;
            PangaeaThingEntry entry = null;
            foreach (Thing ingredient in ingredients)
            {
                PangaeaDatabase.TryGetEntryFromThing(ingredient, out entry, out bool shouldHaveEntry);
                if (shouldHaveEntry)
                {
                    entryIngredient = ingredient;
                    break;
                }
            }
            if (entry == null)
            {
                foreach (Thing result in originalResult)
                {
                    yield return result;
                }
                yield break;
            }

            foreach (Thing result in originalResult)
            {
                if (yieldDNAExtension != null && result is PangaeaThing pt && pt.IsOfType(typeof(DNA)))
                {
                    pt.Resource = entry.DNA;
                    float efficiency = yieldDNAExtension.ResolveEfficiencyFromIngredient(entryIngredient);
                    pt.stackCount = Mathf.Max(1, Mathf.CeilToInt(pt.stackCount * efficiency));
                }
                else if (createLifeExtension != null && result.def.IsEgg)
                {
                    var eggLayer = entry.ThingDef.GetCompProperties<CompProperties_EggLayer>();
                    ThingDef eggDef = eggLayer?.eggFertilizedDef;
                    if (eggDef != null)
                    {
                        float efficiency = createLifeExtension.ResolveEfficiencyFromIngredient(entryIngredient);
                        Thing egg = ThingMaker.MakeThing(eggDef);
                        egg.stackCount = Mathf.Max(1, Mathf.CeilToInt(result.stackCount * efficiency));
                        yield return egg;
                    }
                    continue;
                }
                //TODO embryo

                yield return result;
            }
        }
    }
}
