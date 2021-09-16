using RimWorld;
using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class PangaeaResourceBill: Bill_Production
    {
        public PangaeaThingFilter ResourceFilter { get; }

        public IReadOnlyList<PangaeaThingEntry> AllPossibleEntries { get; }

        public PangaeaBillCounter Counter { get; }

        public PangaeaResourceBill(RecipeDef recipeDef, List<PangaeaThingEntry> possibleEntries, PangaeaBillCounter counter) : base(recipeDef)
        {
            AllPossibleEntries = possibleEntries.AsReadOnly();
            Counter = counter;
            ResourceFilter = new PangaeaThingFilter(possibleEntries);
            ResourceFilter.CopyAllowancesFrom(ingredientFilter);
            ingredientFilter = ResourceFilter;
        }
    }
}
