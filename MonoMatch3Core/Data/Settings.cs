// ReSharper disable InconsistentNaming

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
            public float destroyBySpecialDisappearDuration { get; set; }
            public float bombExplodeDelay { get; set; }
            public float lineDestroyerFlySpeed { get; set; }
        }

        public byte width { get; set; }
        public byte height { get; set; }
        public TileColor[] generatorPool { get; set; }
        public Timings timings { get; set; }
    }

    public struct View
    {
        public struct TypedTile
        {
            public struct ColoredTile
            {
                public TileColor color { get; set; }
                public string spriteId { get; set; }
                public Color tintColor { get; set; }
            }

            public TileType type { get; set; }
            public ColoredTile[] perColor { get; set; }
        }

        public float cellSize { get; set; }
        public string mainMenuLogoSpriteId { get; set; }
        public TypedTile[] tiles { get; set; }
        public string tileDestroyVfxSpriteId { get; set; }
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
