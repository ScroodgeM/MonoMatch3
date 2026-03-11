using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoGameLibrary.Promises;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3.View;

internal static class VfxHelpers
{
    internal static IPromise AnimateSimpleTileDestroyByMatch(this SpriteRenderer spriteRenderer, IGameEvents gameEvents, Settings settings, ushort spriteId)
    {
        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan duration = TimeSpan.FromSeconds(settings.board.timings.successMatchDisappearDuration);
        TimeSpan toTime = fromTime + duration;

        spriteRenderer.AddAnimation(spriteId, new RotateSelf(0f, 10f));
        spriteRenderer.AddAnimation(spriteId, new ChangeTransparency(1f, 0f, fromTime, toTime));
        spriteRenderer.AddAnimation(spriteId, new ChangeScale(1f, 2f, fromTime, toTime));

        return gameEvents.Timer
            .Wait(duration);
    }

    internal static IPromise AnimateSimpleTileDestroyBySpecial(this SpriteRenderer spriteRenderer, IGameEvents gameEvents, Settings settings, ushort spriteId)
    {
        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan duration = TimeSpan.FromSeconds(settings.board.timings.destroyBySpecialDisappearDuration);
        TimeSpan toTime = fromTime + duration;

        spriteRenderer.AddAnimation(spriteId, new ChangeScale(1f, 2f, fromTime, toTime));
        spriteRenderer.AddAnimation(spriteId, new ChangeTransparency(1f, 0f, fromTime, toTime));

        return gameEvents.Timer
            .Wait(duration);
    }

    internal static IPromise AnimateSpecialTileDestroy(this SpriteRenderer spriteRenderer, IGameEvents gameEvents, Settings settings, ushort spriteId)
    {
        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan duration = TimeSpan.FromSeconds(settings.board.timings.destroyBySpecialDisappearDuration);
        TimeSpan toTime = fromTime + duration;

        spriteRenderer.AddAnimation(spriteId, new ChangeTransparency(1f, 0f, fromTime, toTime));

        return gameEvents.Timer
            .Wait(duration);
    }

    internal static void PlaySimpleTileDestroyedBySpecialVfx(this SpriteRenderer spriteRenderer, IGameEvents gameEvents, Settings settings, Vector2 position)
    {
        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan baseDuration = TimeSpan.FromSeconds(settings.board.timings.destroyBySpecialDisappearDuration);

        Transform transform = Transform.Default;
        transform.position = position;
        transform.layerDepth = RenderLayer.VFX.ToLayerDepth();

        for (int i = 0; i <= 3; i++)
        {
            ushort spriteId = spriteRenderer.Add(settings.view.tileDestroyVfxSpriteId, transform);

            TimeSpan duration = baseDuration * (1.0f - i * 0.2f);
            TimeSpan toTime = fromTime + duration;
            float scale = 0.5f + 0.3f * i;
            spriteRenderer.AddAnimation(spriteId, new ChangeScale(0f, scale, fromTime, toTime));

            gameEvents.Timer
                .Wait(duration)
                .Done(() => spriteRenderer.Remove(spriteId));
        }
    }

    internal static void PlayBombExplodeVfx(this SpriteRenderer spriteRenderer, IGameEvents gameEvents, Settings settings, Vector2 position)
    {
        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan duration = TimeSpan.FromSeconds(settings.board.timings.bombExplodeDelay);
        TimeSpan toTime = fromTime + duration;

        Transform transform = Transform.Default;
        transform.position = position;
        transform.layerDepth = RenderLayer.VFX.ToLayerDepth();

        for (int i = 0; i <= 3; i++)
        {
            ushort spriteId = spriteRenderer.Add(settings.view.bombDestroyVfxSpriteId, transform);

            float scaleFrom = 0.5f + 0.2f * i;
            float scaleTo = scaleFrom + 1.0f;
            spriteRenderer.AddAnimation(spriteId, new ChangeScale(scaleFrom, scaleTo, fromTime, toTime));

            gameEvents.Timer
                .Wait(duration)
                .Done(() => spriteRenderer.Remove(spriteId));
        }
    }

    internal static void PlayLineDestroyerVfx(this SpriteRenderer spriteRenderer, IGameEvents gameEvents, Settings settings, Vector2 position, Direction direction)
    {
        Transform transform = Transform.Default;
        transform.position = position;
        transform.layerDepth = RenderLayer.VFX.ToLayerDepth();
        transform.scale = Vector2.One * 0.75f;

        byte boardSize;

        switch (direction)
        {
            case Direction.Up:
                boardSize = settings.board.height;
                transform.rotation = 0f;
                break;

            case Direction.Down:
                boardSize = settings.board.height;
                transform.rotation = MathF.PI;
                break;

            case Direction.Left:
                boardSize = settings.board.width;
                transform.rotation = MathF.PI * 1.5f;
                break;

            case Direction.Right:
                boardSize = settings.board.width;
                transform.rotation = MathF.PI * 0.5f;
                break;

            default:
                throw new InvalidOperationException($"direction {direction} not supported");
        }

        ushort spriteId = spriteRenderer.Add(settings.view.rocketDestroyVfxSpriteId, transform);

        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan duration = TimeSpan.FromSeconds(boardSize / settings.board.timings.lineDestroyerFlySpeed);
        TimeSpan toTime = fromTime + duration;

        Vector2 awayPosition = MonoMatch3Core.Helpers.BoardToScreen(settings, direction) * boardSize;
        spriteRenderer.AddAnimation(spriteId, new OffsetOverTime(Vector2.Zero, awayPosition, fromTime, toTime, OffsetOverTime.MoveMode.FromTo));

        gameEvents.Timer
            .Wait(duration)
            .Done(() => spriteRenderer.Remove(spriteId));
    }
}
