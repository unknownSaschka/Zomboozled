using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.BossLogic
{
    class Malos : Boss
    {

        private Stopwatch stopwatch = new Stopwatch();
        private Random random = new Random();
        public Malos(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints) : base(position, chunk, radius, speed, lifePoints)
        {
            TypeOfBoss = BossType.Malos;
        }

        public void Update(MainModel model)
        {

            Vector2 playerPos = model.ManagerHolder.PlayerManager.Player.Chunk * 5 + model.ManagerHolder.PlayerManager.Player.Position;

            Vector2 direction = Vector2.Normalize(playerPos - (Chunk * 5 + Position));

            CurrentDirection = direction;
            //Console.WriteLine(ViewDirection);

            if (!stopwatch.IsRunning)
            {
                
                //Vector2.Normalize(direction);
                //Console.WriteLine(direction);

                float randomTime = (float)random.NextDouble() * 100 + 50;
                float randomSpeed = (float)random.NextDouble() * 7 + 5;

                model.ManagerHolder.ProjectileManager.AddGrenade(Position, Chunk, direction * ((float)random.NextDouble()*2 + 1), 30, 5, 0.005f, 0.5f, true);

                stopwatch.Restart();
            }
            else
            {
                if(stopwatch.Elapsed.Seconds >= 2)
                {
                    stopwatch.Stop();
                }
            }
        }

    }
}
