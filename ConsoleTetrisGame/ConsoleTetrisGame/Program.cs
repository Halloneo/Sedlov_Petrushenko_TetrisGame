using System;
namespace ConsoleTetrisGame
{
	class Program
	{
		static void Main()
		{
			GameCore game = new GameCore();
            game.StartGame();
            while (true)
            {                
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Enter)
                    game.StartGame();
                else if (key == ConsoleKey.Escape)
                    break;
            }
        }
	}
}