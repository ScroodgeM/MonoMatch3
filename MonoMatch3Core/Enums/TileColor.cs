using System.Text.Json.Serialization;

namespace MonoMatch3Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TileColor : byte
{
    Blue = 10,
    Green = 20,
    Yellow = 30,
    Red = 40,
    Violet = 50,
    White = 60,
}
