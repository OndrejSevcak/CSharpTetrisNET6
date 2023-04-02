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

        private System.Timers.Timer _timer = new System.Timers.Timer(1000);

        public Game()
        {
            _board = new Board(20, 20, Shape.RandomShapeGenerator);
            _state = new GameState();
            _timer.Elapsed += MoveShapeDown;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            InitializeGame();
        }

        private void MoveShapeDown(object sender, System.Timers.ElapsedEventArgs e)
        {
            UI.DrawBoardShape(_board.CurrentShape, clear: true);
            _state.CurrentGameRow++;
            _board.CurrentShape.CurrentBoardStartRow = _state.CurrentGameRow;
            UI.DrawBoardShape(_board.CurrentShape);            
        }

        private void InitializeGame()
        {
            UI.DrawBoard(_board);
            _board.CreateShapes();
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
