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
            return new PangaeaBillCounter(recipe, PangaeaThingCategoryDefOf.PangaeaDNA, specialFilterToDisallow: PangaeaSpecialThingFilterDefOf.Pangaea_NonSplicedDNA);
        }

        public override List<PangaeaThingEntry> GetListerEntries(RecipeDef recipe)
        {
            return PangaeaDatabase.AllEntries.Where(e => DNASplicingWorker.IsSpliced(e.DNA)).ToList();
        }
    }
}
