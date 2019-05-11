using System;

namespace Minesweeper {
    public static class Program {
        public static void Main(string[] args) {
            _board = new Board(9, 9, 10);
            _board.Draw();

            _cursor = (0, 0);
            while (!_board.GameOver && !_board.Victory) {
                DoCommand();
            }
            Console.WriteLine();
            if (_board.GameOver) {
                
                Console.WriteLine("Game over! Press any key to exit...");
                Console.ReadKey(true);
            } else {
                Console.WriteLine("You win! Congratulations! Press any key to exit...");
                Console.ReadKey(true);
            }
        }

        private static (int x, int y) _cursor;
        private static Board _board;
        private static void DoCommand() {
            Console.SetCursorPosition(_cursor.x +1, _cursor.y + 2);
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Console.ReadKey(true).KeyChar) {
                case 'h':
                    if (_cursor.x > 0) _cursor.x--;
                    break;
                case 'j':
                    if (_cursor.y < _board.Height - 1) _cursor.y++;
                    break;
                case 'k':
                    if (_cursor.y > 0) _cursor.y--;
                    break;
                case 'l':
                    if (_cursor.x < _board.Width - 1) _cursor.x++;
                    break;
                case 'y':
                    if (_cursor.y > 0 && _cursor.x > 0) {
                        _cursor.x--;
                        _cursor.y--;
                    }
                    break;
                case 'u':
                    if (_cursor.y > 0 && _cursor.x < _board.Width - 1) {
                        _cursor.x++;
                        _cursor.y--;
                    }
                    break;
                case 'b':
                    if (_cursor.y < _board.Height - 1 && _cursor.x > 0) {
                        _cursor.x--;
                        _cursor.y++;
                    }
                    break;
                case 'n':
                    if (_cursor.y < _board.Height - 1 && _cursor.x < _board.Width - 1) {
                        _cursor.x++;
                        _cursor.y++;
                    }
                    break;
                case ' ':
                    _board.Flag(_cursor.x, _cursor.y);
                    break;
                case '\n':
                    _board.Reveal(_cursor.x, _cursor.y);
                    break;
            }
            _board.Draw();
        }
    }
}
