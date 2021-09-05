using RimWorld;
using Verse;

namespace ProjectPangaea
{
    public abstract class PangaeaResourceThingBase : ThingWithComps
    {
        protected PangaeaResource resource;
        public PangaeaResource Resource
        {
            get
            {
                //TODO: (if) After rarity is implemented, assign a random resource of similar rarity to reduce exploits and whatever blabla
                if (resource == null && PangaeaSettings.AssignRandomPawnToNullResource)
                {
                    resource = GetRandomResource();
                }
                return resource;
            }
        }

        private Graphic graphic;
        public override Graphic Graphic
        {
            get
            {
                if (Resource == null || resource.TexturePath.NullOrEmpty())
                {
                    return BaseContent.BadGraphic;
                }

                if (graphic == null || graphic.path != Resource.TexturePath)
                {
                    graphic = DefaultGraphic.GetCopy(DefaultGraphic.drawSize);
                    graphic.path = Resource.TexturePath;
                }

                return graphic.GetCopy(DefaultGraphic.drawSize);
            }
        }

        public override string LabelNoCount => Resource?.Label ?? "Pangaea_MissingResourceLabel".Translate();
        public override string DescriptionFlavor => Resource?.Description ?? "Pangaea_MissingResourceDescription".Translate();

        protected abstract PangaeaResource GetRandomResource();

        public override bool CanStackWith(Thing other)
        {
            return base.CanStackWith(other) && ((PangaeaResourceThingBase)other).Resource == this.Resource;
        }

        public override Thing SplitOff(int count)
        {
            PangaeaResourceThingBase thing = (PangaeaResourceThingBase)base.SplitOff(count);
            thing.resource = Resource;
            return thing;
        }

        public override void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
        {
            base.PostGeneratedForTrader(trader, forTile, forFaction);
            resource = GetRandomResource();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref resource, "ResourceThingResource");
        }
    }
}
