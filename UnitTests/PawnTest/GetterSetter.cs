using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Numerics;
using Zenseless.Geometry;
using Model.PawnLogic;
using Model.PawnLogic.PlayerLogic;

namespace UnitTests.PawnTest
{
    [TestClass]
    public class GetterSetter
    {
        [TestMethod]
        public void TestSetterGetter()
        {
            int lifePoint = 100;
            float radius = 10;
            Pawn pawn = new Player(new Vector2(1, 1), new Vector2(0, 0), radius, 10, lifePoint);

            pawn.Position = new Vector2(2, 2);

            Assert.AreEqual(new Vector2(2, 2), pawn.Position);

            pawn.Collider.Center = new Vector2(2, 2);

            Assert.AreEqual(new Vector2(2, 2), pawn.Collider.Center);


            pawn.LifePoints = 10;

            Assert.AreEqual(10, pawn.LifePoints);




            Assert.AreEqual(radius, pawn.Radius);
        }

    }
}
