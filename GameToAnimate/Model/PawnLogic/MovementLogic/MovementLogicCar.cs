using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using System.Numerics;
using Model.InterfaceCollection;
namespace Model.PawnLogic
{
    public class MovementLogicCar : IMovementLogic
    {
        private Vehicle _vehicle;
        //private Player _player;
        private float wheelAxis = 0;
        private float steeringFactor = 0.8f;
        //private float accelerationFactor = 0.5f;
        //private float maxAngle = 40;
        public MovementLogicCar(Vehicle vehicle)
        {
            _vehicle = vehicle;
            //_player = player;

        } 
        public void Move(InputData inputData, Pawn pawn, float deltaT)
        {
            
            if(inputData.xAxis != 0)
            {
                wheelAxis += steeringFactor * inputData.xAxis * deltaT;
            }
            else
            {
                if (wheelAxis > 0)
                {
                    wheelAxis -= steeringFactor * deltaT;
                }
                else if (wheelAxis < 0)
                {
                    wheelAxis += steeringFactor * deltaT;
                }
                Console.WriteLine("Resetting: " + wheelAxis);
            }
            
            


            if (inputData.yAxis != 0)
            {
               
                _vehicle.MoveInDirection(_vehicle.currentDir * inputData.yAxis, deltaT);
                _vehicle.currentDir = Vector2.Normalize(Vector2.Transform(_vehicle.currentDir, Matrix3x2.CreateRotation(wheelAxis * deltaT)));
                _vehicle.UpdateCurrentChunk();
                pawn.MoveToPosition(_vehicle.Position, _vehicle.Chunk);
                
                Console.WriteLine("Current Rotation: " + _vehicle.currentDir);
            }
            pawn.UpdateCurrentChunk();
            //_player.MoveToPosition(_vehicle.Position);
            
        }
    }
}
