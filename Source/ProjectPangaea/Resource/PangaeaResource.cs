using Verse;
using UnityEngine;

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

        private Texture2D icon;
        public Texture2D Icon
        {
            get
            {
                if (icon == null)
                {
                    icon = Graphic?.MatSingle?.mainTexture as Texture2D;
                }
                return icon;
            }
        }

        public string overrideLabel;
        public string overrideDescription;
        public string Label => overrideLabel.NullOrEmpty() ? GetLabel() : overrideLabel;
        public string Description => overrideDescription.NullOrEmpty() ? GetDescription() : overrideDescription;

        public PangaeaThing MakeThing()
        {
            PangaeaThing thing = ResourceDef.MakeEmptyThing();
            thing.Resource = this;
            return thing;
        }

        private string GetLabel() => ResourceDef.label.Formatted(Entry.Label);
        private string GetDescription() => ResourceDef.description.Formatted(Entry.Label);

        public static void AssertType(PangaeaResource resource, ResourceTypeDef type, string forS = null)
        {
            if (resource.ResourceDef != type)
            {
                throw new System.Exception(resource + " should be of type " + type.defName + (forS.NullOrEmpty() ? "" : ("for " + forS)));
            }
        }
    }
}
