using Verse;
using System;
using System.Collections.Generic;

namespace ProjectPangaea
{
    [Serializable]
    public class ResourceGraphicDef : Def
    {
        public PangaeaEntryFilter filter;
        public ResourceTypeDef resourceType;
        public GraphicData graphicData;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string err in base.ConfigErrors())
                yield return err;

            if (resourceType == null)
                yield return "null resourceType for " + this;
            if (filter == null)
                yield return "null filter for " + this;
            if (graphicData == null)
                yield return "null graphicData for " + this;
        }
    }
}
