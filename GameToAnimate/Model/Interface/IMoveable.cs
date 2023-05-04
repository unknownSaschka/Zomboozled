using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace Model.InterfaceCollection
{
    interface IMoveable
    {
        void MoveTorwards(Vector2 targetPos, Vector2 targetChunk, float deltaT);
        void MoveTorwards(Vector2 targetPos, float deltaT);
        void MoveInDirection(Vector2 movementDirection, float deltaT);
        void MoveToPosition(Vector2 newPosition);
        void MoveToPosition(Vector2 newPosition, Vector2 newChunk);
    }
}
