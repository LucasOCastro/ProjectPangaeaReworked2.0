using Verse;
using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
    public class RecipeProductsOverride
    {
        public static IEnumerable<Thing> Postfix(IEnumerable<Thing> originalResult, RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, IBillGiver billGiver)
        {
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
                //TODO REMOVE
                Log.Error("Couldnt locate entry!");
                foreach (Thing result in originalResult)
                {
                    yield return result;
                }
                yield break;
            }

            foreach (Thing result in originalResult)
            {
                if (yieldDNAExtension != null && result is DNAThing dnaThing)
                {
                    dnaThing.SetResource(entry.DNA);
                    //TODO currently ignores yieldDNAExtension.baseYieldPerExtraction and corpse efficiency
                    float efficiency = yieldDNAExtension.ResolveEfficiencyFromIngredient(entryIngredient);
                    dnaThing.stackCount = Mathf.Max(1, Mathf.CeilToInt(dnaThing.stackCount * efficiency));
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

                yield return result;
            }
        }

        private static void ProcessDNAResult(DNAThing thing, RecipeDef recipe)
        {
            var yieldDNAExtension = recipe.GetModExtension<Pangaea_YieldDNARecipeExtension>();
            if (yieldDNAExtension == null)
            {
                return;
            }


        }
    }

    /*[HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
    public class RecipeProductsOverride
    {
        public static IEnumerable<Thing> Postfix(IEnumerable<Thing> originalResult, RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, IBillGiver billGiver)
        {
            foreach (Thing original in originalResult)
            {
                yield return original;
            }

            float efficiency = (recipeDef.efficiencyStat != null) ? worker.GetStatValue(recipeDef.efficiencyStat) : 1f;
            if (recipeDef.workTableEfficiencyStat != null && billGiver is Building_WorkTable building_WorkTable)
            {
                efficiency *= building_WorkTable.GetStatValue(recipeDef.workTableEfficiencyStat);
            }

            bool yieldDNA = recipeDef.HasModExtension<Pangaea_YieldDNARecipeExtension>();
            bool createLife = recipeDef.HasModExtension<Pangaea_CreateLifeRecipeExtension>();

            HashSet<ThingDef> processedIngredients = new HashSet<ThingDef>();
            foreach (Thing ingredient in ingredients)
            {
                if (processedIngredients.Contains(ingredient.def))
                {
                    continue;
                }

                if (yieldDNA)
                {
                    foreach (Thing result in ExtractDNAOverride(recipeDef, ingredient, efficiency))
                    {
                        yield return result;
                    }
                }

                if (createLife)
                {
                    foreach (Thing result in CreateLifeOverride(recipeDef, ingredient, efficiency))
                    {
                        yield return result;
                    }
                }

                processedIngredients.Add(ingredient.def);
            }
        }

        private static IEnumerable<Thing> CreateLifeOverride(RecipeDef recipeDef, Thing ingredient, float efficiency)
        {
            if (!(ingredient is DNAThing dnaThing) || dnaThing.Resource == null)
            {
                yield break;
            }

            ThingDef resultDef = null;
            if (dnaThing.Resource.IsFromAnimal)
            {
                ThingDef animalDef = dnaThing.Resource.ParentThingDef;

                CompProperties_EggLayer eggComp = animalDef.GetCompProperties<CompProperties_EggLayer>();
                if (eggComp != null)
                {
                    resultDef = eggComp.eggFertilizedDef;
                }
            }
            else if (dnaThing.Resource.IsFromPlant)
            {
                ThingDef plantDef = dnaThing.Resource.ParentThingDef;
                //TODO: ADD PLANT
            }

            if (resultDef != null)
            {
                var createLifeExtension = recipeDef.GetModExtension<Pangaea_CreateLifeRecipeExtension>();
                int baseCount = createLifeExtension.yieldPerCreation.RandomInRange;
                Thing result = ThingMaker.MakeThing(resultDef);
                result.stackCount = Mathf.CeilToInt(baseCount * efficiency);
                yield return result;
            }
        }

        private static IEnumerable<Thing> ExtractDNAOverride(RecipeDef recipeDef, Thing ingredient, float efficiency)
        {
            PangaeaThingEntry entry = null;
            int baseCount = 1;

            if (ingredient is FossilThing fossilThing && fossilThing.Resource != null)
            {
                var extractExtension = recipeDef.GetModExtension<Pangaea_ExtractFromFossilRecipeExtension>();
                if (extractExtension != null && fossilThing.Resource.ParentThingDef.HasDNA(out entry))
                {
                    baseCount = extractExtension.baseYieldPerExtraction.RandomInRange;
                }
            }
            else if (ingredient is Corpse corpse)
            {

                var dissectExtension = recipeDef.GetModExtension<Pangaea_DissectCorpseRecipeExtension>();
                if (dissectExtension != null && corpse.InnerPawn.def.HasDNA(out entry))
                {
                    baseCount = Mathf.FloorToInt(corpse.InnerPawn.GetStatValue(PangaeaStatDefOf.Pangaea_DNAAmount));
                    efficiency *= dissectExtension.EfficiencyForCorpse(corpse);
                }
            }
            else yield break;

            if (entry != null)
            {
                DNAThing result = DNAThing.MakeDNAThing(entry.DNA);
                result.stackCount = Mathf.CeilToInt(baseCount * efficiency * entry.dissectionYieldEfficiency);
                yield return result;
            }

            if (!entry.ExtraDNAExtractionProducts.NullOrEmpty())
            {
                foreach (ThingEfficiency product in entry.ExtraDNAExtractionProducts)
                {
                    Thing result = ThingMaker.MakeThing(product.thingDef);
                    result.stackCount = Mathf.CeilToInt(product.baseCount * efficiency * product.efficiency);
                    yield return result;
                }
            }
        }
    }*/
}
