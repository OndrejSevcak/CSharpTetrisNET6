using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tetris23.Classes;
using Tetris23.Interfaces;
using Tetris23.Services;
using Xunit;

namespace Tetris23_xUnit_Tests.BoardTests
{

    public class BoardTests
    {
        private readonly IUI_Service _uiService;

        public BoardTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IUI_Service, UI>()
                .BuildServiceProvider();

            _uiService = serviceProvider.GetService<IUI_Service>();
        }

        [Fact]
        public void BT_00_IsMergePossible_EmptyBoard_FirstRow_True()
        {
            //Arrange
            Board board = new Board(21, 21, Shape.RandomShapeGenerator, _uiService);
            Shape shape = new Shape(Tetris23.Enums.ShapeTypeEnum.S);
            shape.CurrentBoardStartRow = 0;
            shape.CurrentBoardStartCol = 11;

            //Act
            bool mergeResult = board.IsMergePossible(shape,0,10);

            //Assert
            Assert.True(mergeResult);
        }

        [Fact]
        public void BT_01_IsMergePossible_TargetOneRowAboveMergedIShape_True()
        {
            //Arrange
            Board board = new Board(21, 21, Shape.RandomShapeGenerator, _uiService);
            board.BoardGrid[20, 0].IsOccupied = true;
            board.BoardGrid[20, 0].ShapeType = Tetris23.Enums.ShapeTypeEnum.I;
            board.BoardGrid[20, 1].IsOccupied = true;
            board.BoardGrid[20, 1].ShapeType = Tetris23.Enums.ShapeTypeEnum.I;
            board.BoardGrid[20, 2].IsOccupied = true;
            board.BoardGrid[20, 2].ShapeType = Tetris23.Enums.ShapeTypeEnum.I;

            Shape shape = new Shape(Tetris23.Enums.ShapeTypeEnum.S);
            shape.CurrentBoardStartRow = 16;
            shape.CurrentBoardStartCol = 0;

            //Act
            bool mergeResult = board.IsMergePossible(shape, 17, 0);

            //Assert
            Assert.True(mergeResult);
        }

        [Fact]
        public void BT_02_IsMergePossible_TargetRowsOccupiedByIShape_False()
        {
            //Arrange
            Board board = new Board(21, 21, Shape.RandomShapeGenerator, _uiService);
            board.BoardGrid[20, 0].IsOccupied = true;
            board.BoardGrid[20, 0].ShapeType = Tetris23.Enums.ShapeTypeEnum.I;
            board.BoardGrid[20, 1].IsOccupied = true;
            board.BoardGrid[20, 1].ShapeType = Tetris23.Enums.ShapeTypeEnum.I;
            board.BoardGrid[20, 2].IsOccupied = true;
            board.BoardGrid[20, 2].ShapeType = Tetris23.Enums.ShapeTypeEnum.I;

            Shape shape = new Shape(Tetris23.Enums.ShapeTypeEnum.S);
            shape.CurrentBoardStartRow = 17;
            shape.CurrentBoardStartCol = 0;

            //Act
            bool mergeResult = board.IsMergePossible(shape, 18, 0);

            //Assert
            Assert.False(mergeResult);
        }

        [Fact]
        public void BT_03_MergeShapeToBoardContent_Success()
        {
            //Arrange
            Board emptyBoard = new Board(21, 21, Shape.RandomShapeGenerator, _uiService);
            emptyBoard.ShapeMergedEvent += _shape_Merged_To_Board_Listener;
            Board mergedBoard = new Board(21, 21, Shape.RandomShapeGenerator, _uiService);
            mergedBoard.BoardGrid[18, 1].IsOccupied = true;
            mergedBoard.BoardGrid[18, 1].ShapeType = Tetris23.Enums.ShapeTypeEnum.I;
            mergedBoard.BoardGrid[19, 1].IsOccupied = true;
            mergedBoard.BoardGrid[19, 1].ShapeType = Tetris23.Enums.ShapeTypeEnum.I;
            mergedBoard.BoardGrid[20, 1].IsOccupied = true;
            mergedBoard.BoardGrid[20, 1].ShapeType = Tetris23.Enums.ShapeTypeEnum.I;

            Shape shapeToMerge = new Shape(Tetris23.Enums.ShapeTypeEnum.I);

            //Act
            emptyBoard.MergeShapeIntoBoardContent(shapeToMerge, 18, 0);

            string expectedJson = JsonConvert.SerializeObject(mergedBoard);
            string actualJson = JsonConvert.SerializeObject(emptyBoard);

            //Assert
            Assert.Equal(expectedJson, actualJson);
        }

        private async Task _shape_Merged_To_Board_Listener(object sender, EventArgs e)
        {
            Console.WriteLine("Merged event received");
            await Task.Delay(100);
        }
    }
}
