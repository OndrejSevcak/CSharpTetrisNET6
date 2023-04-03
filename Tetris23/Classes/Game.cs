using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Exceptions;
using Tetris23.Services;

namespace Tetris23.Classes
{
    public class Game
    {
        private Board _board;
        private GameState _state;
        private System.Timers.Timer _timer = new System.Timers.Timer(500);

        public Game()
        {
            InitializeGame();
            _timer.Elapsed += MoveShapeDown;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        private void MoveShapeDown(object sender, System.Timers.ElapsedEventArgs e)
        {
            UI.DrawBoardShape(_board.CurrentShape, clear: true);
            if (!_board.TryMove(Enums.DirectionEnum.Down))
            {
                UI.DrawBoardShape(_board.CurrentShape);
                _board.MergeShapeIntoBoardContent(_board.CurrentShape, _board.CurrentShape.CurrentBoardStartRow, _board.CurrentShape.CurrentBoardStartCol);
                _board.CreateShapes();
            }
            else
            {
                _board.CurrentShape.CurrentBoardStartRow++;
            }
            _state.CurrentGameRow = _board.CurrentShape.CurrentBoardStartRow;
            UI.DrawBoardShape(_board.CurrentShape);

            _timer.Start();
        }

        private void InitializeGame()
        {
            _board = new Board(20, 20, Shape.RandomShapeGenerator);
            _state = new GameState();

            _board.CreateShapes();
            _state.CurrentGameRow = _board.CurrentShape.CurrentBoardStartRow;

            UI.DrawBoard(_board);
            UI.DrawBoardShape(_board.CurrentShape);
        }

        public void Run()
        {
            try
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var pressedKey = Console.ReadKey();
                        switch (pressedKey.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                _board.TryMove(Enums.DirectionEnum.Left);
                                break;
                            case ConsoleKey.RightArrow:
                                _board.TryMove(Enums.DirectionEnum.Right);
                                break;
                            case ConsoleKey.DownArrow:
                                _board.TryMove(Enums.DirectionEnum.Down);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (GameOverException ex)
            {
                //GAME OVER
            }
            catch (Exception ex)
            {

            }
        }
    }
}
