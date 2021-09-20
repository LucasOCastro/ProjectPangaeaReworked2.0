using Verse;

namespace ProjectPangaea
{
    //TODO maybe turn this into a single class defined by a custom def
    public abstract class PangaeaResource : IExposable
    {
        private ThingDef parentThingDef;
        public ThingDef ParentThingDef => parentThingDef;
        //public string TexturePath { get; protected set; }
        public abstract Graphic Graphic { get; }

        public bool IsFromAnimal => ParentThingDef.race != null;
        public bool IsFromPlant => ParentThingDef.plant != null;

        public string overrideLabel;
        public string overrideDescription;
        public string Label => overrideLabel.NullOrEmpty() ? GetLabel() : overrideLabel;
        public string Description => overrideDescription.NullOrEmpty() ? GetDescription() : overrideDescription;

        protected abstract string GetLabel();
        protected abstract string GetDescription();

        public PangaeaResource(ThingDef parent)
        {
            parentThingDef = parent;
        }

        //Constructor for saving/loading
        public PangaeaResource()
        {
        }

        public virtual void ExposeData()
        {
            Scribe_Defs.Look(ref parentThingDef, "ResourceParentThingDef");
        }
    }
}
