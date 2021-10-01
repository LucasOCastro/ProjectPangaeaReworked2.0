using RimWorld;

namespace ProjectPangaea
{
    public class StatWorker_DNAAmount : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            return PangaeaDatabase.TryGetEntry(req.Thing?.def, out _);
        }
    }
}
