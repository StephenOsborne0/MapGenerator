using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MapGenerator.Extensions;
using Newtonsoft.Json;

namespace MapGenerator.Models
{
    public class Map
    {
        public string Name { get; }

        [JsonIgnore]
        public Bitmap MapImage { get; }

        [JsonIgnore]
        public Screen[,] Screens { get; private set; }

        [JsonIgnore]
        public Tile[,] Tiles { get; private set; }

        public Tile Tile(int x, int y) => Tiles[x, y];

        public Screen Screen(int x, int y) => Screens[x, y];

        public List<Screen> ScreenList => Screens.ToList();

        [JsonIgnore]
        public List<Bitmap> Tilesheets;

        private List<Tile> _distinctTiles;

        [JsonIgnore]
        public List<Tile> DistinctTiles => _distinctTiles ?? (_distinctTiles = Tiles.ToList().GetDistinct());

        public List<Tile> TileList => Tiles.ToList();

        //public string MapTiles => string.Join(", ", Tiles.ToList().Select(t => t.Id.ToString()));

        public Map(string filepath)
        {
            Name = Path.GetFileNameWithoutExtension(filepath);
            MapImage = new Bitmap(Image.FromFile(filepath));
            GetTiles();
            Tilesheets = DistinctTiles.CreateTilesheets();
            GetScreens();
        }

        private void GetTiles()
        {
            if (MapImage.Width % Constants.TileWidthPixels != 0 || MapImage.Height % Constants.TileHeightPixels != 0)
                throw new InvalidOperationException("Map is not divisible by tile size");

            var tilesPerRow = MapImage.Width / Constants.TileWidthPixels;
            var tilesPerColumn = MapImage.Height / Constants.TileHeightPixels;
            Tiles = new Tile[tilesPerRow, tilesPerColumn];

            for (int y = 0; y < MapImage.Height; y += Constants.TileHeightPixels)
            for (int x = 0; x < MapImage.Width; x += Constants.TileWidthPixels)
            {
                var rect = new Rectangle(x, y, Constants.TileWidthPixels, Constants.TileHeightPixels);
                var tileImage = MapImage.Clone(rect, PixelFormat.Format32bppArgb);
                Tiles[x / Constants.TileWidthPixels, y / Constants.TileWidthPixels] = new Tile(new Point(x, y), tileImage);
            }
        }

        private void GetScreens()
        {
            if (MapImage.Width % Constants.ScreenWidthPixels != 0 || MapImage.Height % Constants.ScreenHeightPixels != 0)
                throw new InvalidOperationException("Map is not divisible by screen size");

            var screensPerRow = MapImage.Width / Constants.ScreenWidthPixels;
            var screensPerColumn = MapImage.Height / Constants.ScreenHeightPixels;
            Screens = new Screen[screensPerRow, screensPerColumn];

            for (int y = 0; y < MapImage.Height; y += Constants.ScreenHeightPixels)
            for (int x = 0; x < MapImage.Width; x += Constants.ScreenWidthPixels)
            {
                var rect = new Rectangle(x, y, Constants.ScreenWidthPixels, Constants.ScreenHeightPixels);
                var screenImage = MapImage.Clone(rect, PixelFormat.Format32bppArgb);
                Screens[x / Constants.ScreenWidthPixels, y / Constants.ScreenHeightPixels] = new Screen(new Point(x, y), screenImage);
            }
            
            GiveScreensTiles();
        }

        private void GiveScreensTiles()
        {
            foreach (var tile in Tiles)
            {
                var screen = Screen(tile.Info.ScreenX, tile.Info.ScreenY);
                screen.Tiles[tile.Info.RelativeX, tile.Info.RelativeY] = tile;
            }
        }

        public void Save(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var mapFile = Path.Combine(outputDir, $"{Name}.json");
            File.WriteAllText(mapFile, json);
        }

        public void ExportImage(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var filePath = Path.Combine(outputDir, $"{Name}.png");
            MapImage.Save(filePath, ImageFormat.Png);
        }
    }
}
