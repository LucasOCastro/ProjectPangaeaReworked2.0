using Verse;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public class PangaeaThingEntry
    {
        public ThingDef ThingDef { get; }

        public ModExt_Extinct ExtinctExtension { get; }

        public OrganismCategory Category { get; }

        private DNA dna;
        public DNA DNA => dna;

        public Fossil Fossil { get; }

        public PangaeaThingEntry(ThingDef thingDef, AnimalType animalType = AnimalType.Unspecified)
        {
            ThingDef = thingDef;
            ExtinctExtension = thingDef.GetModExtension<ModExt_Extinct>();
            Category = GetCategory(thingDef, animalType);

            dna = new DNA(thingDef, Category);

            if (ExtinctExtension != null)
            {
                Fossil = new Fossil(thingDef, ExtinctExtension);
            }
        }

        public PangaeaResource GetResourceOfCategory(ThingCategoryDef category)
        {
            if (category == PangaeaThingCategoryDefOf.PangaeaDNA)
            {
                return DNA;
            }
            if (category == PangaeaThingCategoryDefOf.PangaeaFossils)
            {
                return Fossil;
            }
            return null;
        }

        public PangaeaResource GetResourceOfDef(ThingDef def)
        {
            if (def == PangaeaThingDefOf.Pangaea_DNABase)
            {
                return DNA;
            }
            if (def == PangaeaThingDefOf.Pangaea_FossilBase)
            {
                return Fossil;
            }
            //TODO embryo
            return null;
        }

        private static OrganismCategory GetCategory(ThingDef thingDef, AnimalType animalType)
        {
            if (thingDef.plant != null)
            {
                return new PlantCategory(thingDef);
            }
            return new AnimalCategory(thingDef, animalType);
        }

        public override string ToString()
        {
            return base.ToString() + " " + ThingDef;
        }
    }
}
