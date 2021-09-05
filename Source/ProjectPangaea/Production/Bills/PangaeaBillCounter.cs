using Verse;
using System.Linq;
using UnityEngine;

namespace ProjectPangaea.Production
{
    public class PangaeaBillCounter
    {
        public RecipeDef Recipe { get; }
        public int ResourceRequirement { get; }

        private ThingFilter resourceFilter = new ThingFilter();
        private ThingCategoryDef resourceCategoryDef;

        public PangaeaBillCounter(RecipeDef recipe, ThingCategoryDef resourceCategoryDef, SpecialThingFilterDef specialFilterToAllow = null, SpecialThingFilterDef specialFilterToDisallow = null)
        {
            Recipe = recipe;
            this.resourceCategoryDef = resourceCategoryDef;
            resourceFilter.SetAllow(resourceCategoryDef, true);
            if (specialFilterToAllow != null) resourceFilter.SetAllow(specialFilterToAllow, true);
            if (specialFilterToDisallow != null) resourceFilter.SetAllow(specialFilterToDisallow, false);

            ResourceRequirement = GetRequiredResourceCount();
        }

        private int GetRequiredResourceCount()
        {
            var ingredient = Recipe.ingredients.Find(i => i.filter.AllowedThingDefs.Any(j => resourceFilter.Allows(j)));
            //var ingredient = Recipe.ingredients.Find(i => i.filter.AllowedThingDefs.Intersect(resourceFilter.AllowedThingDefs).Count() == resourceFilter.AllowedThingDefs.Count());
            return Mathf.CeilToInt(ingredient.GetBaseCount());
        }

        public int Count(Map map, PangaeaThingEntry entry)
        {
            PangaeaResourceCounter resourceCounter = map.GetComponent<PangaeaResourceCounter>();
            return resourceCounter.GetCategoryCount(resourceCategoryDef, entry);
        }
    }
}
