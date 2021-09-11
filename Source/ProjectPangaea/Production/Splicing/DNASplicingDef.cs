using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    public class DNASplicingDef : Def
    {
        [System.Serializable]
        public class SplicePortionData
        {
            public ThingDef thing;
            public ThingDef dnaOwner;
            public int count;

            public Thing MakeThing()
            {
                Thing result = null;
                if (thing != null)
                {
                    result = ThingMaker.MakeThing(thing);
                }
                else if (dnaOwner != null && PangaeaDatabase.TryGetEntry(dnaOwner, out PangaeaThingEntry entry))
                {
                    result = DNAThing.MakeDNAThing(entry.DNA);
                }

                if (result != null)
                {
                    result.stackCount = count;
                }
                return result;
            }

            public IngredientCount MakeIngredient()
            {
                ThingFilter filter = null;
                if (thing != null)
                {
                    filter = new ThingFilter();
                    filter.SetAllow(thing, true);
                }
                else if (dnaOwner != null && PangaeaDatabase.TryGetEntry(dnaOwner, out PangaeaThingEntry entry))
                {
                    PangaeaThingFilter pangFilter = new PangaeaThingFilter(entry);
                    pangFilter.SetAllow(PangaeaThingDefOf.Pangaea_DNABase, true);
                    filter = pangFilter;
                }

                if (filter == null)
                {
                    return null;
                }

                IngredientCount ing = new IngredientCount() { filter = filter };
                ing.SetBaseCount(count);
                return ing;
            }

            public bool MatchesThing(Thing thing)
            {
                if (thing is DNAThing dnaThing)
                {
                    return dnaThing.DNAResource?.ParentThingDef == dnaOwner;
                }
                return thing.def == this.thing;
            }
        }

        private ThingDef parentDNAOwner = null;
        public int count;
        public List<SplicePortionData> splicePortions = new List<SplicePortionData>();

        private PangaeaThingEntry parentEntry;
        public PangaeaThingEntry ParentEntry
        {
            get
            {
                if (parentEntry == null)
                {
                    parentEntry = PangaeaDatabase.GetOrNull(parentDNAOwner);
                }
                return parentEntry;

            }
        }

        private DNA parentDNA;
        public DNA ParentDNA
        {
            get
            {
                if (parentDNA == null)
                {
                    parentDNA = ParentEntry?.DNA;
                }
                return parentDNA;
            }
        }

        public IEnumerable<IngredientCount> MakePortionIngredients()
        {
            for (int i = 0; i < splicePortions.Count; i++)
            {
                IngredientCount ing = splicePortions[i].MakeIngredient();
                if (ing != null)
                    yield return ing;
            }
        }

        public IEnumerable<Thing> MakePortionThings()
        {
            for (int i = 0; i < splicePortions.Count; i++)
            {
                Thing result = splicePortions[i].MakeThing();
                if (result != null)
                    yield return result;
            }
        }

        public IngredientCount MakeResultIngredient()
        {
            PangaeaThingFilter filter = new PangaeaThingFilter(ParentEntry);
            filter.SetAllow(PangaeaThingDefOf.Pangaea_DNABase, true);
            IngredientCount ingredient = new IngredientCount() { filter = filter };
            ingredient.SetBaseCount(count);
            return ingredient;
        }

        public DNAThing MakeResultThing()
        {
            DNAThing result = DNAThing.MakeDNAThing(ParentDNA);
            result.stackCount = count;
            return result;
        }

        public override void PostLoad()
        {
            base.PostLoad();
            if (count == 0)
            {
                for (int i = 0; i < splicePortions.Count; i++)
                {
                    count += splicePortions[i].count;
                }
            }
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string error in base.ConfigErrors())
                yield return error;

            if (parentDNAOwner == null)
            {
                yield return "has no " + nameof(parentDNAOwner);
            }

            if (splicePortions.Count == 0)
            {
                yield return "has no " + nameof(splicePortions);
            }

            for (int i = 0; i < splicePortions.Count; i++)
            {
                var splice = splicePortions[i];
                if (splice.dnaOwner != null && splice.thing != null)
                {
                    yield return $"splice portion with index {i} has both dnaOwner and thing.";
                }
                else if (splice.dnaOwner is null && splice.thing is null)
                {
                    yield return $"splice portion with index {i} has neither dnaOwner nor thing.";
                }
            }
        }
    }
}
