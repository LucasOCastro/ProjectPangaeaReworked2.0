using Verse;
using UnityEngine;
using System;

namespace ProjectPangaea
{
    [Serializable]
    public class ResourceGraphicDef : Def
    {
        public ResourceTypeDef resourceType;

        public PangaeaEntryFilter filter;

        public GraphicData graphicData;

        public Graphic Graphic => graphicData.Graphic;
    }
}
