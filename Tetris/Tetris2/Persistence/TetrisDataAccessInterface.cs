using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Persistence
{
    public interface TetrisDataAccessInterface
    {
        Task<TetrisTable> LoadAsync(String path);

        Task SaveAsync(String path, TetrisTable table);
    }
}
