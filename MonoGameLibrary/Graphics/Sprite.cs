using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;

namespace MonoGameLibrary.Graphics;

internal class Sprite(Texture2D texture, Rectangle sourceRectangle, Vector2 pivot, float scale, SpriteEffects effects)
{
    internal class Animator
    {
        private byte animationsIncrementalId = 0;

        private readonly Dictionary<byte, SpriteAnimationBase> animations = new Dictionary<byte, SpriteAnimationBase>();

        internal void Add(SpriteAnimationBase animation)
        {
            if (animations.Count >= byte.MaxValue)
            {
                throw new NotSupportedException($"Sorry, you reached the maximum number of simultaneous animations: {byte.MaxValue}.");
            }

            while (animations.TryAdd(animationsIncrementalId, animation) == false)
            {
                unchecked
                {
                    animationsIncrementalId++;
                }
            }

            byte thisAnimationId = animationsIncrementalId;
            animation.OnCompleted += () => animations.Remove(thisAnimationId);
        }

        internal void Process(ref Transform transform, GameTime gameTime)
        {
            foreach (SpriteAnimationBase spriteAnimation in animations.Values)
            {
                spriteAnimation.ApplyState(ref transform, gameTime);
            }
        }
    }

    internal Rectangle GetRectangle(Vector2 position)
    {
        Rectangle result = sourceRectangle;
        result.X = (int)(position.X - pivot.X * scale);
        result.Y = (int)(position.Y - pivot.Y * scale);
        result.Width = (int)(result.Width * scale);
        result.Height = (int)(result.Height * scale);
        return result;
    }

    internal void Draw(SpriteBatch spriteBatch, Transform transform, Animator animator, GameTime gameTime)
    {
        if (animator != null)
        {
            animator.Process(ref transform, gameTime);
        }

        spriteBatch.Draw(
            texture,
            transform.position,
            sourceRectangle,
            transform.color,
            transform.rotation,
            pivot,
            transform.scale * scale,
            effects,
            transform.layerDepth
        );
    }
}
