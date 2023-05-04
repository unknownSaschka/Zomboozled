using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Numerics;
using Model.PawnLogic;
using Model.PawnLogic.VehicleLogic;

namespace UnitTests.PawnTest.VehicleTest
{
    [TestClass]
    public class VehicleTestClass
    {
        [TestMethod]
        public void TestEnterLeave()
        {
            Vehicle vehicle = new Vehicle(new Vector2(0, 0), new Vector2(0, 0), 2, 10, 400, CarType.Car);

            vehicle.Enter();

            Assert.AreEqual(true, vehicle.CarEnabled);

            vehicle.Leave();

            Assert.AreEqual(false, vehicle.CarEnabled);
        }

    }
}
