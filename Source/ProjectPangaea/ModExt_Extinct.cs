using Verse;

namespace ProjectPangaea
{
    public class ModExt_Extinct : DefModExtension
    {
        public string genus;
        public string species;
        public string fossilTexturePath;
        public AnimalType animalType;

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
