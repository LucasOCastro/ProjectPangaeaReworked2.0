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

            public void Override(PangaeaResource resource)
            {
                if (!label.NullOrEmpty())
                {
                    resource.overrideLabel = label;
                }

                if (!description.NullOrEmpty())
                {
                    resource.overrideDescription = description;
                }
            }
        }

        public ThingDef overridenThingDef;

        public PangaeaOverride dnaOverride;
        public PangaeaOverride fossilOverride;

        public List<ThingEfficiency> dnaExtractionExtraProducts;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string error in base.ConfigErrors())
            {
                yield return error;
            }

            if (overridenThingDef == null)
            {
                yield return $"{nameof(PangaeaOverrideDef)} of defName {defName} has null overridenThingDef!";
            }
        }
    }
}
