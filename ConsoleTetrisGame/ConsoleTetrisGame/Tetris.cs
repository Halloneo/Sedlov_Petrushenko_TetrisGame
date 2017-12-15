namespace ConsoleTetrisGame
{
    public class Tetris
    {
        /// <summary>
        /// Game Over Event Delegate
        /// </summary>
        public delegate void GameOverHandler();
		
        /// <summary>
        /// Block is fixed Event Delegate
        /// </summary>
		public delegate void BlockFixedHandler();

		/// <summary>
        /// At least one line is done Event Delegate
        /// </summary>
        /// <param name="lines">Number of done lines</param>
		public delegate void LinesDoneHandler(int lines);

		/// <summary>
        /// Event that handle end of the game
        /// </summary>
		public event GameOverHandler GameOverEvent;
		
        /// <summary>
        /// Event that handle if the block is fixed
        /// </summary>
		public event BlockFixedHandler BlockFixed;

        /// <summary>
        /// Event that handle at least one done line
        /// </summary>
        public event LinesDoneHandler LinesDone;

        /// <summary>
        /// Keys that may be used as game controls
        /// </summary>
		public enum Key
        {
            Left,
            Right,
            Up,
            Down,
            Space
        }

        /// <summary>
        /// Game field
        /// </summary>
		public int[,] Container { get; set; }

        /// <summary>
        /// X coordinate of current block position
        /// </summary>
		public int PositionX { get; private set; }

        /// <summary>
        /// Y coordinate of current block position
        /// </summary>
		public int PositionY { get; private set; }

        /// <summary>
        /// Current block
        /// </summary>
		public int[,] CurrentBlock { get; private set; }

        /// <summary>
        /// Property shows if the game is running
        /// </summary>
		public bool Running { get; private set; }

        /// <summary>
        /// Next block
        /// </summary>
        public int[,] NextBlock { get; private set; }
        
        /// <summary>
        /// Current game field
        /// </summary>
		public int[,] Level
		{
			get
			{
				var block = (int[,])CurrentBlock.Clone();
				var temp = (int[,])Container.Clone();

				var blockWidth = block.GetUpperBound(0);
				var blockHeight = block.GetUpperBound(1);
				
				for (int i = 0; i <= blockWidth; i++)
				{
					for (int j = 0; j <= blockHeight; j++)
					{
						if (block[i, j] != 0)
						{
							block[i, j] = 8;
						}
					}
				}
				return FixBlock(block, temp, PositionX, PositionY);
			}
		}
        /// <summary>
        /// Object of class Block
        /// </summary>
		public Block BlockGenerator { get; } = new Block();

        /// <summary>
        /// Creates game field
        /// </summary>
        /// <param name="width">Width of level</param>
        /// <param name="height">Height of level</param>
		public Tetris(int width, int height)
        {
            Container = new int[height, width];
        }

        /// <summary>
        /// Starts game or Moves next block to the game field
        /// </summary>
        public void Start()
        {
            Running = true;
            PositionY = 0;
            PositionX = Container.GetUpperBound(1) / 2;
            CurrentBlock = NextBlock ?? BlockGenerator.GetRandomBlock();
            NextBlock = BlockGenerator.GetRandomBlock();
            if (!CanPosition(CurrentBlock, PositionX, PositionY))
                GameOver();
        }

        /// <summary>
        /// End of the game
        /// </summary>
        public void GameOver()
        {
			Running = false;
			GameOverEvent?.Invoke();
		}

        /// <summary>
        /// One tick of game(One move down)
        /// </summary>
		public void Step()
		{
			if (!Running)
				return;
			if (CanPosition(CurrentBlock, PositionX, PositionY + 1))
			{
				PositionY++;
			}
			else
			{
				Container = FixBlock(CurrentBlock, Container, PositionX, PositionY);
				Start();
			}
			var lines = CheckCompletedLines();
			LinesDone?.Invoke(lines);
		}

        /// <summary>
        /// Checks if player can move block to next position
        /// </summary>
        /// <param name="block">Current block</param>
        /// <param name="positionX">X coordinate of position where player wants to move block</param>
        /// <param name="positionY">Y coordinate of position where player wants to move block</param>
        /// <returns></returns>
		private bool CanPosition(int[,] block, int positionX, int positionY)
		{
			int[,] copy = (int[,])block.Clone();

			var blockWidth = block.GetUpperBound(0);
			var blockHeight = block.GetUpperBound(1);

			if (positionX + blockHeight > copy.GetUpperBound(1) || positionY + blockWidth > copy.GetUpperBound(0))
				return false;
			for (int i = 0; i <= blockHeight; i++)
			{
				for (int j = 0; j <= blockWidth; j++)
				{
					if (block[j, i] != 0)
					{
						if (copy[positionY + j, positionX + i] != 0)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

        /// <summary>
        /// Fixes block at current position
        /// </summary>
        /// <param name="block">Current block</param>
        /// <param name="field">Game field</param>
        /// <param name="positionX">X coordinate of block</param>
        /// <param name="positionY">Y coordinate of block</param>
        /// <returns></returns>
		private int[,] FixBlock(int[,] block, int[,] field, int positionX, int positionY)
		{
			var blockWidth = block.GetUpperBound(0);
			var blockHeight = block.GetUpperBound(1);

			if (positionX + blockHeight <= field.GetUpperBound(1) && positionY + blockWidth <= field.GetUpperBound(0))
			{
				for (int i = 0; i <= blockHeight; i++)
				{
					for (int j = 0; j <= blockWidth; j++)
					{
						if (block[j, i] != 0)
						{
							field[positionY + j, positionX + i] = block[j, i];
						}
					}
				}
			}
			BlockFixed?.Invoke();
			return field;
		}

        /// <summary>
        /// Checks for any completed lines to remove them and add amount to score
        /// </summary>
        /// <returns>Number of completed lines</returns>
		private int CheckCompletedLines()
		{
			var containerWidth = Container.GetUpperBound(0);
			int containerHeight = Container.GetUpperBound(1);

			for (int i = 0; i < containerWidth + 1; i++)
			{
				var isFullLine = true;
				for (int j = 0; j < containerHeight + 1; j++)
				{
					isFullLine = isFullLine && Container[i, j] != 0;
				}
				if (isFullLine)
				{
					RemoveLine(i);
					return CheckCompletedLines() + 1;
				}
			}
			return 0;
		}

        /// <summary>
        /// Removes completed line
        /// </summary>
        /// <param name="index">Index of done line</param>
		private void RemoveLine(int index)
		{
			var containerHeight = Container.GetUpperBound(1);
			for (int i = index; i > 0; i--)
			{
				for (int j = 0; j <= containerHeight; j++)
				{
					Container[i, j] = Container[i - 1, j];
				}
			}
			for (int j = 0; j <= containerHeight; j++)
			{
				Container[0, j] = 0;
			}
		}

        /// <summary>
        /// Handles user controls
        /// </summary>
        /// <param name="k">Pressed key</param>
        public void KeyPressing(Key k)
        {
            if (Running)
            {
                switch (k)
                {
                    case Key.Down:
                        Step();
                        break;
                    case Key.Left:
                        if (PositionX > 0 && CanPosition(CurrentBlock, PositionX - 1, PositionY))
                        {
                            PositionX--;
                        }
                        break;
                    case Key.Right:
                        if (PositionX < Container.GetUpperBound(0) - CurrentBlock.GetUpperBound(0) && CanPosition(CurrentBlock, PositionX + 1, PositionY))
                        {
                            PositionX++;
                        }
                        break;
                    case Key.Up:
                        var temp = Block.Rotate(CurrentBlock);
                        if (CanPosition(temp, PositionX, PositionY))
                        {
                            CurrentBlock = Block.Rotate(CurrentBlock);
                        }
                        break;
                    case Key.Space:
                        while (CanPosition(CurrentBlock, PositionX, PositionY + 1))
                        {
                            Step();
                        }
                        Step();
                        break;
                }
            }
        }
    }
}