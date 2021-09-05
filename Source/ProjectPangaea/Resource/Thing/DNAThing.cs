using Verse;

namespace ProjectPangaea
{
    public class DNAThing : PangaeaResourceThingBase
    {
        public void SetResource (DNA dna)
        {
            resource = dna;
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
