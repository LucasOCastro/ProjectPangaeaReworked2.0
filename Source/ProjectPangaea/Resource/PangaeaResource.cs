using Verse;

namespace ProjectPangaea
{
    public class PangaeaResource
    {
        public ResourceTypeDef ResourceDef { get; }

        public PangaeaThingEntry Entry { get; }

        public ThingDef ParentThingDef => Entry.ThingDef;

        public PangaeaResource(ResourceTypeDef resourceDef, PangaeaThingEntry entry)
        {
            ResourceDef = resourceDef;
            Entry = entry;
        }

        private Graphic graphic;
        public Graphic Graphic
        {
            get
            {
                if (graphic == null)
                {
                    graphic = ResourceGraphicLister.GetFor(Entry, ResourceDef)?.graphicData?.Graphic;
                }
                return graphic;
            }
        }

        public string overrideLabel;
        public string overrideDescription;
        public string Label => overrideLabel.NullOrEmpty() ? GetLabel() : overrideLabel;
        public string Description => overrideDescription.NullOrEmpty() ? GetDescription() : overrideDescription;

        public PangaeaThing MakeThing()
        {
            PangaeaThing thing = ResourceDef.MakeThing();
            thing.Resource = this;
            return thing;
        }

        private string GetLabel() => ResourceDef.label.Formatted(Entry.Label);
        private string GetDescription() => ResourceDef.description.Formatted(Entry.Label);
    }
}
