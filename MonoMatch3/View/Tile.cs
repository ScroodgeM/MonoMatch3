using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoGameLibrary.Timers;
using MonoMatch3Core;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3.View;

public class Tile
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly Settings settings;
    private readonly IGameEvents gameEvents;
    private readonly ITimer timer;
    private readonly TileBase tileCore;
    private readonly ushort mySpriteId;
    private Sprite.Transform mySpriteTransform;

    private bool isMoving = false;

    public Tile(SpriteRenderer spriteRenderer, Settings settings, IGameEvents gameEvents, ITimer timer, TileBase tileCore)
    {
        this.spriteRenderer = spriteRenderer;
        this.settings = settings;
        this.gameEvents = gameEvents;
        this.timer = timer;
        this.tileCore = tileCore;

        this.mySpriteTransform = Sprite.Transform.Default;
        this.mySpriteTransform.position = settings.BoardToScreen(tileCore.Position.Value).ToVector2();

        string spriteId = settings.GetSpriteId(tileCore.TileType);
        this.mySpriteId = this.spriteRenderer.AddSprite(spriteId, mySpriteTransform);

        Appear();

        this.gameEvents.OnUpdate += OnUpdate;
        RefreshMovement(gameEvents.CurrentTime.Value);
    }

    private void OnUpdate(GameTime gameTime) => RefreshMovement(gameTime.TotalGameTime);

    private void Appear()
    {
        TimeSpan appearDuration = TimeSpan.FromSeconds(settings.board.timings.firstAppearDuration);
        TimeSpan now = gameEvents.CurrentTime.Value;
        this.spriteRenderer.AddAnimation(mySpriteId, new ChangeTransparency(0f, 1f, now, now + appearDuration));
        this.spriteRenderer.AddAnimation(mySpriteId, new ChangeScale(2f, 1f, now, now + appearDuration));
    }

    private void RefreshMovement(TimeSpan time)
    {
        TileState tileState = tileCore.State.Value;

        if (tileState.movement.HasValue == true)
        {
            TileState.Movement movement = tileState.movement.Value;
            Vector2 moveTo = settings.BoardToScreen(tileCore.Position.Value).ToVector2();
            Vector2 moveFrom = moveTo - settings.BoardToScreen(movement.direction);
            TimeSpan timeElapsed = time - movement.startTime;
            TimeSpan duration = movement.finishTime - movement.startTime;
            double normalizedTime = Math.Clamp(timeElapsed / duration, 0, 1);
            mySpriteTransform.position = Vector2.Lerp(moveFrom, moveTo, (float)normalizedTime);
            spriteRenderer.UpdateTransform(mySpriteId, mySpriteTransform);
            isMoving = true;
        }
        else if (isMoving == true)
        {
            mySpriteTransform.position = settings.BoardToScreen(tileCore.Position.Value).ToVector2();
            spriteRenderer.UpdateTransform(mySpriteId, mySpriteTransform);
            isMoving = false;
        }
    }

    internal void Remove(TileRemoveReason removeReason)
    {
        this.gameEvents.OnUpdate -= OnUpdate;

        switch (removeReason)
        {
            case TileRemoveReason.SuccessMatch:
                TimeSpan disappearDuration = TimeSpan.FromSeconds(settings.board.timings.successMatchDisappearDuration);
                TimeSpan now = gameEvents.CurrentTime.Value;
                this.spriteRenderer.AddAnimation(mySpriteId, new RotateSelf(0f, 10f));
                this.spriteRenderer.AddAnimation(mySpriteId, new ChangeTransparency(1f, 0f, now, now + disappearDuration));
                this.spriteRenderer.AddAnimation(mySpriteId, new ChangeScale(1f, 2f, now, now + disappearDuration));
                timer.Wait(disappearDuration).Done(Die);
                break;
            default:
                Die();
                break;
        }
    }

    internal void Die()
    {
        this.gameEvents.OnUpdate -= OnUpdate;
        this.spriteRenderer.RemoveSprite(mySpriteId);
    }
}
