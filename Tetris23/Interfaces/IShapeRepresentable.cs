using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris23.Enums;

namespace Tetris23.Interfaces
{
    public interface IShapeRepresentable
    {
        public ShapeTypeEnum? ShapeType { get; set; }

    }
}
