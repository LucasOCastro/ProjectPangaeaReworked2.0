using System.Collections.Generic;
using Verse;

namespace ProjectPangaea.Production
{
    [System.Serializable]
    public class ProceduralPangaeaRecipeGenData
    {
        private PangaeaRecipeSettings baseRecipe = null;
        private ProceduralResourceDataLister ingredients = new ProceduralResourceDataLister();
        private ProceduralResourceDataLister results = new ProceduralResourceDataLister();

        public PangaeaRecipeSettings GenRecipe(PangaeaThingEntry entry)
        {
            var recipe = new PangaeaRecipeSettings();
            if (baseRecipe != null)
            {
                recipe.ingredients = new List<PangaeaRecipeSettings.PortionData>(baseRecipe.ingredients);
                recipe.results = new List<PangaeaRecipeSettings.PortionData>(baseRecipe.results);
                recipe.canBeReversed = baseRecipe.canBeReversed;
            }

            bool genFromList(List<ProceduralResourceData> genFrom, List<PangaeaRecipeSettings.PortionData> target)
            {
                foreach (var item in genFrom)
                {
                    var result = item.GenFor(entry);
                    if (result == null)
                        return false;
                    target.Add(result);
                }
                return true;
            }

            if (!genFromList(ingredients, recipe.ingredients) || !genFromList(results, recipe.results))
            {
                return null;
            }
            return recipe;
        }

        [System.Serializable]
        public class ProceduralResourceData
        {
            public ResourceTypeDef resourceType;
            public string taggedThingDef;
            public int count;

            public PangaeaRecipeSettings.PortionData GenFor(PangaeaThingEntry entry)
            {
                if (resourceType != null)
                {
                    return new PangaeaRecipeSettings.PortionData(entry.GetResourceOfDef(resourceType), count);
                }
                if (!taggedThingDef.NullOrEmpty())
                {
                    string name = taggedThingDef + "_" + entry.ThingDef.defName;
                    return new PangaeaRecipeSettings.PortionData(DefDatabase<ThingDef>.GetNamed(name), count);
                }
                return null;
            }
        }

        [System.Serializable]
        public class ProceduralResourceDataLister : List<ProceduralResourceData>
        {
            public void LoadDataFromXmlCustom(System.Xml.XmlNode xmlRoot)
            {
                for (int i = 0; i < xmlRoot.ChildNodes.Count; i++)
                {
                    var child = xmlRoot.ChildNodes[i];

                    string resourceTypeStr = child.Name;
                    ResourceTypeDef resourceType = DefDatabase<ResourceTypeDef>.GetNamed(resourceTypeStr);

                    string countStr = child.Value;
                    int count = ParseHelper.FromString<int>(countStr);

                    if ((resourceType != null || !resourceTypeStr.NullOrEmpty()) && count > 0)
                    {
                        var result = new ProceduralResourceData()
                        {
                            resourceType = resourceType,
                            taggedThingDef = resourceTypeStr,
                            count = count,
                        };
                        Add(result);
                    }
                }
            }
        }
    }    
}