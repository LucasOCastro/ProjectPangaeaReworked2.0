using Verse;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectPangaea.Production.Splicing
{
    public class DNASplicingBill : PangaeaResourceBill
    {
        public DNA Result { get; }
        public List<ThingDefCount> ThingParts { get; } = new List<ThingDefCount>();
        public List<DNA> DNAParts { get; } = new List<DNA>();
        private DNASplicingDef spliceDef;

        public DNASplicingBill(DNASplicingDef spliceDef) : base (PangaeaRecipeDefOf.Pangaea_SpliceDNA, )
        {
            ThingRequest a = new ThingRequest()
            {
                singleDef = null
            };
            a.
            Result = spliceDef.ParentDNA;
            this.spliceDef = spliceDef;
            int stackCount = (int)recipe.ingredients.Find(i => i.filter.Allows(PangaeaThingDefOf.Pangaea_DNABase)).GetBaseCount();
            foreach (var portion in spliceDef.splicePortions)
            {
                int count = Mathf.CeilToInt(portion.portion * stackCount);
                if (portion.thing != null)
                {
                    ThingParts.Add(new ThingDefCount(portion.thing, count));
                }
                else if (portion.dnaOwner != null && PangaeaDatabase.TryGetEntry(portion.dnaOwner, out PangaeaThingEntry entry))
                {
                    DNAParts.Add(entry.DNA);
                }
            }
        }
    }
}
