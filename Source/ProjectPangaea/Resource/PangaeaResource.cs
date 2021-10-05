using Verse;
using HarmonyLib;

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
                    value = PangaeaDatabase.GetOrNull(owner)?.GetResource(def);
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
                System.Type thisType = typeof(PangaeaResourceReference);

                string typeStr = node.Name;
                DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, AccessTools.Field(thisType, nameof(def)), typeStr, assumeFieldType: typeof(ResourceTypeDef));
                string ownerStr = node?.FirstChild?.Value;
                DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, AccessTools.Field(thisType, nameof(owner)), ownerStr, assumeFieldType: typeof(ThingDef));
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
                    graphic = ResourceGraphicLister.GetFor(Entry, ResourceDef)?.graphicData.Graphic ?? BaseContent.BadGraphic;
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
