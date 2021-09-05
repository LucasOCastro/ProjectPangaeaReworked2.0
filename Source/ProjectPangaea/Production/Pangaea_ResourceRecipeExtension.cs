using Verse;
using RimWorld;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public abstract class Pangaea_ResourceRecipeExtension : DefModExtension
    {
        public bool allowMixingResources = false;

        public virtual float ResolveEfficiencyFromIngredient(Thing ingredient) => 1;

        public abstract List<PangaeaThingEntry> GetListerEntries(RecipeDef recipe);
        public abstract PangaeaBillCounter GetBillCounter(RecipeDef recipe);
    }
}
