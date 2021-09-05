using Verse;
using System.Collections.Generic;
using System;

namespace ProjectPangaea
{
    public static class DNAGraphicsLister
    {
        private static Dictionary<Tuple<PangaeaDiet, AnimalType, ExtinctionStatus>, DNAGraphicTypeDef> dict = new Dictionary<Tuple<PangaeaDiet, AnimalType, ExtinctionStatus>, DNAGraphicTypeDef>();

        private static DNAGraphicTypeDef plantDNAGraph;

        private static Tuple<PangaeaDiet, AnimalType, ExtinctionStatus> Tuple(PangaeaDiet diet, AnimalType type, ExtinctionStatus extinction)
            => System.Tuple.Create(diet, type, extinction);


        public static DNAGraphicTypeDef GetDNAGraphicType(OrganismCategory category)
        {
            if (category is AnimalCategory animalCategory)
            {
                return GetDNAGraphicType(animalCategory);
            }
            return plantDNAGraph;
        }

        public static DNAGraphicTypeDef GetDNAGraphicType(AnimalCategory category)
        {
            var tuple = Tuple(category.Diet, category.Type, category.ExtinctionStatus);
            dict.TryGetValue(tuple, out DNAGraphicTypeDef result);
            return result;
        }

        public static void Init()
        {
            foreach (var dnaGraphic in DefDatabase<DNAGraphicTypeDef>.AllDefs)
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
}
