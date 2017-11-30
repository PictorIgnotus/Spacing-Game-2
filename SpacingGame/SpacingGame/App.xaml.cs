using System;
using SpacingGame.Model;
using SpacingGame.ModelView;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace SpacingGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Field 

        private GameModel model;
        private SGViewModel viewModel;
        private MainWindow view;
        private DispatcherTimer moveAsteroidsTimer;
        private DispatcherTimer addNewAsteroidsTimer;
        private DispatcherTimer increaseAsteroidsNumberTimer;
        private const string asteroidImageLocation = "f:\\git\\Spacing-Game-2\\SpacingGame\\SpacingGame\\Images\\asteroid_23x23.png";
        private const string spaceshipImageLocation = "f:\\git\\Spacing-Game-2\\SpacingGame\\SpacingGame\\Images\\spaceship_23x23.png";
        private const int tsize = 23;
        bool isFirstGame;
        bool isPlaying;
        private bool isPauseMode;
        private Stopwatch gameDuration;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // modell létrehozása
            model = new GameModel(new Tablesize(tsize,tsize));
            model.GameOver += new EventHandler<GameEventArgs>(Model_GameOver);
            model.NewGame();

            // nézemodell létrehozása
            viewModel = new SGViewModel(model);
            viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);
            viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            viewModel.MoveSpaceshipLeft += new EventHandler(ViewModel_MoveSpaceshipLeft);
            viewModel.MoveSpaceshipRight += new EventHandler(ViewModel_MoveSpaceshipRight);

            // nézet létrehozása
            view = new MainWindow();
            view.DataContext = viewModel;
            view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            view.Show();

            isFirstGame = true;
            isPlaying = false;
            gameDuration = new Stopwatch();
         }
        #endregion


        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            if (isFirstGame)
            {
                moveAsteroidsTimer = new DispatcherTimer();
                moveAsteroidsTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                moveAsteroidsTimer.Tick += new EventHandler(MoveAsteroidsTimer_Tick);

                addNewAsteroidsTimer = new DispatcherTimer();
                addNewAsteroidsTimer.Interval = new TimeSpan(0, 0, 0, 0, 700);
                addNewAsteroidsTimer.Tick += new EventHandler(AddNewAsteroidsTimer_Tick);

                increaseAsteroidsNumberTimer = new DispatcherTimer();
                increaseAsteroidsNumberTimer.Interval = new TimeSpan(0, 0, 0, 1, 500);
                increaseAsteroidsNumberTimer.Tick += new EventHandler(IncreaseAsteroidsNumberTimer_Tick);

                isFirstGame = false;
            }

            model.NewGame();

            viewModel.SetAsteroidsImage(null);
            viewModel.SetSpaceshipImage(spaceshipImageLocation);

            StartTimers();
            gameDuration.Restart();
            isPauseMode = false;
            isPlaying = true;
        }

        private void ViewModel_PauseGame(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                if (!isPauseMode)
                {
                    StopTimers();
                    gameDuration.Stop();
                }
                else
                {
                    StartTimers();
                    gameDuration.Start();
                }
                isPauseMode = !isPauseMode;
            }
        }

        private void ViewModel_ExitGame(object sender, EventArgs e)
        {
            view.Close(); // ablak bezárása
        }

        private void ViewModel_MoveSpaceshipLeft(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                viewModel.SetSpaceshipImage(null);
                model.MoveSpaceshipLeft();
                viewModel.SetSpaceshipImage(spaceshipImageLocation);
            }
        }


        private void ViewModel_MoveSpaceshipRight(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                viewModel.SetSpaceshipImage(null);
                model.MoveSpaceshipRight();
                viewModel.SetSpaceshipImage(spaceshipImageLocation);
            }
        }
        private void View_Closing(object sender, CancelEventArgs e)
        {
            StopTimers();

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Sudoku", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást

                StartTimers();
                gameDuration.Start();
            }
        }


        private void Model_GameOver(object sender, GameEventArgs e)
        {
            StopTimers();
            isPlaying = false;
            gameDuration.Stop();
            TimeSpan ts = gameDuration.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                                                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            MessageBox.Show("Sajnos a játék itt véget ért, aszteroidának ütköztél!" + Environment.NewLine +
                            "Ennyi időt játszottál: " + elapsedTime,
                                "Játék vége",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
        }

        private void MoveAsteroidsTimer_Tick(object sender, EventArgs e)
        {
            viewModel.SetAsteroidsImage(null);
            model.MoveDownAllAsteroids();
            viewModel.SetAsteroidsImage(asteroidImageLocation);
        }

        private void AddNewAsteroidsTimer_Tick(object sender, EventArgs e)
        {
            model.AddNewAsteroids();
        }

        private void IncreaseAsteroidsNumberTimer_Tick(object sender, EventArgs e)
        {
            model.CreateMoreAsteroids(1);
        }

        private void StartTimers()
        {
            moveAsteroidsTimer.Start();
            addNewAsteroidsTimer.Start();
            increaseAsteroidsNumberTimer.Start();
        }

        private void StopTimers()
        {
            if (moveAsteroidsTimer != null && addNewAsteroidsTimer != null && increaseAsteroidsNumberTimer != null)
            {
                moveAsteroidsTimer.Stop();
                addNewAsteroidsTimer.Stop();
                increaseAsteroidsNumberTimer.Stop();
            }
        }
    }
}
