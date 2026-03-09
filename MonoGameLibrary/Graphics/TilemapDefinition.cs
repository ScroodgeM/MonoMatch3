// ReSharper disable InconsistentNaming

namespace MonoGameLibrary.Graphics;

public class TilemapDefinition
{
    public struct TilesetDefinition
    {
        public string name { get; set; }
        public string texturePath { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public float scale { get; set; }
        public int tileW { get; set; }
        public int tileH { get; set; }
    }

    public TilesetDefinition tileset { get; set; }
    public int mapW { get; set; }
    public int mapH { get; set; }
    public int[] map { get; set; }
}
