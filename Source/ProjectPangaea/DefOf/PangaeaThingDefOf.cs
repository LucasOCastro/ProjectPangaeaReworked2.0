using Verse;
using RimWorld;

namespace ProjectPangaea
{
    [DefOf]
    public class PangaeaThingDefOf
    {
        public static ThingDef Pangaea_FossilBase;
        public static ThingDef Pangaea_DNABase;

        static PangaeaThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
        }
    }
}
