using Verse;
using RimWorld;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class PangaeaBill : Bill_Production
    {
        private PangaeaRecipeSettings recipeSettings;
        public PangaeaRecipeSettings RecipeSettings => recipeSettings;

        private RecipeExtension recipeExtension;
        public RecipeExtension RecipeExtension
        {
            get
            {
                if (recipeExtension == null)
                {
                    recipeExtension = recipe.GetModExtension<RecipeExtension>();
                }
                return recipeExtension;
            }
        }

        public PangaeaBill(RecipeDef recipe, PangaeaRecipeSettings recipeSettings) : base(recipe)
        {
            if (RecipeExtension == null)
            {
                Log.Error(nameof(PangaeaBill) + " was made with recipe of def " + recipe.defName + " which has no " + nameof(Production.RecipeExtension));
                return;
            }

            if (!RecipeExtension.recipes.Contains(recipeSettings))
            {
                Log.Error(nameof(PangaeaBill) + " was made with recipe its def does not contain!");
                return;
            }

            this.recipeSettings = recipeSettings;
        }

        public PangaeaBill() : base()
        {
        }

        public IEnumerable<Thing> MakeResults() => RecipeSettings.MakeResults();

        public IEnumerable<IngredientCount> MakeIngredients() => RecipeSettings.MakeIngredients();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Pangaea.Look(ref recipeSettings, RecipeExtension, "PangaeaBillRecipeSettings");
        }
    }
}
