using Verse;

namespace ProjectPangaea
{
    [System.Serializable]
    //TODO i dont like this :(
    public class PangaeaResourceReference
    {
        private ThingDef owner = null;
        private ResourceTypeDef def = null;

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

        public static implicit operator PangaeaResourceReference(PangaeaResource resource)
        {
            if (resource == null)
            {
                return null;
            }
            return new PangaeaResourceReference() { value = resource };
        }

        public void LoadDataFromXmlCustom(System.Xml.XmlNode xmlRoot)
        {
            var node = xmlRoot.FirstChild;
            if (node != null)
            {
                string typeStr = node.Name;
                ResourceTypeDef type = DefDatabase<ResourceTypeDef>.GetNamed(typeStr);

                string ownerStr = node.Value;
                ThingDef owner = DefDatabase<ThingDef>.GetNamed(ownerStr);

                this.owner = owner;
                this.def = type;
            }
        }
    }

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
                    graphic = ResourceGraphicLister.GetFor(Entry, ResourceDef).graphicData.Graphic;
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
