using Verse;
using RimWorld;

namespace ProjectPangaea
{
    [DefOf]
    public class PangaeaThingDefOf
    {
        public static ThingDef Pangaea_FossilBase;
        public static ThingDef Pangaea_DNABase;
        public static ThingDef Pangaea_CloningVat;
        public static ThingDef Pangaea_EmbryoBase;

        static PangaeaThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PangaeaThingDefOf));
        }
    }
}
