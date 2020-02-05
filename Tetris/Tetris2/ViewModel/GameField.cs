using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris2.ViewModel
{
    public class GameField : ViewModelBase
    {
        private Boolean _isLocked;
        private Int32 _type;

        /// <summary>
        /// Zároltság lekérdezése, vagy beállítása.
        /// </summary>
        public Boolean IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (_isLocked != value)
                {
                    _isLocked = value;
                    OnPropertyChanged();
                }
            }
        }


        /// <summary>
        /// Típus lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Vízszintes koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Függőleges koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Y { get; set; }

        /// <summary>
        /// Sorszám lekérdezése.
        /// </summary>
        public Int32 Number { get; set; }

        /// <summary>
        /// Lépés parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand StepCommand { get; set; }
    }
}
