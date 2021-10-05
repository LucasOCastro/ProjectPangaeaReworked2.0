using RimWorld;
using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class RecipeWorkerCounter_PangaeaBill : RecipeWorkerCounter
    {
        public override bool CanCountProducts(Bill_Production bill)
        {
			return bill is PangaeaBill;
        }

		public override int CountProducts(Bill_Production bill)
		{
			PangaeaBill pangaeaBill = bill as PangaeaBill;

			int num = 0;
			var counter = bill.Map.GetComponent<PangaeaResourceCounter>();
			foreach (var parcel in pangaeaBill.RecipeSettings.results)
            {
				if (parcel.resource != null)
                {
					num += counter.GetCount(parcel.resource?.Value);
				}
				else if (parcel.thing != null)
                {
					num += bill.Map.resourceCounter.GetCount(parcel.thing);
                }
			}
			return num;
		}

		public override string ProductsDescription(Bill_Production bill)
		{
			throw new System.NotImplementedException();
			//return PangaeaThingCategoryDefOf.PangaeaDNA.label;
		}
    }
}
