using Verse;

namespace ProjectPangaea
{
    public static class ExtinctUtility
    {
        public static ExtinctionStatus GetExtinctionStatus(this ThingDef def)
        {
            if (ProjectPangaeaMod.HasInitiatedDatabase)
            {
                return def.IsExtinct() ? ExtinctionStatus.Extinct : ExtinctionStatus.Extant;
            }
            return def.HasModExtension<ModExt_Extinct>() ? ExtinctionStatus.Extinct : ExtinctionStatus.Extant;
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
