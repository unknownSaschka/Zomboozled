using System;
using System.Numerics;

namespace Extension
{
    public static class Vector2Extension
    {

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = (float)Math.Sin((degrees * (float)(180.0 / Math.PI)));
            float cos = (float)Math.Cos((degrees * (float)(180.0 / Math.PI)));

            Vector2 v2 = new Vector2((cos * v.X) - (sin * v.Y), (sin * v.X) + (cos * v.Y));
            return v2;
        }
    }
}
