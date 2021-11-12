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
        public Pawn GeneratedPawn => generatedPawn;

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

        private bool allowPawnRelease = true;
        public bool AllowPawnRelease => allowPawnRelease;

        private bool allowAutomaticFilling = true;
        public bool AllowAutomaticFilling => allowAutomaticFilling;

        public int ExpectedDurationInTicks => CurrentCultivatedEntry?.ThingDef.VatGestationTicks() ?? -1;
        private int TicksLeft => Mathf.RoundToInt(ExpectedDurationInTicks * (1 - Progress));
        private float ProgressPerTick => Mathf.Pow(ExpectedDurationInTicks, -1);
        public override void TickRare()
        {
            base.TickRare();

            if (generatedPawn != null && ProjectPangaeaMod.Settings.instantReleaseFromVat && AllowPawnRelease)
            {
                SpawnCreature();
                return;
            }

            if (!Empty)
            {
                Progress += GenTicks.TickRareInterval * ProgressPerTick;
                //todo make pawn age
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

            Message message = new Message("Pangaea_FinishedGrowingMessage".Translate(generatedPawn.LabelCap), MessageTypeDefOf.PositiveEvent, this);
            if (ProjectPangaeaMod.Settings.instantReleaseFromVat && AllowPawnRelease)
            {
                message.lookTargets = SpawnCreature();
            }
            Messages.Message(message);
        }

        public Pawn SpawnCreature()
        {
            if (!Completed)
            {
                Log.Warning("Tried to spawn vat creature but it's not yet complete.");
                return null;
            }
            Thing spawnedPawn = GenSpawn.Spawn(generatedPawn, InteractionCell, Map, WipeMode.VanishOrMoveAside);
            Clear();
            return spawnedPawn as Pawn;
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
            if (embryo.ResourceDef != ResourceTypeDefOf.Pangaea_Embryo)
            {
                Log.Error(embryo + " should be of type " + ResourceTypeDefOf.Pangaea_Embryo.defName + "for " + this);
            }

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
            Scribe_Values.Look(ref allowAutomaticFilling, nameof(allowAutomaticFilling), defaultValue: true);
            Scribe_Values.Look(ref allowPawnRelease, nameof(allowPawnRelease), defaultValue: true);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
                yield return gizmo;

            yield return new Command_Toggle()
            {
                defaultLabel = "Pangaea_AllowAutoFilling".Translate(),
                defaultDesc = "Pangaea_AllowAutoFillingDescription".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter"),
                isActive = () => AllowAutomaticFilling,
                toggleAction = () => allowAutomaticFilling = !allowAutomaticFilling
            };

            yield return new Command_Toggle()
            {
                defaultLabel = "Pangaea_AllowAutoRelease".Translate(),
                defaultDesc = "Pangaea_AllowAutoReleaseDescription".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject"),
                isActive = () => AllowPawnRelease,
                toggleAction = () => allowPawnRelease = !allowPawnRelease
            };

            if (DebugSettings.godMode)
            {
                foreach (Gizmo gizmo in GetDebugGizmos())
                    yield return gizmo;
            }
        }

        private IEnumerable<Gizmo> GetDebugGizmos()
        {
            yield return DebugActions.GenMenuListerAction("DEBUG: Insert embryo",
                    DebugActions.GenResourceMenuOptions(ResourceTypeDefOf.Pangaea_Embryo, InsertEmbryo));

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
                    action = () => SpawnCreature()
                };
            }
        }
    }
}
