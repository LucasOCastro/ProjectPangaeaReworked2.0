using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ProjectPangaea.Production.Splicing
{
    public class Pangaea_DNASplicingRecipeExtension : Pangaea_ResourceRecipeExtension
    {
        public bool divideDNA;

        public override PangaeaBillCounter GetBillCounter(RecipeDef recipe)
        {
            //TODO consider using the recipeDefs specialFilters instead of passing directly, in corpses bill too
            return new PangaeaBillCounter(recipe, PangaeaThingCategoryDefOf.PangaeaDNA, specialFilterToDisallow: PangaeaSpecialThingFilterDefOf.Pangaea_NonSplicedDNA);
        }

        public override List<PangaeaThingEntry> GetListerEntries(RecipeDef recipe)
        {
            if (divideDNA)
            {
                return PangaeaDatabase.AllEntries.Where(e => DNASplicingWorker.IsSpliced(e.DNA)).ToList();
            }
            return PangaeaThingCategoryDefOf.PangaeaDNA.childThingDefs;
        }
    }
}
