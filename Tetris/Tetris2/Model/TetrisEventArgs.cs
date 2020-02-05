using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Persistence;

namespace Tetris.Model
{
    public class TetrisEventArgs : EventArgs
    {

        private TetrisTable _table;
        private Int32 _gameTime;
        private Boolean _isLost;

        public Byte[,] ReturnTable { get { return _table._tetrisTable; } }

        public Int32 ReturnGameTime { get { return _gameTime; } }

        public Boolean returnIsLost { get { return _isLost; } }

        public TetrisEventArgs(TetrisTable newTable, Int32 newGameTime, Boolean newIsLost)
        {
            _table = newTable;
            _gameTime = newGameTime;
            _isLost = newIsLost;
        }
    }
}
