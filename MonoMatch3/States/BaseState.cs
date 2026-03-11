using System;
using System.Collections.Generic;
using MonoGameLibrary.Graphics;
using MonoMatch3Core.Data;

namespace MonoMatch3.States;

internal abstract class BaseState(
    SpriteRenderer spriteRenderer,
    RenderSystem renderSystem,
    Settings settings)
{
    internal event Action<State> OnNewStateRequest = state => { };

    protected readonly SpriteRenderer spriteRenderer = spriteRenderer;
    protected readonly RenderSystem renderSystem = renderSystem;
    protected readonly Settings settings = settings;

    private readonly List<ushort> spritesToRemoveOnDeath = new List<ushort>();
    private readonly List<uint> textsToRemoveOnDeath = new List<uint>();

    internal abstract void Start();

    internal virtual void Die()
    {
        foreach (ushort spriteId in spritesToRemoveOnDeath)
        {
            spriteRenderer.Remove(spriteId);
        }

        renderSystem.RemoveGraphic(textsToRemoveOnDeath);
    }

    protected void RegisterSpriteToRemoveOnDeath(ushort spriteId) => spritesToRemoveOnDeath.Add(spriteId);
    protected void RegisterTextToRemoveOnDeath(uint textId) => textsToRemoveOnDeath.Add(textId);

    protected void SwitchToState(State state) => OnNewStateRequest(state);
}
