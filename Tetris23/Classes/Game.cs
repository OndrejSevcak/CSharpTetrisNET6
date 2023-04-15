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
            _timer.Elapsed += UpdateGameTimer;
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

            try
            {
                if (!_board.TryMove(Enums.DirectionEnum.Down))
                {
                    UI.DrawBoardShape(_board.CurrentShape);
                    _board.MergeShapeIntoBoardContent(_board.CurrentShape, _board.CurrentShape.CurrentBoardStartRow, _board.CurrentShape.CurrentBoardStartCol);
                    _board.CreateShapes();
                    _state.Score++;
                }
                else
                {
                    _board.CurrentShape.CurrentBoardStartRow++;
                }

                _state.CurrentGameRow = _board.CurrentShape.CurrentBoardStartRow;
                UI.DrawBoardShape(_board.CurrentShape);

                _timer.Start();
            }
            catch (GameOverException ex)
            {
                //GAME OVER
                ((System.Timers.Timer)sender).Enabled = false;
                ((System.Timers.Timer)sender).Elapsed -= UpdateGameTimer;

                UI.DrawGameOverScreen(_state.Score);
            }           
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

                                UI.DrawBoardShape(_board.CurrentShape,clear: true);
                                _board.TryMove(Enums.DirectionEnum.Left);
                                UI.DrawBoardShape(_board.CurrentShape);

                                break;
                            case ConsoleKey.RightArrow:

                                UI.DrawBoardShape(_board.CurrentShape, clear: true);
                                _board.TryMove(Enums.DirectionEnum.Right);
                                UI.DrawBoardShape(_board.CurrentShape);

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
                _timer.Stop();
                _timer.Elapsed -= UpdateGameTimer;
                UI.DrawGameOverScreen(_state.Score);
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateGameTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            UI.DrawScoreAndTime(_state.ElapsedGameTime, _state.Score);
        }
    }
}
