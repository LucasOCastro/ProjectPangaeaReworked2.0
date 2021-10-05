using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    [System.Serializable]
    public partial class PangaeaRecipeSettings
    {
        private PangaeaRecipeSettings reverse;
        public PangaeaRecipeSettings Reverse
        {
            get
            {
                if (!canBeReversed)
                {
                    return null;
                }

                if (reverse == null)
                {
                    reverse = new PangaeaRecipeSettings
                    {
                        ingredients = this.results,
                        results = this.ingredients,
                        canBeReversed = true,
                        reverse = this
                    };
                }
                return reverse;
            }
        }

        public List<PortionData> ingredients = new List<PortionData>();
        public List<PortionData> results = new List<PortionData>();

        public bool canBeReversed = true;

        public IEnumerable<IngredientCount> MakeIngredients()
        {
            for (int i = 0; i < ingredients.Count; i++)
            {
                IngredientCount ing = ingredients[i].MakeIngredient();
                if (ing != null)
                    yield return ing;
            }
        }

        public IEnumerable<Thing> MakeResults()
        {
            for (int i = 0; i < results.Count; i++)
            {
                Thing result = results[i].MakeThing();
                if (result != null)
                    yield return result;
            }
        }

        public PangaeaThingFilter GenerateThingFilter()
        {
            PangaeaThingFilter filter = new PangaeaThingFilter();
            filter.AllowParcelDataList(ingredients);
            return filter;
        }
    }
}
