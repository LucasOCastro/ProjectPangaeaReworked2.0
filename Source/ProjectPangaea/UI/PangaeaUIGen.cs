using UnityEngine;
using Verse;
using RimWorld;

namespace ProjectPangaea
{
    [StaticConstructorOnStartup]
    public static class PangaeaUIGen
    {
        public static Texture2D ArrowUp = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowUp");
        public static Texture2D ArrowDown = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowDown");

        /// <summary>
        /// GenUI.DrawTextureWithMaterial halves GUI.Color when calling Graphics.DrawTexture.
        /// This works around it.
        /// </summary>
        public static void DrawTextureWithMaterialAccurateColor(Rect rect, Texture tex, Material mat, Color color, Rect texCoords = default)
        {
            Color ogColor = GUI.color;

            GUI.color = new Color(color.r * 2, color.g * 2, color.b * 2, color.a * 2);
            GenUI.DrawTextureWithMaterial(rect, tex, mat, texCoords);

            GUI.color = ogColor;
        }

        public static void DrawTexWithMaterialClipped(Rect rect, Rect clipRect, Texture tex, Material mat) => DrawTexWithMaterialClipped(rect, clipRect, tex, mat, Color.white);
        public static void DrawTexWithMaterialClipped(Rect rect, Rect clipRect, Texture tex, Material mat, Color color)
        {
            Rect clipped = rect.Intersection(clipRect);
            Vector2 sizePct = new Vector2(clipped.width / rect.width, clipped.height / rect.height);

            Vector2 texCoordPos = (rect.position.y > clipRect.center.y) ? Vector2.one - sizePct : Vector2.zero;
            Rect texCoords = new Rect(texCoordPos, sizePct);

            DrawTextureWithMaterialAccurateColor(clipped, tex, mat, color, texCoords);
        }

        public static Texture2D GetIcon(this ThingDef thingDef)
        {
            if (thingDef == null)
            {
                return null;
            }
            if (thingDef.IsCorpse)
            {
                Log.Message("is corpse " + thingDef.defName);
                ThingDef corpseOwner = CorpseOwner(thingDef);
                return GetIcon(corpseOwner);
            }
            return GetUIIconForDefaultStuff(thingDef);
        }

        /*private void ResolveIcon()
        {
            icon = resource?.Value?.Icon;
            if (!icon.NullOrBad())
            {
                return;
            }
            icon = thing.GetIcon();
            if (!icon.NullOrBad())
            {
                return;
            }
            icon = ThingFilter.Icon ?? BaseContent.BadTex;
        }*/

        public static Texture2D GetUIIconForDefaultStuff(this ThingDef thingDef)
        {
            if (thingDef == null)
            {
                return null;
            }
            //return thingDef?.GetUIIconForStuff(RimWorld.GenStuff.DefaultStuffFor(thingDef)) ?? BaseContent.BadTex;
            ThingDef stuff = GenStuff.DefaultStuffFor(thingDef);
            Texture2D icon = Widgets.GetIconFor(thingDef, stuff);
            if (icon.NullOrBad())
            {
                icon = thingDef.graphic.MatSingle.mainTexture as Texture2D;
            }
            return icon;
        }

        private static ThingDef CorpseOwner(ThingDef corpseDef)
        {
            /*string defName = corpseDef.defName.Replace("Corpse_", "");
            return DefDatabase<ThingDef>.GetNamedSilentFail(defName);*/
            return corpseDef.ingestible.sourceDef;
        }
    }
}
