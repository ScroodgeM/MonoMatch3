using System;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoGameLibrary.Promises;
using MonoMatch3Core.Data;

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

        return gameEvents.Timer.Wait(duration);
    }

    internal static IPromise AnimateSimpleTileDestroyBySpecial(this SpriteRenderer spriteRenderer, IGameEvents gameEvents, Settings settings, ushort spriteId)
    {
        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan duration = TimeSpan.FromSeconds(settings.board.timings.destroyBySpecialDisappearDuration);
        TimeSpan toTime = fromTime + duration;

        spriteRenderer.AddAnimation(spriteId, new ChangeScale(1f, 2f, fromTime, toTime));
        spriteRenderer.AddAnimation(spriteId, new ChangeTransparency(1f, 0f, fromTime, toTime));

        return gameEvents.Timer.Wait(duration);
    }

    internal static IPromise AnimateSpecialTileDestroy(this SpriteRenderer spriteRenderer, IGameEvents gameEvents, Settings settings, ushort spriteId)
    {
        TimeSpan fromTime = gameEvents.CurrentTime.Value;
        TimeSpan duration = TimeSpan.FromSeconds(settings.board.timings.destroyBySpecialDisappearDuration);
        TimeSpan toTime = fromTime + duration;

        spriteRenderer.AddAnimation(spriteId, new ChangeTransparency(1f, 0f, fromTime, toTime));

        return gameEvents.Timer.Wait(duration);
    }
}
