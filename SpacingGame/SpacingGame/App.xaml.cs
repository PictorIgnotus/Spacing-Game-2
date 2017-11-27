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
        private const string asteroidImageLocation = "..\\..\\Images\\asteroid_23x23.png";
        private const string spaceshipImageLocation = "..\\..\\Images\\spaceship_23x23.png";
        private const int tsize = 23;
        bool isFirstGame;
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
            model = new GameModel(new Tablesize(tsize, tsize));
            model.GameOver += new EventHandler<GameEventArgs>(Model_GameOver);
            isFirstGame = true;

            // nézemodell létrehozása
            viewModel = new SGViewModel(model);
            viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

            // nézet létrehozása
            view = new MainWindow();
            view.DataContext = viewModel;
            view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            view.Show();

            // időzítő létrehozása
            

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

            model = new GameModel(new Tablesize(tsize, tsize));
            viewModel = new SGViewModel(model);
            view.DataContext = viewModel;
            CreateTable();
            SetSpaceshipImage(Image.FromFile(spaceshipImageLocation));

            model.GameOver += new EventHandler<GameEventArgs>(Game_GameOver);




            StartTimers();
            gameDuration.Restart();
            isPauseMode = false;
        }

        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            view.Close(); // ablak bezárása
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
            pauseButton.Hide();
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


        private void StartTimers()
        {
            moveAsteroidsTimer.Start();
            addNewAsteroidsTimer.Start();
            increaseAsteroidsNumberTimer.Start();
        }

        private void StopTimers()
        {
            moveAsteroidsTimer.Stop();
            addNewAsteroidsTimer.Stop();
            increaseAsteroidsNumberTimer.Stop();
        }
    }
}
