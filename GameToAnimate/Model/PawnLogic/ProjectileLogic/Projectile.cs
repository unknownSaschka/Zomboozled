using System.Numerics;
using Model;

namespace Model.PawnLogic.ProjectileLogic
{
    public class Projectile : Pawn
    {
        private Vector2 _direction;
        //private readonly float _speed;
        public double Lifetime;
        private float _damage;
        public Vector2 Direction { get { return _direction; } }
        public enum ProjectileType { Projectile, Penetration, HighExplosive, Thomas};
        public bool Destination { get; set; }

        public float Damage => _damage;

        public Projectile(Vector2 position, Vector2 direction, Vector2 chunk, float speed, double lifetime, float damage, float size, bool destination) : base(position, chunk, size, speed, 1) //bool destination False = soll spielerschaden machen, true = soll gegner schaden machen
        {
            _direction = direction;
            _speed = speed;
            Lifetime = lifetime;
            _damage = damage;
            Destination = destination;
        }

        public void Reset(Vector2 position, Vector2 direction, Vector2 chunk, float lifetime, float damage, float size, bool destination)
        {
            _collision.Center = position;
            _direction = direction;
            _chunk = chunk;
            Lifetime = lifetime;
            _damage = damage;
            Destination = destination;
            Radius = size;
        }
        /*
        public Projectile(float originX, float originY, float speed, Vector2 direction, Vector2 originChunk):base(ObjectType.Projectile, originX, originY)
        {
            IdChunk = originChunk;
            _direction = direction;
            _speed = speed;
            Lifetime = 1;
            Console.WriteLine("New Projectile!");
        }
        */


        public new void Update(float deltaT)
        {
            
            if (Lifetime > 0)
            {
                Lifetime -= deltaT;
                MoveInDirection(_direction, deltaT);
            }
            

        }
    }
}
