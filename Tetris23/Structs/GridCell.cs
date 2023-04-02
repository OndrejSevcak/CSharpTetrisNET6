﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Enums;
using Tetris23.Interfaces;

namespace Tetris23.Structs
{
    /// <summary>
    /// Represent a x,y coordinates of either tetris shape or tetris board
    /// </summary>
    public struct GridCell : IShapeRepresentable
    {
        public int X { get; private set; }  
        public int Y { get; private set; }
        public bool IsOccupied { get; private set; }    //True if this cell represents a Shape

        //Interface members
        public ShapeTypeEnum? ShapeType { get; set; } = null;

        
        public GridCell(int x, int y, bool isOccupied = false)
        {
            X = x;
            Y = y;
            IsOccupied = isOccupied;
        }
    }
}
