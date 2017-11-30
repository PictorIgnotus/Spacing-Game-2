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
        /// Szüneteltetés parancs lekérdezése.
        /// </summary>
        public DelegateCommand PauseGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitGameCommand { get; private set; }

        public DelegateCommand MoveSpaceshipLeftCommand { get; private set; }
        public DelegateCommand MoveSpaceshipRightCommand { get; private set; }

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
        /// Játék szüneteltetése esemény 
        /// </summary>
        public event EventHandler PauseGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitGame;

        public event EventHandler MoveSpaceshipLeft;
        public event EventHandler MoveSpaceshipRight;

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

            MoveSpaceshipLeftCommand = new DelegateCommand(param => OnMoveSpaceshipLeft());
            MoveSpaceshipRightCommand = new DelegateCommand(param => OnMoveSpaceshipRight());
            NewGameCommand = new DelegateCommand(param => { ClearTable(); OnNewGame();});
            PauseGameCommand = new DelegateCommand(param => OnPauseGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());

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
        }

        #endregion

        private void ClearTable()
        {
            foreach (var Field in Fields)
            {
                Field.ImageSource = null;
            }
        }


        private void Model_GameOver(object sender, GameEventArgs e)
        {

        }

        private void OnNewGame()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }

        private void OnPauseGame()
        {
            if (PauseGame != null)
                PauseGame(this, EventArgs.Empty);
        }

        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        private void OnMoveSpaceshipLeft()
        {
            if (MoveSpaceshipLeft != null)
                MoveSpaceshipLeft(this, EventArgs.Empty);
        }

        private void OnMoveSpaceshipRight()
        {
            if (MoveSpaceshipRight != null)
                MoveSpaceshipRight(this, EventArgs.Empty);
        }

        public void SetAsteroidsImage(String imageSource)
        {
            var asteroids = model.ASTEROIDS;
            foreach (var asteroid in asteroids)
            {
                var koordinates = asteroid.KOORDINATE;
                Fields[koordinates.second * model.TABLESIZE.first + koordinates.first].ImageSource = imageSource;
            }
        }

        public void SetSpaceshipImage(String imageSource)
        {
            var spaceshipkoor = model.SPACESHIP.KOORDINATE;
            Fields[spaceshipkoor.second * model.TABLESIZE.first + spaceshipkoor.first].ImageSource = imageSource;
        }
    }
}
