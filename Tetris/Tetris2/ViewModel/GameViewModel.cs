using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Tetris.Model;

namespace Tetris2.ViewModel
{
    class GameViewModel : ViewModelBase
    {

        #region Fields

        private TetrisModel _model;
        private Int32 RowCount;
        private Int32 Heighte;

        #endregion

        #region Properties

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SmallGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand MediumGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LargeGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Szünet parancs lekérdezése.
        /// </summary>
        public DelegateCommand PauseCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<GameField> Fields { get; set; }

        /// <summary>
        /// Táblaméret lekérdezése.
        /// </summary>
        public Int32 GameTableSize { get { return _model.table.Size; } }

        /// <summary>
        /// Játékidő lekérdezése.
        /// </summary>
        public String GameTime { get { return TimeSpan.FromSeconds(_model.table.Time).ToString("g"); } }

        /// <summary>
        /// Alacsony nehézségi szint állapotának lekérdezése.
        /// </summary>
        public Boolean IsGameSmall
        {
            get { return _model.GameSize == GameSize.Small; }
            set
            {
                if (_model.GameSize == GameSize.Small)
                    return;

                _model.GameSize = GameSize.Small;
                OnPropertyChanged("IsGameSmall");
                OnPropertyChanged("IsGameMedium");
                OnPropertyChanged("IsGameLarge");
            }
        }

        /// <summary>
        /// Közepes nehézségi szint állapotának lekérdezése.
        /// </summary>
        public Boolean IsGameMedium
        {
            get { return _model.GameSize == GameSize.Medium; }
            set
            {
                if (_model.GameSize == GameSize.Medium)
                    return;

                _model.GameSize = GameSize.Medium;
                OnPropertyChanged("IsGameSmall");
                OnPropertyChanged("IsGameMedium");
                OnPropertyChanged("IsGameLarge");
            }
        }

        /// <summary>
        /// Magas nehézségi szint állapotának lekérdezése.
        /// </summary>
        public Boolean IsGameLarge
        {
            get { return _model.GameSize == GameSize.Large; }
            set
            {
                if (_model.GameSize == GameSize.Large)
                    return;

                _model.GameSize = GameSize.Large;
                OnPropertyChanged("IsGameSmall");
                OnPropertyChanged("IsGameMedium");
                OnPropertyChanged("IsGameLarge");
            }
        }

        public DelegateCommand MoveRightCommand { get; private set; }

        public DelegateCommand MoveLeftCommand { get; private set; }

        public DelegateCommand MoveUpCommand { get; private set; }

        public DelegateCommand MoveDownCommand { get; private set; }

        public Int32 Rows
        {
            get => RowCount;
            set
            {
                if (RowCount != value)
                {
                    RowCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 WindowHeight
        {
            get => Heighte;
            set
            {
                if (Heighte != value)
                {
                    Heighte = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler NewGame;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler SaveGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitGame;

        /// <summary>
        /// Játék szüneteltetésének eseménye.
        /// </summary>
        public event EventHandler PauseGame;

        public event EventHandler SmallGame;

        public event EventHandler MediumGame;

        public event EventHandler LargeGame;

        public event EventHandler Left;

        public event EventHandler Right;

        public event EventHandler Up;

        public event EventHandler Down;

        #endregion

        #region Constructors

        public GameViewModel(TetrisModel model)
        {
            // játék csatlakoztatása
            _model = model;
            _model.GameAdvanced += new EventHandler<TetrisEventArgs>(model_gameAdvanced);
            _model.GameOver += new EventHandler<TetrisEventArgs>(model_gameOver);

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            PauseCommand = new DelegateCommand(param => OnPauseGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            SmallGameCommand = new DelegateCommand(param => OnSmallGame());
            MediumGameCommand = new DelegateCommand(param => OnMediumGame());
            LargeGameCommand = new DelegateCommand(param => OnLargeGame());
            MoveRightCommand = new DelegateCommand(param => RightPressed());
            MoveLeftCommand = new DelegateCommand(param => LeftPressed());
            MoveUpCommand = new DelegateCommand(param => UpPressed());
            MoveDownCommand = new DelegateCommand(param => DownPressed());

            // játéktábla létrehozása
            Fields = new ObservableCollection<GameField>();
            GenerateFields();
            RefreshTable();

            this.WindowHeight = 400;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Mezők generálása.
        /// </summary>
        private void GenerateFields()
        {
            Fields.Clear();
            for (Int32 i = 0; i < _model.table.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < 16; j++)
                {
                    Fields.Add(new GameField
                    {
                        IsLocked = false,
                        Type = 0,
                        X = i,
                        Y = j,
                        Number = i * _model.table.Size + j,
                        //StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
        }

        /// <summary>
        /// Tábla frissítése.
        /// </summary>
        private void RefreshTable()
        {
            foreach (GameField field in Fields) // inicializálni kell a mezőket is
            {
                field.Type = _model.table.TypeTable[field.X+1,field.Y];
                //field.IsLocked = _model.table.IsLocked(field.X, field.Y);
            }

            OnPropertyChanged("GameTime");
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void model_gameOver(object sender, TetrisEventArgs e)
        {
            foreach (GameField field in Fields)
            {
                field.IsLocked = true;
            }
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void model_gameAdvanced(object sender, TetrisEventArgs e)
        {
            OnPropertyChanged("GameTime");
            RefreshTable();
        }

        #endregion

        #region Event methods

        private void OnSmallGame()
        {
            if (SmallGame != null)
                SmallGame(this, EventArgs.Empty);

            GenerateFields();
            RefreshTable();
        }

        private void OnMediumGame()
        {
            if (MediumGame != null)
                MediumGame(this, EventArgs.Empty);
            GenerateFields();
            RefreshTable();
        }

        private void OnLargeGame()
        {

            if (LargeGame != null)
                LargeGame(this, EventArgs.Empty);
            GenerateFields();
            RefreshTable();
        }

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGame()
        {

            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
            GenerateFields();
            RefreshTable();
        }


        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék szüneteltetésének eseménykiváltása.
        /// </summary>
        private void OnPauseGame()
        {
            if (PauseGame != null)
                PauseGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        private void RightPressed()
        {
            if (Right != null)
                Right(this, EventArgs.Empty);
            _model.GoRight();
        }

        private void LeftPressed()
        {
            if (Left != null)
                Left(this, EventArgs.Empty);
            _model.GoLeft();
        }
        private void UpPressed()
        {
            if (Up != null)
                Up(this, EventArgs.Empty);
            _model.RotateMeSenpai();
        }
        private void DownPressed()
        {
            if (Down != null)
                Down(this, EventArgs.Empty);
            _model.GoDown();
        }

        #endregion
    }
}
