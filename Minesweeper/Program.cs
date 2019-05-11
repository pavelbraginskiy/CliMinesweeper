using System;
using System.Linq;
using Mono.Options;

namespace Minesweeper {
    public static class Program {
        public static int Main(string[] args) {
            var showHelp = false;
            var showVersion = false;
            var ascii = false;
            (int w, int h, int mines) difficulty = (9, 9, 10);

            var options = new OptionSet {
                {"h|help", "Show this message and exit", h => showHelp = h != null},
                {"v|version", "Show the version of this CliMinesweeper", v => showVersion = v != null},
                {"a|ascii", "Draw the gameboard using ASCII characters", a => ascii = a != null},
                {"c|custom=","Set the games difficulty in the form w,h,m.\n" +
                                 "E.g., -d 9,9,10 will create a 9x9 board with 10 mines.",
                    d => {
                        var ds = d.Split(',').Select(int.Parse).ToArray();
                        if (ds.Length != 3) {
                            throw new ArgumentException("Invalid difficulty set");
                        }
                        difficulty = (ds[0], ds[1], ds[2]);
                    }
                },
                {
                    "e|easy", "Set the difficulty to 9,9,10. This is the default difficulty.",
                    e => difficulty = e != null ? (9,9,10) : difficulty
                },
                {
                    "m|medium", "Set the difficulty to 16,16,40.",
                    e => difficulty = e != null ? (16,16,40) : difficulty
                },
                {
                    "d|difficult", "Set the difficulty to 30,16,99.",
                    e => difficulty = e != null ? (30,16,99) : difficulty
                }
            };
            try {
                options.Parse(args);
            } catch {
                showHelp = true;
            }

            if (showHelp) {
                Console.WriteLine("Usage: [mono] Minesweeper.exe [switches]");
                options.WriteOptionDescriptions(Console.Out);
                return 1;
            }

            if (showVersion) {
                Console.WriteLine("This is CliMinesweeper 1.1.");
                return 1;
            }
            
            _board = new Board(difficulty.h, difficulty.w, difficulty.mines, ascii);
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

            return 0;
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
