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
    class MovementLogicCarSimple : IMovementLogic
    {
        //private float _maxSteeringAngle = 40;
        private Vehicle _vehicle;
        private PlayerManager _playerManager;
        private bool _isMoving = false;
        public bool IsMoving() { return _isMoving; }
        public MovementLogicCarSimple(Vehicle vehicle, PlayerManager playerManager)
        {
            _vehicle = vehicle;
            _playerManager = playerManager;
            playerManager.Player.MoveToPosition(vehicle.Position, vehicle.Chunk);
        }

        public void Move(InputData inputData, Pawn target, float deltaT)
        {
            if(inputData.xAxis != 0 || inputData.yAxis != 0)
            {
                //_vehicle.currentDir = new System.Numerics.Vector2(inputData.xAxis, inputData.yAxis);
                _vehicle.CurrentDirection = new System.Numerics.Vector2(inputData.xAxis, inputData.yAxis);

                _vehicle.MoveInDirection(_vehicle.CurrentDirection, deltaT);

                //target.CheckPlayerChunk(model.ManagerHolder, model.World, model.Generator);
                
                if (_vehicle.UpdateCurrentChunk())
                {
                    _playerManager.Player.MoveToPosition(_vehicle.Position, _vehicle.Chunk);
                    _playerManager.PlayerChunkChangedEvent?.Invoke();
                }
                else
                {
                    target.MoveToPosition(_vehicle.Position);
                }
                //
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
        }
    }
}
