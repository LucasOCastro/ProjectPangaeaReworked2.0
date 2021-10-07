﻿using Verse;
using UnityEngine;
using RimWorld;
using System.Text;

namespace ProjectPangaea
{
    public class PangaeaSettings : ModSettings
    {
        public bool assignRandomOwnerToNullResource = false;
        //TODO more complex configurable spawning settings
        public bool spawnExtinctAnimals = false;

        private const float embryoVatTimeDefault = 1f;
        private const float embryoVatTimeMin = 0.5f;
        private const float embryoVatTimeMax = 3f;
        public float embryoVatTimeMultiplier = embryoVatTimeDefault;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref assignRandomOwnerToNullResource, nameof(assignRandomOwnerToNullResource));
            Scribe_Values.Look(ref spawnExtinctAnimals, nameof(spawnExtinctAnimals));
            Scribe_Values.Look(ref embryoVatTimeMultiplier, nameof(embryoVatTimeMultiplier), defaultValue: embryoVatTimeDefault);
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.CheckboxLabeled("PangaeaSetting_RandomResourceToNullName".Translate(),
                ref assignRandomOwnerToNullResource,
                tooltip: "PangaeaSetting_RandomResourceToNullDescription".Translate()
                );
            listing.Gap();

            listing.CheckboxLabeled("PangaeaSetting_WildAnimalsName".Translate(),
                ref spawnExtinctAnimals,
                tooltip: "PangaeaSetting_WildAnimalsDescription".Translate()
                );
            listing.Gap();

            DrawEmbryoTimeSetting(listing);
            listing.End();
        }

        private static ThingDef exampleThing;
        private void DrawEmbryoTimeSetting(Listing_Standard listing)
        {
            string label = "PangaeaSetting_EmbryoGrowTimeName".Translate();
            Rect rect = listing.GetRect(Text.LineHeight);

            Rect labelRect = new Rect(rect) { width = Text.CalcSize(label).x };
            Widgets.Label(labelRect, label);

            string reset = "Reset".Translate().ToString().ToUpper();
            float resetWidth = Text.CalcSize(reset).x * 1.2f;
            Rect resetRect = new Rect(rect) { xMin = rect.xMax - resetWidth };
            if (Widgets.ButtonText(resetRect, reset))
            {
                embryoVatTimeMultiplier = embryoVatTimeDefault;
            }

            Rect sliderRect = new Rect(rect) { xMin = labelRect.xMax, xMax = resetRect.xMin };
            embryoVatTimeMultiplier = Widgets.HorizontalSlider(sliderRect,
                embryoVatTimeMultiplier, embryoVatTimeMin, embryoVatTimeMax,
                label: embryoVatTimeMultiplier.ToString(),
                middleAlignment: true
                ).ToShortDecimal();

            if (exampleThing == null)
            {
                exampleThing = PangaeaDatabase.RandomEntry.ThingDef;
            }
            StringBuilder description = new StringBuilder();
            description.AppendLine("PangaeaSetting_EmbryoGrowTimeDescription".Translate());
            description.AppendLine("PangaeaSetting_EmbryoGrowTimeExample".Translate(exampleThing.label, exampleThing.GestationPeriod(), exampleThing.VatGestationPeriod()));
            TooltipHandler.TipRegion(new Rect(rect) { xMax = resetRect.xMin }, description.ToString());
        }
    }
}
