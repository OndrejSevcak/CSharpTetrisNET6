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

        private void InitializeGame()
        {
            _board = new Board(21, 21, Shape.RandomShapeGenerator);
            _state = new GameState();

            _board.CreateShapes();
            _state.CurrentGameRow = _board.CurrentShape.CurrentBoardStartRow;

            UI.DrawBoard(_board);
            UI.DrawBoardShape(_board.CurrentShape);
            UI.DrawHeading();
            UI.DrawScoreAndTime(_state.ElapsedGameTime, 0);
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
                            case ConsoleKey.UpArrow:
                                UI.DrawBoardShape(_board.CurrentShape,clear: true);
                                _board.CurrentShape.Rotate();
                                UI.DrawBoardShape(_board.CurrentShape);
                                break;
                            case ConsoleKey.DownArrow:
                                UI.DrawBoardShape(_board.CurrentShape, clear: true);
                                _board.DropCurrentShape();
                                UI.DrawBoardShape(_board.CurrentShape);
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
