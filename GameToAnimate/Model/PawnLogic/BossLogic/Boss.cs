using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.BossLogic
{
    public class Boss : Pawn
    {
        public Vector2 _baseChunk;
        public Vector2 CurrentDirection = Vector2.Zero;
        public enum BossType { Malos, Gyorg, Keller, Saitama, Sans }
        public BossType TypeOfBoss { get; internal set; }

        protected Boss(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints) : base(position, chunk, radius, speed, lifePoints)
        {
            _baseChunk = chunk;
        }

        public void GetDamage(int damage)
        {
            _lifePoints -= damage;
        }
    }
}
