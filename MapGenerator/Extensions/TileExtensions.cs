using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using MapGenerator.Models;

namespace MapGenerator.Extensions
{
    public static class TileExtensions
    {
        public static List<Tile> GetDistinct(this IEnumerable<Tile> tiles)
        {
            var distinctTiles = new List<Tile>();

            foreach (var tile in tiles)
            {
                if (!distinctTiles.ContainsTile(tile))
                    distinctTiles.Add(tile);
            }

            return distinctTiles;
        }

        public static bool ContainsTile(this IEnumerable<Tile> tiles, Tile tile)
        {
            var matchedTile = tiles.FirstOrDefault(t => t.TileImage.BitmapEquals(tile.TileImage));
            matchedTile?.MatchedTiles.Add(tile);
            return matchedTile != null;
        }

        public static IEnumerable<Tile> GetBatch(this IEnumerable<Tile> tiles, int batchSize, int batchNumber) => 
            tiles.Skip(batchNumber * batchSize).Take(batchSize);

        public static List<Bitmap> CreateTilesheets(this List<Tile> distinctTiles)
        {
            var tilesheets = new List<Bitmap>();
            var batchSize = Constants.TilesheetTileWidth * Constants.TilesheetTileHeight;
            var remainder = distinctTiles.Count % batchSize;
            var batches = (distinctTiles.Count / batchSize) + (remainder > 0 ? 1 : 0);

            for(int index = 0; index < distinctTiles.Count; index++)
            {
                //Set UUID
                distinctTiles[index].Id = index;
            }

            for (int batchNumber = 0; batchNumber < batches; batchNumber++)
            {
                var tileBatch = distinctTiles.GetBatch(batchSize, batchNumber).ToList();
                tileBatch.ForEach(t => t.Info.TilesheetIndex = batchNumber);

                var tilesheet = tileBatch.CreateTilesheet();
                tilesheets.Add(tilesheet);
            }

            return tilesheets;
        }

        private static Bitmap CreateTilesheet(this IReadOnlyList<Tile> tiles)
        {
            var tilesheet = new Bitmap(Constants.TilesheetPixelWidth, Constants.TilesheetPixelHeight, PixelFormat.Format32bppArgb);

            for (int t = 0; t < tiles.Count; t++)
            {
                var tile = tiles[t];

                var x = t % Constants.TilesheetTileWidth;
                var y = t / Constants.TilesheetTileHeight;

                //Set tile coordinates
                var tilesheetCoordinate = new Point(x, y);
                tile.Info.TilesheetCoordinateX = tilesheetCoordinate.X;
                tile.Info.TilesheetCoordinateY = tilesheetCoordinate.Y;
                tile.Info.TilesheetAbsoluteX = tilesheetCoordinate.TilesheetCoordinateToAbsolute().X;
                tile.Info.TilesheetAbsoluteY = tilesheetCoordinate.TilesheetCoordinateToAbsolute().Y;

                //Set these on matched tiles too
                foreach(var matchedTile in tile.MatchedTiles)
                {
                    matchedTile.Id = tile.Id;
                    matchedTile.Info.TilesheetCoordinateX = tilesheetCoordinate.X;
                    matchedTile.Info.TilesheetCoordinateY = tilesheetCoordinate.Y;
                    matchedTile.Info.TilesheetAbsoluteX = tilesheetCoordinate.TilesheetCoordinateToAbsolute().X;
                    matchedTile.Info.TilesheetAbsoluteY = tilesheetCoordinate.TilesheetCoordinateToAbsolute().Y;
                }

                tilesheet.WriteTileToBitmap(tile);
            }

            return tilesheet;
        }
    }
}
