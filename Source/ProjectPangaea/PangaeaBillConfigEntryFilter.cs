using Verse;
using ProjectPangaea.PangaeaUI;
using UnityEngine;
using Enum = System.Enum;
using ProjectPangaea.Production;

namespace ProjectPangaea
{
    [System.Serializable]
    public class PangaeaBillConfigEntryFilter : PangaeaEntryFilter
    {
        private const string animalTypeLabel = "Pangaea_AnimalGroupFilter";
        private const string extinctionLabel = "Pangaea_ExtinctionStatusFilter";
        private const string dietLabel = "Pangaea_DietFilter";
        private const string unavailableLabel = "Pangaea_ShowUnavailableIngredients";

        public bool allowUnavailable = true;

        public PangaeaResourceBill bill;

        public void DrawMissingIngredientsOption(Rect rect) => PangaeaUIGen.CheckMarkLabel(rect, ref allowUnavailable, unavailableLabel.Translate());
        public void DrawTextFilter(Rect rect) => textFilter = Widgets.TextArea(rect, textFilter);
        public void DrawAnimalTypeFilter(Rect rect) => DrawFilterButton(rect, animalTypeLabel, animalTypeFilter);
        public void DrawExtinctionStatusFilter(Rect rect) => DrawFilterButton(rect, extinctionLabel, extinctionFilter);
        public void DrawDietFilter(Rect rect) => DrawFilterButton(rect, dietLabel, dietFilter);

        private void DrawFilterButton<E>(Rect rect, string translate, EnumDictionary<E, bool> filterDict) where E : Enum
        {
            string label = translate.Translate() + "...";
            if (Widgets.ButtonText(rect, label, drawBackground: false))
            {
                Find.WindowStack.Add(new FloatMenuWithCheckmarks<E>(filterDict));
            }
        }

        public override bool Allows(PangaeaThingEntry entry)
        {
            if (!base.Allows(entry))
            {
                return false;
            }

            return allowUnavailable || !IsUnavailable(entry);
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
