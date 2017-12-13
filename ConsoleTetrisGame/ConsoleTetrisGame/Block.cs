using System;
using System.Collections.Generic;

namespace ConsoleTetrisGame
{
	public class Block
	{
		private readonly List<int[,]> _blocks;

		private readonly Random _random;
		
		public Block()
		{
			_random = new Random(DateTime.Now.Millisecond);

			_blocks = new List<int[,]>
			{
				new[,] { { 1, 1, 1, 1 } },

				new [,] { { 2, 2 },
						  { 2, 2 } },

				new[,] { { 0, 0, 3 },
						 { 3, 3, 3 } },

				new[,] { { 4, 0, 0 },
						 { 4, 4, 4 } },

				new [,] { { 0, 5, 0 },
						  { 5, 5, 5 } },

				new [,] { { 6, 6, 0 },
						  { 0, 6, 6 } },

				new [,] { { 0, 7, 7 },
						  { 7, 7, 0 } }
			};

		}
		public int[,] GetBlock(int id)
		{
			if (_blocks.Count > id && id >= 0)
				return _blocks[id];

			return null;
		}
		
		public int[,] GetRandomBlock() => _blocks[_random.Next(_blocks.Count)];
		
		public static int[,] Rotate(int[,] block)
		{
			var width = block.GetUpperBound(0) + 1;
			var height = block.GetUpperBound(1) + 1;

			var rotated = new int[height, width];

			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
					rotated[j, width - i - 1] = block[i, j];
			return rotated;
		}

	}
}

