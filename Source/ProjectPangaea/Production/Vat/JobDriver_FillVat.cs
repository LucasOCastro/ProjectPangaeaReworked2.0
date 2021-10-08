using Verse.AI;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class JobDriver_FillVat : JobDriver
    {
        private const TargetIndex vatInd = TargetIndex.A;
        private const TargetIndex embryoInd = TargetIndex.B;

        private Building_EmbryoVat Vat => job.GetTarget(vatInd).Thing as Building_EmbryoVat;
        private PangaeaThing Embryo => job.GetTarget(embryoInd).Thing as PangaeaThing;
        private int Duration => job.def.waitAfterArriving;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Vat, job, errorOnFailed: errorOnFailed)
                && pawn.Reserve(Embryo, job, errorOnFailed: errorOnFailed);
            
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(vatInd);
            this.FailOnBurningImmobile(vatInd);
            Toil reserveEmbryo = Toils_Reserve.Reserve(embryoInd);
            yield return reserveEmbryo;
            yield return Toils_Goto.GotoThing(embryoInd, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(embryoInd).FailOnSomeonePhysicallyInteracting(embryoInd);
            yield return Toils_Haul.StartCarryThing(embryoInd, subtractNumTakenFromJobCount: true).FailOnDestroyedNullOrForbidden(embryoInd);
            //yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveEmbryo, embryoInd, TargetIndex.None, takeFromValidStorage: true);
            yield return Toils_Goto.GotoThing(vatInd, PathEndMode.InteractionCell);
            yield return Toils_General.Wait(Duration)
                .FailOnDestroyedNullOrForbidden(vatInd)
                .FailOnCannotTouch(vatInd, PathEndMode.InteractionCell)
                .WithProgressBarToilDelay(vatInd);
            yield return new Toil()
            {
                initAction = () => Vat.InsertEmbryo(Embryo),
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}
