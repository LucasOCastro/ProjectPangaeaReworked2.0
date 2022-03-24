using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectPangaea.Production
{
    [System.Serializable]
    public partial class PangaeaRecipeSettings
    {
        private PangaeaRecipeSettings reversed;
        public PangaeaRecipeSettings Reversed
        {
            get
            {
                if (!canBeReversed)
                {
                    return null;
                }

                if (reversed == null)
                {
                    reversed = new PangaeaRecipeSettings
                    {
                        ingredients = this.results,
                        results = this.ingredients,
                        canBeReversed = true,
                        reversed = this,
                        label = this.label
                    };
                }
                return reversed;
            }
        }

        public string label = "";
        public string LabelCap => label.CapitalizeFirst();

        public List<PortionData> ingredients = new List<PortionData>();
        public List<PortionData> results = new List<PortionData>();

        public List<StackCountProcessor> stackCountProcessors = new List<StackCountProcessor>();

        public bool canBeReversed = true;

        public string Summary
        {
            get
            {
                //TODO placeholder
                return ingredients.ToStringSafeEnumerable() + " to " + results.ToStringSafeEnumerable();
            }
        }

        public PortionData definingPortion;
        public PortionData DefiningPortion
        {
            get
            {
                if (definingPortion == null)
                {
                    definingPortion = results.FirstOrFallback();
                }
                return definingPortion;
            }
        }

        public IEnumerable<IngredientCount> MakeIngredients()
        {
            for (int i = 0; i < ingredients.Count; i++)
            {
                IngredientCount ing = ingredients[i].MakeIngredient();
                if (ing != null)
                    yield return ing;
            }
        }

        public IEnumerable<Thing> MakeResults(List<Thing> ingredients)
        {
            for (int i = 0; i < results.Count; i++)
            {
                Thing result = results[i].MakeThing();
                if (result != null)
                {
                    result.stackCount = GetStackCount(result.stackCount, ingredients);
                    yield return result;
                }
            }
        }

        private int GetStackCount(int baseCount, List<Thing> ingredients)
        {
            foreach (var processor in stackCountProcessors)
            {
                baseCount = processor.Process(baseCount, ingredients);
            }
            return baseCount;
        }

        public PangaeaThingFilter GenerateThingFilter()
        {
            PangaeaThingFilter filter = new PangaeaThingFilter();
            filter.AllowParcelDataList(ingredients);
            return filter;
        }
    }
}
