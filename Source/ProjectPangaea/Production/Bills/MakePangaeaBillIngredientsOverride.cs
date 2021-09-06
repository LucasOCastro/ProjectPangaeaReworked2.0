using RimWorld;
using Verse;
using System.Collections.Generic;
using HarmonyLib;

namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), "MakeIngredientsListInProcessingOrder")]
    public class MakePangaeaBillIngredientsOverride
    {
        public static void Postfix(List<IngredientCount> ingredientsOrdered, Bill bill)
        {
            if (bill is PangaeaResourceBill pangBill)
            {
                foreach (var ing in ingredientsOrdered)
                {
                    PangaeaThingFilter filter = new PangaeaThingFilter();
                    filter.CopyAllowancesFrom(ing.filter);
                    filter.SyncAllowedEntries(pangBill.ResourceFilter);
                    ing.filter = filter;
                }
            }
        }
    }
}
