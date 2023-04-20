using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Exceptions;
using Tetris23.Interfaces;
using Tetris23.Services;

namespace Tetris23.Classes
{
    public class Game : IGame
    {
        private GameState _state;
        private System.Timers.Timer _timer = new System.Timers.Timer(500);
        private readonly IUI_Service UI;
        private readonly IBoard _board;


        public Game(IUI_Service uiService, IBoard board)
        {
            UI = uiService;
            _board = board;
            _board = new Board(21, 21, Shape.RandomShapeGenerator, UI);
            InitializeGame();
            _timer.Elapsed += MoveShapeDown;
            _timer.Elapsed += UpdateGameTimer;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        private void InitializeGame()
        {
            _board.ShapeMergedEvent += _board_ShapeMergedEvent;
            _board.RowClearedEvent += _board_RowClearedEvent;
            _state = new GameState();

            _board.CreateShapes();
            _state.CurrentGameRow = _board.CurrentShape.CurrentBoardStartRow;

            Console.CursorVisible = false;
            UI.DrawEmptyBoard(_board);
            UI.DrawBoardShape(_board.CurrentShape);
            UI.DrawLog("Game started!", _state.ElapsedGameTime);
            UI.DrawHeading();
            UI.DrawScoreAndTime(_state.ElapsedGameTime, 0);
            UI.DrawNextShape(_board.NextShape);
        }

        private async void MoveShapeDown(object sender, System.Timers.ElapsedEventArgs e)
        {
            UI.DrawBoardShape(_board.CurrentShape, clear: true);

            try
            {
                if (!_board.TryMove(Enums.DirectionEnum.Down))
                {
                    UI.DrawBoardShape(_board.CurrentShape);

                    _board.MergeShapeIntoBoardContent(_board.CurrentShape, _board.CurrentShape.CurrentBoardStartRow, _board.CurrentShape.CurrentBoardStartCol);
                    
                    UI.DrawNextShape(_board.NextShape,clear: true);

                    _board.CreateShapes();

                    UI.DrawNextShape(_board.NextShape);

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
                _timer.Enabled = false;
                _timer.Elapsed -= UpdateGameTimer;

                await Task.Delay(100);
                
                UI.DrawLog("Game over", _state.ElapsedGameTime);
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

        private async Task _board_ShapeMergedEvent(object sender, EventArgs e)
        {
            UI.DrawLog("Shape merged to board", _state.ElapsedGameTime);
            UI.DrawSpecialMessage("Whooah!  + 1 Score added!!!");
            await Task.Delay(2000);
            UI.DrawLog("", _state.ElapsedGameTime, clear: true);
            UI.DrawSpecialMessage("", clear: true);
        }

        private async Task _board_RowClearedEvent(object sender, EventArgs e)
        {
            _state.Score += 20;
            UI.DrawSpecialMessage("You cleared a whole row!! +20 score points added!");
            await Task.Delay(3000);
            UI.DrawSpecialMessage("                                                  ");
        } 
    }
}
