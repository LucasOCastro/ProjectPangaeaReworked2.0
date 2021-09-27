using Verse;

namespace ProjectPangaea
{
    /*public class Fossil : PangaeaResource
    {
        public ModExt_Extinct ExtinctExtension { get; private set; }

        public Fossil(ThingDef parent, ModExt_Extinct extinctExtension) : base(parent)
        {
            ExtinctExtension = extinctExtension;
        }

        public Fossil() : base()
        {
        }

        public override Graphic Graphic => ExtinctExtension.fossilGraphicData.Graphic;

        protected override string GetLabel() => "Pangaea_FossilLabel".Translate(ParentThingDef.label);
        protected override string GetDescription() => "Pangaea_FossilDescription".Translate(ExtinctExtension.ScientificName);
            
        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
            {
                PangaeaThingEntry entry = PangaeaDatabase.GetOrNull(ParentThingDef);
                if (entry == null || entry.ExtinctExtension == null) return;

                ExtinctExtension = entry.ExtinctExtension;
            }
        }
    }*/
}
