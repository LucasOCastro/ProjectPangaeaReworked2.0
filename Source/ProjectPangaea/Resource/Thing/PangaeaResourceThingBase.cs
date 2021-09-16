using RimWorld;
using System.Collections.Generic;
using Verse;
using System;
using System.Linq;

namespace ProjectPangaea
{
    public class CompProperties_PangaeaResourceHolder : CompProperties
    {
        public Type resourceType;

        public CompProperties_PangaeaResourceHolder()
        {
            compClass = typeof(CompPangaeaResourceHolder);
        }

        public bool Allows(PangaeaResource resource) => resource.GetType() == resourceType;

        //TODO cache
        public PangaeaResource GetRandomResource() => GetAllPossibleEntries().RandomElement().GetResourceOfType(resourceType);
        public IEnumerable<PangaeaResource> GetAllPossibleResources() => GetAllPossibleEntries().Select(e => e.GetResourceOfType(resourceType));
        public IEnumerable<PangaeaThingEntry> GetAllPossibleEntries() => PangaeaDatabase.AllEntries.Where(e => e.GetResourceOfType(resourceType) != null);

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
                yield return error;
            if (resourceType == null || !typeof(PangaeaResource).IsAssignableFrom(resourceType))
                yield return "Invalid " + nameof(resourceType) + " in " + this;
        }
    }

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

    public class PangaeaThing : ThingWithComps
    {
        private CompPangaeaResourceHolder resourceHolder;
        public CompPangaeaResourceHolder ResourceHolder
        {
            get
            {
                if (resourceHolder == null)
                {
                    resourceHolder = GetComp<CompPangaeaResourceHolder>();
                }
                return resourceHolder;
            }
        }

        public PangaeaResource Resource
        {
            get => ResourceHolder.Resource;
            set => ResourceHolder.Resource = value;
        }

        public bool Allows(PangaeaResource resource) => ResourceHolder.Props.Allows(resource);

        public override Graphic Graphic
        {
            get
            {
                if (Resource == null)
                {
                    return BaseContent.BadGraphic;
                }
                return Resource.Graphic ?? base.Graphic;
            }
        }

        public override string LabelNoCount => Resource?.Label ?? "Pangaea_MissingResourceLabel".Translate();
        public override string DescriptionFlavor => Resource?.Description ?? "Pangaea_MissingResourceDescription".Translate();

        private static Dictionary<Type, ThingDef> resourceTypeDefCache = new Dictionary<Type, ThingDef>();
        public static PangaeaThing MakePangaeaThing(PangaeaResource resource)
        {
            Type type = resource.GetType();
            if (!resourceTypeDefCache.TryGetValue(type, out ThingDef def))
            {
                foreach (var thingDef in DefDatabase<ThingDef>.AllDefs)
                {
                    var compProps = thingDef.GetCompProperties<CompProperties_PangaeaResourceHolder>();
                    if (compProps != null && compProps.Allows(resource))
                    {
                        def = thingDef;
                        break;
                    }
                }
                resourceTypeDefCache.Add(type, def);
            }
            PangaeaThing thing = ThingMaker.MakeThing(def) as PangaeaThing;
            thing.Resource = resource;
            return thing;
        }
    }
}
