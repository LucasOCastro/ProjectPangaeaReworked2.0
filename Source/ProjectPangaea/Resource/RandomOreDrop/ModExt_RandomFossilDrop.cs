using Verse;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public class ModExt_RandomFossilDrop : DefModExtension
    {
        public class RandomOreDropEntry
        {
            public ThingDef fossilParent;
            public float commonality;
            public IntRange yield = new IntRange(-1,-1);
        }

        public IntRange baseYield;
        public List<RandomOreDropEntry> fossils;

        private IntRange YieldForEntry(RandomOreDropEntry entry) 
        {
            if (entry.yield == new IntRange(-1, -1))
            {
                return baseYield;
            }
            return entry.yield;
        }

        public Thing GetRandomDrop()
        {
            if (fossils.NullOrEmpty())
            {
                return null;
            }

            RandomOreDropEntry dropEntry = fossils.RandomElementByWeight(e => e.commonality);
            PangaeaThingEntry databaseEntry = PangaeaDatabase.GetOrNull(dropEntry.fossilParent);
            if (databaseEntry == null || databaseEntry.Fossil == null)
            {
                return null;
            }

            IntRange yieldRange = YieldForEntry(dropEntry);
            Thing result = PangaeaThing.MakePangaeaThing(databaseEntry.Fossil);
            result.stackCount = yieldRange.RandomInRange;
            return result;
        }

        /*private RandomOreDropEntry GetRandomDropEntry()
        {
            if (fossils.NullOrEmpty())
            {
                return null;
            }
            return fossils.RandomElementByWeight(e => e.commonality);
        }

        public Thing GetRandomDrop()
        {
            RandomOreDropEntry dropEntry = GetRandomDropEntry();
            PangaeaThingEntry databaseEntry = PangaeaDatabase.GetOrNull(dropEntry?.fossilParent);
            Fossil fossil = databaseEntry?.Fossil;
            Thing result = FossilThing.MakeFossilThing(fossil);
            if (fossil != null)
            {
                result.stackCount = YieldForEntry(dropEntry).RandomInRange;
            }
            return result;
        }*/
    }
}
