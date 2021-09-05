using System.Collections.Generic;
using UnityEngine;
using ProjectPangaea.PangaeaUI;
using Verse;

namespace ProjectPangaea
{
    public class PangaeaMassToggler
    {
        private IPangaeaEntryAllower allower;
        private IEnumerable<PangaeaThingEntry> entries;

        public string buttonSuffixLabel = "";

        public PangaeaMassToggler(IPangaeaEntryAllower allower, IEnumerable<PangaeaThingEntry> entries)
        {
            this.allower = allower;
            this.entries = entries;
        }

        public void ToggleAll()
        {
            foreach (var entry in entries) allower.Toggle(entry);
        }

        public void AllowAll()
        {
            foreach (var entry in entries) allower.Allow(entry);
        }

        public void DisallowAll()
        {
            foreach (var entry in entries) allower.Disallow(entry);
        }

        public Rect Draw(Vector2 position, float width)
        {
            float lineHeight = PangaeaUIGen.lineHeight;
            float lineSpacing = PangaeaUIGen.lineSpacing;

            Rect toggleButtonRect = new Rect(position.x, position.y, width, lineHeight);
            string toggleLabel = "Pangaea_Toggle".Translate().CapitalizeFirst() + buttonSuffixLabel;
            if (Widgets.ButtonText(toggleButtonRect, toggleLabel))
            {
                ToggleAll();
            }

            Rect allowButtonRect = new Rect(toggleButtonRect)
            {
                y = toggleButtonRect.yMax + lineSpacing,
                width = toggleButtonRect.width / 2
            };
            string allowLabel = "Pangaea_Allow".Translate().CapitalizeFirst() + buttonSuffixLabel;
            if (Widgets.ButtonText(allowButtonRect, allowLabel))
            {
                AllowAll();
            }


            Rect disallowAllButtonRect = new Rect(allowButtonRect) { x = allowButtonRect.xMax };
            string disallowLabel = "Pangaea_Disallow".Translate().CapitalizeFirst() + buttonSuffixLabel;
            if (Widgets.ButtonText(disallowAllButtonRect, disallowLabel))
            {
                DisallowAll();
            }

            return new Rect() { position = position, max = disallowAllButtonRect.max };
        }
    }
}
