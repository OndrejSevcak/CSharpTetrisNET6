using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Delegates;
using Tetris23.Enums;
using Tetris23.Exceptions;
using Tetris23.Extensions;
using Tetris23.Interfaces;
using Tetris23.Services;
using Tetris23.Structs;

namespace Tetris23.Classes
{
    public class Board : IBoard
    {
        public GridCell[,] BoardGrid { get; set; }

        public int Width => BoardGrid.GetLength(0);
        public int Height => BoardGrid.GetLength(1);

        private readonly IUI_Service UI;

        public Board(int width, int height, Func<Shape> shapeGenerator, IUI_Service uiService)
        {
            UI = uiService;
            BoardGrid = new GridCell[width, height];
            _shapeGenerator = shapeGenerator;
            InitializeGrid();
        }

        public Shape CurrentShape { get; set; }
        public Shape NextShape { get; set; }

        private Func<Shape> _shapeGenerator;

        public event ShapeMergedToBoard ShapeMergedEvent;
        public event RowCleared RowClearedEvent;

        private int _occupiedRowLevel; 


        //Methods
        private void InitializeGrid()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    BoardGrid[row, col] = new GridCell(row + 1, col + 1);
                }
            }
        }

        public void CreateShapes()
        {
            CurrentShape = NextShape ?? _shapeGenerator();
            NextShape = _shapeGenerator();

            CurrentShape.CurrentBoardStartCol = Width / 2 + 1;

        }

        public bool TryMove(DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Left:

                    if (IsMovePossible(DirectionEnum.Left))
                    {
                        CurrentShape.CurrentBoardStartCol--;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case DirectionEnum.Right:

                    if (IsMovePossible(DirectionEnum.Right))
                    {
                        CurrentShape.CurrentBoardStartCol++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case DirectionEnum.Down:

                    if (IsMovePossible(DirectionEnum.Down))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    break;
            }

            return false;
        }

        public bool IsMovePossible(DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Left:
                    if(IsMergePossible(CurrentShape, CurrentShape.CurrentBoardStartRow, CurrentShape.CurrentBoardStartCol - 1))
                    {
                        return true;
                    }
                    break;
                case DirectionEnum.Right:
                    if(IsMergePossible(CurrentShape, CurrentShape.CurrentBoardStartRow, CurrentShape.CurrentBoardStartCol + 1))
                    {
                        return true;
                    }
                    break;
                case DirectionEnum.Down:
                    if(IsMergePossible(CurrentShape, CurrentShape.CurrentBoardStartRow + 1, CurrentShape.CurrentBoardStartCol))
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        public bool IsMergePossible(Shape shape, int targetStartRow, int targetStartCol)
        {
            bool mergeIsPossible = true;

            //shapes grid cells that are occupied to be placed on the board
            var shapeCells = shape.ShapeGrid.ToEnumerable().Where(cell => cell.isOccupied).ToList();

            shapeCells.ForEach((cell) =>
            {
                //Are we not inside the board?
                if(cell.row + targetStartRow >= Height ||       
                   cell.col + targetStartCol < 0 ||
                   cell.col + targetStartCol >= Width)
                {
                    mergeIsPossible = false;
                }
                //Are the target board cells free?
                else if (BoardGrid[targetStartRow + cell.row, targetStartCol + cell.col].IsOccupied)
                {
                    mergeIsPossible = false;
                }

            });

            return mergeIsPossible;
        }


        public void MergeShapeIntoBoardContent(Shape shape, int targetStartRow, int targetStartCol)
        {
            shape.ShapeGrid
                    .ToEnumerable()
                    .Where(cell => cell.isOccupied)
                    .ToList()
                    .ForEach((cell) =>
                    {
                        BoardGrid[cell.row + targetStartRow, cell.col + targetStartCol].IsOccupied = true;
                        BoardGrid[cell.row + targetStartRow, cell.col + targetStartCol].ShapeType = shape.Type;

                        if (IsWholeRowOccupied(targetStartRow + cell.row))
                        {
                            RowClearedEvent.Invoke(this, new EventArgs());
                            CleanOccupiedRow(targetStartRow + cell.row);
                            UI.DrawEmptyBoard(this);
                            UI.DrawBoardWithShapes(this);
                        }
                    });

            _occupiedRowLevel = BoardGrid.ToEnumerable().Where(g => g.isOccupied).Select(s => s.row).Min();

            ShapeMergedEvent.Invoke(this, new EventArgs());

            if(_occupiedRowLevel == 0)
            {
                throw new GameOverException("All rows has been occupied");
            }

        }

        public void DropCurrentShape()
        {
            while (IsMergePossible(CurrentShape, CurrentShape.CurrentBoardStartRow + 1, CurrentShape.CurrentBoardStartCol))
            {
                CurrentShape.CurrentBoardStartRow++;
            }
            
            MergeShapeIntoBoardContent(CurrentShape, CurrentShape.CurrentBoardStartRow, CurrentShape.CurrentBoardStartCol);
        }

        /// <summary>
        /// Is the whole board row occupied by tetris shapes? 
        /// </summary>
        private bool IsWholeRowOccupied(int targetBoardMergeRow)
        {
            return !BoardGrid
                    .ToEnumerable()
                    .Where(b => b.row == targetBoardMergeRow)
                    .Any(c => !c.isOccupied);
        }

        private void CleanOccupiedRow(int targetRow)
        {
            for (int i = targetRow;  i > -1; i--)
            {
                if(i == 0)
                {
                    //clean the top row
                    BoardGrid.ToEnumerable2().Where(b => b.row == i).ToList().ForEach((cell) =>
                    {
                        cell.isOccupied = false;
                        cell.type = null;
                    });
                    
                }
                else
                {
                    //set row values from row above(row - 1)
                    BoardGrid.ToEnumerable2().Where(r => r.row == i).ToList().ForEach((cell) =>
                    {
                        //cell.col = stays the same
                        //cell.row = stays the same
                        BoardGrid[cell.row, cell.col].IsOccupied = BoardGrid[cell.row - 1, cell.col].IsOccupied;
                        BoardGrid[cell.row, cell.col].ShapeType = BoardGrid[cell.row - 1, cell.col].ShapeType;
                    });
                }
            }
            
        }
    }
}
