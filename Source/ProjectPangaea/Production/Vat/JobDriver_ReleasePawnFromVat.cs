using Verse.AI;
using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class JobDriver_ReleasePawnFromVat : JobDriver
    {
        private const TargetIndex vatInd = TargetIndex.A;

        private Building_EmbryoVat Vat => job.GetTarget(vatInd).Thing as Building_EmbryoVat;
        private int Duration => job.def.waitAfterArriving;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Vat, job, errorOnFailed: errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(vatInd);
            this.FailOnBurningImmobile(vatInd);
            yield return Toils_Goto.GotoThing(vatInd, PathEndMode.InteractionCell);
            yield return Toils_General.Wait(Duration)
                .FailOnDestroyedNullOrForbidden(vatInd)
                .FailOnCannotTouch(vatInd, PathEndMode.InteractionCell)
                .FailOn(() => !Vat.Completed)
                .WithProgressBarToilDelay(vatInd);
            yield return new Toil()
            {
                initAction = () =>
                {
                    Pawn spawned = Vat.SpawnCreature();
                    if (spawned == null)
                        EndJobWith(JobCondition.Errored);                        
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}
