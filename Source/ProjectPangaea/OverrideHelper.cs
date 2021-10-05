using Verse;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public static class OverrideHelper
    {
        private static List<EntrySettings> entrySettingsList = new List<EntrySettings>();

        public static void Init()
        {
            foreach (var entryDef in DefDatabase<PangaeaEntryDef>.AllDefs)
            {
                EntrySettings settings = entryDef.settings;
                entrySettingsList.Add(settings);
            }
        }

        public static void DoOverrides()
        {
            List<EntrySettings> remainingOverrides = new List<EntrySettings>(entrySettingsList);

            foreach (var entrySetting in entrySettingsList)
            {
                var directFilter = entrySetting.applyFilter.directDefFilter;
                if (directFilter != null)
                {
                    foreach (var def in directFilter)
                    {
                        var entry = PangaeaDatabase.GetOrAddEntry(def);
                        entrySetting.TryOverride(entry);
                    }
                    remainingOverrides.Remove(entrySetting);
                }
            }

            foreach (var entry in PangaeaDatabase.AllEntries)
            {
                foreach (var entrySetting in remainingOverrides)
                {
                    entrySetting.TryOverride(entry);
                }
            }
        }
    }
}
