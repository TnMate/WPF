using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Persistence;

namespace Tetris.Model
{

    public enum GameSize { Small, Medium, Large}

    public class TetrisModel
    {
        #region GameSize constans

        private const Int16 GameSizeSmall = 4;
        private const Int16 GameSizeMedium = 8;
        private const Int16 GameSizeLarge = 12;

        #endregion

        #region Fields

        private TetrisDataAccessInterface _dataAccess;
        private TetrisTable _table;
        private GameSize _gameSize;
        private Boolean _isLost;

        #endregion

        #region Events

        public event EventHandler<TetrisEventArgs> GameAdvanced;

        public event EventHandler<TetrisEventArgs> GameOver;

        #endregion

        #region Constructors

        public TetrisModel(TetrisDataAccessInterface dataAccess = null)
        {
            _gameSize = GameSize.Medium;
            _table = new TetrisTable();
            _dataAccess = dataAccess;
        }

        #endregion

        #region Public methods

        public void NewGame()
        {
            _isLost = false;
            switch (_gameSize)
            {
                case GameSize.Small:
                    _table = new TetrisTable(GameSizeSmall);
                    break;
                case GameSize.Medium:
                    _table = new TetrisTable(GameSizeMedium);
                    break;
                case GameSize.Large:
                    _table = new TetrisTable(GameSizeLarge);
                    break;
                default:
                    _table = new TetrisTable();
                    break;
            }
        }

        public void AdvanceTime()
        {
            if(!_table.gameAdvanced())
            {
                _isLost = true;
                gameOver();
            }
            gameAdvanced();
        }

        public void SetTableSize(Int32 size)
        {
            switch (size)
            {
                case 0:
                    _gameSize = GameSize.Small;
                    break;
                case 1:
                    _gameSize = GameSize.Medium;
                    break;
                case 2:
                    _gameSize = GameSize.Large;
                    break;
                default:
                    _gameSize = GameSize.Medium;
                    break;
            }
        }

        public Int32 getTableSize()
        {
            switch (_gameSize)
            {
                case GameSize.Small:
                    return GameSizeSmall;
                case GameSize.Medium:
                    return GameSizeMedium;
                case GameSize.Large:
                    return GameSizeLarge;
                default:
                    return GameSizeMedium;
            }
        }

        public Int32 getTime()
        {
            return _table.Time;
        }
        public Int32[,] getTable()
        {
            return _table._typeTable;
        }

        public void GoLeft()
        {
            if (_table.checkAndMoveLeftPosition())
                gameAdvanced();
        }
        
        public void GoRight()
        {
            if (_table.checkAndMoveRightPosition())
                gameAdvanced();
        }

        public void GoDown()
        {
            if (!_isLost)
            {
                if (!_table.checkAndMoveDownPosition())
                {
                    _table.deleteCompletedRows();
                    _table.newShapeSpawn();
                }
                gameAdvanced();
            }
        }

        public void RotateMeSenpai()
        {
            if(_table.checkAndRotatePosition())
                gameAdvanced();
        }

        public TetrisTable table
        {
            get { return _table; }
        }

        public GameSize GameSize
        {
            get { return _gameSize; } set { _gameSize = value; }
        }
        
        public Boolean isLost
        {
            get { return _isLost; }
        }

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _isLost = false;
            _table = await _dataAccess.LoadAsync(path);
            

            switch (_table.Size)
            {
                case 4:
                    _gameSize = GameSize.Small;
                    break;
                case 8:
                    _gameSize = GameSize.Medium;
                    break;
                case 12:
                    _gameSize = GameSize.Large;
                    break;
            }
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            if (_isLost)
                throw new Exception("You can't save a lost game!");

            await _dataAccess.SaveAsync(path, _table);
        }
        #endregion

        #region private methods

        private void gameAdvanced()
        {
            if (GameAdvanced != null)
            {
                GameAdvanced(this, new TetrisEventArgs(_table ,_table.Time,_isLost));
            }
        }

        private void gameOver()
        {
            if (GameOver != null)
            {
                GameOver(this, new TetrisEventArgs(_table , _table.Time, _isLost));
            }
        }

        #endregion

    }
}
