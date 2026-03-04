using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class Sprite(Texture2D texture, Rectangle sourceRectangle, Vector2 pivot, SpriteEffects effects)
{
    public struct Transform
    {
        public Vector2 position;
        public float rotation;
        public Vector2 scale;
        public Color color;
        public int layerDepth;

        public static Transform Default
        {
            get
            {
                Transform transform;
                transform.position = Vector2.Zero;
                transform.rotation = 0f;
                transform.scale = Vector2.One;
                transform.color = Color.White;
                transform.layerDepth = 0;
                return transform;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, ref readonly Transform transform)
    {
        spriteBatch.Draw(
            texture,
            transform.position,
            sourceRectangle,
            transform.color,
            transform.rotation,
            pivot,
            transform.scale,
            effects,
            transform.layerDepth
        );
    }
}
