using System.Text.Json.Serialization;

namespace MonoMatch3.Match3Core.Tiles;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TileType : byte
{
    Simple1,
    Simple2,
    Simple3,
    Simple4,
    Simple5,

    DestroyerHorizontalLine,
    DestroyerVerticalLine,
    DestroyerSquare,
}
