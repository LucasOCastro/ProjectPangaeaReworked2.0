using Verse;
using RimWorld;

namespace ProjectPangaea
{
    [RimWorld.DefOf]
    public static class PangaeaThingCategoryDefOf
    {
        public static ThingCategoryDef PangaeaRoot;
        public static ThingCategoryDef PangaeaFossils;
        public static ThingCategoryDef PangaeaDNA;

        static PangaeaThingCategoryDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingCategoryDef));
        }
    }
}