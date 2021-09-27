using Verse;

namespace ProjectPangaea.Production
{
    [System.Serializable]
    //TODO i dont like this :(
    public class PangaeaResourceReference
    {
        private ThingDef owner;
        public bool 
        private ResourceTypeDef def;

        private PangaeaResource value = null;
        public PangaeaResource Value
        {
            get
            {
                if (value == null)
                {
                    value = PangaeaDatabase.GetOrNull(owner)?.GetResourceOfDef(def);
                }
                return value;
            }
        }

        public PangaeaThingFilter GetFilter()
        {
            PangaeaThingFilter filter = new PangaeaThingFilter()
        }
    }

    public class PangaeaResource
    {
        public const string ownerNodeName = "owner";
        public const string defNodeName = "def";

        public ResourceTypeDef ResourceDef { get; }

        public PangaeaThingEntry Entry { get; }

        public ThingDef ThingDef => Entry.ThingDef;

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
                    graphic = ResourceGraphicLister.GetFor(Entry, ResourceDef).Graphic;
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
