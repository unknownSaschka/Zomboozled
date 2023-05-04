using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    class Model {

        public World world;
        public float aspectRatio;

        public Generator generator;

        public Player player;
        public Player Player {
            get {
                return player;
            }
            set {
                player = value;
            }
        }

        public void AspectRatioChanged(float aspect)
        {
            aspectRatio = aspect;
        }
        public World World { get; }

        public Model() {
            world = new World();
         
        }

        public void CheckPlayerChunk()
        {
            Boolean changed = false;
            if(player.position.X > 5)
            {
                //Console.WriteLine("Player Changed Chunk to the right side!");

                player.currentChunk.X += 1;
                player.position.X = 0;

               

                    //player.posY -= 5;
                    
                changed = true;
            }
            
            else if (player.position.X < 0)
            {
                //Console.WriteLine("Player Changed Chunk to the right side!");
                    player.currentChunk = new System.Numerics.Vector2(player.currentChunk.X - 1, player.currentChunk.Y);
                    player.position.X += 5;
                    //player.posY -= 5;
                    //Console.WriteLine("Player Changed Chunk to the left side!");
                changed = true;
            }
            if (player.position.Y > 5)
            {
                player.currentChunk.Y += 1;
                player.position.Y = 0;

                changed = true;

            }
            else if(player.position.Y < 0)
            {
                player.currentChunk.Y -= 1;
                player.position.Y = 5;
                changed = true;
            }

            //Console.WriteLine("Player Pos X: " + player.position.X + " Y:" + player.position.Y);

            if (changed != true) return;
            //Console.WriteLine("Player Changed Chunk to the right side!");
            //Console.WriteLine("Player Pos X: " + player.position.X + " Y:" + player.position.Y);
            //Console.WriteLine("Player Chunk X: " + player.currentChunk.X + " Y:" + player.currentChunk.Y);



            generator.Generate();
        }
    }
}
