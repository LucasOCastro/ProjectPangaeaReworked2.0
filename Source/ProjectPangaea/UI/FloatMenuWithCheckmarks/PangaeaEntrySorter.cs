using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ProjectPangaea.PangaeaUI;
using System;
using ProjectPangaea.Production;


namespace ProjectPangaea
{
    public class PangaeaEntrySorter
    {
        public enum SortByOrder { Ascending = 1, Descending = -1 }
        public enum SortBy { Name, Count, AnimalGroup, Diet, Allowed }

        public SortByOrder sortOrder = SortByOrder.Ascending;
        public SortBy sortBy = SortBy.Name;

        public bool Ascending => (sortOrder == SortByOrder.Ascending);

        public PangaeaResourceBill bill;

        private bool IsNumericalSort
        {
            get
            {
                switch (sortBy)
                {
                    default: return false;
                    case SortBy.Allowed:
                    case SortBy.Count: 
                        return true;
                }
            }
        }

        private void SelectSortBy(SortBy sortBy)
        {
            this.sortBy = sortBy;
        }

        public void ToggleSortOrder()
        {
            sortOrder = Ascending ? SortByOrder.Descending : SortByOrder.Ascending;
        }

        public void Draw(Rect rect)
        {
            float sortDirectionArrowSize = rect.height;

            Rect sortByRect = new Rect(rect) { width = rect.width - sortDirectionArrowSize };
            string sortByLabel = "Pangaea_SortBy".Translate() + ": ";
            if (PangaeaUIGen.ButtonWithPrefixLabel(sortByRect, sortByLabel, sortBy.ToString()))
            {
                List<FloatMenuOption> options = PangaeaUIGen.GenEnumFloatMenuOptions<SortBy>(SelectSortBy);
                FloatMenu floatMenu = new FloatMenu(options);
                Find.WindowStack.Add(floatMenu);
            }

            Rect arrowRect = new Rect(rect) { xMin = sortByRect.xMax };
            TooltipHandler.TipRegion(arrowRect, new TipSignal(sortOrder.ToString()));
            Texture2D arrowTex = Ascending ? PangaeaUIGen.ArrowUp : PangaeaUIGen.ArrowDown;
            if (Widgets.ButtonImage(arrowRect, arrowTex))
            {
                ToggleSortOrder();
            }
        }

        private float GetSortNum(PangaeaThingEntry entry)
        {
            if (sortBy == SortBy.Count && bill != null && bill.Counter != null)
            {
                return bill.Counter.Count(bill.Map, entry);
            }

            if (sortBy == SortBy.Allowed)
            {
                return bill.Allows(entry) ? 1 : 0;
            }

            return 0;
        }

        private string GetSortString(PangaeaThingEntry entry)
        {
            if (sortBy == SortBy.Name)
            {
                return entry.ThingDef.label;
            }

            if (entry.ThingDef.plant != null)
            {
                return "";
            }

            AnimalCategory animalCategory = (AnimalCategory)entry.Category;
            switch (sortBy)
            {
                case SortBy.AnimalGroup: return animalCategory.Type.ToString();
                case SortBy.Diet: return animalCategory.Diet.ToString();
                default: return "";
            }
        }

        public IOrderedEnumerable<PangaeaThingEntry> Sort(IEnumerable<PangaeaThingEntry> entries)
        {
            if (IsNumericalSort)
            {
                return entries.OrderBy(GetSortNum, Ascending).ThenBy(e => e.ThingDef.label, Ascending);
            }
            return entries.OrderBy(GetSortString, Ascending).ThenBy(e => e.ThingDef.label, Ascending);
        }

        public IOrderedEnumerable<T> Sort<T>(IEnumerable<T> items, Func<T, PangaeaThingEntry> entryGetter)
        {
            if (IsNumericalSort)
            {
                return items.OrderBy(i => GetSortNum(entryGetter(i)), Ascending).ThenBy(i => entryGetter(i).ThingDef.label, Ascending);
            }
            return items.OrderBy(i => GetSortString(entryGetter(i)), Ascending).ThenBy(i => entryGetter(i).ThingDef.label, Ascending);
        }
    }
}
