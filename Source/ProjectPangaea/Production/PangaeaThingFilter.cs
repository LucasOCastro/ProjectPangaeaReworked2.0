using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class PangaeaThingFilter : ThingFilter, IPangaeaEntryAllower
    {
        public HashSet<PangaeaThingEntry> AllowedEntries { get; private set; } = new HashSet<PangaeaThingEntry>();

        public PangaeaThingFilter(params PangaeaThingEntry[] allowedEntries)
        {
            if (allowedEntries != null)
            {
                for (int i = 0; i < allowedEntries.Length; i++)
                {
                    Allow(allowedEntries[i]);
                }
            }
        }
        public PangaeaThingFilter(IEnumerable<PangaeaThingEntry> allowedEntries)
        {
            foreach (var entry in allowedEntries)
            {
                Allow(entry);
            }
        }

        public void SyncAllowedEntries(PangaeaThingFilter other)
        {
            this.AllowedEntries = other?.AllowedEntries ?? new HashSet<PangaeaThingEntry>(AllowedEntries);
        }

        public override bool Allows(Thing t)
        {
            Log.Message("Called allow on custom filter for " + t.ToString());

            if (!base.Allows(t))
            {
                return false;
            }

            if (PangaeaDatabase.TryGetEntryFromThing(t, out PangaeaThingEntry entry, out bool shouldHaveEntry))
            {
                Log.Message(t+"is allowed = " + Allows(entry).ToString());
                return Allows(entry);
            }
            return shouldHaveEntry;
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

        public override void CopyAllowancesFrom(ThingFilter other)
        {
            base.CopyAllowancesFrom(other);
            if (other is PangaeaThingFilter ptf)
            {
                AllowedEntries.Clear();
                AllowedEntries = new HashSet<PangaeaThingEntry>(ptf.AllowedEntries);
            }
        }
    }
}
