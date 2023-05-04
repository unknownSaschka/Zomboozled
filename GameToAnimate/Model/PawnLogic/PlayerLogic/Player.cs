using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace Model.PawnLogic.PlayerLogic
{
    public class Player : Pawn
    {
        public bool IsInCar = false;
        private int _money = 0;
        public int Money
        {
            get { return _money; }
            set { _money = value; }
        }

        public int BaseDamage { get; set; } = 0;
        public Player(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints) : base(position, chunk, radius, speed, lifePoints)
        {
            //_collision.Center = position;
            ViewDirection = new Vector2(1, 0);
        }

        public void ChangePlayerChunkToZero()
        {
            _chunk = new Vector2(1, 0);
            
        }
        
        public void ResetPosition()
        {
            Position = new Vector2(2.5f, 2.5f);
            Chunk = new Vector2(0f, 0f);
            
        }

        public void IncreaseBaseDamage()
        {
            BaseDamage += 20;
        }
    }

    

}
