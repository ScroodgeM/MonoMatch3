using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public class PingPongScale(float minScale, float maxScale, float speed) : SpriteAnimationBase
{
    private readonly float center = (maxScale + minScale) * 0.5f;
    private readonly float offset = (maxScale - minScale) * 0.5f;
    private readonly double timeToRadians = Math.PI * 2.0 * speed;

    public override void ApplyState(ref Transform spriteTransform, GameTime gameTime)
    {
        if (isActive == false)
        {
            return;
        }

        spriteTransform.scale *= center + offset * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * timeToRadians);
    }
}
