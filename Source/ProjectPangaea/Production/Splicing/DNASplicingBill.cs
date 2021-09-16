using Verse;
using RimWorld;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    public class DNASplicingBill : Bill_Production
    {;
        private DNASplicingDef spliceDef;
        public DNASplicingDef SpliceDef => spliceDef;

        private Pangaea_DNASplicingRecipeExtension spliceExtension;
        public Pangaea_DNASplicingRecipeExtension SpliceExtension
        {
            get
            {
                if (spliceExtension == null)
                {
                    spliceExtension = recipe.GetModExtension<Pangaea_DNASplicingRecipeExtension>();
                    if (spliceExtension == null)
                    {
                        Log.Error(nameof(DNASplicingBill) + " was made with recipe of def " + recipe.defName + " which has no " + nameof(Pangaea_DNASplicingRecipeExtension));
                    }
                }
                return spliceExtension;
            }
        }
        public DNASplicingBill(RecipeDef recipe, DNASplicingDef spliceDef) : base(recipe)
        {
            this.spliceDef = spliceDef;
        }

        public IEnumerable<Thing> MakeResults()
        {
            if (SpliceExtension.divideDNA)
            {
                return SpliceDef.MakePortionThings();
            }
            return SpliceDef.MakeResultThing().Yield();
        }

        public IEnumerable<IngredientCount> MakeIngredients()
        {
            if (SpliceExtension.divideDNA)
            {
                return SpliceDef.MakeResultIngredient().Yield();
            }
            return SpliceDef.MakePortionIngredients();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref spliceDef, "SplicingBillSpliceDef");
        }
    }
}
