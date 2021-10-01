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
    }
}
