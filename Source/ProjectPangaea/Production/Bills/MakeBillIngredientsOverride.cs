using RimWorld;
using Verse;
using System.Collections.Generic;
using HarmonyLib;
using ProjectPangaea.Production.Splicing;

namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), "MakeIngredientsListInProcessingOrder")]
    public class MakeBillIngredientsOverride
    {
        public static void Postfix(List<IngredientCount> ingredientsOrdered, Bill bill)
        {
            if (bill is PangaeaResourceBill pangBill)
            {
                for (int i = 0; i < ingredientsOrdered.Count; i++)
                {
                    PangaeaThingFilter filter = new PangaeaThingFilter();
                    filter.CopyAllowancesFrom(ingredientsOrdered[i].filter);
                    filter.SyncAllowedEntries(pangBill.ResourceFilter);
                    ingredientsOrdered[i].filter = filter;
                    /*IngredientCount newIngredient = new IngredientCount();
                    PangaeaThingFilter filter = new PangaeaThingFilter();
                    filter.CopyAllowancesFrom(ingredientsOrdered[i].filter);
                    filter.SyncAllowedEntries(pangBill.ResourceFilter);
                    newIngredient.filter = filter;

                    ingredientsOrdered[i] = newIngredient;*/
                }
                return;
            }

            if (bill is DNASplicingBill splicingBill)
            {
                ingredientsOrdered.Clear();
                foreach (var ing in splicingBill.MakeIngredients())
                {
                    ingredientsOrdered.Add(ing);
                }
                Log.Message((ingredientsOrdered[0].filter as PangaeaThingFilter).AllowedEntries.ToStringSafeEnumerable());
                return;
            }
        }
    }
}
