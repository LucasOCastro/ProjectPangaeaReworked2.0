using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea
{
    [DefOf]
    public class ResourceTypeDefOf
    {
        public static ResourceTypeDef Pangaea_Fossil;
        public static ResourceTypeDef Pangaea_DNA;
        public static ResourceTypeDef Pangaea_Embryo;

        //TODO change this as to get rid of DefOf
        private static List<ResourceTypeDef> generalResources;
        public static List<ResourceTypeDef> GeneralResources
        {
            get
            {
                if (generalResources == null)
                {
                    generalResources = DefDatabase<ResourceTypeDef>.AllDefs.Where(d => d.addToAny).ToList();
                }
                return generalResources;
            }
        }

        static ResourceTypeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ResourceTypeDefOf));
        }
    }
}
