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

        public bool AllowsResource(PangaeaResource resource) => ResourceHolder.Props.AllowsTypeOfResource(resource);
        public bool IsOfType(ResourceTypeDef type) => resourceHolder.Props.IsOfType(type);

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
    }
}
