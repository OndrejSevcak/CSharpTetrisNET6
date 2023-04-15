using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Enums;
using Tetris23.Structs;
using Tetris33.Classes;

namespace Tetris23.Classes
{
    public class Shape
    {
        //members
        public GridCell[,] ShapeGrid { get; private set; }

        public ShapeTypeEnum Type { get; private set; }

        public ConsoleColor Color { get; private set; }


        public int Width => ShapeGrid.GetLength(0);
        public int Height => ShapeGrid.GetLength(1);


        //public read-write
        public int CurrentBoardStartRow { get; set; } = 0;
        public int CurrentBoardStartCol { get; set; } = 0;


        //ctor
        public Shape(ShapeTypeEnum type)
        {
            ShapeGrid = new GridCell[3, 3];
            Type = type;
            InitializeGrid();
        }

        
        public static Func<Shape> RandomShapeGenerator = () =>
        {
            Random random = new Random();
            int shapeIndex = random.Next(0, 6);
            return new Shape((ShapeTypeEnum)shapeIndex);
        };

        //methods
        public void Move(DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Left:
                    break;
                case DirectionEnum.Right:
                    break;
                case DirectionEnum.Down:
                    CurrentBoardStartRow++;
                    break;
                default:
                    break;
            }
        }

        public void Rotate()
        {
            //first row => trird coll
            //second row => seconnd coll
            //third row => first coll

            GridCell[,] rotatedGrid = new GridCell[3, 3]
            {
                {ShapeGrid[2,0], ShapeGrid[1, 0], ShapeGrid[0,0]},
                {ShapeGrid[2,1], ShapeGrid[1, 1], ShapeGrid[0,1]},
                {ShapeGrid[2,2], ShapeGrid[1, 2], ShapeGrid[0,2]}
            };

            ShapeGrid = rotatedGrid;
        }

        private void InitializeGrid()
        {
            switch (Type)
            {
                case ShapeTypeEnum.I:
                    ShapeGrid = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.I].Grid;
                    Color = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.I].Color;
                    break;
                case ShapeTypeEnum.J:
                    ShapeGrid = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.J].Grid;
                    Color = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.J].Color;
                    Color = ConsoleColor.Red;
                    break;
                case ShapeTypeEnum.L:
                    ShapeGrid = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.L].Grid;
                    Color = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.L].Color;
                    break;
                case ShapeTypeEnum.S:
                    ShapeGrid = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.S].Grid;
                    Color = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.S].Color;
                    break;
                case ShapeTypeEnum.Z:
                    ShapeGrid = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.Z].Grid;
                    Color = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.Z].Color;
                    break;
                case ShapeTypeEnum.T:
                    ShapeGrid = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.T].Grid;
                    Color = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.T].Color;
                    break;
                case ShapeTypeEnum.O:
                    ShapeGrid = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.O].Grid;
                    Color = ShapeConfiguration.ShapeTypeDict[ShapeTypeEnum.O].Color;
                    break;
                default:
                    break;
            }
        }
    }
}
