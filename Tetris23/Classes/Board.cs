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

        public void TryMove(DirectionEnum direction)
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
                        CurrentShape.CurrentBoardStartRow++;
                    }
                    break;
                default:
                    break;
            }
        }

        public bool IsMovePossible(DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Left:
                    if(IsWithingBoard() &&
                       IsMergePossible(CurrentShape, CurrentShape.CurrentBoardStartRow, CurrentShape.CurrentBoardStartCol - 1))
                    {
                        return true;
                    }
                    break;
                case DirectionEnum.Right:
                    if(IsWithingBoard() &&
                       IsMergePossible(CurrentShape, CurrentShape.CurrentBoardStartRow, CurrentShape.CurrentBoardStartCol + 1))
                    {
                        return true;
                    }
                    break;
                case DirectionEnum.Down:
                    if(IsNextRowAvailable() &&
                       IsMergePossible(CurrentShape, CurrentShape.CurrentBoardStartRow + 1, CurrentShape.CurrentBoardStartCol))
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }


        public bool IsNextRowAvailable()
        {
            return CurrentShape.CurrentBoardStartRow + CurrentShape.Height == Height ? false : true;    //+1 row is included in currentRow + Height
        }

        public bool IsWithingBoard()
        {
            if(CurrentShape.CurrentBoardStartCol >= 0 &&
               CurrentShape.CurrentBoardStartCol + CurrentShape.Width - 1 <= Width)
            {
                return true;
            }
            return false;
        }

        public bool IsMergePossible(Shape shape, int targetStartRow, int targetStartCol)
        {
            bool mergeIsPossible = true;

            //all cells from the grid that are already occupied by any shape
            var occupiedBoardCells = BoardGrid.ToEnumerable().Where(cell => cell.isOccupied).ToList();

            //cells that are actually part of the shape type
            var shapeCells = shape.ShapeGrid.ToEnumerable().Where(cell => cell.isOccupied).ToList().Select(m => new {X = m.row, Y = m.col});

            //check if the new target possition of shape is out of those cells
            occupiedBoardCells.ForEach((cell) =>
            {
                if(shapeCells.Any(a => a.X == cell.row && a.Y == cell.col))
                {
                    mergeIsPossible = false;
                }
            });

            return mergeIsPossible;
        }


        public void MergeCurrentShapeIntoBoardContent()
        {

        }
    }
}
