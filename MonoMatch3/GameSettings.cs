using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3;

public class GameSettings(XElement rootElement)
{
    public readonly struct Board(XElement element)
    {
        public readonly struct Timings(XElement element)
        {
            public readonly float delayBeforeFallIntoEmptyCell = XMLHelpers.GetFloatOrDefault(element, nameof(delayBeforeFallIntoEmptyCell));
            public readonly float fallDownDuration = XMLHelpers.GetFloatOrDefault(element, nameof(fallDownDuration));
        }

        public readonly byte width = XMLHelpers.GetByteOrDefault(element, nameof(width));
        public readonly byte height = XMLHelpers.GetByteOrDefault(element, nameof(height));
        public readonly TileType[] generatorPool = XMLHelpers.GetEnumsValues<TileType>(element.Element("GeneratorPool").Elements("Tile"), "type");
        public readonly Timings timings = new(element.Element(nameof(Timings)));
    }

    public readonly struct View(XElement element)
    {
        public readonly struct Tile(XElement element)
        {
            public readonly TileType type = XMLHelpers.GetEnumOrDefault<TileType>(element, nameof(type));
            public readonly string spriteId = XMLHelpers.GetStringOrDefault(element, nameof(spriteId));
        }

        public readonly float cellSize = XMLHelpers.GetFloatOrDefault(element, nameof(cellSize));
        public readonly Tile[] tiles = XMLHelpers.GetCustoms(element.Element("Tiles").Elements(nameof(Tile)), x => new Tile(x));
    }

    public readonly Board board = new(rootElement.Element(nameof(Board)));
    public readonly View view = new(rootElement.Element(nameof(View)));

    public static GameSettings FromFile(ContentManager content)
    {
        string filePath = Path.Combine(content.RootDirectory, ContentStructure.game_settings);
        using Stream stream = TitleContainer.OpenStream(filePath);
        using XmlReader reader = XmlReader.Create(stream);
        return new GameSettings(XDocument.Load(reader).Root);
    }
}
