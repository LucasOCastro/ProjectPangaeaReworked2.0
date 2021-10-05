using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea
{
    [System.Serializable]
    public class PangaeaEntryFilter
    {
        private EnumDictionary<AnimalType, bool> animalTypes = new EnumDictionary<AnimalType, bool>(true);
        private EnumDictionary<ExtinctionStatus, bool> extinctionStatus = new EnumDictionary<ExtinctionStatus, bool>(true);
        private EnumDictionary<PangaeaDiet, bool> diets = new EnumDictionary<PangaeaDiet, bool>(true);

        public List<ThingDef> directDefFilter = null;

        public bool this[AnimalType type]
        {
            get => animalTypes[type];
            set => animalTypes[type] = value;
        }

        public bool this[ExtinctionStatus extinction]
        {
            get => extinctionStatus[extinction];
            set => extinctionStatus[extinction] = value;
        }

        public bool this[PangaeaDiet diet]
        {
            get => diets[diet];
            set => diets[diet] = value;
        }

        public virtual bool Allows(PangaeaThingEntry entry)
        {
            if (directDefFilter != null)
            {
                return directDefFilter.Contains(entry.ThingDef);
            }

            if (entry.ThingDef.plant != null || entry.Category == null)
            {
                return true;
            }

            return Allows(entry.Category);
        }

        //TODO this shall be used for plant category and stuff like that
        public bool Allows(OrganismCategory category)
        {
            if (category is PlantCategory plant)
            {
                return Allows(plant);
            }
            return Allows(category as AnimalCategory);
        }

        public bool Allows(PlantCategory category)
        {
            return this[category.ExtinctionStatus];//&& allowsPlant;
        }

        public bool Allows(AnimalCategory category)
        {
            return this[category.Type] && this[category.ExtinctionStatus] && this[category.Diet];
        }
    }
}
