using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace ProjectPangaea.Production
{
    public class Building_EmbryoVat : Building
    {
        private PangaeaThingEntry currentCultivatedEntry;
        public PangaeaThingEntry CurrentCultivatedEntry => currentCultivatedEntry;

        private Pawn generatedPawn;

        private CompPowerTrader powerComp;

        private float progress;
        public float Progress
        {
            get => progress;
            set
            {
                progress = Mathf.Clamp(value, 0, 1);
                if (Completed && generatedPawn == null)
                {
                    GeneratePawn();
                }
            }
        }

        public bool Completed => Progress >= 1;
        public bool Empty => CurrentCultivatedEntry == null;

        public int ExpectedDurationInTicks => CurrentCultivatedEntry?.ThingDef.VatGestationTicks() ?? -1;
        private int TicksLeft => Mathf.RoundToInt(ExpectedDurationInTicks * (1 - Progress));
        private float ProgressPerTick => Mathf.Pow(ExpectedDurationInTicks, -1);
        public override void TickRare()
        {
            base.TickRare();
            if (!Empty)
            {
                Progress += GenTicks.TickRareInterval * ProgressPerTick;
            }
        }

        private void GeneratePawn()
        {
            if (Empty)
            {
                Log.Warning("Tried to create creature for empty embryo vat!");
                return;
            }

            if (generatedPawn != null)
            {
                Log.Warning("Tried to create creature for embryo vat when one already exists!");
                return;
            }

            var pawnKind = CurrentCultivatedEntry.ThingDef.race.AnyPawnKind;
            var request = new PawnGenerationRequest(pawnKind, Faction.OfPlayer, newborn: true);
            generatedPawn = PawnGenerator.GeneratePawn(request);
        }

        public void SpawnCreature()
        {
            if (!Completed)
            {
                Log.Warning("Tried to spawn vat creature but it's not yet complete.");
                return;
            }
            GenSpawn.Spawn(generatedPawn, InteractionCell, Map, WipeMode.VanishOrMoveAside);
            Clear();
        }

        public void Abort()
        {
            if (currentCultivatedEntry == null)
            {
                return;
            }

            generatedPawn?.Destroy();
            Clear();
            //graphic stuff
        }

        private void Clear()
        {
            currentCultivatedEntry = null;
            Progress = 0;
            generatedPawn = null;
        }

        public void InsertEmbryo(PangaeaThing thing)
        {
            InsertEmbryo(thing.Resource);
            thing.SplitOff(1).Destroy();
        }

        public void InsertEmbryo(PangaeaResource embryo)
        {
            PangaeaResource.AssertType(embryo, ResourceTypeDefOf.Pangaea_Embryo, nameof(Building_EmbryoVat));

            if (!powerComp.PowerOn)
            {
                Log.Error("Tried to insert embryo into powerless vat!");
                return;
            }

            if (!Empty)
            {
                Abort();
            }
            currentCultivatedEntry = embryo.Entry;
            //graphic stuff
        }        

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            powerComp = GetComp<CompPowerTrader>();
            powerComp.powerStoppedAction += Abort;
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.GetInspectString());
            if (!Empty)
            {
                if (Completed)
                {
                    stringBuilder.AppendLine("Pangaea_GrownEmbryo".Translate(CurrentCultivatedEntry.Label, generatedPawn.ageTracker.AgeNumberString));
                }
                else
                {
                    stringBuilder.AppendLine("Pangaea_GrowingEmbryo".Translate(CurrentCultivatedEntry.Label, progress.ToStringPercent()));
                    stringBuilder.AppendLine("Pangaea_EstimatedTimeLeft".Translate(TicksLeft.ToStringTicksToPeriod(allowSeconds: false)));
                }
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Pangaea.Look(ref currentCultivatedEntry, nameof(currentCultivatedEntry));
            Scribe_References.Look(ref generatedPawn, nameof(generatedPawn));
            Scribe_Values.Look(ref progress, nameof(progress));
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
                yield return gizmo;

            if (DebugSettings.godMode)
            {
                yield return DebugActions.GenMenuListerAction("DEBUG: Insert embryo",
                    DebugActions.GenResourceMenuOptions(ResourceTypeDefOf.Pangaea_Embryo, InsertEmbryo)
                    );
                if (!Empty)
                {
                    yield return new Command_Action()
                    {
                        defaultLabel = "DEBUG: Abort",
                        action = Abort
                    };
                    yield return new Command_Action()
                    {
                        defaultLabel = "DEBUG: +0.1 progress",
                        action = () => Progress += 0.1f
                    };
                    yield return new Command_Action()
                    {
                        defaultLabel = "DEBUG: Max progress",
                        action = () => Progress = 1
                    };
                }
                if (Completed)
                {
                    yield return new Command_Action()
                    {
                        defaultLabel = "DEBUG: Spawn",
                        action = SpawnCreature
                    };
                }
            }
        }
    }
}
