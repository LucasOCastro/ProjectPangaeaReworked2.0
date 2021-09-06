using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    public static class DNASplicingWorker
    {
        private static Dictionary<DNA, DNASplicingDef> dict = new Dictionary<DNA, DNASplicingDef>();

        public static bool IsSpliced(DNA dna) => dict.ContainsKey(dna);

        private static Bill GenSplicingBill(DNASplicingDef spliceDef)
        {
            BillUtility.MakeNewBill();
        }

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
                Thing result = splice.MakeThing(dnaThing.stackCount);
                if (result == null) continue;
                totalCount += result.stackCount;
                spliceResults.Add(result);
            }
            EqualizePortions(dnaThing.stackCount, totalCount);

            return spliceResults;
        }        

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
