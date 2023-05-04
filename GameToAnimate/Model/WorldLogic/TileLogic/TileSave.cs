using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;
using static Model.WorldLogic.TileLogic.Types;

namespace Model.WorldLogic.TileLogic
{
    [Serializable]
    public class TileSave
    {
        public int TileID;
        //public bool up = false, down = false, left = false, right = false;      //Straßen
        public StreetOut streetOut;
        public String description;
        public List<Box2D> collisionBoxes = new List<Box2D>();
        public ChunkType chunkType;
        //public struct StreetOut { public bool up; public bool down; public bool left; public bool right; };
        //public StreetOut streetOut;
        //public Dictionary<Vector2, GameObject> gameObjects = new Dictionary<Vector2, GameObject>();
        public List<GameObjectSave> gameObjects = new List<GameObjectSave>();
    }

}
