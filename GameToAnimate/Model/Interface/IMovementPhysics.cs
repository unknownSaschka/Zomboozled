using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interface
{
    interface IMovementPhysics
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 Acceleration { get; set; }
        float Mass { get; set; }
        void ApplyForce(Vector2 force);
    }
}
