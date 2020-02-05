using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Persistence
{

    #region Coordinates
    /*
    { { -1,  0 }, {  0,  0 }, {  1,  0 }, {  0, -1 }, },  // 00: T up
    { { 0, -1 }, { 0,  0 }, { 1,  0 }, { 0,  1 }, },  // 01: T right
    { { -1,  0 }, { 0,  0 }, { 1,  0 }, { 0,  1 }, },  // 02: T down(spawn)
    { { 0, -1 }, { -1,  0 }, { 0,  0 }, { 0,  1 }, },  // 03: T left

    { { 0, -1 }, { 0,  0 }, { -1,  1 }, { 0,  1 }, },  // 04: J left
    { { -1, -1 }, { -1,  0 }, { 0,  0 }, { 1,  0 }, },  // 05: J up
    { { 0, -1 }, { 1, -1 }, { 0,  0 }, { 0,  1 }, },  // 06: J right
    { { -1,  0 }, { 0,  0 }, { 1,  0 }, { 1,  1 }, },  // 07: J down(spawn)

    { { -1,  0 }, { 0,  0 }, { 0,  1 }, { 1,  1 }, },  // 08: Z horizontal(spawn)
    { { 1, -1 }, { 0,  0 }, { 1,  0 }, { 0,  1 }, },  // 09: Z vertical
    
    { { -1,  0 }, { 0,  0 }, { -1,  1 }, { 0,  1 }, },  // 0A: O(spawn)

    { { 0,  0 }, { 1,  0 }, { -1,  1 }, { 0,  1 }, },  // 0B: S horizontal(spawn)
    { { 0, -1 }, { 0,  0 }, { 1,  0 }, { 1,  1 }, },  // 0C: S vertical

    { { 0, -1 }, { 0,  0 }, { 0,  1 }, { 1,  1 }, },  // 0D: L right
    { { -1,  0 }, { 0,  0 }, { 1,  0 }, { -1,  1 }, },  // 0E: L down(spawn)
    { { -1, -1 }, { 0, -1 }, { 0,  0 }, { 0,  1 }, },  // 0F: L left
    { { 1, -1 }, { -1,  0 }, { 0,  0 }, { 1,  0 }, },  // 10: L up

    { { 0, -2 }, { 0, -1 }, { 0,  0 }, { 0,  1 }, },  // 11: I vertical
    { { -2,  0 }, { -1,  0 }, { 0,  0 }, { 1,  0 }, },  // 12: I horizontal(spawn)
    */
    #endregion

    class Shape
    {
        #region Fields
        protected Int32[][,] state;
        protected Int32 currentState;

        protected Int32 posX;
        protected Int32 posY;
        #endregion

        #region changeFunctions
        public void rotateShape()
        {
            if (state.Length == currentState + 1)
            {
                currentState = 0;
            }
            else
            {
                currentState++;
            }
        }
        public void moveToLeft()
        {
            posY--;
        }
        public void moveToRight()
        {
            posY++;
        }
        public void moveDown()
        {
            posX++;
        }

        #endregion

        #region currentStateFunctions

        public Int32 getPosX()
        {
            return posX;
        }
        public Int32 getPosY()
        {
            return posY;
        }
        public Int32 getIntState()
        {
            return currentState;
        }
        public Int32[,] getCurrentState()
        {
            return state[currentState];
        }
        public Int32[,] getNextRotateState()
        {
            Int32 temporaryState = currentState;
            if (state.Length == temporaryState + 1)
            {
                temporaryState = 0;
            }
            else
            {
                temporaryState++;
            }
            return state[temporaryState];
        }
        #endregion
    }
    
    #region Childs
    class TShape : Shape
    {
        public TShape(Int32 xCord = 2, Int32 yCord = 7, Int32 whichState = 2) {
            currentState = whichState;
            posX = xCord;
            posY = yCord;
            state = new Int32[4][,]
            {
                new Int32[,]{ { -1,  0 }, {  0,  0 }, {  1,  0 }, {  0, -1 } },
                new Int32[,]{ {  0, -1 }, {  0,  0 }, {  1,  0 }, {  0,  1 } },
                new Int32[,]{ { -1,  0 }, {  0,  0 }, {  1,  0 }, {  0,  1 } },
                new Int32[,]{ { 0, -1 }, { -1, 0 }, { 0, 0 }, { 0, 1 } }
            };
        }
    }
    class JShape : Shape
    {
        public JShape(Int32 xCord = 2, Int32 yCord = 7, Int32 whichState = 3)
        {
            currentState = whichState;
            posX = xCord;
            posY = yCord;
            state = new Int32[4][,]
            {
                new Int32[,]{ {  0, -1 }, {  0,  0 }, { -1,  1 }, {  0,  1 } },
                new Int32[,]{ { -1, -1 }, { -1,  0 }, {  0,  0 }, {  1,  0 } },
                new Int32[,]{ {  0, -1 }, {  1, -1 }, {  0,  0 }, {  0,  1 } },
                new Int32[,]{ { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, 1 } }
            };
        }
    }
    class ZShape : Shape
    {
        public ZShape(Int32 xCord = 2, Int32 yCord = 7, Int32 whichState = 0)
        {
            currentState = whichState;
            posX = xCord;
            posY = yCord;
            state = new Int32[2][,]
            {
                new Int32[,]{ { -1,  0 }, {  0,  0 }, {  0,  1 }, {  1,  1 } },
                new Int32[,]{ { 1, -1 }, { 0, 0 }, { 1, 0 }, { 0, 1 } }
            };
        }
    }
    class OShape : Shape
    {
        public OShape(Int32 xCord = 2, Int32 yCord = 7, Int32 whichState = 0)
        {
            currentState = whichState;
            posX = xCord;
            posY = yCord;
            state = new Int32[1][,]
            {
                new Int32[,]{ { -1, 0 }, { 0, 0 }, { -1, 1 }, { 0, 1 } }
            };
        }
    }
    class SShape : Shape
    {
        public SShape(Int32 xCord = 1, Int32 yCord = 7,Int32 whichState = 1)
        {
            currentState = whichState;
            posX = xCord;
            posY = yCord;
            state = new Int32[2][,]
            {
                new Int32[,]{ {  0,  0 }, {  1,  0 }, { -1,  1 }, {  0,  1 } },
                new Int32[,]{ {  0, -1 }, {  0,  0 }, {  1,  0 }, {  1,  1 } }
            };
        }
    }
    class LShape : Shape
    {
        public LShape(Int32 xCord = 2, Int32 yCord = 7,Int32 whichState = 1)
        {
            currentState = whichState;
            posX = xCord;
            posY = yCord;
            state = new Int32[4][,]
            {
                new Int32[,]{ {  0, -1 }, {  0,  0 }, {  0,  1 }, {  1,  1 } },
                new Int32[,]{ { -1,  0 }, {  0,  0 }, {  1,  0 }, { -1,  1 } },
                new Int32[,]{ { -1, -1 }, {  0, -1 }, {  0,  0 }, {  0,  1 } },
                new Int32[,]{ { 1, -1 }, { -1, 0 }, { 0, 0 }, { 1, 0 } }
            };
        }
    }
    class IShape : Shape
    {
        public IShape(Int32 xCord = 2, Int32 yCord = 7, Int32 whichState = 1)
        {
            currentState = whichState;
            posX = xCord;
            posY = yCord;
            state = new Int32[2][,]
            {
                new Int32[,]{ {  0, -2 }, {  0, -1 }, {  0,  0 }, {  0,  1 } },
                new Int32[,]{ { -2,  0 }, { -1,  0 }, {  0,  0 }, {  1,  0 } }
            };
        }
    }
    #endregion

}
