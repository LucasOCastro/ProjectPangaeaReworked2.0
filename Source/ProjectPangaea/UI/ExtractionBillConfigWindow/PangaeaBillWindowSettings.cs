using Verse;
using UnityEngine;

namespace ProjectPangaea.PangaeaUI
{
    public class PangaeaBillWindowSettings
    {
        public PangaeaEntrySorter sorter { get; } = new PangaeaEntrySorter();
        public PangaeaEntryFilter filter { get; } = new PangaeaEntryFilter();

        public PangaeaBillConfigWindow Window { get; }

        public PangaeaBillWindowSettings(PangaeaBillConfigWindow window)
        {
            Window = window;
            filter.bill = window.ExtractionBill;
            sorter.bill = window.ExtractionBill;
        }

        public Rect Draw(Vector2 position, float width)
        {
            Verse.Text.Font = GameFont.Small;
            Verse.Text.Anchor = TextAnchor.MiddleCenter;

            float lineHeight = PangaeaUIGen.lineHeight;
            float lineSpacing = PangaeaUIGen.lineSpacing;

            Rect allowedIngredientsRect = new Rect(position.x, position.y, width, lineHeight);
            filter.DrawMissingIngredientsOption(allowedIngredientsRect);

            Rect textFilterBarRect = new Rect(allowedIngredientsRect) { y = allowedIngredientsRect.yMax + lineSpacing };
            filter.DrawTextFilter(textFilterBarRect);

            Rect animalGroupFilterRect = new Rect(textFilterBarRect) { y = textFilterBarRect.yMax + (2 * lineSpacing) };
            filter.DrawAnimalTypeFilter(animalGroupFilterRect);
            Rect extinctionFilterRect = new Rect(animalGroupFilterRect) { y = animalGroupFilterRect.yMax + lineSpacing };
            filter.DrawExtinctionStatusFilter(extinctionFilterRect);
            Rect dietFilterRect = new Rect(extinctionFilterRect) { y = extinctionFilterRect.yMax + lineSpacing };
            filter.DrawDietFilter(dietFilterRect);

            Rect sortByRect = new Rect(dietFilterRect) { y = dietFilterRect.yMax + (2 * lineSpacing) };
            sorter.Draw(sortByRect);

            Verse.Text.Anchor = TextAnchor.UpperLeft;

            return new Rect() { position = position, max = sortByRect.max };
        }
    }
}
