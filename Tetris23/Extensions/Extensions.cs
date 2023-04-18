using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Enums;
using Tetris23.Structs;

namespace Tetris23.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<(int row, int col, bool isOccupied)> ToEnumerable(this GridCell[,] structArray)
        {
            for (int row = 0; row < structArray.GetLength(0); row++)
            {
                for (int col = 0; col < structArray.GetLength(1); col++)
                {
                    yield return (row, col, structArray[row, col].IsOccupied);
                }
            }
        }

        public static IEnumerable<(int row, int col, bool isOccupied, ShapeTypeEnum? type)> ToEnumerable2(this GridCell[,] structArray)
        {
            for (int row = 0; row < structArray.GetLength(0); row++)
            {
                for (int col = 0; col < structArray.GetLength(1); col++)
                {
                    yield return (row, col, structArray[row, col].IsOccupied, structArray[row,col].ShapeType);
                }
            }
        }
    }
}
