using System;
using System.Media;
using System.Threading;

namespace ConsoleTetrisGame
{
	class Program
	{
		static void Main()
		{
			GameCore game = new GameCore();
			SoundPlayer sp = new SoundPlayer();
			
			sp.SoundLocation = Environment.CurrentDirectory + "\\Music.wav";
			
			sp.PlayLooping();
            game.StartGame();
			sp.Stop();
			
			while (true)
            {                
                var key = Console.ReadKey(true).Key;
				if (key == ConsoleKey.Enter)
				{
					sp.PlayLooping();
					game.StartGame();
					sp.Stop();
				}
                else if (key == ConsoleKey.Escape)
                    break;
            }
        }
}
}