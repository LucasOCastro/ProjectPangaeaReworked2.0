using Verse;
using RimWorld;

namespace ProjectPangaea
{
    [DefOf]
    public static class PangaeaRecipeDefOf
    {
        public static RecipeDef Pangaea_ExtractDNA;
        public static RecipeDef Pangaea_DissectCorpse;
        public static RecipeDef Pangaea_SpliceDNA;

        static PangaeaRecipeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PangaeaRecipeDefOf));
        }
    }
}
