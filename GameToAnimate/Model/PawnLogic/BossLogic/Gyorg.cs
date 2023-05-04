using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.BossLogic
{
    public class Gyorg : Boss
    {
        private Stopwatch stopwatch = new Stopwatch();
        //private Random random = new Random();
        private int reloadCount = 0;
        private bool waiting = false;
        public Gyorg(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints) : base(position, chunk, radius, speed, lifePoints)
        {
            TypeOfBoss = BossType.Gyorg;
        }

        public void Update(MainModel model, float deltaT)
        {
            Vector2 playerPos = model.ManagerHolder.PlayerManager.Player.Chunk * 5 + model.ManagerHolder.PlayerManager.Player.Position;
            Vector2 direction = Vector2.Normalize(playerPos - (Chunk * 5 + Position));

            CurrentDirection = direction;
            //Console.WriteLine(ViewDirection);

            //Einfache Lauflogic
            if (model.World.GetChunk(model.ManagerHolder.PlayerManager.Player.Chunk).TileId >= 16 && model.World.GetChunk(model.ManagerHolder.PlayerManager.Player.Chunk).TileId <= 24)
            {
                MoveInDirection(direction, deltaT);
            }

            
            //Schießlogik
            if (!stopwatch.IsRunning)
            {
                stopwatch.Restart();
            }

            if(stopwatch.Elapsed.Milliseconds >= 200f)
            {
                model.ManagerHolder.ProjectileManager.Add(Position, Chunk, direction, 20, 0.06f, false);
                model.ManagerHolder.ProjectileManager.Add(Position, Chunk, new Vector2(direction.X + 0.05f, direction.Y + 0.05f), 20, 0.06f, false);
                model.ManagerHolder.ProjectileManager.Add(Position, Chunk, new Vector2(direction.X - 0.05f, direction.Y - 0.05f), 20, 0.06f, false);
                reloadCount++;
                stopwatch.Restart();
            }

            if(reloadCount >= 6)
            {
                reloadCount = 0;
                waiting = true;
            }

            if(waiting == true && stopwatch.Elapsed.Seconds > 3)
            {
                waiting = false;
                stopwatch.Restart();
            }
            
        }
    }
}
