using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.BossLogic
{
    public class Sans : Boss
    {
        private Stopwatch stopwatch = new Stopwatch();
        private Stopwatch grenadeStopwatch = new Stopwatch();
        private int shootCountProj = 0;
        private int grenadeCount = 0;
        public Sans(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints) : base(position, chunk, radius, speed, lifePoints)
        {
            TypeOfBoss = BossType.Sans;
        }

        public void Update(MainModel model, float deltaT)
        {
            Vector2 playerPos = model.ManagerHolder.PlayerManager.Player.Chunk * 5 + model.ManagerHolder.PlayerManager.Player.Position;
            Vector2 direction = Vector2.Normalize(playerPos - (Chunk * 5 + Position));

            CurrentDirection = direction;

            if (model.World.GetChunk(model.ManagerHolder.PlayerManager.Player.Chunk).TileId >= 16 && model.World.GetChunk(model.ManagerHolder.PlayerManager.Player.Chunk).TileId <= 24)
            {
                MoveInDirection(CurrentDirection, deltaT);
            }

            if (grenadeCount >= 5)
            {
                shootCountProj = 0;
                grenadeCount = 0;
                stopwatch.Stop();
            }

            if (shootCountProj < 220 && !stopwatch.IsRunning)
            {
                model.ManagerHolder.ProjectileManager.Add(Position, Chunk, direction, 20, 0.06f, false);
                shootCountProj++;
            }
            else if (grenadeCount < 5 && stopwatch.Elapsed.Seconds > 1)
            {
                model.ManagerHolder.ProjectileManager.AddGrenade(Position, Chunk, direction, 40, 10, 0.005f, 0.5f, true);
                stopwatch.Restart();
                grenadeCount++;
            }
            else if(!stopwatch.IsRunning)
            {
                stopwatch.Restart();
            }

            if(LifePoints < 25000 && grenadeStopwatch.Elapsed.Seconds >= 4)
            {
                model.ManagerHolder.ProjectileManager.AddGrenade(Position, Chunk, Vector2.Normalize(new Vector2(1, 1)), 40, 10, 0.005f, 0.5f, true);
                model.ManagerHolder.ProjectileManager.AddGrenade(Position, Chunk, Vector2.Normalize(new Vector2(1, -1)), 40, 10, 0.005f, 0.5f, true);
                model.ManagerHolder.ProjectileManager.AddGrenade(Position, Chunk, Vector2.Normalize(new Vector2(-1, -1)), 40, 10, 0.005f, 0.5f, true);
                model.ManagerHolder.ProjectileManager.AddGrenade(Position, Chunk, Vector2.Normalize(new Vector2(-1, 1)), 40, 10, 0.005f, 0.5f, true);
                grenadeStopwatch.Restart();
            }
            else if (!grenadeStopwatch.IsRunning)
            {
                grenadeStopwatch.Restart();
            }
        }
    }
}
