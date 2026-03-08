using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public class ChangeTransparency(float from, float to, TimeSpan fromTime, TimeSpan toTime) : SpriteAnimationBase
{
    public override void ApplyState(ref Transform spriteTransform, GameTime gameTime)
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

        if (timeNormalized < 0)
        {
            return;
        }

        spriteTransform.color *= from + (to - from) * (float)timeNormalized;
    }
}
