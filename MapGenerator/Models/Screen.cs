using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MapGenerator.Extensions;
using Newtonsoft.Json;

namespace MapGenerator.Models
{
    public class Screen
    {
        //Absolute position
        [JsonIgnore]
        public int AbsoluteX { get; }

        [JsonIgnore]
        public int AbsoluteY { get; }

        //The screen coordinates
        public int ScreenX { get; }
        public int ScreenY { get; }

        [JsonIgnore]
        public Tile[,] Tiles { get; set; }

        [JsonIgnore]
        public Bitmap ScreenImage { get; set; }

        public List<Tile> TileList => Tiles.ToList();
        
        //public string ScreenTiles => string.Join(", ", Tiles.ToList().Select(t => t.Id.ToString()));

        public Screen(Point absoluteCoordinates, Bitmap screenImage)
        {
            AbsoluteX = absoluteCoordinates.X;
            AbsoluteY = absoluteCoordinates.Y;

            var screenCoordinates = absoluteCoordinates.AbsoluteToScreen();
            ScreenX = screenCoordinates.X;
            ScreenY = screenCoordinates.Y;

            ScreenImage = screenImage;

            var tilesPerScreenRow = Constants.ScreenWidthPixels / Constants.TileWidthPixels;
            var tilesPerScreenColumn = Constants.ScreenHeightPixels / Constants.TileHeightPixels;
            Tiles = new Tile[tilesPerScreenRow, tilesPerScreenColumn];
        }

        public void Save(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var mapFile = Path.Combine(outputDir, $"s{ScreenX}-{ScreenY}.json");
            File.WriteAllText(mapFile, json);
        }

        public void ExportImage(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var filePath = Path.Combine(outputDir, $"s{ScreenX}-{ScreenY}.png");
            ScreenImage.Save(filePath, ImageFormat.Png);
        }
    }
}