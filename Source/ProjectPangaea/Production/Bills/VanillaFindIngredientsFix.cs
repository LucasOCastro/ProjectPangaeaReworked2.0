using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System;

namespace ProjectPangaea.Production
{
    /*
     * Basically, this fixes some stuff from the vanilla FindIngredients that, in my opinion, should be 
     * in the base game code. I've decided to not actually transpile the method, as to avoid any possible
     * incompatibilities in the future. Therefore, these fixes will only be applied in my own Reverse Patches.
     * The issues are:
     * 
     * 1-The game creates a "sorted" IngredientCount list to use when finding the best Things to use as
     * ingredients. The problem is, whenever this sorted list is iterated, its contents are never 
     * actually accessed, and only the IngredientCounts from the RecipeDef are taken into consideration.
     * This basically makes the sorted list useless, and seems like an oversight by Ludeon.
     * Because I had overriden the 'MakeIngredientsListInProcessingOrder' method to change the ingredients
     * based on the bill, this would throw a index out of bounds exception. I've changed the code to use this
     * sorted list properly. This one actually feels like a bug.
     * 
     * 2-ThingFilter has a Allows(Thing) method, aside from a Allows(ThingDef).
     * While the ThingDef method is called during the processing of ingredients, the Thing method is not.
     * A custom ThingFilter class with an overriden Allows(Thing) method, therefore, had ZERO impact on
     * wheter an ingredient is considered proper or not. I see no reason that the method wouldn't be called
     * in this situation, which seems to be exactly why such a method would exist (and be overrideable).
     * I've changed the code to use the Allows(Thing) method when processing possible ingredients.
     */
    public static class VanillaFindIngredientsFix
    {
        private static FieldInfo orderedIngFieldInfo = AccessTools.Field(typeof(WorkGiver_DoBill), "ingredientsOrdered");
        private static FieldInfo ingCountFilterField = AccessTools.Field(typeof(IngredientCount), "filter");
        private static FieldInfo thingDefFieldInfo = AccessTools.Field(typeof(Thing), "def");
        private static MethodInfo filterAllowsMethodInfo = AccessTools.Method(typeof(ThingFilter), "Allows", new Type[] { typeof(Thing) });

        public static List<CodeInstruction> FixFindIngredientBugs(IEnumerable<CodeInstruction> instructions, out int skipThingCheckIndex, out object skipThingDestination)
        {
            List<CodeInstruction> og = instructions.ToList();

            //Makes it use the ordered list instead of normal list.
            var orderedListPatch = og.ForwardUntil(i => og[i].opcode == OpCodes.Ldloc_1,
                matchCallback: i => {
                    og[i].opcode = OpCodes.Ldsfld;
                    og[i].operand = orderedIngFieldInfo;
                    og.RemoveAt(i + 1);
                });

            //Gets the adress of the correct ingredientCount local variable
            object localIngCountVarAdress = null;
            var saveVarAdress = og.ForwardUntil(i => og[i].opcode == OpCodes.Stloc_S, orderedListPatch.index,
                matchCallback: i => localIngCountVarAdress = og[i].operand);

            
            List<CodeInstruction> insert = new List<CodeInstruction>();

            //Push the ingredientCount's filter
            insert.Add(new CodeInstruction(OpCodes.Ldloc_S, localIngCountVarAdress));
            insert.Add(new CodeInstruction(OpCodes.Ldfld, ingCountFilterField));

            //Get thing from the array
            int stopIndex = -1;
            var getThing = og.ForwardUntil(i => og[i].LoadsField(thingDefFieldInfo), saveVarAdress.index,
                matchCallback: i => stopIndex = i - 1)
                .BackUntil(i => og[i].opcode == OpCodes.Br)//og[i-1].opcode == OpCodes.Br)
                .ForwardUntil(i => i == stopIndex,
                action: i => insert.Add(new CodeInstruction(og[i].opcode, og[i].operand)));

            //Makes it take the filter's Allows(thing) into consideration when filtering.
            insert.Add(new CodeInstruction(OpCodes.Callvirt, filterAllowsMethodInfo));

            //Continue if filter doesnt allow
            object continueDestination = null;
            var getContinueDestination = og.ForwardUntil(i => og[i].opcode == OpCodes.Bne_Un, getThing.index,
            matchCallback: i => continueDestination = og[i].operand);
            insert.Add(new CodeInstruction(OpCodes.Brfalse, continueDestination));

            int insertIndex = getContinueDestination.index + 1;
            og.InsertRange(insertIndex, insert);

            skipThingCheckIndex = insertIndex + insert.Count;
            skipThingDestination = continueDestination;

            return og;
        }

        //For compatibility sake, dont transpile.
        /*
        [HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredientsInSet_NoMix")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return FixFindIngredientBugs(instructions, out _, out _);
        }
        */
    }
}