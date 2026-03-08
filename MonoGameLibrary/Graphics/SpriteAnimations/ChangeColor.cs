using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public class ChangeColor(Color from, Color to, TimeSpan fromTime, TimeSpan toTime) : SpriteAnimationBase
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

        spriteTransform.color *= Color.Lerp(from, to, (float)timeNormalized);
    }
}
