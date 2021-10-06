using System.Collections.Generic;
using Verse;

namespace ProjectPangaea
{
    public class ModExt_RandomFossilDrop : DefModExtension
    {
        //TODO Change this stuff to ResourceReference instead, get rid of ResourceTypeDefOf
        public class RandomOreDropEntry
        {
            public ThingDef fossilParent;
            public float commonality;
            public IntRange yield = new IntRange(-1,-1);

            private PangaeaResource fossil;
            public PangaeaResource Fossil
            {
                get
                {
                    if (fossil == null)
                    {
                        fossil = PangaeaDatabase.GetOrNull(fossilParent)?.GetResource(ResourceTypeDefOf.Pangaea_Fossil);
                    }

                    if (fossil == null)
                    {
                        throw new System.Exception(this + " with null fossil!");
                    }
                    return fossil;
                }
            }

            public override string ToString() => base.ToString() + $"[{fossilParent}]({commonality})";
            
        }

        public IntRange baseYield = new IntRange(1,1);
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
            Thing result = dropEntry.Fossil.MakeThing();
            IntRange yieldRange = YieldForEntry(dropEntry);
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
