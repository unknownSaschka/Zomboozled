using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Zenseless.Geometry;
using Model.WorldLogic;
using Model.InterfaceCollection;
using Model.Interface;

namespace Model.PawnLogic
{
    public abstract class Pawn : IMoveable, ICollider, IWorldObject, IHealth, IMovementPhysics
    {
        protected Vector2 _chunk;
        protected float _speed;
        protected Circle _collision;
        protected Vector2 _entryPoint;
        protected int _maxLifePoints;
        protected int _lifePoints;
        private int _armour = 0;
        public Vector2 ViewDirection { get; set; }

        protected Pawn(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints)
        {
            _chunk = chunk;
            _collision = new Circle(position.X, position.Y, radius);
            _speed = speed;
            _lifePoints = lifePoints;
            _maxLifePoints = lifePoints;
        }

        public Vector2 Position
        {
            get{
                return _collision.Center;
            }
            set
            {
                _collision.Center = value;
            }
        }

        public float Radius
        {
            get
            {
                return _collision.Radius;
            } 
            internal set
            {
                _collision.Radius = value;
            }
        }

        public int LifePoints
        {
            get { return _lifePoints; }
            set { _lifePoints = value; }
        }


        public Circle Collider
        {
            get
            {
                return _collision;
            } 

        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        

        public Vector2 Chunk
        {
            get
            {
                return _chunk;
            }
            protected set
            {
                _chunk = value;
            }
        }

        public int MaxLifePoints
        {
            get { return _maxLifePoints; }
            set { _maxLifePoints = value; }
        }

        public int Armor
        {
            get { return _armour; }
            set { _armour = value; }
        }

        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public Vector2 Acceleration { get; set; } = Vector2.Zero;
        public float Mass { get; set; }

        public bool UpdateCurrentChunk()
        {
            bool changed = false;
            if (Position.X > 5)
            {
                _chunk += new System.Numerics.Vector2(1, 0);
                _collision.CenterX -= 5;
                changed = true;
            }

            else if (_collision.CenterX < 0)
            {
                _chunk.X -= 1; 
                _collision.CenterX += 5;

                changed = true;
            }
            if (_collision.CenterY > 5)
            {
                _chunk.Y += 1;
                _collision.CenterY -= 5;

                changed = true;

            }
            else if (_collision.CenterY < 0)
            {
                _chunk.Y -= 1;
                _collision.CenterY += 5;
                changed = true;
            }
            if (changed)
            {
                _entryPoint = _collision.Center;
            }
            return changed;
        }



        public void MoveToPosition(Vector2 newPosition) => _collision.Center = newPosition;

        public void MoveToPosition(Vector2 newPosition, Vector2 newChunk)
        {
            _collision.Center = newPosition;
            _chunk = newChunk;
        }

        public void MoveTorwards(Vector2 targetPos, Vector2 targetChunk, float deltaT)
        {
            Vector2 direction = World.ChunkToGlobalSpace(targetChunk, targetPos) - World.ChunkToGlobalSpace(_chunk, _collision.Center);

            direction = Vector2.Normalize(direction);
            direction = direction * _speed * deltaT;
            //Console.WriteLine(_collision.Center);
            _collision.Center += direction;
            //Console.WriteLine(_collision.Center);
            //_collision.Center += World.ChunkToGlobalSpace(_chunk, targetPos) * _speed * deltaT;
        }

        public void MoveTorwards(Vector2 targetPos, float deltaT)
        {
            _collision.Center += Vector2.Normalize(targetPos - _collision.Center) *deltaT * _speed;
        }

        public void MoveInDirection(Vector2 movementDirection, float deltaT)
        {
            Vector2 direction = movementDirection;
            _collision.Center += direction * _speed * deltaT;
        }
        public void MoveInDirectionNormalized(Vector2 movementDirection, float deltaT)
        {
            Vector2 direction = Vector2.Normalize(movementDirection);
            _collision.Center += direction * _speed * deltaT;
        }
        public int AddDamage(int attackValue, int ignoringValue)
        {
            if(_armour < ignoringValue) {
                _lifePoints -= attackValue;
                return attackValue;
            }
            else
            {
                var damage = attackValue - (_armour - ignoringValue);
                _lifePoints -= damage;
                return damage;
            }
        }
        public void Update(float deltaT)
        {
            //Newtons 1st law
            Velocity += Acceleration * deltaT;
            Position += Velocity * deltaT;
            //force was spend reset Acceleration
            Acceleration = Vector2.Zero;
        }
        public void ApplyForce(Vector2 force)
        {
            Acceleration += force / Mass;
        }
    }
}
