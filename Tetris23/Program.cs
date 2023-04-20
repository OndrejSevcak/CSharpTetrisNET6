using Microsoft.Extensions.DependencyInjection;
using Tetris23.Classes;
using Tetris23.Interfaces;
using Tetris23.Services;


//DI setup
var serviceProvider = new ServiceCollection()
    .AddSingleton<IUI_Service, UI>()
    .AddSingleton<IGame, Game>()
    .AddSingleton<IBoard, Board>(provider =>
    {
        int width = 21;
        int heigth = 21;
        Func<Shape> shapeGenerator = Shape.RandomShapeGenerator;
        IUI_Service uiService = provider.GetRequiredService<IUI_Service>();

        return new Board(width, heigth, shapeGenerator, uiService);
    })
    .BuildServiceProvider();


//Start the game
try
{
    serviceProvider.GetRequiredService<IGame>().Run();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred when starting the app, detail: {ex.Message}");
}

