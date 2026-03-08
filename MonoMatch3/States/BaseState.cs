using System;
using System.Collections.Generic;
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

    private readonly List<ushort> spritesToRemoveOnDeath = new List<ushort>();
    private readonly List<byte> textsToRemoveOnDeath = new List<byte>();

    internal abstract void Start();

    internal virtual void Die()
    {
        foreach (ushort spriteId in spritesToRemoveOnDeath)
        {
            spriteRenderer.RemoveSprite(spriteId);
        }

        foreach (byte textId in textsToRemoveOnDeath)
        {
            textRenderer.RemoveText(textId);
        }
    }

    protected void RegisterSpriteToRemoveOnDeath(ushort spriteId) => spritesToRemoveOnDeath.Add(spriteId);
    protected void RegisterTextToRemoveOnDeath(byte textId) => textsToRemoveOnDeath.Add(textId);

    protected void SwitchToState(State state) => OnNewStateRequest(state);
}
