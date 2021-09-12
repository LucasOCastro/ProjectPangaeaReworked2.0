using Verse;
using UnityEngine;

namespace ProjectPangaea
{
    public class DNAThing : PangaeaResourceThingBase
    {
        public DNA DNAResource { get; private set; }
        public void SetResource (DNA dna)
        {
            Resource = dna;
            DNAResource = dna;
        }

        protected override PangaeaResource GetRandomResource() => PangaeaDatabase.RandomEntry().DNA;

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
            {
                DNA dna = PangaeaDatabase.GetOrAddEntry(Resource.ParentThingDef).DNA;
                SetResource(dna);
            }
        }

        public static DNAThing MakeDNAThing(DNA dna)
        {
            DNAThing thing = (DNAThing)ThingMaker.MakeThing(PangaeaThingDefOf.Pangaea_DNABase);
            thing.SetResource(dna);
            return thing;
        }
    }
}
