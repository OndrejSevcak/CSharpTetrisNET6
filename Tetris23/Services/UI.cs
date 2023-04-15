using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Classes;

namespace Tetris23.Services
{
    public static class UI
    {
        private static void RestoreOriginalState(Action drawingFunction)
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

        public static void DrawBoard(Board board, int offsetLeft = 3, int offsetTop = 3)
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
    }
}
