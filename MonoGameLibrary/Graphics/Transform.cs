using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics;

public struct Transform
{
    public Vector2 position;
    public float rotation;
    public Vector2 scale;
    public Color color;
    public float layerDepth;

    public static Transform Default
    {
        get
        {
            Transform transform;
            transform.position = Vector2.Zero;
            transform.rotation = 0f;
            transform.scale = Vector2.One;
            transform.color = Color.White;
            transform.layerDepth = 0f;
            return transform;
        }
    }
}
