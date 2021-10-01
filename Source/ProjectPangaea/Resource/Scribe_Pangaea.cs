using Verse;
using ProjectPangaea.Production;

namespace ProjectPangaea
{
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
        private const string resouceEntrySuffix = "Entry";
        public static void Look(ref PangaeaResource resource, string label)
        {
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                ResourceTypeDef def = resource?.ResourceDef;
                Scribe_Defs.Look(ref def, label + resourceDefSuffix);

                PangaeaThingEntry entry = resource?.Entry;
                Scribe_Pangaea.Look(ref entry, label + resouceEntrySuffix);
            }
            else if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                ResourceTypeDef def = null;
                Scribe_Defs.Look(ref def, label + resourceDefSuffix);

                PangaeaThingEntry entry = null;
                Scribe_Pangaea.Look(ref entry, label + resouceEntrySuffix);

                resource = entry?.GetResourceOfDef(def);
            }
        }

        public static void Look(ref PangaeaRecipeSettings recipe, RecipeExtension extension, string label)
        {
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                int recipeIndex = extension?.recipes.IndexOf(recipe) ?? -1;
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
