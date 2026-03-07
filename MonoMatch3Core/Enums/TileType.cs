using System.Text.Json.Serialization;

namespace MonoMatch3Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TileType : byte
{
    Simple1,
    Simple2,
    Simple3,
    Simple4,
    Simple5,
    Simple6,

    DestroyerHorizontalLine,
    DestroyerVerticalLine,
    DestroyerSquare,
}
