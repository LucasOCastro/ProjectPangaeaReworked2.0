using RimWorld;
using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Overrides
{
    public partial class PangaeaOverrideDef : Def
    {
        [System.Serializable]
        public class PangaeaOverride
        {
            public string label;
            public string description;
        }

        public ThingDef overridenThingDef;

        public PangaeaOverride dnaOverride;
        public PangaeaOverride fossilOverride;

        public List<ThingEfficiency> dnaExtractionExtraProducts;

        public ThingDef dnaExtractionYieldOverride;
        public float dnaExtractionYieldEfficiency = -1;
    }
}
