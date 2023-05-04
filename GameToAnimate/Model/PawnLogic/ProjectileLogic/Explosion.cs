using Model.PawnLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.ProjectileLogic
{
    public class Explosion : Pawn
    {
        public double LifeTimeMax { get; internal set; } = 0.9;
        public double LifeTime { get; internal set; } = 0.75;

        public bool PlayerDamage { get; internal set; }
        
        public bool Exploded { get; set; } = false;
        public float Damage { get; internal set; }

        public Explosion(Vector2 position, Vector2 chunk, float radius, float damage, bool playerDamage) : base(position, chunk, radius, 0, 1)
        {
            Damage = damage;
            Exploded = false;
            LifeTime = LifeTimeMax;
            PlayerDamage = playerDamage;
            Console.WriteLine($"Grenade DMG {Damage}");
        }

        public new void Update(float deltaT)
        {
            if(LifeTime > 0)
            {
                
                LifeTime -= deltaT;
                //Console.WriteLine("Updated Explosion! " + LifeTime);
            }
            
        }
    }
}
