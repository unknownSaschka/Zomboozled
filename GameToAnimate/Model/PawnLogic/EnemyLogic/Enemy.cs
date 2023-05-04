using System;
using System.Numerics;
using Extension;
using Model;
using Model.WorldLogic;
using Model.WorldLogic.TileLogic;

namespace Model.PawnLogic.EnemyLogic
{
    public class Enemy : Pawn
    {
        private static Random ran = new Random();
        private float _movementBias;
        private float _positionBias;
        private const float _mappingValue = 0.4000f;
        private float _health;
        protected float _lerpFactor = 0.09f;
        private static float _defaultHealth = 100;
        public int BaseDamage { get; set; }
        public Vector2 CurrentDirection = Vector2.Zero;
        public Vector2 DestinationDirection { get; set; } = Vector2.Zero;
        public enum EnemyType { Zombie, Bird };
        public EnemyType Type { get; internal set;}
        public Enemy(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints, int baseDamage) : base(position, chunk, radius, speed, lifePoints)
        {
            if(speed > 1)
            {
                _speed = speed + (float)ran.NextDouble() / speed;
            }
            else
            {
                _speed = speed + (float)ran.NextDouble()*speed;
            }
            
            _movementBias = (float)ran.NextDouble();
            _movementBias = _movementBias.Map(0, 1, -_mappingValue, _mappingValue);
            _positionBias = (float)ran.NextDouble();
            _positionBias = _positionBias.Map(0, 1, -_mappingValue, _mappingValue);
            _collision.Radius = _collision.Radius - _speed * 0.02f;
            _defaultHealth = lifePoints;
            _health = _defaultHealth;
            BaseDamage = baseDamage;
            Mass = 1f;
            Type = EnemyType.Zombie;
        }

        public void GetDamage(float damage)
        {

            _health -= damage;
            //Console.WriteLine("New Health: " + _health);
        }

        public float Health
        {
            get { return _health; }
        }

        public void MoveSmooth(Vector2 target, Vector2 handle, float deltaT, float t)
        {
            Vector2 AtoB = Vector2.Lerp(Position, handle, t);
            Vector2 BtoC = Vector2.Lerp(handle, target, t);
            Vector2 final = Vector2.Lerp(AtoB, BtoC, t);
            //_collision.Center += Vector2.Normalize(final) * _speed * deltaT;
            MoveTorwards(final, deltaT);
        }


        public void Update(Pawn target, Chunk chunk, Tile tile, float deltaT)
        {
            if (!tile.streetOut.free || chunk.Distance > 1)
            {
                if (!(chunk.Direction == WorldLogic.Chunk.DirectionToPlayer.NoWay))
                {
                    GridMovement(target, chunk);
                    
                }
                else
                {
                    if (target.Chunk.X == chunk.chunkX)
                    {
                        if (Position.Y > 2f && Position.Y < 3f)
                        {
                            if (tile.streetOut.left && tile.streetOut.right)
                            {
                                if (_movementBias > 0)
                                {
                                    DestinationDirection = new Vector2(1, 0);
                                }
                                else
                                {
                                    DestinationDirection = new Vector2(-1, 0);
                                }
                            }
                            else if (tile.streetOut.right)
                            {
                                DestinationDirection = new Vector2(1, 0);
                            }
                            else if (tile.streetOut.left)
                            {
                                DestinationDirection = new Vector2(-1, 0);
                            }
                        }
                        else
                        {
                            if (tile.streetOut.left || tile.streetOut.right)
                            {
                                DestinationDirection = new Vector2(2.5f + _movementBias, 2.5f + _movementBias) - Position;
                                DestinationDirection = Vector2.Normalize(DestinationDirection);
                            }
                        }
                    }
                    else if (target.Chunk.Y == chunk.chunkY)
                    {
                        if (Position.X > 2f && Position.X < 3f)
                        {
                            if (tile.streetOut.down)
                            {
                                DestinationDirection = new Vector2(0, -1);
                            }
                            else if (tile.streetOut.up)
                            {
                                DestinationDirection = new Vector2(0, 1);
                            }
                        }
                        else
                        {
                            DestinationDirection = new Vector2(2.5f + _movementBias, 2.5f + _movementBias) - Position;
                            DestinationDirection = Vector2.Normalize(DestinationDirection);
                        }

                    }
                    else
                    {
                        DestinationDirection = new Vector2(0, 0);
                    }
                }

            }
            else
            {
                DestinationDirection = Vector2.Normalize((target.Position + target.Chunk * 5) - (Position + Chunk * 5));
                CurrentDirection = Vector2.Lerp(CurrentDirection, DestinationDirection, _lerpFactor);
                MoveInDirection(CurrentDirection, deltaT);
                return;
                //MoveTorwards(target.Position, target.Chunk, deltaT);
            }

            if ((Position.X < 1.5f || Position.X > 3.5f) && (Position.Y < 1.5f || Position.Y > 3.5f))
            {
                CurrentDirection = DestinationDirection*0.25f;
            }
            else
            {
                CurrentDirection = Vector2.Lerp(CurrentDirection, DestinationDirection, _lerpFactor);
            }


                
            if (CurrentDirection.Length() > 1)
            {
                CurrentDirection = Vector2.Normalize(CurrentDirection);
            }
            MoveInDirection(CurrentDirection, deltaT);
        }

        public void GridMovement(Pawn target, Chunk chunk)
        {
            switch (chunk.Direction)
            {
                case WorldLogic.Chunk.DirectionToPlayer.ThisChunk:

                    DestinationDirection = target.Position - Position;
                    DestinationDirection = Vector2.Normalize(DestinationDirection);

                    break;

                case Model.WorldLogic.Chunk.DirectionToPlayer.Left:
                    if (_collision.Center.Y > 2f + _movementBias && _collision.Center.Y < 3f + _movementBias)
                    {
                        DestinationDirection = new Vector2(-1, 0);
                    }
                    else
                    {
                        DestinationDirection = new Vector2(2.5f + _movementBias, 2.5f + _movementBias) - Position;
                        DestinationDirection = Vector2.Normalize(DestinationDirection);
                    }

                    break;
                case Model.WorldLogic.Chunk.DirectionToPlayer.Right:
                    if (_collision.Center.Y > 2f + _movementBias && _collision.Center.Y < 3f + _movementBias)
                    {
                        DestinationDirection = new Vector2(1, 0);
                    }
                    else
                    {
                        DestinationDirection = new Vector2(2.5f, 2.5f) - Position;
                    }
                    DestinationDirection = Vector2.Normalize(DestinationDirection);
                    break;
                case Model.WorldLogic.Chunk.DirectionToPlayer.Top:
                    if (_collision.Center.X > 2f + _movementBias && _collision.Center.X < 3f + _movementBias)
                    {
                        DestinationDirection = new Vector2(0, 1);
                    }
                    else
                    {
                        DestinationDirection = new Vector2(2.5f + _movementBias, 2.5f + _movementBias) - Position;
                    }
                    DestinationDirection = Vector2.Normalize(DestinationDirection);
                    break;
                case Model.WorldLogic.Chunk.DirectionToPlayer.Down:
                    if (_collision.Center.X > 2f + _movementBias && _collision.Center.X < 3f + _movementBias)
                    {
                        DestinationDirection = new Vector2(0, -1);
                    }
                    else
                    {
                        DestinationDirection = new Vector2(2.5f + _movementBias, 2.5f + _movementBias) - Position;
                    }
                    DestinationDirection = Vector2.Normalize(DestinationDirection);
                    break;
                case Model.WorldLogic.Chunk.DirectionToPlayer.NoWay:
                    DestinationDirection = new Vector2(0, 0);

                    break;

                default:
                    break;
            }
        }

        public void Reset(Vector2 chunkPos, Vector2 pos)
        {
            _chunk = chunkPos;
            _collision.Center = pos;
            _health = _defaultHealth;
        }

    }
}
