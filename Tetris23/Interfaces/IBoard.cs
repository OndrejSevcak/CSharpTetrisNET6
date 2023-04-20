using System;
using Tetris23.Classes;
using Tetris23.Delegates;
using Tetris23.Enums;
using Tetris23.Structs;

namespace Tetris23.Interfaces
{
    public interface IBoard
    {
        void CreateShapes();
        bool TryMove(DirectionEnum direction);
        bool IsMovePossible(DirectionEnum direction);
        bool IsMergePossible(Shape shape, int targetStartRow, int targetStartCol);
        void MergeShapeIntoBoardContent(Shape shape, int targetStartRow, int targetStartCol);
        void DropCurrentShape();

        public event ShapeMergedToBoard ShapeMergedEvent;
        public event RowCleared RowClearedEvent;

        public Shape CurrentShape { get; set; }
        public Shape NextShape { get; set; }

        public GridCell[,] BoardGrid { get; set; }

        public int Width => BoardGrid.GetLength(0); 
        public int Height => BoardGrid.GetLength(1);
    }
}
