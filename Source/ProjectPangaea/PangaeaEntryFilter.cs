using Verse;
using ProjectPangaea.PangaeaUI;
using UnityEngine;
using Enum = System.Enum;
using ProjectPangaea.Production;

namespace ProjectPangaea
{
    public class PangaeaEntryFilter
    {
        public string currentTextFilter = "";

        private EnumDictionary<AnimalType, bool> animalTypeFilterDict = new EnumDictionary<AnimalType, bool>(true);//, AnimalType.Default);
        private EnumDictionary<ExtinctionStatus, bool> extinctionFilterDict = new EnumDictionary<ExtinctionStatus, bool>(true);
        private EnumDictionary<PangaeaDiet, bool> dietFilterDict = new EnumDictionary<PangaeaDiet, bool>(true);

        private const string animalTypeLabel = "Pangaea_AnimalGroupFilter";
        private const string extinctionLabel = "Pangaea_ExtinctionStatusFilter";
        private const string dietLabel = "Pangaea_DietFilter";
        private const string unavailableLabel = "Pangaea_ShowUnavailableIngredients";

        public bool allowUnavailable = true;

        public PangaeaResourceBill bill;

        public bool this[AnimalType type]
        {
            get => animalTypeFilterDict[type];
            set => animalTypeFilterDict[type] = value;
        }

        public bool this[ExtinctionStatus extinction]
        {
            get => extinctionFilterDict[extinction];
            set => extinctionFilterDict[extinction] = value;
        }

        public bool this[PangaeaDiet diet]
        {
            get => dietFilterDict[diet];
            set => dietFilterDict[diet] = value;
        }

        public void DrawMissingIngredientsOption(Rect rect) => PangaeaUIGen.CheckMarkLabel(rect, ref allowUnavailable, unavailableLabel.Translate());
        public void DrawTextFilter(Rect rect) => currentTextFilter = Widgets.TextArea(rect, currentTextFilter);
        public void DrawAnimalTypeFilter(Rect rect) => DrawFilterButton(rect, animalTypeLabel, animalTypeFilterDict);
        public void DrawExtinctionStatusFilter(Rect rect) => DrawFilterButton(rect, extinctionLabel, extinctionFilterDict);
        public void DrawDietFilter(Rect rect) => DrawFilterButton(rect, dietLabel, dietFilterDict);

        private void DrawFilterButton<E>(Rect rect, string translate, EnumDictionary<E, bool> filterDict) where E : Enum
        {
            string label = translate.Translate() + "...";
            if (Widgets.ButtonText(rect, label, drawBackground: false))
            {
                Find.WindowStack.Add(new FloatMenuWithCheckmarks<E>(filterDict));
            }
        }

        public bool Allows(PangaeaThingEntry entry)
        {
            if (!entry.ThingDef.label.Contains(currentTextFilter.ToLower()))
            {
                return false;
            }

            if (entry.ThingDef.plant != null || entry.Category == null)
            {
                return true;
            }

            if (!allowUnavailable && IsUnavailable(entry))
            {
                return false;
            }

            var category = (AnimalCategory)entry.Category;
            return animalTypeFilterDict[category.Type] 
                && extinctionFilterDict[category.ExtinctionStatus] 
                && dietFilterDict[category.Diet];
        }

        private bool IsUnavailable(PangaeaThingEntry entry)
        {
            if (bill == null || bill.Counter == null)
            {
                return false;
            }
            var counter = bill.Counter;
            return counter.Count(bill.Map, entry) < counter.ResourceRequirement;
        }
    }

}
