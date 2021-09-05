using Verse;
using HarmonyLib;
using System;

namespace ProjectPangaea.PangaeaUI
{
    //DoGUI returns true on click so the parent FloatMenu can close, so this makes it return null if shouldnt close.
    [HarmonyPatch(typeof(FloatMenuOption), "DoGUI")]
    public static class FloatMenuClickCloseOverride
    {
        public static void Postfix(ref bool __result, FloatMenu floatMenu)
        {
            Type type = floatMenu.GetType();
            if (!type.IsGenericType)
            {
                return;
            }

            if (type.GetGenericTypeDefinition() == typeof(FloatMenuWithCheckmarks<>))
            {
                __result = false;
            }
        }    
    }
}
