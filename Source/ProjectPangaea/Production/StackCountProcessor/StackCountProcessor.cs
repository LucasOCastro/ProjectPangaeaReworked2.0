using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    [System.Serializable]
    public abstract class StackCountProcessor
    {
        public abstract int Process(int stackCount, List<Thing> ingredients);
    }
}
