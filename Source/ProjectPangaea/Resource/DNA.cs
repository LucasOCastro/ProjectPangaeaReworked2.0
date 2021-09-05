using Verse;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace ProjectPangaea
{
    [Serializable]
    public class PangaeaDNAGraphicTypeDef : Def
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

    public static class DNAGraphicsLister
    {
        private static Dictionary<Tuple<PangaeaDiet, AnimalType, ExtinctionStatus>, PangaeaDNAGraphicTypeDef> dict = new Dictionary<Tuple<PangaeaDiet, AnimalType, ExtinctionStatus>, PangaeaDNAGraphicTypeDef>();

        private static PangaeaDNAGraphicTypeDef plantDNAGraph;

        private static Tuple<PangaeaDiet, AnimalType, ExtinctionStatus> Tuple(PangaeaDiet diet, AnimalType type, ExtinctionStatus extinction)
            => System.Tuple.Create(diet, type, extinction);


        public static PangaeaDNAGraphicTypeDef GetDNAGraphicType(OrganismCategory category)
        {
            if (category is AnimalCategory animalCategory)
            {
                return GetDNAGraphicType(animalCategory);
            }
            return plantDNAGraph;
        }

        public static PangaeaDNAGraphicTypeDef GetDNAGraphicType(AnimalCategory category)
        {
            var tuple = Tuple(category.Diet, category.Type, category.ExtinctionStatus);
            dict.TryGetValue(tuple, out PangaeaDNAGraphicTypeDef result);
            return result;
        }

        public static void Init()
        {
            foreach (var dnaGraphic in DefDatabase<PangaeaDNAGraphicTypeDef>.AllDefs)
            {
                if (dnaGraphic.isPlant)
                {
                    plantDNAGraph = dnaGraphic;
                    continue;
                }

                var tuple = Tuple(dnaGraphic.diet, dnaGraphic.animalType, dnaGraphic.extinctionStatus);
                if (!dict.ContainsKey(tuple))
                {
                    dict.Add(tuple, dnaGraphic);
                }
                else dict[tuple] = dnaGraphic;
                Log.Message("JUST ADDED: " + tuple + " AND THE DEFNAME IS: " + dnaGraphic.defName);
            }
        }
    }

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
