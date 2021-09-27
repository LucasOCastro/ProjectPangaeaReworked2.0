using Verse;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea
{
    public static class ExtinctUtility
    {
        public static bool IsExtinct(this ThingDef def)
        {
            return GetExtinctExtension(def) != null;
        }

        public static ModExt_Extinct GetExtinctExtension(this ThingDef def)
        {
            if (ProjectPangaeaMod.HasInitiatedDatabase)
            {
                return PangaeaDatabase.GetOrNull(def)?.ExtinctExtension;
            }
            return def.GetModExtension<ModExt_Extinct>();
        }

        public static ExtinctionStatus GetExtinctionStatus(this ThingDef def)
        {
            return def.IsExtinct() ? ExtinctionStatus.Extinct : ExtinctionStatus.Extant;
        }

        public static bool CanSpawn(this ThingDef def)
        {
            if (def.race != null && def.IsExtinct())
            {
                return PangaeaSettings.SpawnExtinctAnimals;
            }
            if (def.plant != null && def.IsExtinct())
            {
                return PangaeaSettings.SpawnExtinctPlants;
            }
            return true;
        }
    }
}
