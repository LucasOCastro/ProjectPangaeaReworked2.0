using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea
{
    [System.Serializable]
    public class PangaeaEntryFilter
    {
        public string textFilter = "";


        private List<AnimalType> animalTypes = new List<AnimalType>();
        private List<ExtinctionStatus> extinctionStatus = new List<ExtinctionStatus>();
        private List<PangaeaDiet> diets = new List<PangaeaDiet>();

        public HashSet<ThingDef> directDefFilter = null;

        public bool this[AnimalType type]
        {
            get => animalTypes.Contains(type);
            set 
            {
                if (value == true) animalTypes.Add(type);
                else animalTypes.Remove(type);
            }
        }

        public bool this[ExtinctionStatus extinction]
        {
            get => extinctionStatus.Contains(extinction);
            set
            {
                if (value == true) extinctionStatus.Add(extinction);
                else extinctionStatus.Remove(extinction);
            }
        }

        public bool this[PangaeaDiet diet]
        {
            get => diets.Contains(diet);
            set
            {
                if (value == true) diets.Add(diet);
                else diets.Remove(diet);
            }
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
