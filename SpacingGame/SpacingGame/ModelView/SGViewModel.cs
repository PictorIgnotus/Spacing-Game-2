using System;
using System.Collections.ObjectModel;
using SpacingGame.Model;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpacingGame.ModelView
{
    public class SGViewModel : ViewModelBase
    {
        #region Fields

        private GameModel model;

        #endregion

        #region Properties 

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<SGField> Fields { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler NewGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitGame;

        #endregion

        #region Constructors

        /// <summary>
        /// Sudoku nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public SGViewModel(GameModel model)
        {
            // játék csatlakoztatása
            this.model = model;
            this.model.GameOver += new EventHandler<GameEventArgs>(Model_GameOver);

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => { OnNewGame(); RefreshTable(); });
            ExitCommand = new DelegateCommand(param => OnExitGame());

            // játéktábla létrehozása
            Fields = new ObservableCollection<SGField>();
            for (Int32 i = 0; i < model.TABLESIZE.first; ++i) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < model.TABLESIZE.second; ++j)
                {
                    Fields.Add(new SGField
                    {
                        ImageSource = null,
                        X = i,
                        Y = j,
                        Number = i * model.TABLESIZE.first + j
                    });
                }
            }

            SetSpaceshipImage()
        }

        #endregion

        private void Model_GameOver(object sender, GameEventArgs e)
        {
            
        }

        private void OnNewGame()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }

        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        private void SetAsteroidsImage(String imageSource)
        {
            var asteroids = model.ASTEROIDS;
            foreach (var asteroid in asteroids)
            {
                var koordinates = asteroid.KOORDINATE;
                Fields[koordinates.first * model.TABLESIZE.second + koordinates.second].ImageSource = imageSource;
            }
        }

        private void SetSpaceshipImage(String imageSource)
        {
            var spaceshipkoor = model.SPACESHIP.KOORDINATE;
            Fields[spaceshipkoor.first * model.TABLESIZE.second + spaceshipkoor.second].ImageSource = imageSource;
        }
    }
}
