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
        public static bool Prefix(List<IngredientCount> ingredientsOrdered, Bill bill)
        {
            if (bill is PangaeaBill pangaeaBill) 
            {
                ingredientsOrdered.Clear();
                foreach (var ing in pangaeaBill.MakeIngredients())
                {
                    ingredientsOrdered.Add(ing);
                }
                return false;
            }
            return true;
        }
    }
}
