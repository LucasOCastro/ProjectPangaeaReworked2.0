using UnityEngine;
using Verse;

namespace ProjectPangaea
{
    public static class PangaeaRectUtility
    {
        public static Rect Intersection(this Rect a, Rect b)
        {
            Vector2 min = new Vector2(Mathf.Max(a.xMin, b.xMin), Mathf.Max(a.yMin, b.yMin));
            Vector2 max = new Vector2(Mathf.Min(a.xMax, b.xMax), Mathf.Min(a.yMax, b.yMax));
            return new Rect()
            {
                min = min,
                max = max
            };
        }

        public static Rect LowerRight(this Rect a, float size) => LowerRight(a, size, size);
        public static Rect LowerRight(this Rect a, float width, float height) => a.RightPartPixels(width).BottomPartPixels(height);

        public static Rect LowerLeft(this Rect a, float size) => LowerLeft(a, size, size);
        public static Rect LowerLeft(this Rect a, float width, float height) => a.LeftPartPixels(width).BottomPartPixels(height);

        public static Rect TopRight(this Rect a, float size) => TopRight(a, size, size);
        public static Rect TopRight(this Rect a, float width, float height) => a.RightPartPixels(width).TopPartPixels(height);

        public static Rect TopLeft(this Rect a, float size) => TopLeft(a, size, size);
        public static Rect TopLeft(this Rect a, float width, float height) => a.LeftPart(width).TopPart(height);
    }
}
