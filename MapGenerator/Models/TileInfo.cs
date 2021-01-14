namespace MapGenerator.Models
{
    public class TileInfo
    {
        //Absolute position
        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        //The parent screen coordinates
        public int ScreenX { get; set; }
        public int ScreenY { get; set; }

        //Position relative to the screen
        public int RelativeX { get; set; }
        public int RelativeY { get; set; }

        //Tilesheet positioning
        public int TilesheetIndex { get; set; }
        public int TilesheetAbsoluteX { get; set; }
        public int TilesheetAbsoluteY { get; set; }
        public int TilesheetCoordinateX { get; set; }
        public int TilesheetCoordinateY { get; set; }
    }
}
