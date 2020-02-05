using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Persistence
{

    public enum ShapeTypes { TShape,JShape,ZShape,OShape,SShape,LShape,IShape}

    public class TetrisTable
    {
        static ShapeTypes RandomEnumValue()
        {
            var v = Enum.GetValues(typeof(ShapeTypes));
            return (ShapeTypes)v.GetValue(new Random().Next(v.Length));
        }

        #region Fields

        public Byte[,] _tetrisTable;
        public Int32[,] _typeTable;
        private Shape currentShape;
        private Int32 currentShapeIndex;
        private Int32 mapSize;
        private Int32 gameTime;

        #endregion

        #region Constructors

        public TetrisTable(Int32 size = 8)
        {
            _tetrisTable = new byte[size + 1, 16];
            _typeTable = new Int32[size + 1, 16];
            mapSize = size;
            newShapeSpawn();
            gameTime = 0;
        }
        public TetrisTable(Int32 size, Int32 time, Int32 shape, Int32 cordx, Int32 cordy, Int32 state, byte[,] table, Int32[,] colorTable)
        {
            mapSize = size;
            gameTime = time;
            currentShapeIndex = shape;
            switch (shape)
            {
                case 1:
                    currentShape = new TShape(cordx, cordy, state);
                    currentShapeIndex = 1;
                    break;
                case 2:
                    currentShape = new JShape(cordx, cordy, state);
                    currentShapeIndex = 2;
                    break;
                case 3:
                    currentShape = new ZShape(cordx, cordy, state);
                    currentShapeIndex = 3;
                    break;
                case 4:
                    currentShape = new OShape(cordx, cordy, state);
                    currentShapeIndex = 4;
                    break;
                case 5:
                    currentShape = new SShape(cordx, cordy, state);
                    currentShapeIndex = 5;
                    break;
                case 6:
                    currentShape = new LShape(cordx, cordy, state);
                    currentShapeIndex = 6;
                    break;
                case 7:
                    currentShape = new IShape(cordx, cordy, state);
                    currentShapeIndex = 7;
                    break;
                default:
                    break;
            }
            _tetrisTable = table;
            _typeTable = colorTable;
        }

        #endregion

        #region public methods

        public Boolean gameAdvanced()
        {
            gameTime++;
            if (!checkAndMoveDownPosition())
            {
                deleteCompletedRows();
                if (!newShapeSpawn())
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        public Boolean newShapeSpawn()
        {
            ShapeTypes value = RandomEnumValue();
            switch (value)
            {
                case ShapeTypes.TShape:
                    currentShape = new TShape();
                    currentShapeIndex = 1;
                    break;
                case ShapeTypes.JShape:
                    currentShape = new JShape();
                    currentShapeIndex = 2;
                    break;
                case ShapeTypes.ZShape:
                    currentShape = new ZShape();
                    currentShapeIndex = 3;
                    break;
                case ShapeTypes.OShape:
                    currentShape = new OShape();
                    currentShapeIndex = 4;
                    break;
                case ShapeTypes.SShape:
                    currentShape = new SShape();
                    currentShapeIndex = 5;
                    break;
                case ShapeTypes.LShape:
                    currentShape = new LShape();
                    currentShapeIndex = 6;
                    break;
                case ShapeTypes.IShape:
                    currentShape = new IShape();
                    currentShapeIndex = 7;
                    break;
                default:
                    break;
            }

            var x = currentShape.getPosX();
            var y = currentShape.getPosY();
            var ImaginaryPos = currentShape.getCurrentState();

            Boolean l = true;

            for (int i = 0; i < 4 && l; i++)
            {
                var a = x + ImaginaryPos[i, 0];
                var b = y + ImaginaryPos[i, 1];

                if (a < 0 || a > mapSize || b < 0 || b > 15)
                {
                    l = false;
                }

                l = l && _tetrisTable[a, b] == 0;
            }


            if (l)
            {

                for (int i = 0; i < 4 && l; i++)
                {
                    var a = x + ImaginaryPos[i, 0];
                    var b = y + ImaginaryPos[i, 1];

                    _tetrisTable[a, b] = 2;
                    _typeTable[a, b] = currentShapeIndex;
                }
            }
            else
            {
                currentShape = null;
                //gameover();
            }
            return l;

        }

        public Boolean newShapeSpawn(Int32 type, Int32 xCord, Int32 yCord)
        {
            ShapeTypes value = RandomEnumValue();
            switch (type)
            {
                case 1:
                    currentShape = new TShape(xCord, yCord);
                    currentShapeIndex = 1;
                    break;
                case 2:
                    currentShape = new JShape(xCord, yCord);
                    currentShapeIndex = 2;
                    break;
                case 3:
                    currentShape = new ZShape(xCord, yCord);
                    currentShapeIndex = 3;
                    break;
                case 4:
                    currentShape = new OShape(xCord, yCord);
                    currentShapeIndex = 4;
                    break;
                case 5:
                    currentShape = new SShape(xCord, yCord);
                    currentShapeIndex = 5;
                    break;
                case 6:
                    currentShape = new LShape(xCord, yCord);
                    currentShapeIndex = 6;
                    break;
                case 7:
                    currentShape = new IShape(xCord, yCord);
                    currentShapeIndex = 7;
                    break;
                default:
                    break;
            }

            var x = currentShape.getPosX();
            var y = currentShape.getPosY();
            var ImaginaryPos = currentShape.getCurrentState();

            Boolean l = true;

            for (int i = 0; i < 4 && l; i++)
            {
                var a = x + ImaginaryPos[i, 0];
                var b = y + ImaginaryPos[i, 1];

                if (a < 0 || a > mapSize || b < 0 || b > 15)
                {
                    l = false;
                }

                l = l && _tetrisTable[a, b] == 0;
            }


            if (l)
            {

                for (int i = 0; i < 4 && l; i++)
                {
                    var a = x + ImaginaryPos[i, 0];
                    var b = y + ImaginaryPos[i, 1];

                    _tetrisTable[a, b] = 2;
                    _typeTable[a, b] = currentShapeIndex;
                }
            }
            else
            {
                currentShape = null;
                //gameover();
            }
            return l;
        }

        public void deleteCompletedRows()
        {
            for (int i = 0; i <= mapSize; i++)
            {
                Boolean l = true;
                for (int j = 0; j < 16 && l; j++)
                {
                    l = _tetrisTable[i, j] == 1;
                }
                if (l)
                {
                    for (int j = i; j > 0; j--)
                    {
                        for (int k = 0; k < 16; k++)
                        {
                            _tetrisTable[j, k] = _tetrisTable[j - 1, k];
                            _typeTable[j, k] = _typeTable[j - 1, k];
                        }
                    }
                }
            }
        }

        public Boolean checkAndMoveLeftPosition()
        {
            if (currentShape != null)
            {
                var x = currentShape.getPosX();
                var y = currentShape.getPosY();
                var blocks = currentShape.getCurrentState();

                y--;

                Boolean l = true;

                for (int i = 0; i < 4 && l; i++)
                {
                    var a = x + blocks[i, 0];
                    var b = y + blocks[i, 1];

                    if (a < 0 || a > mapSize || b < 0 || b > 15)
                    {
                        l = false;
                    }

                    l = l && (_tetrisTable[a, b] == 0 || _tetrisTable[a, b] == 2);
                }

                if (l)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var a = currentShape.getPosX() + blocks[i, 0];
                        var b = currentShape.getPosY() + blocks[i, 1];

                        _tetrisTable[a, b] = 0;
                        _typeTable[a, b] = 0;
                    }
                    currentShape.moveToLeft();
                    for (int i = 0; i < 4; i++)
                    {
                        var a = currentShape.getPosX() + blocks[i, 0];
                        var b = currentShape.getPosY() + blocks[i, 1];

                        _tetrisTable[a, b] = 2;
                        _typeTable[a, b] = currentShapeIndex;
                    }
                }
                return l;
            }
            return false;
        }
        public Boolean checkAndMoveDownPosition()
        {
            if (currentShape != null)
            {
                var x = currentShape.getPosX();
                var y = currentShape.getPosY();
                var blocks = currentShape.getCurrentState();

                x++;

                Boolean l = true;

                for (int i = 0; i < 4 && l; i++)
                {
                    var a = x + blocks[i, 0];
                    var b = y + blocks[i, 1];

                    if (a < 0 || a > mapSize || b < 0 || b > 15)
                    {
                        l = false;
                    }

                    l = l && (_tetrisTable[a, b] == 0 || _tetrisTable[a, b] == 2);
                }

                if (l)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var a = currentShape.getPosX() + blocks[i, 0];
                        var b = currentShape.getPosY() + blocks[i, 1];

                        _tetrisTable[a, b] = 0;
                        _typeTable[a, b] = 0;
                    }
                    currentShape.moveDown();
                    for (int i = 0; i < 4; i++)
                    {
                        var a = currentShape.getPosX() + blocks[i, 0];
                        var b = currentShape.getPosY() + blocks[i, 1];

                        _tetrisTable[a, b] = 2;
                        _typeTable[a, b] = currentShapeIndex;
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var a = currentShape.getPosX() + blocks[i, 0];
                        var b = currentShape.getPosY() + blocks[i, 1];

                        _tetrisTable[a, b] = 1;
                    }
                }

                return l;
            }
            return false;

        }
        public Boolean checkAndMoveRightPosition()
        {
            if (currentShape != null)
            {
                var x = currentShape.getPosX();
                var y = currentShape.getPosY();
                var blocks = currentShape.getCurrentState();

                y++;

                Boolean l = true;

                for (int i = 0; i < 4 && l; i++)
                {
                    var a = x + blocks[i, 0];
                    var b = y + blocks[i, 1];

                    if (a < 0 || a > mapSize || b < 0 || b > 15)
                    {
                        l = false;
                    }

                    l = l && (_tetrisTable[a, b] == 0 || _tetrisTable[a, b] == 2);
                }

                if (l)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var a = currentShape.getPosX() + blocks[i, 0];
                        var b = currentShape.getPosY() + blocks[i, 1];

                        _tetrisTable[a, b] = 0;
                        _typeTable[a, b] = 0;
                    }
                    currentShape.moveToRight();
                    for (int i = 0; i < 4; i++)
                    {
                        var a = currentShape.getPosX() + blocks[i, 0];
                        var b = currentShape.getPosY() + blocks[i, 1];

                        _tetrisTable[a, b] = 2;
                        _typeTable[a, b] = currentShapeIndex;
                    }
                }
                return l;
            }
            return false;
        }
        public Boolean checkAndRotatePosition()
        {
            if (currentShape != null)
            {
                var x = currentShape.getPosX();
                var y = currentShape.getPosY();
                var blocks = currentShape.getNextRotateState();

                Boolean l = true;

                for (int i = 0; i < 4 && l; i++)
                {
                    var a = x + blocks[i, 0];
                    var b = y + blocks[i, 1];

                    if (a < 0 || a > mapSize || b < 0 || b > 15)
                    {
                        l = false;
                    }

                    l = l && (_tetrisTable[a, b] == 0 || _tetrisTable[a, b] == 2);
                }

                if (l)
                {
                    var realShape = currentShape.getCurrentState();
                    for (int i = 0; i < 4; i++)
                    {
                        var a = x + realShape[i, 0];
                        var b = y + realShape[i, 1];

                        _tetrisTable[a, b] = 0;
                        _typeTable[a, b] = 0;
                    }
                    currentShape.rotateShape();
                    for (int i = 0; i < 4; i++)
                    {
                        var a = currentShape.getPosX() + blocks[i, 0];
                        var b = currentShape.getPosY() + blocks[i, 1];

                        _tetrisTable[a, b] = 2;
                        _typeTable[a, b] = currentShapeIndex;
                    }
                }
                return l;
            }
            return false;
        }

        #endregion

        #region Get methods

        public Int32 Time
        {
            get { return gameTime; }
        }
        public Int32 Size
        {
            get { return mapSize; }
        }
        public Int32 Shape
        {
            get { return currentShapeIndex; }
        }
        public Int32 ShapeCordX
        {
            get { return currentShape.getPosX(); }
        }
        public Int32 ShapeCordY
        {
            get { return currentShape.getPosY(); }
        }
        public Byte[,] Table
        {
            get { return _tetrisTable; }
        }
        public Int32[,] TypeTable
        {
            get { return _typeTable; }
        }
        public Int32 ShapeRotation
        {
            get { return currentShape.getIntState(); }
        }

        #endregion
    }

}
