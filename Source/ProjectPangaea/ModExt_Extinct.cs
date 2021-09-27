using Verse;

namespace ProjectPangaea
{
    public class ModExt_Extinct : DefModExtension
    {
        public string genus;
        public string species;
        public GraphicData fossilGraphicData;

        private string scientificName;
        public string ScientificName
        {
            get
            {
                if (scientificName.NullOrEmpty())
                {
                    scientificName = $"{genus.CapitalizeFirst()} {species}".Trim();
                }
                return scientificName;
            }
        }
    }
}
