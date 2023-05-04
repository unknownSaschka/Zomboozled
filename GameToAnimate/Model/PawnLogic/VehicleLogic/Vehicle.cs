using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Model.PawnLogic.VehicleLogic;

namespace Model.PawnLogic.VehicleLogic
{
    public class Vehicle : Pawn
    {
        private bool _isActive;
        //private Vector2 _movementDirection;

     
        public Vector2 CurrentDirection { get; set; }
        public CarType CarType { get; internal set; }
        public Vehicle(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints, CarType carType) : base(position, chunk, radius, speed, lifePoints)
        {
           CurrentDirection = new Vector2(1, 0);
            CarType = carType;
            
        }


        public bool CarEnabled
        {
            get { return _isActive; }
        }

        public void Enter()
        {
            _isActive = true;
            

        }

        public void Leave()
        {
            _isActive = false;
        }
    }
}
