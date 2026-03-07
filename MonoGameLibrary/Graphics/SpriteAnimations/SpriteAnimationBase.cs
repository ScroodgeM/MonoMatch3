using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public abstract class SpriteAnimationBase
{
    public event Action OnCompleted = () => { };

    protected bool isActive = true;
    private bool isCompleted = false;

    public void SetActive(bool isActive)
    {
        if (isCompleted == true)
        {
            throw new InvalidOperationException("can't activate animation again - OnCompleted already called");
        }

        this.isActive = isActive;
    }

    public abstract void ApplyState(ref Sprite.Transform spriteTransform, GameTime gameTime);

    protected void CallOnCompleted()
    {
        if (isCompleted == true)
        {
            throw new InvalidOperationException("OnCompleted already called");
        }

        isCompleted = true;
        OnCompleted();
    }
}
