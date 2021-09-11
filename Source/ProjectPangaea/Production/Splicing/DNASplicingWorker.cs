using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea.Production.Splicing
{
    //TODO organize this holy shit
    public static class DNASplicingWorker
    {
        private static Dictionary<DNA, DNASplicingDef> dict = new Dictionary<DNA, DNASplicingDef>();

        public static IEnumerable<DNASplicingDef> AllSpliceDefs => dict.Values;

        private static bool TryGetDef(DNA dna, out DNASplicingDef splicingDef)
        {
            if (dna == null)
            {
                splicingDef = null;
                return false;
            }
            return dict.TryGetValue(dna, out splicingDef);
        }

        public static bool IsSpliced(DNA dna) => dict.ContainsKey(dna);

        public static bool TryGetDefFromPortions(IEnumerable<Thing> portionThings, out DNASplicingDef splicingDef)
        {
            splicingDef = dict.Values.FirstOrDefault(d => portionThings.All(t => d.splicePortions.Any(p => p.MatchesThing(t))));
            return splicingDef != null;
        }

        public static bool TryGuessDefFromIngredients(IEnumerable<Thing> ingredients, out DNASplicingDef splicingDef, out bool divideDNA)
        {
            if (TryGetDefFromPortions(ingredients, out splicingDef))
            {
                divideDNA = false;
                return true;
            }

            foreach (Thing thing in ingredients)
            {
                if (thing is DNAThing dnaThing)
                {
                    divideDNA = true;
                    return TryGetDef(dnaThing.DNAResource, out splicingDef);
                }
            }
            divideDNA = false;
            return false;
        }

        /*private static List<Thing> results = new List<Thing>();

        /// <summary>
        /// Tries to find spliceDef for dna and generates its portions Things which sum up to stackCount.
        /// </summary>
        public static IEnumerable<Thing> GetDivisionResults(DNA dna)
        {
            if (!TryGetDef(dna, out DNASplicingDef splicingDef))
            {
                return Enumerable.Empty<Thing>();
            }
            return GetDivisionResults(splicingDef);
        }*/

        /*/// <summary>
        /// Generates the portions of a spliceDef as Things which sum up to stackCount.
        /// </summary>
        public static IEnumerable<Thing> GetDivisionResults(DNASplicingDef splicingDef)
        {
            results.Clear();
            int totalCount = 0;
            foreach (var splice in splicingDef.splicePortions)
            {
                Thing result = splice.MakeThing();
                if (result == null) continue;
                totalCount += result.stackCount;
                results.Add(result);
            }

            return results;
        }*/

        /*public static IEnumerable<Thing> GenRecipeResults(RecipeDef recipe, List<Thing> ingredients)
        {
            if (!SpliceUtility.TryGetEtension(recipe, out var extension))
            {
                return Enumerable.Empty<Thing>();
            }

            //If recipe is to divide, will return divided portions
            if (extension.divideDNA)
            {
                if (!(ingredients.First() is DNAThing dnaThing))
                {
                    return Enumerable.Empty<Thing>();
                }
                return GetDivisionResults(dnaThing.DNAResource, dnaThing.stackCount);
            }

            //If recipe is to splice, will return final DNA
            if (TryGetDefFromPortions(ingredients, out DNASplicingDef splicingDef, out int totalStack))
            {
                DNAThing thing = DNAThing.MakeDNAThing(splicingDef.ParentDNA);
                thing.stackCount = totalStack;
                return thing.Yield();
            }

            return Enumerable.Empty<Thing>();
        }*/

        /*private static IEnumerable<IngredientCount> GenIngredientsForDNADivision(RecipeDef recipe)
        {
            if (!TryGetDef(dnaThing.DNAResource, out DNASplicingDef spliceDef))
            {
                yield break;
            }

            IngredientCount definingIngredient = SpliceUtility.DefiningIngredient(recipe);
            PangaeaThingFilter filter = new PangaeaThingFilter(spliceDef.ParentEntry);
            filter.CopyAllowancesFrom(definingIngredient.filter);
            IngredientCount ingredient = new IngredientCount() { filter = filter };
            ingredient.SetBaseCount(definingIngredient.GetBaseCount());
            yield return ingredient;
        }*/

        /*private static IEnumerable<IngredientCount> GenIngredientsForDNASplicing(RecipeDef recipedients)
        {
            if (!TryGetDefFromPortions(ingredients, out DNASplicingDef spliceDef, out int stackCount))
            {
                yield break;
            }

            IngredientCount definingIngredient = SpliceUtility.DefiningIngredient(recipe);
            foreach (var portion in spliceDef.splicePortions)
            {
                ThingFilter filter = null;
                if (portion.thing != null)
                {
                    filter = new ThingFilter();
                    filter.SetAllow(portion.thing, true);
                }
                else if (PangaeaDatabase.TryGetEntry(portion.dnaOwner, out var entry))
                {
                    filter = new PangaeaThingFilter(entry);
                    filter.CopyAllowancesFrom(definingIngredient.filter);
                }
                IngredientCount ingredient = new IngredientCount() { filter = filter };
                ingredient.SetBaseCount(portion.portion * stackCount);
                yield return ingredient;
            }
        }*/

        /*public static IEnumerable<IngredientCount> GenIngredientCounts(RecipeDef recipe)
        {
            if (!SpliceUtility.TryGetEtension(recipe, out var extension))
            {
                return Enumerable.Empty<IngredientCount>();
            }

            IngredientCount definingIngredient = SpliceUtility.DefiningIngredient(recipe);
            //If recipe divides DNA, return only the main DNA as an ingredient.
            if (extension.divideDNA)
            {
                return GenIngredientsForDNADivision(recipe, ingredients);
            }

            //If recipe splices DNA, return all portions as ingredients. 
            //TODO maybe equalize this
            return GenIngredientsForDNASplicing(recipe, ingredients);
        }*/

        public static void Init()
        {
            foreach (DNASplicingDef def in DefDatabase<DNASplicingDef>.AllDefs)
            {
                dict.Add(def.ParentDNA, def);
            }
        }
    }
}
