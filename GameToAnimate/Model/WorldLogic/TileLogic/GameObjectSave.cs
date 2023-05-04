using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Model.WorldLogic.TileLogic.Types;

namespace Model.WorldLogic.TileLogic
{
    [Serializable]
    public class GameObjectSave
    {
        public ObjectType type;
        public Vector2Save position;
        public GameObjectSave(ObjectType type, int x, int y)
        {
            this.type = type;
            position = new Vector2Save(x, y);
        }

    }
}
