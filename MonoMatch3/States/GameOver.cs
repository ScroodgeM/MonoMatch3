using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoGameLibrary.Input;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3.View;
using MonoMatch3Core.Data;

namespace MonoMatch3.States;

internal class GameOver(
    RenderSystem renderSystem,
    InputManager inputManager,
    Settings settings,
    ProfileState profileState,
    PresentationParameters presentationParameters)
    : BaseState(renderSystem, settings)
{
    private ScreenButton screenButton;

    internal override void Start()
    {
        Transform transform = Transform.Default;
        transform.position = new Vector2(presentationParameters.BackBufferWidth, presentationParameters.BackBufferHeight) * 0.5f;
        transform.layerDepth = RenderLayer.Background.ToLayerDepth();

        uint spriteId = renderSystem.AddSprite(settings.system.gameOverLogoSpriteId, transform);
        renderSystem.AddSpriteAnimation(spriteId, new PingPongScale(1.0f, 1.1f, 0.16f));
        RegisterGraphicToRemoveOnDeath(spriteId);

        transform.layerDepth = RenderLayer.Text.ToLayerDepth();
        RegisterGraphicToRemoveOnDeath(renderSystem.AddText(StatefulEventInt.Create("Game Over"), transform));

        transform.position = new Vector2(settings.view.gameOverTopScorePositionX, settings.view.gameOverTopScorePositionY);
        RegisterGraphicToRemoveOnDeath(renderSystem.AddText(StatefulEventInt.Create($"Top score: {profileState.GetTopScore():#,##0}"), transform));

        transform.position = new Vector2(settings.view.gameOverLastScorePositionX, settings.view.gameOverLastScorePositionY);
        RegisterGraphicToRemoveOnDeath(renderSystem.AddText(StatefulEventInt.Create($"Your score: {profileState.GetLastScore():#,##0}"), transform));

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
