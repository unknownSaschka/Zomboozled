using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;
using Model.PawnLogic.EnemyLogic;
using Model.PawnLogic.ProjectileLogic;
using Model.WorldLogic;
using Model.ItemLogic;
using Model.WorldLogic.PathfindingLogic;
using Model.WorldLogic.TileLogic;

namespace Model.WorldLogic 
{
    public class World
    {

        private static float _chunkMultiplier = 5;
        public float FogDistanceStart { get; internal set; } = 10;
        public float FogDistanceEnd { get; internal set; } = 15;
        public Vector2 FogCenterChunk { get; internal set; } = new Vector2(0, 0);
        public Dictionary<Vector2, Chunk> chunks;

        public World(IManagerHolder managerHolder){

            chunks = new Dictionary<Vector2, Chunk>();


            }

        public Chunk GetChunk(int positionX, int positionY)
        {
            return chunks[new Vector2(positionX, positionY)];
        }
        public Chunk GetChunk(float positionX, float positionY)
        {
            return chunks[new Vector2(positionX, positionY)];
        }
        public Chunk GetChunk(Vector2 chunkPos)
        {
            return chunks[chunkPos];
        }
        public void SetChunk(int positionX, int positionY, Chunk chunk)
        {
            chunks.Add(new Vector2(positionX, positionY), chunk);
        }

        public Chunk GenerateChunkWithReturn(int positionX, int positionY)
        {
            GenerateChunk(positionX, positionY);

            return chunks[new Vector2(positionX, positionY)];
        }
        public void GenerateChunk(int positionX, int positionY)
        {
            try
            {
                chunks.Add(new Vector2(positionX, positionY), new Chunk(positionX, positionY));
            }catch(Exception e)
            {
                Console.WriteLine("Beim hinzufügen ist etwas schief gegangen! X:  " + positionX + "Y: " + positionY + "\n " + e.StackTrace);
            }
            
        }
        // public IEnumerable<Chunk> Chunks { get { return chunks.Select(o => o); } }
        //public IEnumerable Chunks { get { yield return chunks.ToList().GetEnumerator(); } }
        //public IEnumerable<Chunk> Chunks { get { yield return chunks.GetEnumerator(); } }

        public static Vector2 ChunkToGlobalSpace(Vector2 chunk, Vector2 positionInChunk)
        {
            return chunk * _chunkMultiplier + positionInChunk;
        }
    }
}
