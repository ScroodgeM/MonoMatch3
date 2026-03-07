using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Data;

public class Settings
{
    public struct System
    {
        public string[] textureAtlases { get; set; }
    }

    public struct Board
    {
        public struct Timings
        {
            public float firstAppearDuration { get; set; }
            public float delayBeforeFallIntoFreeCell { get; set; }
            public float fallDownDuration { get; set; }
            public float swapTilesDuration { get; set; }
            public float successMatchDisappearDuration { get; set; }
        }

        public byte width { get; set; }
        public byte height { get; set; }
        public TileType[] generatorPool { get; set; }
        public Timings timings { get; set; }
    }

    public struct View
    {
        public struct Tile
        {
            public TileType type { get; set; }
            public string spriteId { get; set; }
        }

        public float cellSize { get; set; }
        public string mainMenuLogoSpriteId { get; set; }
        public Tile[] tiles { get; set; }
    }

    public System system { get; set; }
    public Board board { get; set; }
    public View view { get; set; }

    public static Settings Load(ContentManager content)
    {
        string fileName = Path.ChangeExtension(nameof(Settings), "json");
        string filePath = Path.Combine(content.RootDirectory, fileName);
        using Stream stream = TitleContainer.OpenStream(filePath);
        return JsonSerializer.Deserialize<Settings>(stream);
    }
}
