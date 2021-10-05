using System.Collections.Generic;
using Verse;

namespace ProjectPangaea
{
    public class ResourceTypeDef : Def
    {
        public ThingDef thingDef;

        //TODO this could go away with one of those entrydefs
        public bool addToExtinct;

        public bool addToAny;

        public PangaeaThing MakeThing() => ThingMaker.MakeThing(thingDef) as PangaeaThing;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string error in base.ConfigErrors())
                yield return error;

            if (thingDef == null)
            {
                yield return "null " + nameof(thingDef) + "in " + this;
                yield break;
            }

            var compProp = thingDef.GetCompProperties<CompProperties_PangaeaResourceHolder>();
            if (compProp == null || !compProp.IsOfType(this))
                yield return thingDef + " can't support resource of def " + this;
        }
    }
}
