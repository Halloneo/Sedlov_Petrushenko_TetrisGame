using System;

namespace ConsoleTetrisGame
{
	public class Writer
	{
		private const string Block = "■ ";
		private const string Border = "■ ";
		private const string Empty = "  ";
        /// <summary>
        /// Clears block
        /// </summary>
		public static int[,] ClearBlock { get; } =
		{
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0}
		};
        /// <summary>
        /// Writes array and border of array if needed
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="isWriteBorder">Is it needed to write border</param>
		public static void WriteArray(int[,] array, bool isWriteBorder)
		{
			int x = Console.CursorLeft;
			int width = array.GetUpperBound(0);
			int heigth = array.GetUpperBound(1);
			for (int i = 0; i <= width; i++)
			{
				if (isWriteBorder)
					Console.Write(Border);
				for (int j = 0; j <= heigth; j++)
				{
					switch (array[i, j])
					{
						case 1:
							Write(Block, ConsoleColor.Cyan);
							break;
						case 2:
							Write(Block, ConsoleColor.Magenta);
							break;
						case 3:
							Write(Block, ConsoleColor.Blue);
							break;
						case 4:
							Write(Block, ConsoleColor.Green);
							break;
						case 5:
							Write(Block, ConsoleColor.Yellow);
							break;
						case 6:
							Write(Block, ConsoleColor.Red);
							break;
						case 7:
							Write(Block, ConsoleColor.DarkGreen);
							break;
						default:
							Console.Write(Empty);
							break;
					}
				}
				if (isWriteBorder)
					Console.WriteLine(Border);
				else
					Console.WriteLine();
				Console.CursorLeft = x;
			}

			for (int i = 0; i <= heigth + 2; i++)
			{
				if (isWriteBorder)
					Console.Write(Border);
			}
		}
        /// <summary>
        /// Writes string with chosen color
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="color">Color</param>
		public static void Write(string str, ConsoleColor color)
		{
			ConsoleColor save = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write(str);

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.ForegroundColor = save;
		}
        /// <summary>
        /// Prints current game field
        /// </summary>
        /// <param name="drawLock">Lock of Drawing Function</param>
        /// <param name="points">Score</param>
        /// <param name="container">Game field</param>
        /// <param name="level">Block</param>
        /// <param name="nextBlock">Next block</param>
		public static void DrawField(ref bool drawLock, int points, int[,] container, int[,] level, int[,] nextBlock)
		{
			//locks the Field
			while (drawLock) { }

			drawLock = true;

			int posX = Console.CursorLeft;
			int posY = Console.CursorTop;

			var width = container.GetLength(0) + 5;

			//Draw points
			Console.CursorLeft = width;
			Console.CursorTop = 1;
			Console.WriteLine($"Lines: {points}");

			//Draw Field
			Console.SetCursorPosition(posX, posY);
			WriteArray(level, true);

			//Draw next Block
			Console.CursorLeft = width;
			Console.CursorTop = 3;
			Console.WriteLine("■ ■ ■ ■ ■ ■");
			Console.CursorLeft = width;
			Console.CursorTop = 4;
			WriteArray(ClearBlock, true);
			Console.CursorLeft = width + 2;
			Console.CursorTop = 5;
			WriteArray(nextBlock, false);

			Console.SetCursorPosition(posX, posY);
			drawLock = false;
		}
        /// <summary>
        /// Prints Start menu on console
        /// </summary>
		public static void PrintStart()
		{
			Write(@"
	Controls:
	[Right Arrow]	Move Block Right
	[Left Arrow]	Move Block Left
	[Up Arrow]	Turn Clockwise
	[Dow Arrow]	Push block down 1 step
	[Space]		Smash Bloock down
	[ESC]		Exit Game
	
	Press a Key to start", ConsoleColor.White);
		}
        /// <summary>
        /// Prints End menu and current score on console
        /// </summary>
        /// <param name="points"></param>
		public static void PrintEnd(int points)
		{

			Write($@"
		    Game Over!

	        You made {points} lines. 
    Press Esc to exit or press Enter to restart
    ",
			ConsoleColor.White);
		}
	}
	
}
