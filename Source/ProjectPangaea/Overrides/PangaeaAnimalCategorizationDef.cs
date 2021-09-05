using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Overrides
{
    public class PangaeaAnimalCategorizationDef : Def
    {
        [System.Serializable]
        public class GroupEntry
        {
            public AnimalType animalType;
            public List<ThingDef> animals = new List<ThingDef>();
        }

        public List<GroupEntry> groups = new List<GroupEntry>();

        public bool ContainsDef(ThingDef def, out GroupEntry entry)
        {
            foreach (var group in groups)
            {
                foreach (ThingDef thingDef in group.animals)
                {
                    if (thingDef == def)
                    {
                        entry = group;
                        return true;
                    }
                }
            }
            entry = null;
            return false;
        }
    }
}
