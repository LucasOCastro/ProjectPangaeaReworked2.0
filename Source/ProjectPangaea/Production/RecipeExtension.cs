using System.Collections.Generic;
using Verse;

namespace ProjectPangaea.Production
{
    public class RecipeExtension : DefModExtension
    {
        public List<PangaeaRecipeSettings> recipes = new List<PangaeaRecipeSettings>();
        public List<ProceduralPangaeaRecipeGenData> proceduralRecipeDefs = new List<ProceduralPangaeaRecipeGenData>();
    }
}
