using RimWorld;
using UnityEngine;
using Verse;
using System.Linq;
using System.Collections.Generic;
using ProjectPangaea.Production;

namespace ProjectPangaea.PangaeaUI
{
    public class PangaeaBillConfigWindow : Dialog_BillConfig
    {
        private const float baseBlockSize = 130;
        private const float optionBarWidthMultiplier = 0.3f;
        private const float selectedAnimalViewHeight = 0.3f;
        private const float optionsTabPadding = 5, infoBlockPadding = 2, previewPadding = 4;
        private const float blockCornerDetailSize = Widgets.CheckboxSize;

        public PangaeaResourceBill ExtractionBill { get; }
        private PangaeaBillWindowSettings settings;
        private PangaeaMassToggler globalToggler, visibleToggler;
        private List<InfoBlock> allPossibleInfoBlocks;

        private PangaeaThingEntry currentSelectedEntry;

        public PangaeaBillConfigWindow(PangaeaResourceBill bill, IntVec3 billGiverPos) : base(bill, billGiverPos)
        {
            ExtractionBill = bill;
            allPossibleInfoBlocks = GetAllPossibleInfoBlocks();
            doCloseButton = false;
            settings = new PangaeaBillWindowSettings(this);
            globalToggler = new PangaeaMassToggler(bill, ExtractionBill.AllPossibleEntries);
            visibleToggler = new PangaeaMassToggler(bill, GetVisibleInfoBlocks().Select(b => b.PangaeaEntry));

            globalToggler.buttonSuffixLabel = " " + "Pangaea_All".Translate();
            visibleToggler.buttonSuffixLabel = " " + "Pangaea_Visible".Translate();
        }

        private List<InfoBlock> GetAllPossibleInfoBlocks()
        {
            List<InfoBlock> blocks = new List<InfoBlock>();
            foreach (var entry in ExtractionBill.AllPossibleEntries)
            {
                blocks.Add(new InfoBlock(entry, ExtractionBill));
            }
            return blocks;
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (allPossibleInfoBlocks == null)
            {
                return;
            }

            float optionBarWidth = optionBarWidthMultiplier * inRect.width;
            float selectedViewHeight = selectedAnimalViewHeight * inRect.height;

            Rect selectedViewRect = new Rect(inRect) { width = optionBarWidth, yMin = inRect.yMax - selectedViewHeight };
            DrawSelectedView(selectedViewRect);

            Rect optionsRect = new Rect(inRect) { width = optionBarWidth, yMax = selectedViewRect.yMin };
            DrawOptions(optionsRect);

            Rect entriesRect = new Rect(inRect) { xMin = optionsRect.xMax };
            DrawEntries(entriesRect);
        }

        private void DrawOptions(Rect rect)
        {
            Widgets.DrawMenuSection(rect);

            Rect paddedRect = rect.ContractedBy(optionsTabPadding);
            Rect settingsRect = settings.Draw(paddedRect.position, paddedRect.width);

            float lineSpacing = PangaeaUIGen.lineSpacing;

            Vector2 linePos = new Vector2(rect.x, settingsRect.yMax + (3 * lineSpacing));
            Widgets.DrawLineHorizontal(linePos.x, linePos.y, rect.width);

            Vector2 globalTogglerPos = new Vector2(paddedRect.x, linePos.y + (3 * lineSpacing));
            Rect globalTogglerRect = globalToggler.Draw(globalTogglerPos, paddedRect.width);
            Vector2 visibleTogglerPos = new Vector2(paddedRect.x, globalTogglerRect.yMax + (2 * lineSpacing));
            Rect visibleTogglerRect = visibleToggler.Draw(visibleTogglerPos, paddedRect.width);
        }

        private IEnumerable<InfoBlock> GetVisibleInfoBlocks()
        {
            foreach (var block in allPossibleInfoBlocks)
            {
                if (settings.filter.Allows(block.PangaeaEntry))
                {
                    yield return block;
                }
            }
        }

        private void DrawEntries(Rect rect)
        {
            Widgets.DrawBox(rect);

            int horizontalBlockCount = Mathf.FloorToInt(rect.width / baseBlockSize);
            float distanceToBorder = rect.width - (horizontalBlockCount * baseBlockSize);

            int resourcesRequired = ExtractionBill.Counter.ResourceRequirement;
            int i = 0;
            foreach (var block in GetVisibleInfoBlocks().PangeaSort(b => b.PangaeaEntry, settings.sorter))
            {
                float blockSize = baseBlockSize;
                if (allPossibleInfoBlocks.Count >= horizontalBlockCount)
                {
                    blockSize += distanceToBorder / horizontalBlockCount;
                }
                float x = (i % horizontalBlockCount) * blockSize;
                float y = Mathf.FloorToInt(i / horizontalBlockCount) * blockSize;

                Rect blockRect = new Rect(rect.x + x, rect.y + y, blockSize, blockSize);
                if (i % 2 == 0)
                {
                    Widgets.DrawAltRect(blockRect);
                }
                Rect paddedBlockRect = blockRect.ContractedBy(infoBlockPadding);
                block.Draw(paddedBlockRect, blockCornerDetailSize, true);

                if (Widgets.ButtonInvisible(blockRect, doMouseoverSound: false))
                {
                    OnClickEntry(block);
                }

                i++;
            }
        }

        private void OnClickEntry(InfoBlock block)
        {
            if (block.InfoBlockParent != currentSelectedEntry?.ThingDef)
            {
                currentSelectedEntry = PangaeaDatabase.GetOrNull(block.InfoBlockParent);
            }
        }

        private void DrawSelectedView(Rect rect)
        {
            Widgets.DrawBox(rect);
            Widgets.DrawWindowBackground(rect);

            if (currentSelectedEntry == null)
            {
                return;
            }

            Rect contentRect = rect.ContractedBy(previewPadding);

            Widgets.ThingIcon(contentRect, currentSelectedEntry.ThingDef, scale: 0.95f);

            Verse.Text.Anchor = TextAnchor.UpperCenter;
            Verse.Text.Font = GameFont.Medium;
            Widgets.Label(rect, currentSelectedEntry.ThingDef.LabelCap);

            Widgets.InfoCardButton(contentRect.x, contentRect.y, currentSelectedEntry.ThingDef);

            Verse.Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}
