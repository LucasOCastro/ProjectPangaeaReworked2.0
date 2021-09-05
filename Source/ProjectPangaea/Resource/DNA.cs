﻿using Verse;

namespace ProjectPangaea
{

    public class DNA : PangaeaResource
    {
        public OrganismCategory Category { get; private set; }
        public DNA(ThingDef parent, OrganismCategory category) : base(parent)
        {
            Category = category;
        }

        public DNA() : base()
        {
        }

        private Graphic graphic;
        public override Graphic Graphic
        {
            get
            {
                if (graphic == null)
                {
                    graphic = DNAGraphicsLister.GetDNAGraphicType(Category)?.Graphic;
                }
                return graphic;
            }
        }

        protected override string GetLabel() => "Pangaea_DNALabel".Translate(ParentThingDef.label);
        protected override string GetDescription()
        {
            if (ParentThingDef.IsExtinct(out PangaeaThingEntry entry))
            {
                return "Pangaea_ExtinctDNADescription".Translate(entry.ExtinctExtension.ScientificName);
            }
            return "Pangaea_ExtantDNADescription".Translate(ParentThingDef.label);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
            {
                PangaeaThingEntry entry = PangaeaDatabase.GetOrNull(ParentThingDef);
                if (entry == null) return;

                Category = entry.Category;
            }
        }
    }
}