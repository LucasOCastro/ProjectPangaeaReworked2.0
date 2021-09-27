using RimWorld;
using Verse;

namespace ProjectPangaea
{
    [DefOf]
    public class ResourceTypeDefOf
    {
        public static ResourceTypeDef Pangaea_Fossil;
        public static ResourceTypeDef Pangaea_DNA;
        public static ResourceTypeDef Pangaea_Embryo;

        static ResourceTypeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ResourceTypeDef));
        }
    }
}
