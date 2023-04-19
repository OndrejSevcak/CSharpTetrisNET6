using Microsoft.Extensions.DependencyInjection;
using Tetris23.Classes;
using Tetris23.Interfaces;
using Tetris23.Services;


//DI setup
var serviceProvider = new ServiceCollection()
    .AddSingleton<IUI_Service, UI>()
    .AddSingleton<IGame, Game>()
    .BuildServiceProvider();

//Start the game
IGame game = serviceProvider.GetRequiredService<IGame>();
game.Run();



Console.CursorVisible = false;
