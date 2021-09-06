using Verse;
using UnityEngine;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    public static class DNASplicingWorker
    {
        private static Dictionary<DNA, DNASplicingDef> dict = new Dictionary<DNA, DNASplicingDef>();

        private static List<Thing> spliceResults = new List<Thing>();
        public static IEnumerable<Thing> GetSpliceResults(DNAThing dnaThing)
        {
            spliceResults.Clear();
            if (!(dnaThing.Resource is DNA dna) || !dict.TryGetValue(dna, out DNASplicingDef splicingDef))
            { 
                return spliceResults;
            }

            int totalCount = 0;
            foreach (var splice in splicingDef.splicePortions)
            {
                Thing result = null;
                if (splice.thing != null)
                {
                    result = ThingMaker.MakeThing(splice.thing);
                }
                else if (splice.dnaOwner != null)
                {
                    DNA spliceDNA = PangaeaDatabase.GetOrNull(splice.dnaOwner)?.DNA;
                    result = (spliceDNA != null) ? DNAThing.MakeDNAThing(spliceDNA) : null;
                }
                if (result == null) continue;

                int stackCount = Mathf.CeilToInt(splice.portion * dnaThing.stackCount);
                result.stackCount = stackCount;
                totalCount += stackCount;

                spliceResults.Add(result);
            }

            EqualizePortions(dnaThing.stackCount, totalCount);

            return spliceResults;
        }

        public static bool IsSpliced(DNA dna) => dict.ContainsKey(dna);

        private static void EqualizePortions(int stackCount, int currentCount)
        {
            if (currentCount > stackCount)
            {
                int i = spliceResults.Count - 1;
                while (currentCount > stackCount && i >= 0)
                {
                    spliceResults[i].stackCount--;
                    currentCount--;
                    i--;
                    if (i < 0)
                    {
                        i = spliceResults.Count - 1;
                    }
                }
                return;
            }

            if (currentCount < stackCount)
            {
                int i = 0;
                while (currentCount < stackCount && i < spliceResults.Count)
                {
                    spliceResults[i].stackCount++;
                    currentCount++;
                    i++;
                    if (i >= spliceResults.Count)
                    {
                        i = 0;
                    }
                }
            }
        }

        public static void Init()
        {
            foreach (DNASplicingDef def in DefDatabase<DNASplicingDef>.AllDefs)
            {
                dict.Add(def.ParentDNA, def);
            }
        }
    }
}
