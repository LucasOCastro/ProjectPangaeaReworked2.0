using RimWorld;
using System.Collections.Generic;
using Verse;

namespace ProjectPangaea
{
    public class CompPangaeaResourceHolder : ThingComp
    {
        public CompProperties_PangaeaResourceHolder Props => (CompProperties_PangaeaResourceHolder)props;

        private PangaeaResource resource;
        public PangaeaResource Resource
        {
            get
            {
                if (resource == null && PangaeaSettings.AssignRandomPawnToNullResource)
                {
                    resource = Props.GetRandomResource();
                }
                return resource;
            }
            set
            {
                resource = value;
                if (parent.Map != null)
                {
                    parent.DirtyMapMesh(parent.Map);
                }
            }
        }

        public override bool AllowStackWith(Thing other)
        {
            return base.AllowStackWith(other) && other is PangaeaThing pt && pt.Resource == Resource;
        }

        public override void PostSplitOff(Thing piece)
        {
            (piece as PangaeaThing).Resource = Resource;
        }

        public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
        {
            Resource = Props.GetRandomResource();
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (DebugSettings.godMode)
            {
                yield return DebugActions.GenAction(this);
            }
        }

        public override void PostExposeData()
        {
            Scribe_Deep.Look(ref resource, "ResourceThingResource");
        }
    }
}
