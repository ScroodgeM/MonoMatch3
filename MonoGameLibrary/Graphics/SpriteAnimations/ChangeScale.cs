using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public class ChangeScale(float fromValue, float toValue, TimeSpan fromTime, TimeSpan toTime) : SpriteAnimationBase
{
    public override void ApplyState(ref Sprite.Transform spriteTransform, GameTime gameTime)
    {
        if (isActive == false)
        {
            return;
        }

        double timeNormalized = (gameTime.TotalGameTime - fromTime) / (toTime - fromTime);
        if (timeNormalized > 1)
        {
            CallOnCompleted();
        }

        spriteTransform.scale *= fromValue + (toValue - fromValue) * (float)timeNormalized;
    }
}
