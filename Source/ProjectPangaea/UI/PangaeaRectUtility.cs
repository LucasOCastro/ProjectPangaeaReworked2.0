using UnityEngine;

namespace ProjectPangaea
{
    public static class PangaeaRectUtility
    {
        public static Rect LowerRightCorner(this Rect rect, float size) => rect.LowerRightCorner(new Vector2(size, size));
        public static Rect LowerRightCorner(this Rect rect, Vector2 size)
        {
            Vector2 pos = rect.position + (rect.size - size);
            return new Rect(pos, size);
        }

        public static Rect LowerLeftCorner(this Rect rect, float size) => rect.LowerLeftCorner(new Vector2(size, size));
        public static Rect LowerLeftCorner(this Rect rect, Vector2 size)
        {
            return new Rect(rect)
            {
                size = size,
                y = rect.y + (rect.height - size.y)
            };
        }
    }
}
