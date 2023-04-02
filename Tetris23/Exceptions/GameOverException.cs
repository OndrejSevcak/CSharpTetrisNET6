using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris23.Exceptions
{
    public class GameOverException : ApplicationException
    {
        public GameOverException(string message) : base(message) { }
    }
}
