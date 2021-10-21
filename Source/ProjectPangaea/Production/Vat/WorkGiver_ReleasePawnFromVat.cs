using Verse;
using RimWorld;
using Verse.AI;

namespace ProjectPangaea.Production
{

    public class WorkGiver_ReleasePawnFromVat : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(PangaeaThingDefOf.Pangaea_CloningVat);

        public override PathEndMode PathEndMode => PathEndMode.InteractionCell;

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!(t is Building_EmbryoVat vat)) 
            {
                return false;
            }
            if (!forced && !vat.AllowPawnRelease)
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
            if (t.IsBurning())
            {
                return false;
            }
            return vat.Completed;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_EmbryoVat vat = t as Building_EmbryoVat;
            return JobMaker.MakeJob(PangaeaJobDefOf.Pangaea_ReleasePawnFromVat, vat, vat.GeneratedPawn);
        }
    }
}
