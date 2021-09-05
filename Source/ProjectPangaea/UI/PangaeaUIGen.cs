using UnityEngine;
using Verse;
using System.Collections.Generic;
using System;

namespace ProjectPangaea.PangaeaUI
{
    [StaticConstructorOnStartup]
    public static class PangaeaUIGen
    {
        public static Texture2D ArrowUp = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowUp");
        public static Texture2D ArrowDown = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowDown");

        public const float lineHeight = 24;
        public const float lineSpacing = 3;

        public const float checkmarkSpacing = 0.3f;
        public static bool ButtonWithPrefixLabel(Rect rect, string prefixLabel, string buttonLabel, float spacing = 0, float minButtonWidth = 0)
        {
            float prefixWidth = Verse.Text.CalcSize(prefixLabel).x;

            if (rect.width - prefixWidth < minButtonWidth)
            {
                prefixWidth = rect.width - minButtonWidth;
            }

            Rect prefixRect = new Rect(rect) { width = prefixWidth };
            Widgets.Label(prefixRect, prefixLabel);

            Rect buttonRect = new Rect(rect) { xMin = prefixRect.xMax + spacing };
            return Widgets.ButtonText(buttonRect, buttonLabel);
        }

        public static List<FloatMenuOption> GenEnumFloatMenuOptions<T>(Action<T> action) where T: Enum
        {
            T[] enumVals = (T[])Enum.GetValues(typeof(T));
            List<FloatMenuOption> options = new List<FloatMenuOption>(enumVals.Length);            
            for (int i = 0; i < enumVals.Length; i++)
            {
                T t = enumVals[i];
                FloatMenuOption option = new FloatMenuOption(t.ToString(), () => action(t));
                options.Add(option);
            }

            return options;
        }

        public static void CheckMarkLabel(Rect rect, ref bool value, string label, float spacing = checkmarkSpacing)
        {
            float checkSize = rect.height;
            Widgets.Checkbox(rect.position, ref value, size: checkSize);

            Rect labelRect = new Rect(rect) { xMin = rect.x + checkSize + spacing };
            Widgets.Label(labelRect, "Pangaea_ShowUnavailableIngredients".Translate());
        }

        public static bool CheckMarkButton(Rect rect, bool isChecked)
        {
            Texture2D checkTexture = isChecked ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
            return Widgets.ButtonImageFitted(rect, checkTexture);
        }


        public static Vector2 CalcRequiredAmountLabelSize(int count, int required)
        {
            return Verse.Text.CalcSize($"{count}/{required}");
        }
        public static void RequiredAmountLabel(Rect rect, int count, int required)
        {
            GUI.color = (count < required) ? Color.red : Color.green;
            Widgets.Label(rect, $"{count}/{required}");
            GUI.color = Color.white;
        }
    }
}
