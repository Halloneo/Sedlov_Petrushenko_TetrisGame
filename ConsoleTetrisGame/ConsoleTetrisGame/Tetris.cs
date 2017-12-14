using System;

namespace ConsoleTetrisGame
{
    public class Tetris
    {
        public enum Key
        {
            Left,
            Right,
            Up,
            Down,
            Space
        }

        private readonly int[,] _container;

        private int _positionX;

        private int _positionY;

        private int[,] _currentBlock;

        public bool Running { get; private set; }

        public int[,] NextBlock { get; private set; }

        private readonly Block _blockGenerator = new Block();

        public Tetris(int width, int height)
        {
            _container = new int[height, width];
        }

        public void Start()
        {
            Running = true;
            _positionY = 0;
            _positionX = _container.GetUpperBound(1) / 2;
            _currentBlock = NextBlock ?? _blockGenerator.GetRandomBlock();
            NextBlock = _blockGenerator.GetRandomBlock();
            if (!CanPosition(_currentBlock, _positionX, _positionY))
                GameOver();
        }

        private bool CanPosition(int[,] block, int positionX, int positionY)
        {
            throw new NotImplementedException();
        }

        public void GameOver()
        {
            throw new NotImplementedException();
        }
    }
}