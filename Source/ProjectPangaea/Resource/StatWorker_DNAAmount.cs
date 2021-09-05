using RimWorld;

namespace ProjectPangaea
{
    public class StatWorker_DNAAmount : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            return req.Thing?.def?.HasDNA() ?? false;
        }
    }
}
