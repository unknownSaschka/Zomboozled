using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;

namespace Extension
{
    public static class Box2DOverlap
    {

        public static int GetCollisionBoxNormal(this Box2D rectangleA, IReadOnlyBox2D rectangleB)
        {
            if (!rectangleA.Intersects(rectangleB)) return 0;

            Vector2[] directions = new Vector2[]
            {
                new Vector2(rectangleB.MaxX - rectangleA.MinX, 0), // push distance A in positive X-direction
				new Vector2(rectangleB.MinX - rectangleA.MaxX, 0), // push distance A in negative X-direction
				new Vector2(0, rectangleB.MaxY - rectangleA.MinY), // push distance A in positive Y-direction
				new Vector2(0, rectangleB.MinY - rectangleA.MaxY)  // push distance A in negative Y-direction
			};
            float[] pushDistSqrd = new float[4];
            for (int i = 0; i < 4; ++i)
            {
                pushDistSqrd[i] = directions[i].LengthSquared();
            }
            //find minimal positive overlap amount
            int minId = 0;
            for (int i = 1; i < 4; ++i)
            {
                minId = pushDistSqrd[i] < pushDistSqrd[minId] ? i : minId;
            }

            return minId;
        }
    }
}
