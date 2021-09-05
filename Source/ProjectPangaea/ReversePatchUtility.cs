using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using HarmonyLib;

namespace ProjectPangaea
{
    public static class ReversePatchUtility
    {
        public class ReversePatchContainer
        {
            public CodeInstruction code;
            public IList<CodeInstruction> list;
            public int index;
            public int startingIndex;

            public ReversePatchContainer(CodeInstruction code, IList<CodeInstruction> list, int index, int startingIndex)
            {
                this.code = code;
                this.list = list;
                this.index = index;
                this.startingIndex = startingIndex;
            }
        }

        public static ReversePatchContainer BackUntil(this ReversePatchContainer rp, Func<int, bool> condition, Action<int> callback = null, Action<int> matchCallback = null)
        {
            return MoveUntil(-1, rp, condition, callback, matchCallback);
        }
        public static ReversePatchContainer ForwardUntil(this ReversePatchContainer rp, Func<int, bool> condition, Action<int> callback = null, Action<int> matchCallback = null)
        {
            return MoveUntil(1, rp, condition, callback, matchCallback);
        }
        private static ReversePatchContainer MoveUntil(int loopModifier, ReversePatchContainer rp, Func<int, bool> breakCondition, Action<int> callback, Action<int> matchCallback)
        {
            var result = MoveUntil(loopModifier, rp.list, rp.index + loopModifier, breakCondition, callback, matchCallback);
            result.startingIndex = rp.startingIndex;
            return result;
        }


        public static ReversePatchContainer BackUntil(this IList<CodeInstruction> list, Func<int, bool> condition, int startingIndex = 0, Action<int> callback = null, Action<int> matchCallback = null)
        {
            return MoveUntil(-1, list, startingIndex, condition, callback, matchCallback);
        }
        public static ReversePatchContainer ForwardUntil(this IList<CodeInstruction> list, Func<int, bool> condition, int startingIndex = 0, Action<int> callback = null, Action<int> matchCallback = null)
        {
            return MoveUntil(1, list, startingIndex, condition, callback, matchCallback);
        }
        private static ReversePatchContainer MoveUntil(int loopModifier, IList<CodeInstruction> list, int startingIndex, Func<int, bool> breakCondition, Action<int> callback, Action<int> matchCallback)
        {
            int count = list.Count;
            for (int i = startingIndex; i >= 0 && i < count; i += loopModifier)
            {
                var code = list[i];
                callback?.Invoke(i);
                if (breakCondition != null && breakCondition(i))
                {
                    matchCallback?.Invoke(i);
                    return new ReversePatchContainer(code, list, i, startingIndex);
                }
            }
            return new ReversePatchContainer(null, null, -1, startingIndex);
        }
    }
}
