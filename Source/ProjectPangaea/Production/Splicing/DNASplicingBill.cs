using Verse;
using RimWorld;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    public class DNASplicingBill : Bill_Production
    {
        public DNASplicingDef SpliceDef { get; }
        public Pangaea_DNASplicingRecipeExtension SpliceExtension { get; }
        public DNASplicingBill(RecipeDef recipe, DNASplicingDef spliceDef) : base(recipe)
        {
            SpliceDef = spliceDef;
            SpliceExtension = recipe.GetModExtension<Pangaea_DNASplicingRecipeExtension>();
            if (SpliceExtension == null)
            {
                Log.Error(nameof(DNASplicingBill) + " was made with recipe of def " + recipe.defName + " which has no " + nameof(Pangaea_DNASplicingRecipeExtension));
            }
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
    }
}
