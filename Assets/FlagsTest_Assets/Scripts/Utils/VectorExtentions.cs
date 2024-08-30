using UnityEngine;

namespace FlagsTest
{
    public static class VectorExtentions
    {
        public static Vector3 ToVector3 (this Vector2 v2)
        {
            return new Vector3 (v2.x, 0, v2.y);
        }
    }
}
