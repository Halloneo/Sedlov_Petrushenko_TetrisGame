using System;
using System.Collections.Generic;

namespace ConsoleTetrisGame
{
	public class Block
	{
		/// <summary>
		/// Random variable
		/// </summary>
		public Random Random { get; }

		public List<int[,]> Blocks { get; }

		/// <summary>
		/// Constructor of class Block
		/// </summary>
		public Block()
		{
			Random = new Random(DateTime.Now.Millisecond);

			Blocks = new List<int[,]>
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

        /// <summary>
        /// Returns block depending on id
        /// </summary>
        /// <param name="id">Number of block</param>
        /// <returns>Block or null</returns>
	public int[,] GetBlock(int id)
	{
		if (Blocks.Count > id && id >= 0)
			return Blocks[id];

		return null;
	}
		
        /// <summary>
        /// Returns random block
        /// </summary>
        /// <returns>Block</returns>
	public int[,] GetRandomBlock() => Blocks[Random.Next(Blocks.Count)];

		/// <summary>
        /// Rotates block clockwise
        /// </summary>
        /// <param name="block">Block to be rotated</param>
        /// <returns>Rotated block</returns>
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
