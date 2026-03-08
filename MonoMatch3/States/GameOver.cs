using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoGameLibrary.Input;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3.View;
using MonoMatch3Core.Data;

namespace MonoMatch3.States;

internal class GameOver(
    SpriteRenderer spriteRenderer,
    TextRenderer textRenderer,
    InputManager inputManager,
    Settings settings,
    Rectangle windowRect)
    : BaseState(spriteRenderer, textRenderer, settings)
{
    private ScreenButton screenButton;

    internal override void Start()
    {
        Transform transform = Transform.Default;
        transform.position = new Vector2(windowRect.Width, windowRect.Height) * 0.5f;
        transform.layerDepth = RenderLayer.Background.ToLayerDepth();

        ushort spriteId = spriteRenderer.AddSprite(settings.system.gameOverLogoSpriteId, transform);
        spriteRenderer.AddAnimation(spriteId, new PingPongScale(1.0f, 1.1f, 0.16f));
        RegisterSpriteToRemoveOnDeath(spriteId);

        transform.layerDepth = RenderLayer.Text.ToLayerDepth();

        byte textId = textRenderer.AddText(StatefulEventInt.Create("Game Over"), transform);
        RegisterTextToRemoveOnDeath(textId);

        ScreenButton.Transform screenButtonTransform;
        screenButtonTransform.position = new Vector2(settings.view.gameOverOKButtonPositionX, settings.view.gameOverOKButtonPositionY);
        screenButtonTransform.scale = Vector2.One;
        screenButtonTransform.spriteId = settings.system.buttonBackgroundSpriteId;
        screenButtonTransform.text = "OK";
        screenButtonTransform.spriteLayerDepth = RenderLayer.UIEements.ToLayerDepth();
        screenButtonTransform.textLayerDepth = RenderLayer.Text.ToLayerDepth();
        screenButton = inputManager.CreateScreenButton(screenButtonTransform);
        screenButton.OnClick += () => SwitchToState(State.MainMenu);
    }

    internal override void Die()
    {
        screenButton.Die();
        base.Die();
    }
}
