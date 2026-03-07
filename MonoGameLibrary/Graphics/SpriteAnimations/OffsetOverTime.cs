using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public class OffsetOverTime(Vector2 from, Vector2 to, TimeSpan fromTime, TimeSpan toTime, OffsetOverTime.MoveMode mode) : SpriteAnimationBase
{
    public enum MoveMode : byte
    {
        FromTo,
        FromToFrom,
    }

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

        if (timeNormalized < 0)
        {
            return;
        }

        switch (mode)
        {
            case MoveMode.FromToFrom:
                timeNormalized = 1.0 - Math.Abs(timeNormalized * 2.0 - 1.0);
                break;
        }

        spriteTransform.position += from + (to - from) * (float)timeNormalized;
    }
}
