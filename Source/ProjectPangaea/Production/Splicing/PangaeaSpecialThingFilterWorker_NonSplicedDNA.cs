using Verse;

namespace ProjectPangaea.Production.Splicing
{
    public class PangaeaSpecialThingFilterWorker_NonSplicedDNA : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            return t is PangaeaThing pt && pt.Resource is DNA dna && !DNASplicingWorker.IsSpliced(dna);
        }

        public override bool CanEverMatch(ThingDef def)
        {
            return base.CanEverMatch(def) && def == PangaeaThingDefOf.Pangaea_DNABase;
        }
    }
}
