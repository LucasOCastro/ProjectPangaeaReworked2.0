using Verse;
using RimWorld;

namespace ProjectPangaea
{
    [RimWorld.DefOf]
    public class PangaeaThingDefOf
    {
        public static ThingDef Pangaea_FossilBase;
        public static ThingDef Pangaea_DNABase;

        static PangaeaThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDef));
        }
    }
}
