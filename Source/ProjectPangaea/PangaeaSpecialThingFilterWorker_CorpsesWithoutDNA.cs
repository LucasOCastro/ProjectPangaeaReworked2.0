using Verse;

namespace ProjectPangaea
{
    public class PangaeaSpecialThingFilterWorker_CorpsesWithoutDNA : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            return t is Corpse corpse && PangaeaDatabase.GetOrNull(corpse.InnerPawn.def)?.GetResource(ResourceTypeDefOf.Pangaea_DNA) != null;
        }
    }
}
