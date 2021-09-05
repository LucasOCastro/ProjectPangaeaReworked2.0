using Verse;
using UnityEngine;
using System;

namespace ProjectPangaea
{
    [Serializable]
    public class DNAGraphicTypeDef : Def
    {
        public PangaeaDiet diet;
        public AnimalType animalType;
        public ExtinctionStatus extinctionStatus;

        public Color contentColor = Color.white;
        public Color lidColor = Color.white;

        public GraphicData graphicData;

        public bool isPlant = false;

        private Graphic graphic;
        public Graphic Graphic
        {
            get
            {
                if (graphic == null)
                {
                    Graphic baseGraphic = graphicData.Graphic;
                    graphic = baseGraphic.GetColoredVersion(baseGraphic.Shader, contentColor, lidColor);
                }
                return graphic;
            }
        }
    }
}
