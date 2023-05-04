using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Numerics;
using Zenseless.Geometry;
using Model.PawnLogic;
using Model.PawnLogic.PlayerLogic;

namespace UnitTests.Collision
{
    [TestClass]
    public class CollisionTest
    {
        MainModel model = new MainModel();

        [TestMethod]
        public void CheckBuildingPlayer()
        {
            Pawn player = new Player(new Vector2(1, 0.5f), new Vector2(0,0), 0.5f, 5, 100);
            Collisions collisions = new Collisions(model);
            InputData inputData = new InputData();

            //Player Intersected und wird zurückgesetzt
            List<Box2D> colBoxes = new List<Box2D>();
            colBoxes.Add(new Box2D(0, 0, 1, 1));
            collisions.CheckPlayerWithBuildingCollisions(player, colBoxes, ref inputData);

            Assert.AreEqual(player.Position, new Vector2(colBoxes[0].MaxX + player.Radius, 0.5f));

            //Player intersected nicht
            colBoxes = new List<Box2D>();
            player = new Player(new Vector2(4, 4), new Vector2(0, 0), 0.5f, 5, 100);
            colBoxes.Add(new Box2D(0, 0, 1, 1));
            collisions.CheckPlayerWithBuildingCollisions(player, colBoxes, ref inputData);

            Assert.AreEqual(player.Position, new Vector2(4, 4));
        }
    }
}
