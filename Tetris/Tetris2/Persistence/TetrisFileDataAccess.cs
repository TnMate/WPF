using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tetris.Persistence
{
    class TetrisFileDataAccess : TetrisDataAccessInterface
    {
        public async Task<TetrisTable> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = await reader.ReadLineAsync();
                    String[] numbers = line.Split(' ');
                    Int32 tableSize = Int32.Parse(numbers[0]);
                    Int32 time = Int32.Parse(numbers[1]);

                    line = await reader.ReadLineAsync();
                    numbers = line.Split(' ');
                    Int32 shape = Int32.Parse(numbers[0]);
                    Int32 shapeX = Int32.Parse(numbers[1]);
                    Int32 shapeY = Int32.Parse(numbers[2]);
                    Int32 state = Int32.Parse(numbers[3]);

                    var temporaryTable = new byte[tableSize + 1, 16];
                    var temporaryColorTable = new Int32[tableSize + 1, 16];

                    for (Int32 i = 0; i < tableSize+1; i++)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < 16; j++)
                        {
                            temporaryTable[i, j] = Byte.Parse(numbers[j]);
                        }
                    }
                    for (Int32 i = 0; i < tableSize + 1; i++)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < 16; j++)
                        {
                            temporaryColorTable[i, j] = Byte.Parse(numbers[j]);
                        }
                    }
                    TetrisTable table = new TetrisTable(tableSize, time, shape, shapeX, shapeY, state, temporaryTable, temporaryColorTable);

                    return table;
                }
            }
            catch
            {
                throw new TetrisDataException();
            }


        }
        public async Task SaveAsync(String path, TetrisTable table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(table.Size);
                    await writer.WriteLineAsync(" " + table.Time);
                    await writer.WriteLineAsync(table.Shape + " " + table.ShapeCordX + " " + table.ShapeCordY + " " + table.ShapeRotation);
                    for (Int32 i = 0; i < table.Size+1; i++)
                    {
                        for (Int32 j = 0; j < 16; j++)
                        {
                            await writer.WriteAsync(table.Table[i,j] + " ");
                        }
                        await writer.WriteLineAsync();
                    }
                    for (Int32 i = 0; i < table.Size + 1; i++)
                    {
                        for (Int32 j = 0; j < 16; j++)
                        {
                            await writer.WriteAsync(table.TypeTable[i, j] + " ");
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new TetrisDataException();
            }
        }
    }
}
