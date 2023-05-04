using Model.WorldLogic;
using Model.WorldLogic.TileLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.EnemyLogic
{
    class FlyingEnemy : Enemy
    {
        public FlyingEnemy(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints, int baseDamage) : base(position, chunk, radius, speed, lifePoints, baseDamage)
        {
            Type = EnemyType.Bird;
            _lerpFactor = 0.009f;

        }

        public new void Update(Pawn target, Chunk chunk, Tile tile, float deltaT)
        {
            DestinationDirection = Vector2.Normalize((target.Position + target.Chunk * 5) - (Position + Chunk * 5));
            CurrentDirection = Vector2.Lerp(CurrentDirection, DestinationDirection, _lerpFactor);
            MoveInDirectionNormalized(CurrentDirection, deltaT);
        }
    }
}
