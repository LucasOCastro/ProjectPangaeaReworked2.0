using System.Collections.Generic;
using Verse;

namespace ProjectPangaea.Production
{
    public class RecipeExtension : DefModExtension
    {
        //TODO thinkin about moving these outside the recipe tbh
        public List<PangaeaRecipeSettings> recipes = new List<PangaeaRecipeSettings>();
        public List<ProceduralPangaeaRecipeGenData> proceduralRecipeDefs = new List<ProceduralPangaeaRecipeGenData>();

        private int reverseableRecipeCount = -1;
        public int ReverseableRecipeCount
        {
            get
            {
                if (reverseableRecipeCount < 0)
                {
                    reverseableRecipeCount = 0;
                    for (int i = 0; i < recipes.Count; i++)
                    {
                        if (recipes[i].canBeReversed)
                        {
                            reverseableRecipeCount++;
                        }
                    }
                }
                return reverseableRecipeCount;
            }
        }

        public PangaeaThingFilter GenerateGeneralThingFilter()
        {
            PangaeaThingFilter filter = new PangaeaThingFilter();
            for (int i = 0; i < recipes.Count; i++)
            {
                filter.AllowParcelDataList(recipes[i].ingredients);
            }
            return filter;
        }

        public bool Contains(PangaeaRecipeSettings recipe)
        {
            return recipes.Contains(recipe) || (recipe.canBeReversed && recipes.Contains(recipe.Reversed));
        }

        public void GenProceduralRecipes()
        {
            foreach (var procedural in proceduralRecipeDefs)
            {
                foreach (var entry in PangaeaDatabase.AllEntries)
                {
                    var generated = procedural.GenRecipe(entry);
                    if (generated != null)
                    {
                        recipes.Add(generated);
                    }
                }
            }
        }

        public void ResolveReferences()
        {
            foreach (var recipe in recipes)
            {
                foreach (var ing in recipe.ingredients)
                {
                    ing.ThingFilter.ResolveReferences();
                }
                foreach (var res in recipe.results)
                {
                    res.ThingFilter.ResolveReferences();
                }
            }
        }
    }
}
