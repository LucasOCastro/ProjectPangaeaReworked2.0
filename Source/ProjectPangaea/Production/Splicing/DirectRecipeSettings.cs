using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    public class DirectRecipeSettings
    {
        [System.Serializable]
        public class PortionData
        {
            public PangaeaResourceReference resource;
            public ThingDef thing;
            public int count;

            public Thing MakeThing()
            {
                Thing result = null;
                if (thing != null)
                {
                    result = ThingMaker.MakeThing(thing);
                }
                else if (resource?.Value != null)
                {
                    result = resource.Value.MakeThing();
                }

                if (result != null)
                {
                    result.stackCount = count;
                }
                return result;
            }

            public IngredientCount MakeIngredient()
            {
                ThingFilter filter = null;
                if (thing != null)
                {
                    filter = new ThingFilter();
                    filter.SetAllow(thing, true);
                }
                else if (resource?.Value != null)
                {
                    PangaeaThingFilter pangFilter = new PangaeaThingFilter(resource.Value.Entry);
                    pangFilter.SetAllow(PangaeaThingDefOf.Pangaea_DNABase, true);
                    filter = pangFilter;
                }

                if (filter == null)
                {
                    return null;
                }

                IngredientCount ing = new IngredientCount() { filter = filter };
                ing.SetBaseCount(count);
                return ing;
            }
        }

        private DirectRecipeSettings reverse;
        public DirectRecipeSettings Reverse
        {
            get
            {
                if (!canBeReversed)
                {
                    return null;
                }

                if (reverse == null)
                {
                    reverse = new DirectRecipeSettings();
                    reverse.ingredients = this.results;
                    reverse.results = this.ingredients;
                }
                return reverse;
            }
        }

        public bool canBeReversed = true;
        public List<PortionData> ingredients = new List<PortionData>();
        public List<PortionData> results = new List<PortionData>();

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

    }
}
