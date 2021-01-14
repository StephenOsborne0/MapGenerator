namespace MapGenerator
{
    public class Constants
    {
        public static int TileWidthPixels = 16;
        public static int TileHeightPixels = 16;

        public static int ScreenWidthPixels = 160;
        public static int ScreenHeightPixels = 128;

        public static int TilesheetPixelWidth = 1024;
        public static int TilesheetPixelHeight = 1024;

        public static int TilesheetTileWidth => TilesheetPixelWidth / TileWidthPixels;
        public static int TilesheetTileHeight => TilesheetPixelHeight / TileHeightPixels;
    }
}
