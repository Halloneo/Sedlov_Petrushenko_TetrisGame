using System;

namespace ConsoleTetrisGame
{
	public class Writer
	{
		private const string Block = "■";
		private const string Border = "■";
		private const string Empty = " │";
		private const string Grid = "│";

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
				{
					Console.Write(Border);
					Write(Grid, ConsoleColor.DarkGray, false);
				}
				for (int j = 0; j <= heigth; j++)
				{
					switch (array[i, j])
					{
						case 1:
							Write(Block, ConsoleColor.Cyan, true);
							break;
						case 2:
							Write(Block, ConsoleColor.Yellow, true);
							break;
						case 3:
							Write(Block, ConsoleColor.DarkCyan, true);
							break;
						case 4:
							Write(Block, ConsoleColor.Blue, true);
							break;
						case 5:
							Write(Block, ConsoleColor.Magenta, true);
							break;
						case 6:
							Write(Block, ConsoleColor.Red, true);
							break;
						case 7:
							Write(Block, ConsoleColor.Green, true);
							break;
						default:
							Write(Empty, ConsoleColor.DarkGray, false);
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
					Console.Write(Border+" ");
			}
		}

		/// <summary>
		/// Writes string with chosen color
		/// </summary>
		/// <param name="str">String</param>
		/// <param name="color">Color</param>
		/// <param name="isWriteGrid">Is it needed to write grid</param>
		public static void Write(string str, ConsoleColor color, bool isWriteGrid)
		{
			ConsoleColor save = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write(str);
			if (isWriteGrid)
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("│");
			}
			Console.ForegroundColor = save;
		}

		/// <summary>
		/// Prints current game field
		/// </summary>
		/// <param name="drawLock">Lock of Drawing Function</param>
		/// <param name="lines">Did lines</param>
		/// <param name="points">Score</param>
		/// <param name="width">Width of game field</param>
		/// <param name="level">Game field</param>
		/// <param name="nextBlock">Next block</param>
		public static void DrawField(ref bool drawLock, int lines, int points, int width, int[,] level, int[,] nextBlock)
		{
			//locks the Field
			while (drawLock) { }

			drawLock = true;

			int posX = Console.CursorLeft;
			int posY = Console.CursorTop;

			//Draw lines
			Console.CursorLeft = width;
			Console.CursorTop = 1;
			Console.WriteLine($"Lines: {lines}");
			Console.CursorLeft = width;
			Console.CursorTop = 2;
			Console.WriteLine($"Points: {points}");

			//Draw Field
			Console.SetCursorPosition(posX, posY);
			WriteArray(level, true);

			//Draw next Block
			Console.CursorLeft = width;
			Console.CursorTop = 4;
			Console.WriteLine("■ ■ ■ ■ ■ ■");
			Console.CursorLeft = width;
			Console.CursorTop = 5;
			WriteArray(ClearBlock, true);
			Console.CursorLeft = width + 2;
			Console.CursorTop = 6;
			WriteArray(nextBlock, false);

			Console.SetCursorPosition(posX, posY);
			drawLock = false;
			
		}
		
        /// <summary>
        /// Prints Start menu on console
        /// </summary>
		public static void PrintStart()
		{
			Console.Write(@"
	Controls:
	[Right Arrow]	Move Block Right
	[Left Arrow]	Move Block Left
	[Up Arrow]	Turn Clockwise
	[Down Arrow]	Push block down 1 step
	[Space]		Smash Bloock down
	[ESC]		Exit Game

	Points:
	[1 Line at once]	100 Points
	[2 Line at once]	300 Points
	[3 Line at once]	700 Points
	[4 Line at once]	1500 Points
	
	Press a Key to start");
		}
		
        /// <summary>
        /// Prints End menu and current score on console
        /// </summary>
        /// <param name="lines"></param>
		public static void PrintEnd(int lines, int points)
		{

			Console.Write($@"
		    Game Over!
	  You made {lines} lines and scored {points} points. 
    Press Esc to exit or press Enter to restart game
      ");
		}
	}
	
}
