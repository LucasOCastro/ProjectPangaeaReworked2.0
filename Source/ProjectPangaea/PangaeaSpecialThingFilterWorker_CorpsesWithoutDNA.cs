using Verse;

namespace ProjectPangaea
{
    public class PangaeaSpecialThingFilterWorker_CorpsesWithoutDNA : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            return t is Corpse corpse && PangaeaDatabase.GetOrNull(corpse.InnerPawn.def)?.GetResourceOfDef(ResourceTypeDefOf.Pangaea_DNA) != null;
        }
    }
}
