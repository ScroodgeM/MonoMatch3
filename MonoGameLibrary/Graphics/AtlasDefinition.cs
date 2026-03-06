namespace MonoGameLibrary.Graphics;

public class AtlasDefinition
{
    public struct Region
    {
        public string name { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public float scale { get; set; }
        public int pivotX { get; set; }
        public int pivotY { get; set; }
        public bool flipVertically { get; set; }
        public bool flipHorizontally { get; set; }
    }

    public string texturePath { get; set; }
    public Region[] regions { get; set; }
}
