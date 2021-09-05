using RimWorld;
using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class PangaeaResourceBill: Bill_Production, IPangaeaEntryAllower
    {
        public IReadOnlyList<PangaeaThingEntry> AllPossibleEntries { get; }

        public HashSet<PangaeaThingEntry> AllowedEntries { get; } = new HashSet<PangaeaThingEntry>();

        public PangaeaBillCounter Counter { get; }

        public PangaeaResourceBill(RecipeDef recipeDef, List<PangaeaThingEntry> possibleEntries, PangaeaBillCounter counter) : base(recipeDef)
        {
            AllPossibleEntries = possibleEntries.AsReadOnly();
            AllowedEntries = new HashSet<PangaeaThingEntry>(possibleEntries);
            Counter = counter;
        }

        public bool Allows(PangaeaThingEntry entry) => AllowedEntries.Contains(entry);

        public void Toggle(PangaeaThingEntry entry)
        {
            if (AllowedEntries.Contains(entry))
            {
                Disallow(entry);
            }
            else
            {
                Allow(entry);
            }
        }

        public void Allow(PangaeaThingEntry entry) => AllowedEntries.Add(entry);
        public void Disallow(PangaeaThingEntry entry) => AllowedEntries.Remove(entry);
    }
}
