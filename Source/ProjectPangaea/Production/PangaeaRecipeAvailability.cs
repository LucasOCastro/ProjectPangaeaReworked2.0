using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea.Production
{
    public static class PangaeaRecipeAvailability
    {
        public static bool IsAvailableIn(this PangaeaRecipeSettings recipe, Map map)
        {
            return recipe.ingredients.All(i => i.CountInMap(map) >= i.count);
        }

        public static bool IsAlreadyAddedTo(this PangaeaRecipeSettings recipe, BillStack billStack)
        {
            return billStack.Bills.Any(b => b is PangaeaBill pb && pb.RecipeSettings == recipe);
        }

        //TODO got rid of this cacheing
        /*private static Dictionary<PangaeaRecipeSettings, int> lastAvailableTicks = new Dictionary<PangaeaRecipeSettings, int>();
        private static Dictionary<PangaeaRecipeSettings, bool> availableCache = new Dictionary<PangaeaRecipeSettings, bool>();
        public static bool IsAvailableIn(this PangaeaRecipeSettings recipe, Map map)
        {
            int curTick = GenTicks.TicksGame;
            if (lastAvailableTicks.TryGetValue(recipe, out int lastTick) && curTick == lastTick)
            {
                return availableCache[recipe];
            }

            bool available = recipe.ingredients.All(i => i.CountInMap(map) >= i.count);
            availableCache.SetOrAdd(recipe, available);
            lastAvailableTicks.SetOrAdd(recipe, curTick);
            return available;
        }

        private static Dictionary<PangaeaRecipeSettings, int> lastAddedTicks = new Dictionary<PangaeaRecipeSettings, int>();
        private static Dictionary<PangaeaRecipeSettings, bool> addedCache = new Dictionary<PangaeaRecipeSettings, bool>();
        public static bool IsAlreadyAddedTo(this PangaeaRecipeSettings recipe, BillStack billStack)
        {
            int curTick = GenTicks.TicksAbs;
            if (lastAddedTicks.TryGetValue(recipe, out int lastTick) && curTick == lastTick)
            {
                return addedCache[recipe];
            }

            bool added = billStack.Bills.Any(b => b is PangaeaBill pb && pb.RecipeSettings == recipe);
            addedCache.SetOrAdd(recipe, added);
            lastAddedTicks.SetOrAdd(recipe, curTick);
            return added;
        }*/
    }
}
