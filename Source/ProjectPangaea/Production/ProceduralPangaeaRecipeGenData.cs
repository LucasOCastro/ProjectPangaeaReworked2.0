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
                recipe.ingredients = new List<PortionData>(baseRecipe.ingredients);
                recipe.results = new List<PortionData>(baseRecipe.results);
                recipe.canBeReversed = baseRecipe.canBeReversed;
            }

            bool genFromList(List<ProceduralResourceData> genFrom, List<PortionData> target)
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
            public ResourceTypeDef ResourceType
            {
                get
                {
                    return DefDatabase<ResourceTypeDef>.GetNamedSilentFail(taggedThingDef);
                }
            }
            //TODO ill change this for a special like EntryThingGetter class or whatever
            //For example: get egg from entry, get corpse, etc etc
            public string taggedThingDef;
            public int count;

            public PortionData GenFor(PangaeaThingEntry entry)
            {
                var resourceType = ResourceType;
                if (resourceType != null && entry.TryGetResource(resourceType, out PangaeaResource resource))
                {
                    return new PortionData(resource, count);
                }
                if (!taggedThingDef.NullOrEmpty())
                {
                    string name = taggedThingDef + "_" + entry.ThingDef.defName;
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                    if (def != null)
                    {
                        return new PortionData(def, count);
                    }
                }
                return null;
            }
        }

        [System.Serializable]
        public class ProceduralResourceDataLister : List<ProceduralResourceData>
        {
            public void LoadDataFromXmlCustom(System.Xml.XmlNode xmlRoot)
            {
                System.Xml.XmlNode node = xmlRoot.FirstChild;
                while (node != null)
                {
                    string resourceTypeStr = node.Name;
                    string countStr = node.FirstChild.Value;

                    var result = new ProceduralResourceData
                    {
                        count = ParseHelper.FromString<int>(countStr),
                        taggedThingDef = resourceTypeStr
                    };
                    Add(result);

                    node = node.NextSibling?.NextSibling;
                }

                /*for (int i = 0; i < xmlRoot.ChildNodes.Count; i++)
                {
                    var child = xmlRoot.ChildNodes[i];

                    string resourceTypeStr = child.Name;
                    string countStr = child.Value;
                    var result = new ProceduralResourceData
                    {
                        count = ParseHelper.FromString<int>(countStr),
                        taggedThingDef = resourceTypeStr
                    };
                    Add(result);
                }*/
            }
        }
    }    
}