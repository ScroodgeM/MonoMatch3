using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public class RotateAround(float radiansInitialRotation, float radiansPerSecond, float radius) : SpriteAnimationBase
{
    public override void ApplyState(ref Sprite.Transform spriteTransform, GameTime gameTime)
    {
        if (isActive == false)
        {
            return;
        }

        double uncycledCurrentRotationRadians = radiansInitialRotation + gameTime.TotalGameTime.TotalSeconds * radiansPerSecond;

        float currentRotationRadians = (float)(uncycledCurrentRotationRadians % (Math.PI * 2.0));

        Vector2 offsetDirection = new Vector2(MathF.Sin(currentRotationRadians), MathF.Cos(currentRotationRadians));

        spriteTransform.position += offsetDirection * radius;
    }
}
