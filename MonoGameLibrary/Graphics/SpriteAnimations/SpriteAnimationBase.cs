using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.SpriteAnimations;

public abstract class SpriteAnimationBase
{
    public event Action OnCompleted = () => { };

    protected bool isActive = true;

    public void SetActive(bool isActive) => this.isActive = isActive;

    public abstract void ApplyState(ref Sprite.Transform spriteTransform, GameTime gameTime);

    protected void CallOnCompleted() => OnCompleted();
}
