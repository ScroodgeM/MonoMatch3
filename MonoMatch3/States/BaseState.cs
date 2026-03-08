using MonoGameLibrary.Graphics;
using MonoMatch3Core.Data;

namespace MonoMatch3.States;

internal abstract class BaseState(
    SpriteRenderer spriteRenderer,
    TextRenderer textRenderer,
    Settings settings)
{
    protected readonly SpriteRenderer spriteRenderer = spriteRenderer;
    protected readonly TextRenderer textRenderer = textRenderer;
    protected readonly Settings settings = settings;

    internal abstract void Start();
    internal abstract void Die();
}
