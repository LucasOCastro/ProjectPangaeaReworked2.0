using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using FieldInfo = System.Reflection.FieldInfo;

namespace ProjectPangaea.PangaeaUI
{
    public class FloatMenuWithCheckmarks<T> : FloatMenu
    {
        private Dictionary<T, bool> dictionary;
        private Dictionary<T, FloatMenuOption> optionDict;

        public FloatMenuWithCheckmarks(Dictionary<T, bool> dictionary) 
            : base(new List<FloatMenuOption>() { new FloatMenuOption("", null, null, Color.white) })
        {
            this.dictionary = dictionary;
            optionDict = new Dictionary<T, FloatMenuOption>(dictionary.Count);

            options.Clear();
            foreach (T t in dictionary.Keys)
            {
                void onClickOption(){
                    dictionary[t] = !dictionary[t];
                    UpdateCheckmark(t);
                }

                string label = t.ToString();
                FloatMenuOption option = new FloatMenuOption(label, onClickOption, Widgets.CheckboxOnTex, Color.white);

                options.Add(option);
                optionDict.Add(t, option);

                UpdateCheckmark(t);
            }

            foreach (FloatMenuOption option in options)
            {
                option.SetSizeMode(SizeMode);
            }
        }

        private void UpdateCheckmark(T t)
        {
            FloatMenuOption option = optionDict[t];
            bool value = dictionary[t];
            SetCheckmark(option, value);
        }

        private static FieldInfo optionIconField = AccessTools.Field(typeof(FloatMenuOption), "itemIcon");
        private void SetCheckmark(FloatMenuOption option, bool value)
        {
            Texture2D tex = value ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
            optionIconField.SetValue(option, tex);
        }
    }
}
