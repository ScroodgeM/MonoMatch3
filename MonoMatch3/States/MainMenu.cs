using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3.View;
using MonoMatch3Core.Data;

namespace MonoMatch3.States;

internal class MainMenu(
    SpriteRenderer spriteRenderer,
    TextRenderer textRenderer,
    Settings settings,
    Rectangle windowRect)
    : BaseState(spriteRenderer, textRenderer, settings)
{
    internal override void Start()
    {
        Transform transform = Transform.Default;
        transform.position = new Vector2(windowRect.Width, windowRect.Height) * 0.5f;
        transform.layerDepth = RenderLayer.MainMenuLogo.ToLayerDepth();

        ushort spriteId = spriteRenderer.AddSprite(settings.view.mainMenuLogoSpriteId, transform);
        spriteRenderer.AddAnimation(spriteId, new PingPongColorChannels(0.5f, 1f, 0.20f, 0.25f, 0.33f));
        spriteRenderer.AddAnimation(spriteId, new PingPongScale(1.0f, 1.1f, 0.16f));
        RegisterSpriteToRemoveOnDeath(spriteId);

        transform.layerDepth = RenderLayer.Text.ToLayerDepth();
        byte textId = textRenderer.AddText(StatefulEventInt.Create("Test 42"), transform);
        RegisterTextToRemoveOnDeath(textId);
    }
}
