using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper {
    public class Board {
        public Board(int height, int width, int mines) {
            Height = height;
            Width = width;
            _tiles = new Tile[width,height];
            _remaining = mines;

            var random = new Random();
            for (var i = mines; i != 0;) {
                var w = random.Next(0, width);
                var h = random.Next(0, height);
                if (_tiles[w, h].HasMine) continue;
                _tiles[w, h].HasMine = true;
                i--;
            }
        }

        private IEnumerable<(int x, int y)> Surrounding(int x, int y) {
            (int x, int y)[] tiles = new[] {
                (x-1, y-1), (x, y-1), (x+1, y-1),
                (x-1, y), (x+1, y),
                (x-1, y+1), (x, y+1), (x+1, y+1)
            };
            return tiles
                .Where(t => t.x >= 0 && t.x < Width && t.y >= 0 && t.y < Height);
        }
        
        private int SurroundingMines(int x, int y) {
            return Surrounding(x, y)
                .Count(t => _tiles[t.x,t.y].HasMine);
        }
        public int Height { get; }
        public int Width { get; }
        public bool GameOver { get; private set; }

        public bool Victory {
            get {
                return _tiles.Cast<Tile>().All(t => t.Status != Tile.TileStatus.None)
                       && _remaining == 0
                       && !GameOver;
            }
        }

        private readonly Tile[,] _tiles;
        private int _remaining;

        public void Flag(int x, int y) {
            switch (_tiles[x, y].Status) {
                case Tile.TileStatus.None:
                    _tiles[x,y].Status = Tile.TileStatus.Flagged;
                    _remaining--;
                    break;
                case Tile.TileStatus.Flagged:
                    _tiles[x,y].Status = Tile.TileStatus.None;
                    _remaining++;
                    break;
                case Tile.TileStatus.Revealed:
                    break;
                case Tile.TileStatus.Detonated:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Reveal(int x, int y) {
            switch (_tiles[x, y].Status) {
                case Tile.TileStatus.None:
                    if (_tiles[x, y].HasMine) {
                        _tiles[x, y].Status = Tile.TileStatus.Detonated;
                        GameOver = true;
                    } else {
                        _tiles[x, y].Status = Tile.TileStatus.Revealed;
                        if (SurroundingMines(x, y) == 0) {
                            foreach (var (xx, yy) in Surrounding(x,y)) {
                                Reveal(xx, yy);
                            }
                        }
                    }
                    break;
                case Tile.TileStatus.Flagged:
                    break;
                case Tile.TileStatus.Revealed:
                    break;
                case Tile.TileStatus.Detonated:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void Draw() {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Mines remaining: {_remaining}");
            Console.Write('┌');
            Console.Write(new string('─', Width));
            Console.WriteLine('┐');
            for (var i = 0; i < Height; i++) {
                Console.Write('│');
                for (var j = 0; j < Width; j++) {
                    var t = _tiles[j, i];
                    switch (t.Status) {
                        case Tile.TileStatus.None:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write('.');
                            break;
                        case Tile.TileStatus.Flagged:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('⚑');
                            break;
                        case Tile.TileStatus.Revealed:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(SurroundingMines(j, i));
                            break;
                        case Tile.TileStatus.Detonated:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write('✶');
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine('│');
            }
            Console.Write('└');
            Console.Write(new string('─', Width));
            Console.Write('┘');
        }
    }
}
