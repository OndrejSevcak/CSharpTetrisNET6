using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Enums;
using Tetris23.Exceptions;
using Tetris23.Extensions;
using Tetris23.Structs;

namespace Tetris23.Classes
{
    public class Board
    {
        public GridCell[,] BoardGrid { get; set; }

        public int Width => BoardGrid.GetLength(0);
        public int Height => BoardGrid.GetLength(1);

        public Board(int width, int height, Func<Shape> shapeGenerator)
        {
            BoardGrid = new GridCell[width, height];
            _shapeGenerator = shapeGenerator;
            InitializeGrid();
        }


        public Shape CurrentShape { get; set; }
        public Shape NextShape { get; set; }

        private Func<Shape> _shapeGenerator;


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
                    break;
                case DirectionEnum.Right:
                    break;
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
                //Are we inside the board?
                if(cell.row - 1 + targetStartRow > Height ||       
                   cell.col - 1 + targetStartCol <= 0 ||
                   cell.col - 1 + targetStartCol > Width)
                {
                    mergeIsPossible = false;
                }

                //Are the target board cells free?
                if(BoardGrid[targetStartRow - 2 + cell.row - 1, targetStartCol - 1 + cell.col - 1].IsOccupied)  //row and colls are not zero based
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
                        BoardGrid[cell.row - 1 + targetStartRow - 1, cell.col - 1 + targetStartCol].IsOccupied = true;
                        BoardGrid[cell.row - 1 + targetStartRow - 1, cell.col - 1 + targetStartCol].ShapeType = shape.Type;
                    });
        }
    }
}
