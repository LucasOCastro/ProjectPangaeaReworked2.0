using Verse;
using RimWorld;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class StackCountProcessor_RotStage : WeightStackCountProcessor
    {
        public EnumDictionary<RotStage, float> rotWeights = new EnumDictionary<RotStage, float>(1);

        protected override float GetWeight(List<Thing> ingredients)
        {
            foreach (var ing in ingredients)
            {
                if (ing is Corpse corpse)
                {
                    return GetEfficiencyFrom(corpse);
                }
            }
            return 1;
        }

        private float GetEfficiencyFrom(Corpse corpse)
        {
            return rotWeights[corpse.GetRotStage()];
        }
    }
}
