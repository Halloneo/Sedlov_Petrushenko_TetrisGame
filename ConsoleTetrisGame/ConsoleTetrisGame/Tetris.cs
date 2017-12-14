namespace ConsoleTetrisGame
{
    public class Tetris
    {
		public delegate void GameOverHandler();
		
		public delegate void BlockFixedHandler();
		
		public delegate void LinesDoneHandler(int lines);
		
		public event GameOverHandler GameOverEvent;
		
		public event BlockFixedHandler BlockFixed;
		
		public event LinesDoneHandler LinesDone;

		public enum Key
        {
            Left,
            Right,
            Up,
            Down,
            Space
        }

		public int[,] Container { get; set; }

		public int PositionX { get; private set; }

		public int PositionY { get; private set; }

		public int[,] CurrentBlock { get; private set; }

		public bool Running { get; private set; }

        public int[,] NextBlock { get; private set; }

		public bool InGame { get; private set; }

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


		public Block BlockGenerator { get; } = new Block();

		public Tetris(int width, int height)
        {
            Container = new int[height, width];
        }

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

        public void GameOver()
        {
			InGame = false;
			GameOverEvent?.Invoke();
		}

		public void Step()
		{
			if (!InGame)
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
	}
}