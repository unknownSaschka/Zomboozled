using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WorldLogic.TileLogic
{
    [Serializable]
    public class Vector2Save
    {
        public float X, Y;
        public Vector2Save(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
