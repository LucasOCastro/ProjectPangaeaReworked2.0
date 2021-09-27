using Verse;
using RimWorld;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    public class PangaeaDirectBill : Bill_Production
    {
        private PangaeaDirectRecipeDef directRecipeDef;
        public PangaeaDirectRecipeDef DirectRecipeDef => directRecipeDef;

        public DirectRecipeSettings RecipeSettings => SpliceExtension.divideDNA ? DirectRecipeDef.settings.Reverse : DirectRecipeDef.settings;

        private DirectRecipeExtension spliceExtension;
        public DirectRecipeExtension SpliceExtension
        {
            get
            {
                if (spliceExtension == null)
                {
                    spliceExtension = recipe.GetModExtension<DirectRecipeExtension>();
                    if (spliceExtension == null)
                    {
                        Log.Error(nameof(PangaeaDirectBill) + " was made with recipe of def " + recipe.defName + " which has no " + nameof(DirectRecipeExtension));
                    }
                }
                return spliceExtension;
            }
        }
        public PangaeaDirectBill(RecipeDef recipe, PangaeaDirectRecipeDef directRecipeDef) : base(recipe)
        {
            this.directRecipeDef = directRecipeDef;
        }

        public IEnumerable<Thing> MakeResults() => RecipeSettings.MakeResults();

        public IEnumerable<IngredientCount> MakeIngredients() => RecipeSettings.MakeIngredients();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref directRecipeDef, "SplicingBillSpliceDef");
        }
    }
}
