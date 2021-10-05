using Verse;
using RimWorld;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class StackCountProcessor_PawnStat : WeightStackCountProcessor
    {
        public StatDef stat;
        public float statWeight = 1;

        protected override float GetWeight(List<Thing> ingredients)
        {
            foreach (var ing in ingredients)
            {
                if (GetStatFromThing(ing, out float statVal))
                {
                    return statVal * statWeight;
                }
            }
            return 1;
        }

        private bool GetStatFromThing(Thing thing, out float statVal)
        {
            if (thing is Pawn pawn && !stat.Worker.IsDisabledFor(pawn))
            {
                statVal = pawn.GetStatValue(stat);
                return true;
            }
            if (thing is Corpse corpse && !stat.Worker.IsDisabledFor(corpse.InnerPawn))
            {
                statVal = corpse.InnerPawn.GetStatValue(stat);
                return true;
            }
            statVal = 0;
            return false;
        }
    }
}
