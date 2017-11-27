using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacingGame.Model
{
    public class GameModel
    {
        private Tablesize tablesize;
        private Spaceship spaceship;
        private int numberOfAsteroidsToCreate;
        private List<Asteroid> asteroids;
        public event EventHandler<GameEventArgs> GameOver;

        #region Properties
        public Tablesize TABLESIZE
        {
            get { return tablesize; }
        }

        public Spaceship SPACESHIP
        {
            get { return spaceship; }
        }

        public int NUMOFASTEROIDTOCREATE
        {
            get { return numberOfAsteroidsToCreate; }
        }

        public List<Asteroid> ASTEROIDS
        {
            get { return asteroids; }
        }
        #endregion

        public GameModel(Tablesize tablesize)
        {
            this.tablesize = tablesize;
            this.spaceship = new Spaceship(new Koordinate(this.tablesize.first / 2, this.tablesize.second - 1));
            this.asteroids = new List<Asteroid>();
            this.numberOfAsteroidsToCreate = 1;
        }

        #region AsteroidMethods
        public void CreateMoreAsteroids(int howMany)
        {
            this.numberOfAsteroidsToCreate += howMany;
        }

        public void AddNewAsteroids()
        {
            //to do not generate twice the same number
            List<int> possibleXkoords = new List<int>();

            List<int> xkoords = new List<int>();

            for (int i = 0; i < numberOfAsteroidsToCreate; ++i)
            {
                int x = RandomXkoorGenerator(tablesize.first);
                while (xkoords.Contains(x) && xkoords.Count < tablesize.first)
                {
                    x = RandomXkoorGenerator(tablesize.first);
                }
                xkoords.Add(x);
                asteroids.Add(new Asteroid(x));
            }
        }

        public void MoveDownAllAsteroids()
        {
            foreach (var asteroid in this.asteroids)
            {
                asteroid.MoveTo(SpaceObject.Direction.Down);
            }
            DeleteAsteroidsWichLeftTheBoard();
            if (IsGameOver())
            {
                OnGameOver();
            }
        }

        private void DeleteAsteroidsWichLeftTheBoard()
        {
            for (int i = 0; i < asteroids.Count && !IsAsteroidOnBoard(i); ++i)
            {
                asteroids.RemoveAt(i--);
            }
        }

        private bool IsAsteroidOnBoard(int which)
        {
            return (this.asteroids[which].KOORDINATE.second < this.tablesize.second);
        }
        #endregion

        #region SpaceshipMethods
        public void MoveSpaceShipLeft()
        {
            if (spaceship.KOORDINATE.first > 0)
                spaceship.MoveTo(SpaceObject.Direction.Left);
        }

        public void MoveSpaceShipRight()
        {
            if (spaceship.KOORDINATE.first < tablesize.first - 1)
                spaceship.MoveTo(SpaceObject.Direction.Right);
        }
        #endregion

        #region GameFunctions
        public bool IsGameOver()
        {
            bool gameOver = false;
            for (int i = 0; i < asteroids.Count && asteroids[i].KOORDINATE.second == tablesize.second - 1 && !gameOver; ++i)
            {
                if (AsteroidCrashedSpaceship(i))
                    gameOver = true;
            }
            return gameOver;
        }

        private bool AsteroidCrashedSpaceship(int whichAsteroid)
        {
            return asteroids[whichAsteroid].KOORDINATE.first == spaceship.KOORDINATE.first;
        }

        private int RandomXkoorGenerator(int max)
        {
            Random rand = new Random();
            return rand.Next(0, max);
        }
        #endregion

        private void OnGameOver()
        {
            GameOver?.Invoke(this, new GameEventArgs());
        }
    }
}
