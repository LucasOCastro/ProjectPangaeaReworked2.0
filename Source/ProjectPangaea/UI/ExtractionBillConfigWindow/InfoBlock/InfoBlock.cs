using UnityEngine;
using Verse;
using ProjectPangaea.Production;

namespace ProjectPangaea.PangaeaUI
{
    public class InfoBlock
    {
        public PangaeaThingEntry PangaeaEntry { get; }
        public ThingDef InfoBlockParent => PangaeaEntry.ThingDef;

        public PangaeaResourceBill Bill { get; }

        public InfoBlock(PangaeaThingEntry entry, PangaeaResourceBill bill)
        {
            PangaeaEntry = entry;
            Bill = bill;
        }

        public void Draw(Rect rect, float cornerDetailsSize, bool doCheckmark)
        {
            GUI.color = Color.white;

            //Widgets.DefIcon(textureRect, InfoBlockParent, scale: 0.8f);
            //Widgets.DrawTextureFitted(rect, PangaeaEntry.ThingDef.race.AnyPawnKind.lifeStages.FindLast(f=>true).bodyGraphicData.Graphic.MatEast.mainTexture, scale: 1f);
            Widgets.ThingIcon(rect, PangaeaEntry.ThingDef, scale: 0.6f);

            Verse.Text.Font = GameFont.Small;

            Verse.Text.Anchor = TextAnchor.UpperCenter;
            Widgets.Label(rect, InfoBlockParent.LabelCap);

            if (cornerDetailsSize > 0 && Bill != null)
            {
                Verse.Text.Anchor = TextAnchor.MiddleCenter;
                int required = Bill.Counter.ResourceRequirement;
                int count = Bill.Counter.Count(Bill.Map, PangaeaEntry);
                Vector2 size = PangaeaUIGen.CalcRequiredAmountLabelSize(count, required);
                Rect countRect = rect.LowerLeftCorner(size);
                PangaeaUIGen.RequiredAmountLabel(countRect, count, Bill.Counter.ResourceRequirement);
            }

            if (doCheckmark && cornerDetailsSize > 0 && Bill != null)
            {
                Rect checkMarkRect = rect.LowerRightCorner(cornerDetailsSize);
                bool allowed = Bill.Allows(PangaeaEntry);
                if (PangaeaUIGen.CheckMarkButton(checkMarkRect, allowed))
                {
                    Bill.Toggle(PangaeaEntry);
                }
            }

            Verse.Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}
