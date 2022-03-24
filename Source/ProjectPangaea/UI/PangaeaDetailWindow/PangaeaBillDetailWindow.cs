using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using ProjectPangaea.Production;

namespace ProjectPangaea
{
    public class PangaeaBillDetailWindow : Window
    {
        private PangaeaBill bill;
        public PangaeaBillDetailWindow(PangaeaBill bill, IntVec3 billGiverPosition)
        {
            this.bill = bill;
            forcePause = true;
            doCloseX = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
        }

        public override Vector2 InitialSize => new Vector2(800f, 634f);

        private const float leftPartPct = .25f;
        private const float headerPct = .25f, headerTitlePct = .15f;

        public override void DoWindowContents(Rect inRect)
        {
            Rect leftRect = inRect.LeftPart(leftPartPct);
            Rect rightRect = inRect.RightPart(1 - leftPartPct);
            Widgets.DrawMenuSection(leftRect);
            DrawRightPart(rightRect);
            DrawLeftPart(leftRect);
        }

        private void DrawLeftPart(Rect rect)
        {
            Rect headerRect = rect.TopPart(headerPct);
            DrawHeader(headerRect);

            Widgets.DrawLineHorizontal(headerRect.xMin, headerRect.yMax, rect.width);

            Rect selectedInfoRect = rect.BottomPart(1 - headerPct);
            DrawSelectedInfo(selectedInfoRect);
        }

        private void DrawHeader(Rect rect)
        {
            Text.Anchor = TextAnchor.MiddleCenter;
            Rect titleRect = rect.TopPart(headerTitlePct);
            Text.Anchor = default;
            Widgets.LabelFit(titleRect, bill.Label);

        }

        private void DrawSelectedInfo(Rect rect)
        {

        }

        private const float minBlockWidth = 125f;
        private void DrawRightPart(Rect rect)
        {
            int lineCount = Mathf.FloorToInt(rect.width / minBlockWidth);
            float emptySpace = rect.width - (lineCount * minBlockWidth);
            float blockWidth = minBlockWidth + (emptySpace / lineCount);
            for (int i = 0; i < bill.RecipeExtension.recipes.Count; i++)
            {
                int xIndex = i % lineCount;
                int yIndex = Mathf.FloorToInt(i / lineCount);

                //float x = rect.x + (blockWidth * xIndex);
                //float y = rect.y + Mathf.FloorToInt(i / lineCount);
                float x = rect.x + (xIndex * blockWidth);
                float y = rect.y + (yIndex * blockWidth);
                Rect blockRect = new Rect(x, y, blockWidth, blockWidth);
                DrawRecipeBlock(blockRect, bill.RecipeExtension.recipes[i]);
            }
        }

        private void DrawRecipeBlock(Rect rect, PangaeaRecipeSettings recipe)
        {
            Widgets.DrawBox(rect);
            //Widgets.DrawTextureFitted(rect, recipe.DefiningIcon, 1.0f);
            //PangaeaUIGen.DrawTexWithMaterialClipped(rect, rect, recipe.DefiningIcon, recipe.DefiningGraphic.MatSingle);
            recipe.DefiningPortion.DrawIcon(rect, rect);

            Widgets.LabelFit(rect, recipe.label);
        }
    }
}
