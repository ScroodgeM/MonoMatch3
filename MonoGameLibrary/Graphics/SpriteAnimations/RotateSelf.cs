using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public class RotateSelf(float radiansInitialRotation, float radiansPerSecond) : SpriteAnimationBase
{
    public override void ApplyState(ref Sprite.Transform spriteTransform, GameTime gameTime)
    {
        if (isActive == false)
        {
            return;
        }

        double uncycledCurrentRotationRadians = radiansInitialRotation + gameTime.TotalGameTime.TotalSeconds * radiansPerSecond;

        float currentRotationRadians = (float)(uncycledCurrentRotationRadians % (Math.PI * 2.0));

        spriteTransform.rotation += currentRotationRadians;
    }
}
