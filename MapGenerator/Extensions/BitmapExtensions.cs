using System;
using System.Drawing;
using MapGenerator.Models;

namespace MapGenerator.Extensions
{
    public static class BitmapExtensions
    {
        public static bool BitmapEquals(this Bitmap b1, Bitmap b2)
        {
            if (b1 == null)
                throw new ArgumentNullException(nameof(b1));

            if (b2 == null)
                throw new ArgumentNullException(nameof(b2));

            if (b1.Width != b2.Width || b1.Height != b2.Height)
                return false;

            for (int y = 0; y < b1.Height; y++)
            for (int x = 0; x < b1.Width; x++)
            {
                if (b1.GetPixel(x, y) != b2.GetPixel(x, y))
                    return false;
            }

            return true;
        }

        public static void WriteTileToBitmap(this Bitmap bitmap, Tile tile)
        {
            for (int yOffset = 0; yOffset < tile.TileImage.Height; yOffset++)
            for (int xOffset = 0; xOffset < tile.TileImage.Width; xOffset++)
            {
                var color = tile.TileImage.GetPixel(xOffset, yOffset);
                bitmap.SetPixel(tile.Info.TilesheetAbsoluteX + xOffset, tile.Info.TilesheetAbsoluteY + yOffset, color);
            }
        }
    }
}
