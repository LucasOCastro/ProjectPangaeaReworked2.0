using RimWorld;
using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class RecipeWorkerCounter_ExtractDNA : RecipeWorkerCounter
    {
        public override bool CanCountProducts(Bill_Production bill)
        {
            return bill is PangaeaResourceBill && !bill.recipe.HasModExtension<Splicing.Pangaea_DNASplicingRecipeExtension>();
        }

		public override int CountProducts(Bill_Production bill)
		{
			PangaeaResourceBill resourceBill = (PangaeaResourceBill)bill;

			int num = 0;

			foreach (var allowedEntry in resourceBill.AllowedEntries)
            {
				num += bill.Map.GetComponent<PangaeaResourceCounter>().GetCount(allowedEntry.DNA);
			}
			//TODO possible option for count all DNA instead of only allowed
			/*List<ThingDef> childThingDefs = PangaeaThingCategoryDefOf.PangaeaDNA.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}*/
			return num;
		}

		public override string ProductsDescription(Bill_Production bill)
		{
			return PangaeaThingCategoryDefOf.PangaeaDNA.label;
		}
	}
}
