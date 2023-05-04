using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.ProjectileLogic
{
    public class Grenade : Pawn
    {
        public double LifeTime {get; private set;}
        private float _explosionRadius = 0;
        public float Damage { get; private set; }
        public bool PlayerDamage { get; internal set; }

        public enum WallToBounceOffDirection { XAxis, YAxis}

        private Vector2 _direction;
        /*
        public Grenade(Vector2 position, Vector2 direction, Vector2 chunk, float radius, float speed, float explosionRadius, float lifetime) : base(position, chunk, radius, speed, 1)
        {
            _explosionRadius = explosionRadius;
            _speed = speed;
            Lifetime = lifetime;
            _collision.Center = position;
            _direction = direction;
            _chunk = chunk;
        }
        */
        public Grenade(Vector2 position, Vector2 direction, Vector2 chunk, float speed, double lifetime, float damage, float explosionRadius, bool playerDamage) : base(position, chunk, 0.1f, speed, 1)
        {
            _direction = direction;
            _speed = speed;
            LifeTime = lifetime;
            Damage = damage;
            _explosionRadius = explosionRadius;
            PlayerDamage = playerDamage;
        }

        public void BounceOfWall(WallToBounceOffDirection dir)
        {
            if(dir == WallToBounceOffDirection.XAxis)
            {
                _direction.Y = -_direction.Y;
            }
            else
            {
                _direction.X = -_direction.X;
            }
        }

        public new void Update(float deltaT)
        {
            if (LifeTime > 0)
            {
                LifeTime -= deltaT;
                MoveInDirection(_direction, deltaT);
                //Console.WriteLine("moved grenade");
            }
            else
            {
                //Console.WriteLine("Grenade no life!");
                LifePoints = 0;
            }
        }

        public Explosion CreateExplosion()
        {
            Explosion exp = new Explosion(Position, Chunk, _explosionRadius, Damage, PlayerDamage);
            //Console.WriteLine("Made Explosion! with Damage: "  + Damage);
            return exp; 

        }
    }
}
