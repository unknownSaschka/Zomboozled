using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using System.Numerics;
using Model.InterfaceCollection;
using Model.PawnLogic.PlayerLogic;

namespace Model.PawnLogic
{
    public class MovementLogicFoot : IMovementLogic
    {
        private Player _player;
        bool _isMoving = false;
        public MovementLogicFoot(Player player)
        {
            _player = player;
        }

        public bool IsMoving() { return _isMoving; }
        public void Move(InputData inputData, Pawn target, float deltaT)
        {
            if (inputData.xAxis != 0 || inputData.yAxis != 0)
            {
                if (Math.Abs(inputData.xAxis).Equals(Math.Abs(inputData.yAxis)))
                {
                    target.MoveInDirection(new Vector2((inputData.xAxis / 1.4f), (float)(inputData.yAxis / 1.4f)), deltaT);

                }
                else
                {
                    target.MoveInDirection(new Vector2(inputData.xAxis, inputData.yAxis), deltaT);
                    
                }

                _player.ViewDirection = new Vector2(inputData.xAxis, inputData.yAxis);
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
        }
    }
}
