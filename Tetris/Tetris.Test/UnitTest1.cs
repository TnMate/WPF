using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Model;

namespace Tetris.Test
{
    [TestClass]
    public class UnitTest1
    {
        private TetrisModel _model;

        [TestInitialize]
        public void Initialize()
        {
            _model = new TetrisModel();

            _model.GameAdvanced += new EventHandler<TetrisEventArgs>(model_gameAdvanced);
            _model.GameOver += new EventHandler<TetrisEventArgs>(model_gameOver);
        }

        [TestMethod]
        public void TetrisSmallGame()
        {

            _model.SetTableSize(0);
            _model.NewGame();

            Assert.IsTrue(_model.getTime() == 0);
            Assert.IsTrue(_model.getTableSize() == 4);

            var table = _model.getTable();
            Int32 countit1 = 0;
            Int32 countit2 = 0;


            for (int i = 0; i < _model.getTableSize() + 1; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (table[i, j] == 0)
                    {
                        countit1++;
                    }
                    else
                    {
                        countit2++;
                    }
                }
            }
            Assert.AreEqual(76, countit1);
            Assert.AreEqual(4, countit2);
            Assert.AreEqual(80, countit1 + countit2);
        }

        [TestMethod]
        public void TetrisMediumGame()
        {

            _model.NewGame();

            Assert.IsTrue(_model.getTime() == 0);
            Assert.IsTrue(_model.getTableSize() == 8);

            var table = _model.getTable();
            Int32 countit1 = 0;
            Int32 countit2 = 0;


            for (int i = 0; i < _model.getTableSize() + 1; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (table[i, j] == 0)
                    {
                        countit1++;
                    }
                    else
                    {
                        countit2++;
                    }
                }
            }
            Assert.AreEqual(140, countit1);
            Assert.AreEqual(4, countit2);
            Assert.AreEqual(144, countit1 + countit2);
        }

        [TestMethod]
        public void TetrisLargeGame()
        {

            _model.SetTableSize(2);
            _model.NewGame();
            Assert.IsTrue(_model.getTime() == 0);
            Assert.IsTrue(_model.getTableSize() == 12);

            var table = _model.getTable();
            Int32 countit1 = 0;
            Int32 countit2 = 0;


            for (int i = 0; i < _model.getTableSize() + 1; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (table[i, j] == 0)
                    {
                        countit1++;
                    }
                    else
                    {
                        countit2++;
                    }
                }
            }
            Assert.AreEqual(204, countit1);
            Assert.AreEqual(4, countit2);
            Assert.AreEqual(208, countit1 + countit2);
        }

        [TestMethod]
        public void TetrisAdvanceTime()
        {
            _model.NewGame();

            var pos1 = _model.table.ShapeCordX;

            _model.AdvanceTime();
            Assert.IsTrue(_model.getTime() == 1);
            Assert.AreEqual(pos1 + 1, _model.table.ShapeCordX);

            var pos2 = _model.table.ShapeCordY;

            _model.GoLeft();

            Assert.IsTrue(_model.getTime() == 1);
            Assert.AreEqual(pos2 - 1, _model.table.ShapeCordY);

            _model.GoRight();

            Assert.IsTrue(_model.getTime() == 1);
            Assert.AreEqual(pos2, _model.table.ShapeCordY);

            _model.GoRight();

            Assert.IsTrue(_model.getTime() == 1);
            Assert.AreEqual(pos2 + 1, _model.table.ShapeCordY);

            _model.AdvanceTime();
            Assert.IsTrue(_model.getTime() == 2);
            Assert.AreEqual(pos1 + 2, _model.table.ShapeCordX);

            _model.AdvanceTime();
            Assert.IsTrue(_model.getTime() == 3);
            Assert.AreEqual(pos1 + 3, _model.table.ShapeCordX);
        }

        [TestMethod]
        public void TetrisLeftSideTest()
        {
            _model.NewGame();
            var pos1 = _model.table.ShapeCordY;

            for (int i = 0; i < 10; i++)
            {
                _model.GoLeft();
            }

            Assert.IsTrue(_model.table.ShapeCordY < 2 && _model.table.ShapeCordY >= 0);
            Assert.AreNotEqual(pos1, _model.table.ShapeCordY);
        }

        [TestMethod]
        public void TetrisRightSideTest()
        {
            _model.NewGame();
            var pos1 = _model.table.ShapeCordY;

            for (int i = 0; i < 10; i++)
            {
                _model.GoRight();
            }

            Assert.IsTrue(_model.table.ShapeCordY < 16 && _model.table.ShapeCordY >= 14);
            Assert.AreNotEqual(pos1, _model.table.ShapeCordY);
        }

        [TestMethod]
        public void TetrisDownMovement()
        {
            _model.NewGame();
            var pos1 = _model.table.ShapeCordY;

            for (int i = 0; i < 30; i++)
            {
                _model.GoDown();
            }
            Assert.IsTrue(_model.table.Time >= 0);
        }

        private void model_gameAdvanced(Object sender, TetrisEventArgs e)
        {
            Assert.IsTrue(_model.getTime() >= 0);
            Assert.AreEqual(e.ReturnGameTime, _model.getTime());
        }
        private void model_gameOver(Object sender, TetrisEventArgs e)
        {
            Assert.AreEqual(e.ReturnGameTime, _model.getTime());
            Assert.IsTrue(e.returnIsLost);
        }
    }
}
