using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.PawnLogic;
using Model.PawnLogic.PlayerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Model.PawnLogic.Tests
{
    [TestClass()]
    public class PawnTests
    {
        [TestMethod()]
        public void AddDamageTestWithoutIgnoringAndArmour()
        {
            var p = new Player(Vector2.Zero, Vector2.Zero, 0, 0, 100);

            p.AddDamage(10, 0);

            Assert.AreEqual(90, p.LifePoints);
        }

        [TestMethod()]
        public void AddDamageTestWithIgnoringAndWithoutArmour()
        {
            var p = new Player(Vector2.Zero, Vector2.Zero, 0, 0, 100);

            p.AddDamage(10, 5);

            Assert.AreEqual(90, p.LifePoints);
        }
    }
}