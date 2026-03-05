using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Input;

namespace MonoMatch3.Match3Core;

public class BoardInput
{
    public event Action<TilePosition> OnTileClick = position => { };

    private readonly IGameEvents gameEvents;
    private readonly InputManager inputManager;
    private readonly GameSettings gameSettings;

    public BoardInput(IGameEvents gameEvents, InputManager inputManager, GameSettings gameSettings)
    {
        this.gameEvents = gameEvents;
        this.inputManager = inputManager;
        this.gameSettings = gameSettings;

        this.gameEvents.OnUpdate += OnUpdate;
    }

    ~BoardInput()
    {
        this.gameEvents.OnUpdate -= OnUpdate;
    }

    private void OnUpdate(GameTime gameTime)
    {
        if (inputManager.Mouse.WasButtonJustPressed(MouseButton.Left) == true
            &&
            gameSettings.TryScreenToBoard(inputManager.Mouse.Position, out TilePosition tilePosition) == true)
        {
            OnTileClick(tilePosition);
        }
    }
}
