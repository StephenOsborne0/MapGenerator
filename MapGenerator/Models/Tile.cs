using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MapGenerator.Extensions;
using Newtonsoft.Json;

namespace MapGenerator.Models
{
    public class Tile
    {
        //Unique Tile ID
        public int Id { get; set; }

        public TileInfo Info { get; set; } = new TileInfo();

        //Matching tiles
        [JsonIgnore]
        public List<Tile> MatchedTiles { get; set; } = new List<Tile>();

        [JsonIgnore]
        public Bitmap TileImage { get; set; }

        public Tile(Point absoluteCoordinates, Bitmap tileImage)
        {
            Info.AbsoluteX = absoluteCoordinates.X;
            Info.AbsoluteY = absoluteCoordinates.Y;

            var screenCoordinates = absoluteCoordinates.AbsoluteToScreen();
            Info.ScreenX = screenCoordinates.X;
            Info.ScreenY = screenCoordinates.Y;

            var relativeCoordinates = absoluteCoordinates.AbsoluteToScreenTile();
            Info.RelativeX = relativeCoordinates.X;
            Info.RelativeY = relativeCoordinates.Y;

            TileImage = tileImage;
        }

        public void Save(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var mapFile = Path.Combine(outputDir, $"s{Info.ScreenX}-{Info.ScreenY} t{Info.RelativeX}-{Info.RelativeY}.json");
            File.WriteAllText(mapFile, json);
        }

        public void ExportImage(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var filePath = Path.Combine(outputDir, $"s{Info.ScreenX}-{Info.ScreenY} t{Info.RelativeX}-{Info.RelativeY}.png");
            TileImage.Save(filePath, ImageFormat.Png);
        }
    }
}