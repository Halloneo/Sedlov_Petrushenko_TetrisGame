using System;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleTetrisGame
{
	class Program
	{
		public static Tetris Tetris { get; private set; }
		
		public static Thread Mover { get; private set; }

		/// <summary>
		/// Lock of Drawing Function
		/// </summary>
		public static bool DrawLock { get; private set; }
		
		public static bool IsWriteGrid { get; private set; }
		
		public static int Points { get; private set; }
		
		public static int ThreadCounter { get; private set; }
		
		

		#region Drawing Symbols
		private const string Block = "■ ";
		private const string Border = "■ ";
		private const string Empty = "  ";
		#endregion

		private const int Step = 10;

		public static int[,] ClearBlock { get; } =
		{
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0}
		};
		
		static void T_LinesDone(int lines) => Points += lines;

		public static void Main()
		{
			//Default Values and Game initialization
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.CursorVisible = false;
			Points = 0;
			DrawLock = false;
			Tetris = new Tetris(10, 20);
			Tetris.LinesDone += T_LinesDone;
			IsWriteGrid = false;
			Console.WindowWidth = 52;
			Console.WindowHeight = 22;
			

			Console.Write(@"
	Controls:
	[Right Arrow]	Move Block Right
	[Left Arrow]	Move Block Left
	[Up Arrow]	Turn Clockwise
	[Dow Arrow]	Push block down 1 step
	[Space]		Smash Bloock down
	[ESC]		Exit Game
	
	Press a Key to start");
			
			Song();
			Console.ReadKey(true);
			Console.Clear();
			
			Tetris.Start();
			Mover = new Thread(Stepper) { IsBackground = true };
			Mover.Start();
			
			while (Tetris.Running)
			{
				Thread.Sleep(10);
				if (Console.KeyAvailable)
				{
					lock (Tetris)
					{
						switch (Console.ReadKey(true).Key)
						{
							case ConsoleKey.LeftArrow:
								Tetris.KeyPressing(Tetris.Key.Left);
								break;
							case ConsoleKey.RightArrow:
								Tetris.KeyPressing(Tetris.Key.Right);
								break;
							case ConsoleKey.UpArrow:
								Tetris.KeyPressing(Tetris.Key.Up);
								ThreadCounter = 0;
								break;
							case ConsoleKey.DownArrow:
								Tetris.KeyPressing(Tetris.Key.Down);
								ThreadCounter = 0;
								break;
							case ConsoleKey.Spacebar:
								Tetris.KeyPressing(Tetris.Key.Space);
								break;
							case ConsoleKey.Escape:
								Tetris.GameOver();
								break;
						}
						DrawField();
					}
				}
			}
			
			Thread.Sleep(100);
			Console.Clear();
			Write($@"
		    Game Over!

	You made {Points} lines. Press Esc to exit
    ",
			ConsoleColor.Red);
			while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
			Console.ResetColor();
			Console.CursorVisible = true;
		}
		
		static void Stepper()
		{
			while (Tetris.Running)
			{
				ThreadCounter = 500;
				Tetris.Step();
				DrawField();
				
				//Increase Speed when Lines are made
				while (ThreadCounter < 1000 - Points * 5 && Tetris.Running)
				{
					Thread.Sleep(Step);
					ThreadCounter += Step;
				}
			}
		}
		
		private static void DrawField()
		{
			//locks the Field
			while (DrawLock) { }

			DrawLock = true;
			
			int posX = Console.CursorLeft;
			int posY = Console.CursorTop;

			var width = Tetris.Container.GetLength(0) + 5;

			//Draw points
			Console.CursorLeft = width;
			Console.CursorTop = 1;
			Console.WriteLine($"Lines: {Points}");

			//Draw Field
			Console.SetCursorPosition(posX, posY);
			WriteArray(Tetris.Level, true);

			//Draw next Block
			Console.CursorLeft = width;
			Console.CursorTop = 3;
			Console.WriteLine("■ ■ ■ ■ ■ ■");
			Console.CursorLeft = width;
			Console.CursorTop = 4;
			WriteArray(ClearBlock, true);
			Console.CursorLeft = width+2;
			Console.CursorTop = 5;
			WriteArray(Tetris.Next, false);

			Console.SetCursorPosition(posX, posY);
			DrawLock = false;
		}
		
		static void WriteArray(int[,] array, bool isWriteBorder)
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
		
		static void Write(string str, ConsoleColor color)
		{
			ConsoleColor save = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write(str);
			
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.ForegroundColor = save;
		}

		static void Song()
		{
			Console.Beep(1320, 500);
			Console.Beep(990, 250);
			Console.Beep(1056, 250);
			Console.Beep(1188, 250);
			Console.Beep(1320, 125);
			Console.Beep(1188, 125);
			Console.Beep(1056, 250);
			Console.Beep(990, 250);
			Console.Beep(880, 500);
			Console.Beep(880, 250);
			Console.Beep(1056, 250);
			Console.Beep(1320, 500);
			Console.Beep(1188, 250);
			Console.Beep(1056, 250);
			Console.Beep(990, 750);
			Console.Beep(1056, 250);
			Console.Beep(1188, 500);
			Console.Beep(1320, 500);
			Console.Beep(1056, 500);
			Console.Beep(880, 500);
			Console.Beep(880, 500);
			Thread.Sleep(250);
			Console.Beep(1188, 500);
			Console.Beep(1408, 250);
			Console.Beep(1760, 500);
			Console.Beep(1584, 250);
			Console.Beep(1408, 250);
			Console.Beep(1320, 750);
			Console.Beep(1056, 250);
			Console.Beep(1320, 500);
			Console.Beep(1188, 250);
			Console.Beep(1056, 250);
			Console.Beep(990, 500);
			Console.Beep(990, 250);
			Console.Beep(1056, 250);
			Console.Beep(1188, 500);
			Console.Beep(1320, 500);
			Console.Beep(1056, 500);
			Console.Beep(880, 500);
			Console.Beep(880, 500);
		}

	}
}
