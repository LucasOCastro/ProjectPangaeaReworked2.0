using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    public static class DNASplicingWorker
    {
        private static Dictionary<DNA, DNASplicingDef> dict = new Dictionary<DNA, DNASplicingDef>();

        public static IEnumerable<DNASplicingDef> AllSpliceDefs => dict.Values;

        private static bool TryGetDef(DNA dna, out DNASplicingDef splicingDef)
        {
            if (dna == null)
            {
                splicingDef = null;
                return false;
            }
            return dict.TryGetValue(dna, out splicingDef);
        }

        public static bool IsSpliced(DNA dna) => dict.ContainsKey(dna);

        public static void Init()
        {
            foreach (DNASplicingDef def in DefDatabase<DNASplicingDef>.AllDefs)
            {
                dict.Add(def.ParentDNA, def);
            }
        }
    }
}
