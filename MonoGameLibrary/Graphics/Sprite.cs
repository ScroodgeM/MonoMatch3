using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;

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

    private byte animationsIncrementalId = 0;
    private readonly Dictionary<byte, SpriteAnimationBase> spriteAnimations = new Dictionary<byte, SpriteAnimationBase>();

    public void AddAnimation(SpriteAnimationBase animation)
    {
        if (spriteAnimations.Count >= byte.MaxValue)
        {
            throw new NotSupportedException($"Sorry, you reached the maximum number of simultaneous animations: {byte.MaxValue}.");
        }

        while (spriteAnimations.TryAdd(animationsIncrementalId, animation) == false)
        {
            unchecked
            {
                animationsIncrementalId++;
            }
        }

        byte thisAnimationId = animationsIncrementalId;
        animation.OnCompleted += () => spriteAnimations.Remove(thisAnimationId);
    }

    public void Draw(SpriteBatch spriteBatch, Transform transform, GameTime gameTime)
    {
        foreach (SpriteAnimationBase spriteAnimation in spriteAnimations.Values)
        {
            spriteAnimation.ApplyState(ref transform, gameTime);
        }

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
