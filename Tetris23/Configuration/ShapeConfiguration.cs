using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Enums;
using Tetris23.Structs;

namespace Tetris33.Classes
{
    public static class ShapeConfiguration
    {
        public static readonly Dictionary<ShapeTypeEnum, (GridCell[,] Grid, ConsoleColor Color)> ShapeTypeDict =
            new Dictionary<ShapeTypeEnum, (GridCell[,] grid, ConsoleColor color)>
            {
                [ShapeTypeEnum.I] = (new GridCell[,]
                {
                    { new GridCell(1, 1), new GridCell(1, 2, true), new GridCell(1, 3) },
                    { new GridCell(2, 1), new GridCell(2, 2, true), new GridCell(2, 3) },
                    { new GridCell(3, 1), new GridCell(3, 2, true), new GridCell(3, 3) },
                }, ConsoleColor.Blue),

                [ShapeTypeEnum.J] = (new GridCell[,]
                {
                    { new GridCell(1, 1, true), new GridCell(1, 2, true), new GridCell(1, 3) },
                    { new GridCell(2, 1), new GridCell(2, 2, true), new GridCell(2, 3) },
                    { new GridCell(3, 1), new GridCell(3, 2, true), new GridCell(3, 3) },
                }, ConsoleColor.Red),

                [ShapeTypeEnum.L] = (new GridCell[,]
                {
                    { new GridCell(1, 1), new GridCell(1, 2, true), new GridCell(1, 3, true) },
                    { new GridCell(2, 1), new GridCell(2, 2, true), new GridCell(2, 3) },
                    { new GridCell(3, 1), new GridCell(3, 2, true), new GridCell(3, 3) },
                }, ConsoleColor.DarkCyan),

                [ShapeTypeEnum.S] = (new GridCell[,]
                {
                    { new GridCell(1, 1,true), new GridCell(1, 2, true), new GridCell(1, 3) },
                    { new GridCell(2, 1), new GridCell(2, 2, true), new GridCell(2, 3) },
                    { new GridCell(3, 1), new GridCell(3, 2, true), new GridCell(3, 3, true) },
                }, ConsoleColor.Magenta),

                [ShapeTypeEnum.Z] = (new GridCell[,]
                {
                    { new GridCell(1, 1), new GridCell(1, 2, true), new GridCell(1, 3, true) },
                    { new GridCell(2, 1), new GridCell(2, 2, true), new GridCell(2, 3) },
                    { new GridCell(3, 1, true), new GridCell(3, 2, true), new GridCell(3, 3) },
                }, ConsoleColor.Green),

                [ShapeTypeEnum.T] = (new GridCell[,]
                {
                    { new GridCell(1, 1), new GridCell(1, 2, true), new GridCell(1, 3) },
                    { new GridCell(2, 1), new GridCell(2, 2, true), new GridCell(2, 3) },
                    { new GridCell(3, 1, true), new GridCell(3, 2, true), new GridCell(3, 3,true) },
                }, ConsoleColor.DarkYellow),

                [ShapeTypeEnum.O] = (new GridCell[,]
                {
                    { new GridCell(1, 1, true), new GridCell(1, 2, true), new GridCell(1, 3, true) },
                    { new GridCell(2, 1, true), new GridCell(2, 2), new GridCell(2, 3, true) },
                    { new GridCell(3, 1, true), new GridCell(3, 2, true), new GridCell(3, 3, true) },
                }, ConsoleColor.White),
            };
    }
}
