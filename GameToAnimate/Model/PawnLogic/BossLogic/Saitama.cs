using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.BossLogic
{
    public class Saitama : Boss
    {
        private Stopwatch stopwatch = new Stopwatch();
        private int shootCount = 0;
        public Saitama(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints) : base(position, chunk, radius, speed, lifePoints)
        {
            TypeOfBoss = BossType.Saitama;
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

            if(shootCount < 60 && !stopwatch.IsRunning)
            {
                model.ManagerHolder.ProjectileManager.Add(Position, Chunk, direction, 20, 0.06f, false);
                shootCount++;
            }
            else if(!stopwatch.IsRunning)
            {
                shootCount = 0;
                stopwatch.Restart();
            }

            if(stopwatch.Elapsed.Seconds > 2)
            {
                stopwatch.Stop();
            }
        }
    }
}
