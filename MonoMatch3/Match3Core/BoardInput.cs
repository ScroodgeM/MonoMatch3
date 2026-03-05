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
        if (inputManager.Mouse.WasButtonJustPressed(MouseButton.Left) == true)
        {
            Point mousePosition = inputManager.Mouse.Position;

            int tilePositionX = (int)MathF.Floor(mousePosition.X / gameSettings.view.cellSize);
            int tilePositionY = (int)MathF.Floor(mousePosition.Y / gameSettings.view.cellSize);

            if (Helpers.IsPositionValid(tilePositionX, tilePositionY, gameSettings.board.width, gameSettings.board.height) == true)
            {
                OnTileClick(new TilePosition((byte)tilePositionX, (byte)tilePositionY));
            }
        }
    }
}
