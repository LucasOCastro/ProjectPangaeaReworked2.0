using Verse;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea
{
    [System.Serializable]
    public class PangaeaEntryFilter
    {
        public string textFilter = "";

        public EnumDictionary<AnimalType, bool> animalTypeFilter = new EnumDictionary<AnimalType, bool>(true);//, AnimalType.Default);
        public EnumDictionary<ExtinctionStatus, bool> extinctionFilter = new EnumDictionary<ExtinctionStatus, bool>(true);
        public EnumDictionary<PangaeaDiet, bool> dietFilter = new EnumDictionary<PangaeaDiet, bool>(true);

        public HashSet<ThingDef> directDefFilter = null;

        public bool this[AnimalType type]
        {
            get => animalTypeFilter[type];
            set => animalTypeFilter[type] = value;
        }

        public bool this[ExtinctionStatus extinction]
        {
            get => extinctionFilter[extinction];
            set => extinctionFilter[extinction] = value;
        }

        public bool this[PangaeaDiet diet]
        {
            get => dietFilter[diet];
            set => dietFilter[diet] = value;
        }

        public virtual bool Allows(PangaeaThingEntry entry)
        {
            if (directDefFilter != null)
            {
                return directDefFilter.Contains(entry.ThingDef);
            }

            if (!entry.ThingDef.label.Contains(textFilter.ToLower()))
            {
                return false;
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
