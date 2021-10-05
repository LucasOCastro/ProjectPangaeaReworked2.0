using System.Collections.Generic;
using Verse;

namespace ProjectPangaea.Production
{
    public class RecipeExtension : DefModExtension
    {
        //TODO thinkin about moving these outside the recipe tbh
        public List<PangaeaRecipeSettings> recipes = new List<PangaeaRecipeSettings>();
        public List<ProceduralPangaeaRecipeGenData> proceduralRecipeDefs = new List<ProceduralPangaeaRecipeGenData>();


        public PangaeaThingFilter GenerateGeneralThingFilter()
        {
            PangaeaThingFilter filter = new PangaeaThingFilter();
            for (int i = 0; i < recipes.Count; i++)
            {
                filter.AllowParcelDataList(recipes[i].ingredients);
            }
            return filter;
        }
    }
}
