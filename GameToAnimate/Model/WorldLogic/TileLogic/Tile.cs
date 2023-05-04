using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenseless.Geometry;
using static Model.WorldLogic.TileLogic.Types;

namespace Model.WorldLogic.TileLogic
{
    public class Tile
    {

        //public struct StreetOut { public bool up; public bool down; public bool left; public bool right; };
        public ChunkType chunkType;
        public StreetOut streetOut;
        public List<Box2D> collisionBoxes;
        public List<GameObject> gameObjects;

        public Tile(List<GameObjectSave> gameObjectSave, List<Box2D> collisionBoxes, StreetOut streetOut, ChunkType chunkType)
        {
            this.collisionBoxes = collisionBoxes;
            this.streetOut = streetOut;
            this.chunkType = chunkType;
            gameObjects = new List<GameObject>();
            GameObject temp;
            foreach (GameObjectSave save in gameObjectSave)
            {
                temp = new GameObject(save.type, (int)save.position.X, (int)save.position.Y);
                gameObjects.Add(temp);
                //Console.WriteLine(save.type);
            }
        }
    }
}
