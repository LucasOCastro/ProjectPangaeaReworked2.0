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
            public float portion;
        }

        private ThingDef parentDNAOwner = null;
        public List<SplicePortionData> splicePortions = new List<SplicePortionData>();

        private DNA parentDNA;
        public DNA ParentDNA
        {
            get
            {
                if (parentDNA == null)
                {
                    parentDNA = PangaeaDatabase.GetOrNull(parentDNAOwner)?.DNA;
                }
                return parentDNA;
            }
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string error in base.ConfigErrors())
                yield return error;

            if (parentDNAOwner == null)
            {
                yield return $"{this.defName} has no parentThingDef!";
            }

            float sum = 0;
            for (int i = 0; i < splicePortions.Count; i++)
            {
                var splice = splicePortions[i];
                if (splice.dnaOwner != null && splice.thing != null)
                {
                    yield return $"Splice portion in {this.defName} with index {i} has both dnaOwner and thing.";
                }
                sum += splice.portion;
            }

            if (sum > 1)
                yield return $"The splice portions of {this.defName} sum to a number higher than 1 ({sum}).";
        }
    }
}
