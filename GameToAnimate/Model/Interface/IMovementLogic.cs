using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Controller;
using Model.PawnLogic;

namespace Model.InterfaceCollection
{
    public interface IMovementLogic
    {
        void Move(InputData inputData, Pawn target, float deltaT);

        bool IsMoving();


    }
}
