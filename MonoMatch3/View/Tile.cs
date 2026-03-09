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
    private Transform mySpriteTransform;

    private SpriteAnimationBase selectedAnimation;

    internal Tile(SpriteRenderer spriteRenderer, Settings settings, IGameEvents gameEvents, TileBase tileCore)
    {
        this.spriteRenderer = spriteRenderer;
        this.settings = settings;
        this.gameEvents = gameEvents;
        this.tileCore = tileCore;

        settings.GetSpriteView(tileCore.Type, tileCore.Color, out string spriteId, out Color tintColor);

        this.mySpriteTransform = Transform.Default;
        this.mySpriteTransform.layerDepth = RenderLayer.GameElements.ToLayerDepth();
        this.mySpriteTransform.position = settings.BoardToScreen(tileCore.Position.Value).ToVector2();
        this.mySpriteTransform.color = tintColor;

        this.mySpriteId = this.spriteRenderer.AddSprite(spriteId, mySpriteTransform);

        Appear();

        this.tileCore.Position.OnValueChanged += OnPositionChanged;
        this.tileCore.State.OnValueChanged += OnStateChanged;
        this.tileCore.OnMoveAttemptFailed += OnMoveAttemptFailed;

        OnStateChanged(this.tileCore.State.Value);
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
        switch (tileCore.Type)
        {
            case TileType.DestroyerHorizontalLine:
                spriteRenderer
                    .AnimateSpecialTileDestroy(gameEvents, settings, mySpriteId)
                    .Done(Die);

            {
                TimeSpan fromTime = gameEvents.CurrentTime.Value;

                Transform rocketTransform = mySpriteTransform;
                rocketTransform.layerDepth = RenderLayer.VFX.ToLayerDepth();
                rocketTransform.scale = Vector2.One * 0.75f;
                byte boardSize = settings.board.width;
                Vector2 rocketAwayPosition = Vector2.UnitX * settings.view.cellSize * boardSize;
                TimeSpan rocketDuration = TimeSpan.FromSeconds(boardSize / settings.board.timings.lineDestroyerFlySpeed);

                rocketTransform.rotation = MathF.PI * 0.5f;
                ushort rocketSpriteId1 = spriteRenderer.AddSprite(settings.view.rocketDestroyVfxSpriteId, rocketTransform);
                spriteRenderer.AddAnimation(rocketSpriteId1, new OffsetOverTime(Vector2.Zero, rocketAwayPosition, fromTime, fromTime + rocketDuration, OffsetOverTime.MoveMode.FromTo));
                gameEvents.Timer.Wait(rocketDuration).Done(() => spriteRenderer.RemoveSprite(rocketSpriteId1));

                rocketTransform.rotation = MathF.PI * 1.5f;
                ushort rocketSpriteId2 = spriteRenderer.AddSprite(settings.view.rocketDestroyVfxSpriteId, rocketTransform);
                spriteRenderer.AddAnimation(rocketSpriteId2, new OffsetOverTime(Vector2.Zero, -rocketAwayPosition, fromTime, fromTime + rocketDuration, OffsetOverTime.MoveMode.FromTo));
                gameEvents.Timer.Wait(rocketDuration).Done(() => spriteRenderer.RemoveSprite(rocketSpriteId2));
            }
                break;

            case TileType.DestroyerVerticalLine:
                spriteRenderer
                    .AnimateSpecialTileDestroy(gameEvents, settings, mySpriteId)
                    .Done(Die);

            {
                TimeSpan fromTime = gameEvents.CurrentTime.Value;

                Transform rocketTransform = mySpriteTransform;
                rocketTransform.layerDepth = RenderLayer.VFX.ToLayerDepth();
                rocketTransform.scale = Vector2.One * 0.75f;
                byte boardSize = settings.board.height;
                Vector2 rocketAwayPosition = Vector2.UnitY * settings.view.cellSize * boardSize;
                TimeSpan rocketDuration = TimeSpan.FromSeconds(boardSize / settings.board.timings.lineDestroyerFlySpeed);

                ushort rocketSpriteId1 = spriteRenderer.AddSprite(settings.view.rocketDestroyVfxSpriteId, rocketTransform);
                spriteRenderer.AddAnimation(rocketSpriteId1, new OffsetOverTime(Vector2.Zero, -rocketAwayPosition, fromTime, fromTime + rocketDuration, OffsetOverTime.MoveMode.FromTo));
                gameEvents.Timer.Wait(rocketDuration).Done(() => spriteRenderer.RemoveSprite(rocketSpriteId1));

                rocketTransform.rotation = MathF.PI;
                ushort rocketSpriteId2 = spriteRenderer.AddSprite(settings.view.rocketDestroyVfxSpriteId, rocketTransform);
                spriteRenderer.AddAnimation(rocketSpriteId2, new OffsetOverTime(Vector2.Zero, rocketAwayPosition, fromTime, fromTime + rocketDuration, OffsetOverTime.MoveMode.FromTo));
                gameEvents.Timer.Wait(rocketDuration).Done(() => spriteRenderer.RemoveSprite(rocketSpriteId2));
            }
                break;

            case TileType.DestroyerSquare:
                spriteRenderer
                    .AnimateSpecialTileDestroy(gameEvents, settings, mySpriteId)
                    .Done(Die);

                for (int i = 0; i <= 3; i++)
                {
                    TimeSpan fromTime = gameEvents.CurrentTime.Value;
                    TimeSpan duration = TimeSpan.FromSeconds(settings.board.timings.bombExplodeDelay);
                    Transform boomTransform = mySpriteTransform;
                    boomTransform.layerDepth = RenderLayer.VFX.ToLayerDepth();
                    ushort boomSpriteId = spriteRenderer.AddSprite(settings.view.bombDestroyVfxSpriteId, boomTransform);
                    spriteRenderer.AddAnimation(boomSpriteId, new ChangeScale(0.5f, 1.5f, fromTime, fromTime + duration));
                    gameEvents.Timer.Wait(duration).Done(() => spriteRenderer.RemoveSprite(boomSpriteId));
                }

                break;

            case TileType.Simple:
                switch (removeReason)
                {
                    case TileRemoveReason.SuccessMatch:
                        spriteRenderer
                            .AnimateSimpleTileDestroyByMatch(gameEvents, settings, mySpriteId)
                            .Done(Die);
                        break;

                    case TileRemoveReason.DestroyedBySpecial:
                        spriteRenderer
                            .AnimateSimpleTileDestroyBySpecial(gameEvents, settings, mySpriteId)
                            .Done(Die);

                        for (int i = 0; i <= 3; i++)
                        {
                            TimeSpan fromTime = gameEvents.CurrentTime.Value;
                            TimeSpan duration = TimeSpan.FromSeconds(settings.board.timings.destroyBySpecialDisappearDuration);
                            TimeSpan vfxDuration = duration * (1.0f - i * 0.2f);
                            float vfxScale = 0.5f + 0.3f * i;
                            Transform vfxTransform = mySpriteTransform;
                            vfxTransform.layerDepth = RenderLayer.VFX.ToLayerDepth();
                            ushort vfxSpriteId = spriteRenderer.AddSprite(settings.view.tileDestroyVfxSpriteId, vfxTransform);
                            spriteRenderer.AddAnimation(vfxSpriteId, new ChangeScale(0f, vfxScale, fromTime, fromTime + vfxDuration));
                            gameEvents.Timer.Wait(vfxDuration).Done(() => spriteRenderer.RemoveSprite(vfxSpriteId));
                        }

                        break;

                    default:
                        Die();
                        break;
                }

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
        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan toTime = fromTime + TimeSpan.FromSeconds(settings.board.timings.firstAppearDuration);
        spriteRenderer.AddAnimation(mySpriteId, new ChangeTransparency(0f, 1f, fromTime, toTime));
    }
}
