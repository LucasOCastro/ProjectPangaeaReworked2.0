using Verse;
using ProjectPangaea.Production;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public static class Scribe_PangaeaCollection
    {
        public static void Look(ref List<PangaeaResource> resourceList, string label)
        {
            
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                if (Scribe.EnterNode(label))
                {
                    try
                    {
                        if (resourceList == null) resourceList = new List<PangaeaResource>();
                        for (int i = 0; i < resourceList.Count; i++)
                        {
                            PangaeaResource resource = resourceList[i];
                            Scribe_Pangaea.Look(ref resource, "li");
                        }
                    }
                    finally
                    {
                        Scribe.ExitNode();
                    }
                }
                
            }
            else if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (Scribe.EnterNode(label))
                {
                    try
                    {
                        if (resourceList == null) resourceList = new List<PangaeaResource>();

                        System.Xml.XmlNodeList liNodes = Scribe.loader.curXmlParent.SelectNodes("li");
                        for (int i = 0; i < liNodes.Count; i++) 
                        {
                            System.Xml.XmlNode li = liNodes.Item(i);
                            PangaeaResource resource = Scribe_Pangaea.ResourceFromNode(li);
                            if (resource != null)
                            {
                                resourceList.Add(resource);
                            }
                        }
                    }
                    finally
                    {
                        Scribe.ExitNode();
                    }
                }
            }
        }

    }

    public static class Scribe_Pangaea
    {
        public static void Look(ref PangaeaThingEntry entry, string label)
        {
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                string value = entry?.ThingDef?.defName ?? "null";
                Scribe_Values.Look(ref value, label, "null");
            }
            else if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                ThingDef def = ScribeExtractor.DefFromNode<ThingDef>(Scribe.loader.curXmlParent[label]);
                entry = PangaeaDatabase.GetOrNull(def);
            }
        }

        private const string resourceDefSuffix = "Def";
        private const string resourceEntrySuffix = "Entry";
        public static PangaeaResource ResourceFromNode(System.Xml.XmlNode node)
        {
            var resourceDefNode = node[resourceDefSuffix];
            ResourceTypeDef resourceDef = ScribeExtractor.DefFromNode<ResourceTypeDef>(resourceDefNode);
            if (resourceDef == null) return null;

            var entryDefNode = node[resourceEntrySuffix];
            ThingDef entryDef = ScribeExtractor.DefFromNode<ThingDef>(entryDefNode);
            if (entryDef == null) return null;

            return PangaeaDatabase.GetOrNull(entryDef)?.GetResource(resourceDef);
        }
        public static void Look(ref PangaeaResource resource, string label)
        {
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                if (Scribe.EnterNode(label))
                {
                    try
                    {
                        string defName = resource?.ResourceDef?.defName ?? "null";
                        Scribe.saver.WriteElement(resourceDefSuffix, defName);

                        string entryName = resource?.Entry?.ThingDef?.defName ?? "null";
                        Scribe.saver.WriteElement(resourceEntrySuffix, entryName);
                    }
                    finally
                    {
                        Scribe.ExitNode();
                    }
                }
            }
            else if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (Scribe.EnterNode(label))
                {
                    try
                    {
                        /*var resourceDefNode = Scribe.loader.curXmlParent[resourceDefSuffix];
                        ResourceTypeDef resourceDef = ScribeExtractor.DefFromNode<ResourceTypeDef>(resourceDefNode);

                        var entryDefNode = Scribe.loader.curXmlParent[resourceEntrySuffix];
                        ThingDef entryDef = ScribeExtractor.DefFromNode<ThingDef>(entryDefNode);

                        resource = PangaeaDatabase.GetOrNull(entryDef)?.GetResource(resourceDef);*/
                        resource = ResourceFromNode(Scribe.loader.curXmlParent);
                    }
                    finally
                    {
                        Scribe.ExitNode();
                    }
                }
            }
        }

        public static void Look(ref PangaeaRecipeSettings recipe, RecipeExtension extension, string label)
        {
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                int recipeIndex = (extension != null && recipe != null) ? extension.recipes.IndexOf(recipe) : -1;
                Scribe_Values.Look(ref recipeIndex, label, defaultValue: -1);
            }
            else if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                int recipeIndex = -1;
                Scribe_Values.Look(ref recipeIndex, label, defaultValue: -1);
                if (recipeIndex >= 0)
                {
                    recipe = extension?.recipes[recipeIndex];
                }
            }
        }
    }
}
