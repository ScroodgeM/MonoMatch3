using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoMatch3.States;
using MonoMatch3Core.Data;

namespace MonoMatch3;

public class GameManager() : Core("Mono Match 3", new Vector2(1024, 1024), false)
{
    private readonly ProfileState profileState = new ProfileState();

    private Settings settings;
    private BaseState currentState;

    protected override void LoadContent()
    {
        settings = Settings.Load(Content);

        RenderSystem.LoadAtlases(settings.system.textureAtlases);
        RenderSystem.LoadTilemaps(settings.system.textureTilemaps);
        RenderSystem.LoadFont(settings.system.fontName);

        base.LoadContent();
    }

    protected override void Initialize()
    {
        base.Initialize();

        StartNewState(State.MainMenu);
    }

    private void StartNewState(State state)
    {
        if (currentState != null)
        {
            currentState.Die();
            currentState = null;
        }

        currentState = CreateState(state);
        if (currentState != null)
        {
            currentState.OnNewStateRequest += StartNewState;
            currentState.Start();
        }
    }

    private BaseState CreateState(State state)
    {
        switch (state)
        {
            case State.MainMenu:
                return new MainMenu(RenderSystem, Input, settings, GraphicsDevice.PresentationParameters);
            case State.Gameplay:
                return new Gameplay(RenderSystem, this, Input, settings, profileState);
            case State.GameOver:
                return new GameOver(RenderSystem, Input, settings, profileState, GraphicsDevice.PresentationParameters);
        }

        return null;
    }
}
