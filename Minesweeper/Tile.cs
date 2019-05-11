namespace Minesweeper {
    public struct Tile {
        public bool HasMine;
        public TileStatus Status;
        
        public enum TileStatus {
            None, Flagged, Revealed, Detonated
        }
    }
}
