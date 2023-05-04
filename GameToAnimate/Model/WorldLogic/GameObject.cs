using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.WorldLogic.TileLogic;
using static Model.WorldLogic.TileLogic.Types;

namespace Model.WorldLogic
{
    public class GameObject {

        public enum Ground { Street, Grass, Sidewalk};
        //public enum Type { Street, Grass, Sidewalk, Building, Wall, Ground, Structure, Object, Item, Enemy ,Player, TypeCount, Projectile };
        //public enum Ground { Street, Grass, Path}
        //public float posX, posY;
        public ObjectType type;
        static Random ran = new Random();
        public Vector2 position;
        //Debug
        public double r, g, b;

        public GameObject(ObjectType type, float x, float y)
        {
            this.type = type;
            position = new Vector2(x, y);
        }


        public GameObject(ObjectType type, int x, int y){

            this.type = type;

            switch (type)
            {
                case ObjectType.Grass:
                    r = 0;
                    g = 0.5f + ran.NextDouble() * 0.5f;
                    b = 0;
                    break;
                case ObjectType.Street:
                    r = 0.5f;
                    g = 0.5f;
                    b = 0.5;
                    break;
                case ObjectType.Building:
                    
                    r = 0.8f;
                    g = 0.8f;
                    b = 0.7f;
                    /*
                    r = ran.NextDouble();
                    g = ran.NextDouble();
                    b = ran.NextDouble();
                    */
                    break;
                case ObjectType.Wall:
                    r = 0.7f;
                    g = 0.7f;
                    b = 0.6f;
                    break;
                case ObjectType.Sidewalk:
                    r = 0.75f;
                    g = 0.55f;
                    b = 0;
                    break;
                case ObjectType.StartHouse:
                    r = 0.7f;
                    g = 0f;
                    b = 0f;
                    break;
                case ObjectType.ShopGround:
                    r = 0.6f;
                    g = 0.4f;
                    b = 0f;
                    break;
            }
            position = new Vector2(x, y);

        }
        public GameObject(ObjectType type, int x, int y, double red, double green, double blue)
        {

            this.type = type;
            r = red;
            g = green;
            b = blue;

            position = new Vector2(x, y);

        }

    }
}
