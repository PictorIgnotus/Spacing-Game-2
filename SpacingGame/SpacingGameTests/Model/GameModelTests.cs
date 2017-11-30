using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpacingGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacingGame.Model.Tests
{
    [TestClass()]
    public class GameModelTests
    {
        #region SpaceshipTest
        Spaceship spaceship = new Spaceship(new Koordinate(5, 5));
        [TestMethod]
        public void SpaceShipTest()
        {
            Assert.AreEqual(new Koordinate(5, 5), spaceship.KOORDINATE);
        }

        [TestMethod]
        public void SpaceShipMoveLeftTest()
        {
            spaceship.MoveTo(SpaceObject.Direction.Left);
            Assert.AreEqual(new Koordinate(4, 5), spaceship.KOORDINATE);
        }

        [TestMethod]
        public void SpaceShipMoveRightTest()
        {
            spaceship.MoveTo(SpaceObject.Direction.Right);
            Assert.AreEqual(new Koordinate(6, 5), spaceship.KOORDINATE);
        }
        #endregion

        #region AsteroidTest

        Asteroid asteroid = new Asteroid(10);
        [TestMethod]
        public void AsteroidTest()
        {
            Assert.AreEqual(new IntPair(10, 0), asteroid.KOORDINATE);
        }

        [TestMethod]
        public void AsteroidMoveDownTest()
        {
            asteroid.MoveTo(SpaceObject.Direction.Down);
            Assert.AreEqual(new IntPair(10, 1), asteroid.KOORDINATE);
        }
        #endregion

        #region GameModel

        GameModel model = new GameModel(new Tablesize(21, 40));
        [TestMethod]
        public void GameModelTest()
        {
            Assert.AreEqual(new Tablesize(21, 40), model.TABLESIZE);
            Assert.AreEqual(1, model.NUMOFASTEROIDTOCREATE);
            Assert.AreEqual(0, model.ASTEROIDS.Count);
        }

        [TestMethod]
        public void GameModelSpaceshipInitializationTest()
        {
            Assert.AreEqual(new Koordinate(10, 39), model.SPACESHIP.KOORDINATE);
        }

        [TestMethod]
        public void GameModelCreateThreeMoreAsteroid()
        {
            model.CreateMoreAsteroids(3);
            Assert.AreEqual(4, model.NUMOFASTEROIDTOCREATE);
        }

        [TestMethod]
        public void GameModelAddNewAsteroids()
        {
            model.CreateMoreAsteroids(3);
            model.AddNewAsteroids();
            Assert.AreEqual(4, model.ASTEROIDS.Count);
        }

        [TestMethod]
        public void GameModelMoveAllAsteroids()
        {
            model.CreateMoreAsteroids(2);
            model.AddNewAsteroids();
            Assert.AreEqual(3, model.ASTEROIDS.Count);
            foreach (var asteroid in model.ASTEROIDS)
            {
                Assert.AreEqual(0, asteroid.KOORDINATE.second);
            }
            model.MoveDownAllAsteroids();
            foreach (var asteroid in model.ASTEROIDS)
            {
                Assert.AreEqual(1, asteroid.KOORDINATE.second);
            }

        }

        [TestMethod]
        public void GameModelMoveAllAsteroidsTenTimes()
        {
            model.CreateMoreAsteroids(2);
            model.AddNewAsteroids();
            Assert.AreEqual(3, model.ASTEROIDS.Count);
            foreach (var asteroid in model.ASTEROIDS)
            {
                Assert.AreEqual(0, asteroid.KOORDINATE.second);
            }
            for (int i = 0; i < 10; ++i)
            {
                model.MoveDownAllAsteroids();
            }
            foreach (var asteroid in model.ASTEROIDS)
            {
                Assert.AreEqual(10, asteroid.KOORDINATE.second);
            }
        }

        [TestMethod]
        public void GameModelDeleteAsteroidsWhichLeftTheBoard()
        {
            model.CreateMoreAsteroids(2);
            model.AddNewAsteroids();
            Assert.AreEqual(3, model.ASTEROIDS.Count);
            for (int i = 0; i < 39; ++i)
            {
                model.MoveDownAllAsteroids();
            }
            Assert.AreEqual(3, model.ASTEROIDS.Count);
            model.AddNewAsteroids();
            Assert.AreEqual(6, model.ASTEROIDS.Count);
            model.MoveDownAllAsteroids();
            foreach (var asteroid in model.ASTEROIDS)
            {
                Assert.AreEqual(1, asteroid.KOORDINATE.second);
            }
            Assert.AreEqual(3, model.ASTEROIDS.Count);
        }

        [TestMethod]
        public void GameModelMoveLeftSpaceShip()
        {
            model.MoveSpaceshipLeft();
            Assert.AreEqual(new Koordinate(9, 39), model.SPACESHIP.KOORDINATE);
        }

        [TestMethod]
        public void GameModelMoveRightSpaceShip()
        {
            model.MoveSpaceshipRight();
            Assert.AreEqual(new Koordinate(11, 39), model.SPACESHIP.KOORDINATE);
        }

        [TestMethod]
        public void GameModelMoveTooLeftSpaceShip()
        {
            for (int i = 0; i < 30; ++i)
            {
                model.MoveSpaceshipLeft();
            }
            Assert.AreEqual(new Koordinate(0, 39), model.SPACESHIP.KOORDINATE);
        }

        [TestMethod]
        public void GameModelMoveTooRightSpaceShip()
        {
            for (int i = 0; i < 30; ++i)
            {
                model.MoveSpaceshipRight();
            }
            Assert.AreEqual(new Koordinate(20, 39), model.SPACESHIP.KOORDINATE);
        }

        [TestMethod]
        public void GameModelIsGameOverReturnFalse()
        {
            Assert.AreEqual(false, model.IsGameOver());
        }

        [TestMethod]
        public void GameModelAddUniqueAsteroids()
        {
            model.CreateMoreAsteroids(20);
            model.AddNewAsteroids();
            Assert.AreEqual(21, model.ASTEROIDS.Count);
            for (int i = 0; i < model.ASTEROIDS.Count - 1; ++i)
            {
                for (int j = i + 1; j < model.ASTEROIDS.Count; ++j)
                {
                    Assert.AreNotEqual(model.ASTEROIDS[i].KOORDINATE.first, model.ASTEROIDS[j].KOORDINATE.first);
                    Assert.AreEqual(model.ASTEROIDS[i].KOORDINATE.second, model.ASTEROIDS[j].KOORDINATE.second);
                }
            }
        }

        [TestMethod]
        public void GameModelIsGameOverReturnTrue()
        {
            model.CreateMoreAsteroids(20);
            model.AddNewAsteroids();
            Assert.AreEqual(21, model.ASTEROIDS.Count);
            for (int i = 0; i < 10; ++i)
            {
                model.MoveSpaceshipLeft();
            }
            for (int i = 0; i < 39; ++i)
            {
                model.MoveDownAllAsteroids();
            }
            foreach (var asteroid in model.ASTEROIDS)
            {
                Assert.AreEqual(39, asteroid.KOORDINATE.second);
            }
            Assert.AreEqual(new Koordinate(0, 39), model.SPACESHIP.KOORDINATE);
            Assert.AreEqual(true, model.IsGameOver());
        }
        #endregion
    }
}