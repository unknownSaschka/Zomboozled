using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Numerics;
using Model.PawnLogic;
using Model.InterfaceCollection;
using Model.PawnLogic.PlayerLogic;

namespace UnitTests.Movement
{
    [TestClass]
    public class MovementInChunk
    {
        /*
        [TestMethod]
        public void TestDirectionMovement()
        {
            float speed = 1;
            Vector2 originPos = new Vector2(1, 1);
            float deltaT = 100;
            Pawn pawn = new Player(originPos, new Vector2(0, 0), 2, speed, 100);

            pawn.MoveInDirection(new Vector2(1, 1), deltaT);

            Assert.AreEqual(pawn.Position, deltaT * speed * Vector2.Normalize(originPos)+ originPos);
        }
        */
        [TestMethod]
        public void TestTorwardsPoint()
        {
            float speed = 1;
            Vector2 originPos = new Vector2(1, 1);
            Vector2 newPos = new Vector2(2, 2);
            Pawn pawn = new Player(originPos, new Vector2(0, 0), 2, speed, 100);

            for(int i=0; i<240; ++i)
            {
                pawn.MoveTorwards(newPos, 0.05f);
            }
            

            Assert.AreEqual(newPos, new Vector2((float)System.Math.Round(pawn.Position.X), (float)System.Math.Round(pawn.Position.Y)));
            
        }

        [TestMethod]
        public void TestTorwardsPointInAnotherChunk()
        {
            float speed = 1;
            Vector2 originPos = new Vector2(1, 1);
            Vector2 newPos = new Vector2(2, 2);
            Pawn pawn = new Player(originPos, new Vector2(0, 0), 2, speed, 100);

            for (int i = 0; i < 240; ++i)
            {
                pawn.MoveTorwards(newPos, new Vector2(1,1), 0.05f);
            }

            pawn.UpdateCurrentChunk();

            Assert.AreEqual(newPos, new Vector2((float)System.Math.Round(pawn.Position.X), (float)System.Math.Round(pawn.Position.Y)));

        }
    }
}
