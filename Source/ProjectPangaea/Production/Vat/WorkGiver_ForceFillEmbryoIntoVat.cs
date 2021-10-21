using RimWorld;
using Verse;
using Verse.AI;

namespace ProjectPangaea.Production
{
    public class WorkGiver_ForceFillEmbryoIntoVat : WorkGiver_Haul
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(PangaeaThingDefOf.Pangaea_EmbryoBase);

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            return base.ShouldSkip(pawn, forced) || !forced;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!forced)
            {
                return false;
            }
            if (!(t is PangaeaThing pt) || !pt.IsOfType(ResourceTypeDefOf.Pangaea_Embryo))
            {
                return false;
            }
            if (!base.HasJobOnThing(pawn, t, forced))
            {
                return false;
            }
            if (FindVat(pawn) == null)
            {
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            PangaeaThing embryo = t as PangaeaThing;
            Thing vat = FindVat(pawn);
            Job job = JobMaker.MakeJob(PangaeaJobDefOf.Pangaea_FillVat, vat, embryo);
            job.count = 1;
            return job;
        }

        private Thing FindVat(Pawn pawn)
        {
            bool validator(Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t) && t is Building_EmbryoVat vat && vat.Empty;
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(PangaeaThingDefOf.Pangaea_CloningVat), PathEndMode.InteractionCell, TraverseParms.For(pawn), validator: validator);
        }
    }
}
