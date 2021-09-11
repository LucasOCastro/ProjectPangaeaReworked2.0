using System;
using System.Linq;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public static class PangaeaCollectionExtension
    {
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> entries, Func<T, string> keySelector, bool ascending)
        {
            return ascending ? entries.OrderBy(keySelector) : entries.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> entries, Func<T, float> keySelector, bool ascending)
        {
            return ascending ? entries.OrderBy(keySelector) : entries.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> entries, Func<T, float> keySelector, bool ascending)
        {
            return ascending ? entries.ThenBy(keySelector) : entries.ThenByDescending(keySelector);
        }

        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> entries, Func<T, string> keySelector, bool ascending)
        {
            return ascending ? entries.ThenBy(keySelector) : entries.ThenByDescending(keySelector);
        }

        public static IOrderedEnumerable<PangaeaThingEntry> PangeaSort(this IEnumerable<PangaeaThingEntry> entries, PangaeaEntrySorter sorter)
        {
            return sorter.Sort(entries);
        }

        public static IOrderedEnumerable<T> PangeaSort<T>(this IEnumerable<T> items, Func<T, PangaeaThingEntry> entryGetter, PangaeaEntrySorter sorter)
        {
            return sorter.Sort(items, entryGetter);
        }

        public static IEnumerable<T> Yield<T>(this T t)
        {
            yield return t;
        }
    }
}
