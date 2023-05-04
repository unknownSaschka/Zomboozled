using System.Numerics;
using Model;

namespace Model.PawnLogic.ProjectileLogic
{
    public class Thomas : Pawn
    {
        private Vector2 _direction;
        //private readonly float _speed;
        public double Lifetime;
        private float _damage;
        public Vector2 Direction { get { return _direction; } }

        public enum ProjectileType { Projectile, Penetration, HighExplosive, Thomas };

        public float Damage => _damage;

        public Thomas(Vector2 position, Vector2 direction, Vector2 chunk, float speed, double lifetime, float damage) : base(position, chunk, 3f, speed, 1)
        {
            _direction = direction;
            _speed = speed;
            Lifetime = lifetime;
            _damage = damage;
        }

        public void Reset(Vector2 position, Vector2 direction, Vector2 chunk, float lifetime, float damage)
        {
            _collision.Center = position;
            _direction = direction;
            _chunk = chunk;
            Lifetime = lifetime;
            _damage = damage;
        }

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
