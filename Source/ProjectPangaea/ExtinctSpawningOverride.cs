using HarmonyLib;
using RimWorld;
using Verse;

namespace ProjectPangaea
{
    [HarmonyPatch(typeof(BiomeDef))]
    public static class ExtinctSpawningOverride
    {
        [HarmonyPatch("CommonalityOfAnimal")]
        [HarmonyPostfix]
        public static void AnimalCommonalityPostfix(ref float __result, PawnKindDef animalDef)
        {
            if (!animalDef.race.CanSpawn())
            {
                __result = 0f;
            }
        }

        [HarmonyPatch("CommonalityOfPlant")]
        [HarmonyPostfix]
        public static void PlantCommonalityPostfix(ref float __result, ThingDef plantDef)
        {
            if (!plantDef.CanSpawn())
            {
                __result = 0f;
            }
        }
    }
}
