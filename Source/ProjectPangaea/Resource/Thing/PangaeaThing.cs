using System.Collections.Generic;
using Verse;
using System;

namespace ProjectPangaea
{
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

        public bool AllowsTypeOfResource(PangaeaResource resource) => ResourceHolder.Props.AllowsTypeOfResource(resource);
        public bool IsOfType(Type type) => resourceHolder.Props.IsOfType(type);

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
                    if (compProps != null && compProps.IsOfType(type))
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
