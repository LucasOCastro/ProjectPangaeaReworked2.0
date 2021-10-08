using RimWorld;
using Verse;

namespace ProjectPangaea
{
    [DefOf]
    public static class PangaeaJobDefOf
    {
        public static JobDef Pangaea_ReleasePawnFromVat;
        public static JobDef Pangaea_FillVat;

        static PangaeaJobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PangaeaJobDefOf));
        }
    }
}
