using RimWorld;
using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class RecipeWorkerCounter_ExtractDNA : RecipeWorkerCounter
    {
        public override bool CanCountProducts(Bill_Production bill)
        {
            return true;
        }

		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = PangaeaThingCategoryDefOf.PangaeaDNA.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		public override string ProductsDescription(Bill_Production bill)
		{
			return PangaeaThingCategoryDefOf.PangaeaDNA.label;
		}
	}
}
