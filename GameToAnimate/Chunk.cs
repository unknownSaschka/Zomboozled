using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
   
    class Chunk {
        //
        public enum ChunkType { StartStreet, Grass, CityStreet, Building }
        public struct StreetOut { public bool up; public bool down; public bool left; public bool right; };
        public bool noStreets;

        public List<GameObject> gameObjects;
        public List<GameObject> enemies;
        //10x10 large area of Floor Objects
        public List<GameObject> map;

        public int chunkX;
        public int chunkY;
        public ChunkType chunkType;
        public StreetOut streetOut;

        public Chunk(int x, int y) {
            chunkX = x;
            chunkY = y;
            gameObjects = new List<GameObject>();
            enemies = new List<GameObject>();
            map = new List<GameObject>(64);
            //Console.WriteLine("Neuer Chunk in {0} {1}", x, y);
        }

        public Chunk(int x, int y, ChunkType chunkType, StreetOut streetOut)
        {
            chunkX = x;
            chunkY = y;
            gameObjects = new List<GameObject>();
            enemies = new List<GameObject>();
            map = new List<GameObject>(64);
            //Console.WriteLine("Neuer Chunk in {0} {1}", x, y);
            this.streetOut = streetOut;
            this.chunkType = chunkType;
        }


    }
}
