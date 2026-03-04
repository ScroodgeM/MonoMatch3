using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public class PingPongColorChannels(float minValue, float maxValue, float speedR, float speedG, float speedB) : SpriteAnimationBase
{
    private readonly float center = (maxValue + minValue) * 0.5f;
    private readonly float offset = (maxValue - minValue) * 0.5f;

    public override void ApplyState(ref Sprite.Transform spriteTransform, GameTime gameTime)
    {
        if (isActive == false)
        {
            return;
        }

        double timeInRadians = gameTime.TotalGameTime.TotalSeconds * (Math.PI * 2.0);
        float r = center + offset * (float)Math.Sin(timeInRadians * speedR);
        float g = center + offset * (float)Math.Sin(timeInRadians * speedG);
        float b = center + offset * (float)Math.Sin(timeInRadians * speedB);
        spriteTransform.color *= new Color(r, g, b, 1f);
    }
}
