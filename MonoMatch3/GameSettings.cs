using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3;

public class GameSettings(XElement element)
{
    public readonly struct System(XElement element)
    {
        public readonly string[] textureAtlasDefinitions = XMLHelpers.GetArray(element.Element("TextureAtlases").Elements("TextureAtlas"), "pathToDefinitionFile", string.Empty, XMLHelpers.GetStringOrDefault);
    }

    public readonly struct Board(XElement element)
    {
        public readonly struct Timings(XElement element)
        {
            public readonly float delayBeforeFallIntoFreeCell = XMLHelpers.GetFloatOrDefault(element, nameof(delayBeforeFallIntoFreeCell));
            public readonly float fallDownDuration = XMLHelpers.GetFloatOrDefault(element, nameof(fallDownDuration));
        }

        public readonly byte width = XMLHelpers.GetByteOrDefault(element, nameof(width));
        public readonly byte height = XMLHelpers.GetByteOrDefault(element, nameof(height));
        public readonly TileType[] generatorPool = XMLHelpers.GetArray(element.Element("GeneratorPool").Elements("Tile"), "type", TileType.None, XMLHelpers.GetEnumOrDefault);
        public readonly Timings timings = new(element.Element(nameof(Timings)));
    }

    public readonly struct View(XElement element)
    {
        public readonly struct MainMenu(XElement element)
        {
            public readonly string logoSpriteId = XMLHelpers.GetStringOrDefault(element, nameof(logoSpriteId));
        }

        public readonly struct Tile(XElement element)
        {
            public readonly TileType type = XMLHelpers.GetEnumOrDefault<TileType>(element, nameof(type));
            public readonly string spriteId = XMLHelpers.GetStringOrDefault(element, nameof(spriteId));
        }

        public readonly float cellSize = XMLHelpers.GetFloatOrDefault(element, nameof(cellSize));
        public readonly MainMenu mainMenu = new(element.Element(nameof(MainMenu)));
        public readonly Tile[] tiles = XMLHelpers.GetCustoms(element.Element("Tiles").Elements(nameof(Tile)), x => new Tile(x));
    }

    public readonly System system = new(element.Element(nameof(System)));
    public readonly Board board = new(element.Element(nameof(Board)));
    public readonly View view = new(element.Element(nameof(View)));

    public static GameSettings FromFile(ContentManager content)
    {
        string filePath = Path.Combine(content.RootDirectory, ContentStructure.game_settings);
        using Stream stream = TitleContainer.OpenStream(filePath);
        using XmlReader reader = XmlReader.Create(stream);
        return new GameSettings(XDocument.Load(reader).Root);
    }
}
