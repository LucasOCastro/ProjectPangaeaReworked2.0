using Verse;

namespace ProjectPangaea
{
    public class FossilThing : PangaeaResourceThingBase
    {
        public void SetResource(Fossil fossil)
        {
            resource = fossil;
        }

        protected override PangaeaResource GetRandomResource() => PangaeaDatabase.RandomExtinctEntry().Fossil;

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
            {
                Fossil fossil = PangaeaDatabase.GetOrAddEntry(Resource.ParentThingDef).Fossil;
                SetResource(fossil);
            }
        }

        public static FossilThing MakeFossilThing(Fossil fossil)
        {
            FossilThing thing = (FossilThing)ThingMaker.MakeThing(PangaeaThingDefOf.Pangaea_FossilBase);
            thing.SetResource(fossil);
            return thing;
        }
    }
}
