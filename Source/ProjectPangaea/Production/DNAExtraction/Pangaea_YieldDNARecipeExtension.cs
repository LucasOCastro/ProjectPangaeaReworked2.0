using System.Collections.Generic;
using Verse;

namespace ProjectPangaea.Production
{
    public class Pangaea_YieldDNARecipeExtension : Pangaea_ResourceRecipeExtension
    {
        public IntRange baseYieldPerExtraction = new IntRange(1,1);

        public override PangaeaBillCounter GetBillCounter(RecipeDef recipe) 
        {
            return new PangaeaBillCounter(recipe, PangaeaThingCategoryDefOf.PangaeaFossils);
        }

        public override List<PangaeaThingEntry> GetListerEntries(RecipeDef recipe)
        {
            return new List<PangaeaThingEntry>(PangaeaDatabase.AllExtinctEntries);
        }
    }
}
