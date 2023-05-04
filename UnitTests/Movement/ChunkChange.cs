using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Numerics;
using Model.PawnLogic;
using Model.PawnLogic.PlayerLogic;

namespace UnitTests.Movement
{
    [TestClass]
    public class ChunkChange
    {

        public void TestMovement(Vector2 origin, Vector2 newPos, Vector2 newChunk)
        {

            Pawn pawn = new Player(origin, new Vector2(0, 0), 2, 10, 100);

            pawn.MoveToPosition(newPos);
            pawn.UpdateCurrentChunk();

            Assert.AreEqual(pawn.Chunk, newChunk);
        }



        [TestMethod]
        public void TestChangeChunk()
        {
            TestMovement(new Vector2(1, 1), new Vector2(6, 1), new Vector2(1,0));
            TestMovement(new Vector2(1, 1), new Vector2(1, 6), new Vector2(0, 1));
            TestMovement(new Vector2(1, 1), new Vector2(-3, 1), new Vector2(-1, 0));
            TestMovement(new Vector2(1, 1), new Vector2(1, -3), new Vector2(0, -1));

            TestMovement(new Vector2(1, 1), new Vector2(-3, -3), new Vector2(-1, -1));
            TestMovement(new Vector2(1, 1), new Vector2(6, 6), new Vector2(1, 1));
            TestMovement(new Vector2(1, 1), new Vector2(-1, 6), new Vector2(-1, 1));
            TestMovement(new Vector2(1, 1), new Vector2(6, -1), new Vector2(1, -1));
        }

        [TestMethod]
        public void TestChangeChunkAndPosition()
        {
            Pawn pawn = new Player(new Vector2(1,1), new Vector2(0, 0), 2, 10, 100);
            var targetPos = new Vector2(2, 2);
            var targetChunk = new Vector2(1, 2);
            pawn.MoveToPosition(targetPos, targetChunk);


            Assert.AreEqual(targetPos, pawn.Position);
            Assert.AreEqual(targetChunk, pawn.Chunk);


        }


    }
}
