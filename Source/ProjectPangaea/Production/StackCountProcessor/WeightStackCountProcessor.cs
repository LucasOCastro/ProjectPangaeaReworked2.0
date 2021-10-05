using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectPangaea.Production
{
    public abstract class WeightStackCountProcessor : StackCountProcessor
    {
        private bool roundUp = false;
        public override int Process(int stackCount, List<Thing> ingredients)
        {
            float val = stackCount * GetWeight(ingredients);
            return roundUp ? Mathf.CeilToInt(val) : Mathf.FloorToInt(val);
        }

        protected abstract float GetWeight(List<Thing> ingredients);
    }
}
