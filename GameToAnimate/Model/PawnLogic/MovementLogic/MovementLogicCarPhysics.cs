using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model.InterfaceCollection;
using Model.PawnLogic.PlayerLogic;
using Model.PawnLogic.VehicleLogic;
using Model.WorldLogic;

namespace Model.PawnLogic
{
    class MovementLogicCarPhysics : IMovementLogic
    {
        private Vehicle _vehicle;
        private PlayerManager _playerManager;
        private Vector2 _targetDir;
        private Vector2 _movementDir = new Vector2(0, 1);
        private float _multiplier = 0;
        private float _steeringFactor = 1.2f;

        public MovementLogicCarPhysics(Vehicle vehicle, PlayerManager playerManager)
        {
            _vehicle = vehicle;
            _playerManager = playerManager;
            playerManager.Player.MoveToPosition(vehicle.Position, vehicle.Chunk);
        }

        public bool IsMoving()
        {
            if (_multiplier < -0.1f || _multiplier > 0.1f) return true;
            return false;
        }
        public void Move(InputData inputData, Pawn target, float deltaT)
        {
            //bool steering = false;
            if (inputData.yAxis != 0)
            {
                if (inputData.xAxis > 0)
                {
                    _targetDir = Vector2.Transform(_vehicle.CurrentDirection, Matrix3x2.CreateRotation(-_steeringFactor));
                    _movementDir = Vector2.Lerp(_movementDir, _targetDir, 0.02f);
                    _vehicle.CurrentDirection = Vector2.Normalize(_movementDir);
                    //Console.WriteLine(_targetDir);
                }
                else if (inputData.xAxis < 0)
                {
                    _targetDir = Vector2.Transform(_vehicle.CurrentDirection, Matrix3x2.CreateRotation(_steeringFactor));
                    _movementDir = Vector2.Lerp(_movementDir, _targetDir, 0.02f);
                    _vehicle.CurrentDirection = Vector2.Normalize(_movementDir);
                }
                else
                {
                    _movementDir = _vehicle.CurrentDirection;
                    
                }

                if (_multiplier < 1)
                {
                    _multiplier += 0.01f;
                }
                    



                _vehicle.MoveInDirection(_vehicle.CurrentDirection * inputData.yAxis * _multiplier, deltaT);

                target.MoveToPosition(_vehicle.Position);

            }
            else
            {
                if (_multiplier > 0)
                {
                    _multiplier -= 0.05f;
                }
                if(_multiplier > 0.1f)
                {
                    _vehicle.MoveInDirection(_vehicle.CurrentDirection * inputData.yAxis * _multiplier, deltaT);
                }
            }
        }
    }
}
