using UnityEngine;

namespace ProjectPangaea
{
    public static class MathUtility
    {
        public static float ToShortDecimal(this float f)
        {
            return Mathf.Round(f * 100f) / 100f;
        }
    }
}
