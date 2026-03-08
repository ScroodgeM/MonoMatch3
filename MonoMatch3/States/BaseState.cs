using System;
using MonoGameLibrary.Graphics;
using MonoMatch3Core.Data;

namespace MonoMatch3.States;

internal abstract class BaseState(
    SpriteRenderer spriteRenderer,
    TextRenderer textRenderer,
    Settings settings)
{
    internal event Action<State> OnNewStateRequest = state => { };

    protected readonly SpriteRenderer spriteRenderer = spriteRenderer;
    protected readonly TextRenderer textRenderer = textRenderer;
    protected readonly Settings settings = settings;

    internal abstract void Start();
    internal abstract void Die();

    protected void SwitchToState(State state) => OnNewStateRequest(state);
}
