using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using MapGenerator.Extensions;
using MapGenerator.Models;

namespace MapGenerator
{
    class Program
    {
        private static readonly string OutputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");

        static void Main(string[] args)
        {
            if (args == null || !args.Any())
                throw new ArgumentNullException(nameof(args));

            var rootPath = args[0];

            if (!Directory.Exists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);

            foreach (var file in Directory.GetFiles(rootPath, "*.png"))
                GenerateMapFiles(file);

            Console.WriteLine("All map images processed.");
            Console.ReadLine();
        }

        private static void GenerateMapFiles(string file)
        {
            var filename = Path.GetFileNameWithoutExtension(file);
            var outputDir = Path.Combine(OutputDirectory, filename);

            Console.WriteLine($"Started processing {filename}...");
            Console.WriteLine("Generating map...");
            var map = new Map(file);

            Console.WriteLine("Saving map...");
            map.Save(outputDir);
            map.ExportImage(outputDir);

            //SaveScreens(map, outputDir);
            //SaveTiles(map, outputDir);
            SaveTilesheets(map, outputDir);

            Console.WriteLine($"Finished processing {filename}...");
            Console.WriteLine();
        }

        private static void SaveScreens(Map map, string outputDir)
        {
            Console.WriteLine("Saving screens...");
            var screenDir = Path.Combine(outputDir, "Screens");

            foreach (var screen in map.Screens.ToList())
            {
                screen.Save(screenDir);
                screen.ExportImage(screenDir);
            }
        }

        private static void SaveTiles(Map map, string outputDir)
        {
            Console.WriteLine("Saving tiles...");
            var tileDir = Path.Combine(outputDir, "Tiles");

            foreach (var tile in map.Tiles)
            {
                tile.Save(tileDir);
                tile.ExportImage(tileDir);
            }
        }

        private static void SaveTilesheets(Map map, string outputDir)
        {
            Console.WriteLine("Saving tilesheets...");

            foreach (var tilesheet in map.Tilesheets)
            {
                var index = map.Tilesheets.IndexOf(tilesheet);
                var filepath = Path.Combine(outputDir, $"tilesheet-{index}.png");
                tilesheet.Save(filepath, ImageFormat.Png);
            }
        }
    }
}
