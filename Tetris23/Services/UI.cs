using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Classes;
using Tetris23.Enums;
using Tetris33.Classes;

namespace Tetris23.Services
{
    /// <summary>
    /// UI class responsible for drawing to console. 
    /// Will be used by multiple threads so the lock statement is used to ensure that only one thread can access the UI class at a time
    /// </summary>
    public static class UI
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Implements the lock statement and ensures the original console state is persisted after executing a drawing action delegate
        /// Is used in each drawing method of this UI class
        /// </summary>
        private static void RestoreOriginalState(Action drawingFunction)
        {
            lock (_lock)
            {
                var originalLeft = Console.CursorLeft;
                var originalTop = Console.CursorTop;
                var originalColor = Console.BackgroundColor;
                var foregroungColor = Console.ForegroundColor;

                try
                {
                    drawingFunction();
                }
                finally
                {
                    Console.SetCursorPosition(originalLeft, originalTop);
                    Console.BackgroundColor = originalColor;
                    Console.ForegroundColor = foregroungColor;
                }
            }
        }

        public static void DrawEmptyBoard(Board board, int offsetLeft = 3, int offsetTop = 3)
        {
            RestoreOriginalState(() =>
            {
                for (int row = 0; row < board.Height + 1; row++)   //+1 means bottom border, top border does not exists
                {
                    for (int col = 0; col < board.Width + 2; col++)  //+2 means left and right borders
                    {
                        Console.SetCursorPosition(col + offsetLeft, row + offsetTop);

                        if (col == 0 || col == board.Width + 1 || row == board.Height)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        Console.Write(" ");
                    }
                    Console.Write("\n");
                }
            });            
        }

        public static void DrawBoardWithShapes(Board board, int offsetLeft = 3, int offsetTop = 3)
        {
            RestoreOriginalState(() =>
            {
                for (int row = 0; row < board.Height + 1; row++)   //+1 means bottom border, top border does not exists
                {
                    for (int col = 0; col < board.Width + 2; col++)  //+2 means left and right borders
                    {
                        Console.SetCursorPosition(col + offsetLeft, row + offsetTop);

                        if (col == 0 || col == board.Width + 1 || row == board.Height)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                        }
                        else
                        {
                            if (row < 21 && col > 0 && col < 21 && board.BoardGrid[row, col-1].IsOccupied)
                            {
                                if(board.BoardGrid[row, col - 1].ShapeType != null)
                                {
                                    Console.BackgroundColor =
                                        ShapeConfiguration.ShapeTypeDict[(ShapeTypeEnum)board.BoardGrid[row, col - 1].ShapeType].Color;
                                }
                                else
                                {
                                    throw new Exception("Occupied board value without ShapeType!");
                                    //Console.BackgroundColor = ConsoleColor.Black;
                                }
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                        }
                        Console.Write(" ");
                    }
                    Console.Write("\n");
                }
            });
        }

        public static void DrawBoardShape(Shape shape, int offsetLeft = 4, int offsetTop = 3, bool clear = false)
        {
            RestoreOriginalState(() =>
            {
                for (int row = 0; row < shape.ShapeGrid.GetLength(0); row++)
                {
                    for (int col = 0; col < shape.ShapeGrid.GetLength(1); col++)
                    {
                        Console.SetCursorPosition(col + shape.CurrentBoardStartCol + offsetLeft, row + shape.CurrentBoardStartRow + offsetTop);

                        if (shape.ShapeGrid[row, col].IsOccupied)
                        {
                            if (!clear)
                            {
                                Console.BackgroundColor = shape.Color;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            Console.Write(" ");
                        }                        
                    }
                    Console.Write("\n");
                }
            });          
        }

        public static void DrawHeading(int offsetLeft = 50, int offsetTop = 2)
        {
            RestoreOriginalState(() =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.SetCursorPosition(offsetLeft, offsetTop);
                Console.Write("     *** C# TETRIS GAME by Ondrej Sevcak *** ");
                
                Console.SetCursorPosition(offsetLeft, offsetTop + 1);
                Console.Write("-------------------------------------------------");

            });
        }

        public static void DrawScoreAndTime(TimeSpan elapsedTime, int score = 0, int offsetLeft = 50, int offsetTop = 5)
        {
            RestoreOriginalState(() =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.SetCursorPosition(offsetLeft, offsetTop);

                string formattedTime = String.Format("{0:hh\\:mm\\:ss}", elapsedTime);
                Console.Write($"Game time: {formattedTime}        Score: {score}");

            });
        }

        public static void DrawNextShape(Shape shape, int offsetLeft = 50, int offsetTop = 8, bool clear = false)
        {
            RestoreOriginalState(() =>
            {
                Console.SetCursorPosition(offsetLeft, offsetTop);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Next shape: ");
                DrawBoardShape(shape, offsetLeft + 15, offsetTop, clear: clear);
            });
        }

        public static void DrawGameOverScreen(int finalScore, int offsetTop = 5, int offsetLeft = 35)
        {
            Console.Clear();
            Console.Clear();

            Console.SetCursorPosition(offsetLeft, offsetTop);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write(" XXX  !!! GAME OVER  !!! XXX");

            Console.SetCursorPosition(offsetLeft, offsetTop + 3);
            Console.Write($"Your final Score is {finalScore}");

        }

        public static void DrawLog(string logMessage, TimeSpan elapsedTime, int offsetLeft = 3, int offsetTop = 26, bool clear = false)
        {
            RestoreOriginalState(() =>
            {
                Console.SetCursorPosition(offsetLeft, offsetTop);
                Console.ForegroundColor = ConsoleColor.White;

                string timeSpanString = String.Format("{0:hh\\:mm\\:ss}", elapsedTime);

                if (!clear)
                {
                    Console.Write($"Log: {timeSpanString} - {logMessage}");
                }
                else
                {
                    Console.Write($"Log:                                                                           ");

                }
            });
        }

        public static void DrawSpecialMessage(string message, int offsetLeft = 50, int offsetTop = 18)
        {
            RestoreOriginalState(() =>
            {
                Console.SetCursorPosition(offsetLeft, offsetTop);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{message}");
            });
        }
    }
}
