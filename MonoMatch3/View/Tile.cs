using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoMatch3Core;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3.View;

internal class Tile
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly Settings settings;
    private readonly IGameEvents gameEvents;
    private readonly TileBase tileCore;
    private readonly ushort mySpriteId;
    private Sprite.Transform mySpriteTransform;

    private SpriteAnimationBase selectedAnimation;

    internal Tile(SpriteRenderer spriteRenderer, Settings settings, IGameEvents gameEvents, TileBase tileCore)
    {
        this.spriteRenderer = spriteRenderer;
        this.settings = settings;
        this.gameEvents = gameEvents;
        this.tileCore = tileCore;

        settings.GetSpriteView(tileCore.Type, tileCore.Color, out string spriteId, out Color tintColor);

        this.mySpriteTransform = Sprite.Transform.Default;
        this.mySpriteTransform.position = settings.BoardToScreen(tileCore.Position.Value).ToVector2();
        this.mySpriteTransform.color = tintColor;

        this.mySpriteId = this.spriteRenderer.AddSprite(spriteId, mySpriteTransform);

        Appear();

        this.tileCore.Position.OnValueChanged += OnPositionChanged;
        this.tileCore.State.OnValueChanged += OnStateChanged;
        this.tileCore.OnMoveAttemptFailed += OnMoveAttemptFailed;
    }


    internal void SetSelected(bool tileIsSelected)
    {
        if (selectedAnimation == null)
        {
            selectedAnimation = new PingPongScale(0.8f, 1.2f, 1.5f);
            spriteRenderer.AddAnimation(mySpriteId, selectedAnimation);
        }

        selectedAnimation.SetActive(tileIsSelected);
    }

    internal void Remove(TileRemoveReason removeReason)
    {
        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan duration;
        TimeSpan toTime;

        switch (removeReason)
        {
            case TileRemoveReason.SuccessMatch:
                duration = TimeSpan.FromSeconds(settings.board.timings.successMatchDisappearDuration);
                toTime = fromTime + duration;
                spriteRenderer.AddAnimation(mySpriteId, new RotateSelf(0f, 10f));
                spriteRenderer.AddAnimation(mySpriteId, new ChangeTransparency(1f, 0f, fromTime, toTime));
                spriteRenderer.AddAnimation(mySpriteId, new ChangeScale(1f, 2f, fromTime, toTime));
                gameEvents.Timer.Wait(duration).Done(Die);
                break;
            case TileRemoveReason.DestroyedBySpecial:
                duration = TimeSpan.FromSeconds(settings.board.timings.destroyBySpecialDisappearDuration);
                toTime = fromTime + duration;
                spriteRenderer.AddAnimation(mySpriteId, new ChangeScale(1f, 2f, fromTime, toTime));
                spriteRenderer.AddAnimation(mySpriteId, new ChangeTransparency(1f, 0f, fromTime, toTime));
                gameEvents.Timer.Wait(duration).Done(Die);

                for (int i = 0; i <= 3; i++)
                {
                    TimeSpan vfxDuration = duration * (1.0f - i * 0.2f);
                    float vfxScale = 0.5f + 0.3f * i;
                    ushort vfxSpriteId = spriteRenderer.AddSprite(settings.view.tileDestroyVfxSpriteId, mySpriteTransform);
                    spriteRenderer.AddAnimation(vfxSpriteId, new ChangeScale(0f, vfxScale, fromTime, fromTime + vfxDuration));
                    gameEvents.Timer.Wait(vfxDuration).Done(() => spriteRenderer.RemoveSprite(vfxSpriteId));
                }

                break;
            default:
                Die();
                break;
        }
    }

    internal void Die()
    {
        tileCore.Position.OnValueChanged -= OnPositionChanged;
        tileCore.State.OnValueChanged -= OnStateChanged;
        tileCore.OnMoveAttemptFailed -= OnMoveAttemptFailed;
        spriteRenderer.RemoveSprite(mySpriteId);
    }

    private void OnPositionChanged(TilePosition newPosition)
    {
        mySpriteTransform.position = settings.BoardToScreen(newPosition).ToVector2();
        spriteRenderer.UpdateTransform(mySpriteId, mySpriteTransform);
    }

    private void OnStateChanged(TileState newState)
    {
        if (newState.movement.HasValue == true)
        {
            TileState.Movement movement = newState.movement.Value;
            Vector2 moveFrom = -settings.BoardToScreen(movement.direction);

            OffsetOverTime animation = new OffsetOverTime(moveFrom, Vector2.Zero, movement.startTime, movement.finishTime, OffsetOverTime.MoveMode.FromTo);
            spriteRenderer.AddAnimation(mySpriteId, animation);
        }
    }

    private void OnMoveAttemptFailed(TilePosition targetPosition)
    {
        Vector2 offset =
            settings.BoardToScreen(targetPosition).ToVector2()
            -
            settings.BoardToScreen(tileCore.Position.Value).ToVector2();

        TimeSpan startTime = gameEvents.CurrentTime.Value;
        TimeSpan endTime = startTime + TimeSpan.FromSeconds(settings.board.timings.swapTilesDuration);

        OffsetOverTime animation = new OffsetOverTime(Vector2.Zero, offset, startTime, endTime, OffsetOverTime.MoveMode.FromToFrom);
        spriteRenderer.AddAnimation(mySpriteId, animation);
    }

    private void Appear()
    {
        TimeSpan appearDuration = TimeSpan.FromSeconds(settings.board.timings.firstAppearDuration);
        TimeSpan now = gameEvents.CurrentTime.Value;
        this.spriteRenderer.AddAnimation(mySpriteId, new ChangeTransparency(0f, 1f, now, now + appearDuration));
        this.spriteRenderer.AddAnimation(mySpriteId, new ChangeScale(2f, 1f, now, now + appearDuration));
    }
}
