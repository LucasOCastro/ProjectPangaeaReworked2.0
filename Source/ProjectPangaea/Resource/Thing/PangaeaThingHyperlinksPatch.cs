using System.Collections.Generic;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;

namespace ProjectPangaea
{
    [HarmonyPatch(typeof(StatsReportUtility), "DescriptionEntry", typeof(Thing))]
    public static class PangaeaThingHyperlinksPatch
    {
        private static FieldInfo hyperlinksField = AccessTools.Field(typeof(StatDrawEntry), "hyperlinks");
        public static void Postfix(ref StatDrawEntry __result, Thing thing)
        {
            if (thing is PangaeaThing pt)
            {
                Log.Message("POstfifxxx");
                hyperlinksField.SetValue(__result, GetHyperlinksFor(pt));
            }
        }

        private static IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinksFor(PangaeaThing thing)
        {
            if (thing == null) yield break;

            var descriptionHyperlinks = thing.def.descriptionHyperlinks;
            if (descriptionHyperlinks != null)
            {
                foreach (var link in Dialog_InfoCard.DefsToHyperlinks(descriptionHyperlinks))
                {
                    yield return link;
                }
            }
            
            if (thing.Resource != null)
            {
                yield return new Dialog_InfoCard.Hyperlink(thing.Resource.ParentThingDef);
            }
        }
    }
}
