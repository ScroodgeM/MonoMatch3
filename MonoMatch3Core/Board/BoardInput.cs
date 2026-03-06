using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using MonoMatch3Core.Data;

namespace MonoMatch3Core.Board;

internal class BoardInput
{
    internal event Action<TilePosition> OnTileClick = position => { };

    private readonly IGameEvents gameEvents;
    private readonly InputManager inputManager;
    private readonly Settings settings;

    internal BoardInput(IGameEvents gameEvents, InputManager inputManager, Settings settings)
    {
        this.gameEvents = gameEvents;
        this.inputManager = inputManager;
        this.settings = settings;

        this.gameEvents.OnUpdate += OnUpdate;
    }

    internal void Die()
    {
        this.gameEvents.OnUpdate -= OnUpdate;
    }

    private void OnUpdate(GameTime gameTime)
    {
        if (inputManager.Mouse.WasButtonJustPressed(MouseButton.Left) == true
            &&
            settings.TryScreenToBoard(inputManager.Mouse.Position, out TilePosition tilePosition) == true)
        {
            OnTileClick(tilePosition);
        }
    }
}
