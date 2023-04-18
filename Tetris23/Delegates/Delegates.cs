using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris23.Delegates
{
    public delegate Task ShapeMergedToBoard(object sender, EventArgs e);

    public delegate Task RowCleared(object sender, EventArgs e);
}
