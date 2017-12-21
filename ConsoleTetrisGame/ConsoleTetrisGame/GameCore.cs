using System;
using System.Threading;

namespace ConsoleTetrisGame
{
	public class GameCore
	{
        /// <summary>
        /// Object of class Tetris
        /// </summary>
		public Tetris Tetris { get; private set; }

        /// <summary>
        /// Thread for method Stepper
        /// </summary>
		public Thread Mover { get; private set; }

		/// <summary>
		/// Lock of Drawing Function
		/// </summary>
		private  bool _drawLock;

        /// <summary>
        /// Ammount of deleted lines
        /// </summary>
		public int Lines { get; private set; }

		/// <summary>
		/// Score of current game
		/// </summary>
		public int Points { get; private set; }

		/// <summary>
		/// Counter which influences on speed of the game
		/// </summary>
		public int ThreadCounter { get; private set; }

        /// <summary>
        /// Constante that is being added to ThreadCounter
        /// </summary>
		private const int Step = 10;
		
        /// <summary>
        /// Adds amount of done lines to score
        /// </summary>
        /// <param name="lines">Amount of done lines</param>
		private void T_LinesDone(int lines)
		{
			if (lines == 1)
				Points += 100;
			else if (lines == 2)
				Points += 300;
			else if (lines == 3)
				Points += 700;
			else if (lines == 4)
				Points += 1500;
			
			Lines += lines;
		}

		/// <summary>
        /// Core method of the game
        /// </summary>
		public void StartGame()
		{
			//Default Values and Game initialization
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.CursorVisible = false;
			Console.WindowWidth = 55;
			Console.WindowHeight = 22;
			Lines = 0;
			Points = 0;

			_drawLock = false;
			Tetris = new Tetris(10, 20);
			Tetris.LinesDone += T_LinesDone;

			Writer.PrintStart();
			Console.ReadKey(true);
			Console.Clear();

			Tetris.Start();
			Mover = new Thread(Stepper) { IsBackground = false };
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
								break;
							case ConsoleKey.DownArrow:
								Tetris.KeyPressing(Tetris.Key.Down);
								break;
							case ConsoleKey.Spacebar:
								Tetris.KeyPressing(Tetris.Key.Space);
								break;
							case ConsoleKey.Escape:
								Tetris.GameOver();
								break;
						}
						Writer.DrawField(ref _drawLock, Lines, Points, Tetris.Container.GetLength(0)+5, Tetris.Level, Tetris.Next);
					}
				}
			}
			Thread.Sleep(100);
			Console.Clear();
			Writer.PrintEnd(Lines, Points);
            Console.ResetColor();
		}

        /// <summary>
        /// Moves block down
        /// </summary>
		public void Stepper()
		{
			while (Tetris.Running)
			{
				ThreadCounter = 800;
				Tetris.Step();
				Writer.DrawField(ref _drawLock, Lines, Points, Tetris.Container.GetLength(0) + 5, Tetris.Level, Tetris.Next);

				//Increase Speed when Lines are made
				while (ThreadCounter < 1000 - Lines * 3 && Tetris.Running)
				{
					Thread.Sleep(Step);
					ThreadCounter += Step;
				}
			}
		}
	}
}
