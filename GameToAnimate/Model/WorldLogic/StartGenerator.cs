using Model.InterfaceCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.WorldLogic
{
    public class StartGenerator : IGenerator
    {
        private World _world;
        private IManagerHolder _manager;
        static private Random random = new Random();

        public StartGenerator(World world, IManagerHolder manager)
        {
            _world = world;
            _manager = manager;
        }

        public void Generate()
        {
            
        }

        public void GenerateStart()
        {
            int chunkNum = 100;  //Tiles für StartBunker beginnen bei 100
            Chunk startChunk;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    startChunk = _world.GenerateChunkWithReturn(x, y);
                    startChunk.TileId = chunkNum;
                    chunkNum++;
                }
            }
            for (int y = -2; y < 3; y++)
            {
                for (int x = -2; x < 3; x++)
                {
                    if (_world.chunks.ContainsKey(new Vector2(x, y))) continue;
                    startChunk = _world.GenerateChunkWithReturn(x, y);
                    startChunk.TileId = 109;
                    chunkNum++;
                }
            }
        }

        public void GenerateRandomItems()
        {

        }
    }
}
