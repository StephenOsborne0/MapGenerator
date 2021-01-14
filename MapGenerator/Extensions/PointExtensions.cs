using System.Drawing;

namespace MapGenerator.Extensions
{
    public static class PointExtensions
    {
        /// <summary>
        ///     Converts an absolute coordinate to a screen coordinate
        /// </summary>
        /// <param name="absolute"></param>
        /// <returns></returns>
        public static Point AbsoluteToScreen(this Point absolute) => new Point(absolute.X / Constants.ScreenWidthPixels, absolute.Y / Constants.ScreenHeightPixels);

        /// <summary>
        ///     Converts an absolute coordinate to a tile coordinate
        /// </summary>
        /// <param name="absolute"></param>
        /// <returns></returns>
        public static Point AbsoluteToTile(this Point absolute) => new Point(absolute.X / Constants.TileWidthPixels, absolute.Y / Constants.TileHeightPixels);

        /// <summary>
        ///     Converts an absolute coordinate to a relative coordinate from a supplied coordinate
        /// </summary>
        /// <param name="absolute"></param>
        /// <returns></returns>
        public static Point AbsoluteToRelative(this Point absolute, Point coordinate) => new Point(absolute.X % coordinate.X, absolute.Y % coordinate.Y);

        /// <summary>
        ///     Converts an absolute coordinate to a tile coordinate relative to a screen coordinate
        /// </summary>
        /// <param name="absolute"></param>
        /// <returns></returns>
        public static Point AbsoluteToScreenTile(this Point absolute) =>
            new Point(absolute.X % Constants.ScreenWidthPixels / Constants.TileWidthPixels,
                absolute.Y % Constants.ScreenHeightPixels / Constants.TileHeightPixels);

        /// <summary>
        ///     Converts a tilesheet coordinate to an absolute tilesheet position
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static Point TilesheetCoordinateToAbsolute(this Point coordinate) => 
            new Point(coordinate.X * Constants.TileWidthPixels, coordinate.Y * Constants.TileHeightPixels);

        /// <summary>
        ///     Converts an absolute tilesheet position to a relative tilesheet coordinate
        /// </summary>
        /// <param name="absolute"></param>
        /// <returns></returns>
        public static Point TilesheetAbsoluteToCoordinate(this Point absolute) =>
            new Point(absolute.X / Constants.TileWidthPixels, absolute.Y / Constants.TileHeightPixels);
    }
}
