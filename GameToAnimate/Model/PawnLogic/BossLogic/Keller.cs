using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.BossLogic
{
    public class Keller : Boss
    {
        //private Stopwatch stopwatch = new Stopwatch();
        private Random random = new Random();
        private Vector2 sourceChunk;
        public Keller(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints) : base(position, chunk, radius, speed, lifePoints)
        {
            sourceChunk = chunk;
            TypeOfBoss = BossType.Keller;
            CurrentDirection = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            CurrentDirection = Vector2.Normalize(CurrentDirection);
        }

        public void Update(MainModel model, float deltaT)
        {
            Vector2 playerPos = model.ManagerHolder.PlayerManager.Player.Chunk * 5 + model.ManagerHolder.PlayerManager.Player.Position;
            Vector2 direction = Vector2.Normalize(playerPos - (Chunk * 5 + Position));

            if (model.World.GetChunk(model.ManagerHolder.PlayerManager.Player.Chunk).TileId >= 16 && model.World.GetChunk(model.ManagerHolder.PlayerManager.Player.Chunk).TileId <= 24)
            {
                if (Chunk.X >= sourceChunk.X + 1)
                {
                    if (Position.X >= 5f - Radius)
                    {
                        CurrentDirection = Randomize(ToMiddle(sourceChunk, Chunk, Position));
                        /*
                        float x = (float)random.NextDouble() * 0.2f - 0.1f;
                        float y = (float)random.NextDouble() * 0.2f - 0.1f;
                        CurrentDirection = Vector2.Normalize(new Vector2(x, y));
                        MoveInDirection(CurrentDirection, deltaT);
                        */
                    }
                }
                if(Chunk.X <= sourceChunk.X - 1)
                {
                    if (Position.X <= Radius)
                    {
                        CurrentDirection = Randomize(ToMiddle(sourceChunk, Chunk, Position));
                    }
                }
                if(Chunk.Y >= sourceChunk.Y + 1)
                {
                    if (Position.Y >= 5f - Radius)
                    {
                        Console.WriteLine("Stößt oben an");
                        CurrentDirection = Randomize(ToMiddle(sourceChunk, Chunk, Position));
                    }
                }
                if(Chunk.Y <= sourceChunk.Y - 1)
                {
                    if (Position.Y <= Radius)
                    {
                        CurrentDirection = Randomize(ToMiddle(sourceChunk, Chunk, Position));
                    }
                }
                MoveInDirection(CurrentDirection, deltaT);
            }
        }

        public Vector2 ToMiddle(Vector2 srcChunk, Vector2 currentChunk, Vector2 position)
        {
            Vector2 middlePoint = srcChunk * 5 + new Vector2(2.5f, 2.5f);
            middlePoint = middlePoint - (currentChunk * 5 + position);
            return Vector2.Normalize(middlePoint);
        }

        public Vector2 Randomize(Vector2 toMiddle)
        {
            float x = (float)random.NextDouble() * 0.8f - 0.4f;
            float y = (float)random.NextDouble() * 0.8f - 0.4f;
            Vector2 randomized = new Vector2(x, y);
            return Vector2.Normalize(toMiddle + randomized);
        }
    }
}
