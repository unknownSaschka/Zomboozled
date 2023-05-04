using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Model.PawnLogic.EnemyLogic;
using Model.PawnLogic.ProjectileLogic;
using static Model.WorldLogic.TileLogic.Types;

namespace Model.WorldLogic
{
   
    public class Chunk {
        
        //public enum ChunkType { StartStreet, Grass, CityStreet, Building, Park, Arena00, Arena10, Arena01, Arena11, Boss, BossCenter }
        public enum DirectionToPlayer { ThisChunk, Left, TopLeft, Top, TopRight, Right, DownRight, Down, DownLeft, NoWay, FreeELeft, FreeERight, FreeEUp, FreeEDown}
        //public struct StreetOut { public bool up; public bool down; public bool left; public bool right; };

        public List<GameObject> gameObjects;
        //public List<Enemy> enemies;


        //public List<Projectile> Projectiles;
        //10x10 large area of Floor Objects
        //public List<GameObject> map;

        public int TileId;
        public int chunkX;
        public int chunkY;

        //public bool noStreets;

        //public ChunkType chunkType;
        //public StreetOut streetOut;


        //Values for Pathfinding
        private uint _distance = 99;
        private DirectionToPlayer _directionToPlayer;
        private uint _updateId;

        public uint Distance => _distance;
        public DirectionToPlayer Direction
        {
            get { return _directionToPlayer; }
            set { _directionToPlayer = value; }
        }

        public Vector2 ChunkPosition
        {
            get { return new Vector2(chunkX, chunkY); }
        }
        public uint UpdateId { get { return _updateId; } set { _updateId = value; } }

        //public List<Zenseless.Geometry.Box2D> staticBoundingBoxes;
        public List<Zenseless.Geometry.Box2D> staticBoundingBoxes;

        

        public bool UpdatePathInformation(uint newDistance, DirectionToPlayer direction, uint updateId)
        {
            if (updateId != _updateId || _distance > newDistance)
            {
                _distance = newDistance;
                _directionToPlayer = direction;
                
                return true;
            }
            return false;
        }

        public Chunk(int tileId, int x, int y)
        {
            TileId = tileId;
            chunkX = x;
            chunkY = y;
        }
        public Chunk(int x, int y) {
            chunkX = x;
            chunkY = y;
            gameObjects = new List<GameObject>();
            //enemies = new List<Enemy>(30);
            //map = new List<GameObject>(64);
            //Projectiles = new List<Projectile>();
            staticBoundingBoxes = new List<Zenseless.Geometry.Box2D>();
            _directionToPlayer = DirectionToPlayer.NoWay;
        }
        public Chunk()
        {
            gameObjects = new List<GameObject>();
            //enemies = new List<Enemy>(30);
            //map = new List<GameObject>(64);
            //Projectiles = new List<Projectile>();
        }
        //static float chunkSize = 5;



    }


}
