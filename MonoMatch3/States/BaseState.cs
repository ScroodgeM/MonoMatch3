using System;
using System.Collections.Generic;
using MonoGameLibrary.Graphics;
using MonoMatch3Core.Data;

namespace MonoMatch3.States;

internal abstract class BaseState(
    RenderSystem renderSystem,
    Settings settings)
{
    internal event Action<State> OnNewStateRequest = state => { };

    protected readonly RenderSystem renderSystem = renderSystem;
    protected readonly Settings settings = settings;

    private readonly List<uint> graphicIdsToRemoveOnDeath = new List<uint>();

    internal abstract void Start();

    internal virtual void Die()
    {
        renderSystem.RemoveGraphic(graphicIdsToRemoveOnDeath);
    }

    protected void RegisterGraphicToRemoveOnDeath(uint graphicId) => graphicIdsToRemoveOnDeath.Add(graphicId);

    protected void SwitchToState(State state) => OnNewStateRequest(state);
}
