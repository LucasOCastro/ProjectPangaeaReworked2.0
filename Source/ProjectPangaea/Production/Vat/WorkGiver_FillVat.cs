using RimWorld;
using Verse;
using Verse.AI;
using System;

namespace ProjectPangaea.Production
{
    public class WorkGiver_FillVat : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(PangaeaThingDefOf.Pangaea_CloningVat);

        public override PathEndMode PathEndMode => PathEndMode.InteractionCell;

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!(t is Building_EmbryoVat vat) || vat.requestedEntry == null)
            {
                return false;
            }
            if (t.IsForbidden(pawn) || !pawn.CanReserve(t, ignoreOtherReservations: forced))
            {
                return false;
            }
            if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
            {
                return false;
            }
            if (FindEmbryo(pawn, vat) == null)
            {
                return false;
            }
            if (t.IsBurning())
            {
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_EmbryoVat vat = t as Building_EmbryoVat;
            Thing embryo = FindEmbryo(pawn, vat);
            Job job = JobMaker.MakeJob(PangaeaJobDefOf.Pangaea_FillVat, vat, embryo);
            job.count = 1;
            return job;
        }

        private Thing FindEmbryo(Pawn pawn, Building_EmbryoVat vat)
        {
            bool validator(Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t) && t is PangaeaThing pt && pt.Resource.Entry == vat.requestedEntry;
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(PangaeaThingDefOf.Pangaea_EmbryoBase), PathEndMode.ClosestTouch, TraverseParms.For(pawn), validator: validator);
        }
    }
}
