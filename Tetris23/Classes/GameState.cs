using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris23.Classes
{
    public class GameState
    {
        public int Score { get; set; }
        public bool IsGameOver { get; set; }

        public int CurrentGameRow { get; set; } = 1;
        public int CurrentGameCol { get; set; }


    }
}
