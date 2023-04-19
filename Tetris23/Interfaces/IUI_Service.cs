using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Classes;

namespace Tetris23.Interfaces
{
    public interface IUI_Service
    {
        void DrawEmptyBoard(Board board, int offsetLeft = 3, int offsetTop = 3);
        void DrawBoardWithShapes(Board board, int offsetLeft = 3, int offsetTop = 3);
        void DrawBoardShape(Shape shape, int offsetLeft = 4, int offsetTop = 3, bool clear = false);
        void DrawHeading(int offsetLeft = 50, int offsetTop = 2);
        void DrawScoreAndTime(TimeSpan elapsedTime, int score = 0, int offsetLeft = 50, int offsetTop = 5);
        void DrawNextShape(Shape shape, int offsetLeft = 50, int offsetTop = 8, bool clear = false);
        void DrawGameOverScreen(int finalScore, int offsetTop = 5, int offsetLeft = 35);
        void DrawLog(string logMessage, TimeSpan elapsedTime, int offsetLeft = 3, int offsetTop = 26, bool clear = false);
        void DrawSpecialMessage(string message, int offsetLeft = 50, int offsetTop = 18);
    }
}
