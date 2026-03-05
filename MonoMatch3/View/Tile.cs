using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoMatch3.Match3Core;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.View;

public class Tile
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly GameSettings gameSettings;
    private readonly IGameEvents gameEvents;
    private readonly TileBase tileCore;
    private readonly ushort mySpriteId;
    private Sprite.Transform mySpriteTransform;

    private bool isMoving = false;

    public Tile(SpriteRenderer spriteRenderer, GameSettings gameSettings, IGameEvents gameEvents, TileBase tileCore)
    {
        this.spriteRenderer = spriteRenderer;
        this.gameSettings = gameSettings;
        this.gameEvents = gameEvents;
        this.tileCore = tileCore;

        this.mySpriteTransform = Sprite.Transform.Default;
        this.mySpriteTransform.position = gameSettings.BoardToScreen(tileCore.Position.Value).ToVector2();

        string spriteId = gameSettings.GetSpriteId(tileCore.TileType);
        this.mySpriteId = this.spriteRenderer.AddSprite(spriteId, mySpriteTransform);

        this.gameEvents.OnUpdate += OnUpdate;
    }

    private void OnUpdate(GameTime gameTime)
    {
        TileState tileState = tileCore.State.Value;

        if (tileState.movement.HasValue == true)
        {
            TileState.Movement movement = tileState.movement.Value;
            Vector2 moveTo = gameSettings.BoardToScreen(tileCore.Position.Value).ToVector2();
            Vector2 moveFrom = moveTo - gameSettings.BoardToScreen(movement.direction);
            TimeSpan timeElapsed = gameTime.TotalGameTime - movement.startTime;
            TimeSpan duration = movement.finishTime - movement.startTime;
            double normalizedTime = Math.Clamp(timeElapsed / duration, 0, 1);
            mySpriteTransform.position = Vector2.Lerp(moveFrom, moveTo, (float)normalizedTime);
            spriteRenderer.UpdateTransform(mySpriteId, mySpriteTransform);
            isMoving = true;
        }
        else if (isMoving == true)
        {
            mySpriteTransform.position = gameSettings.BoardToScreen(tileCore.Position.Value).ToVector2();
            spriteRenderer.UpdateTransform(mySpriteId, mySpriteTransform);
            isMoving = false;
        }
    }

    public void Die()
    {
        this.gameEvents.OnUpdate -= OnUpdate;

        this.spriteRenderer.RemoveSprite(mySpriteId);
    }
}
