using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoMatch3.States;
using MonoMatch3Core.Data;

namespace MonoMatch3;

public class GameManager() : Core("Mono Match 3", new Vector2(1024, 1024), false)
{
    private static readonly string[] tasks =
    [
        "ракета в действии - может пересечься навстречу падающему шарику",
        "бомба в действии",
        "избавиться от клавиатуры",
        "выкинуть кор-синглтон",
        "свапы?",
        "топ-очки и очки на экрае поражения",
        "3. Игровой Экран представляет из себя поле 8x8 квадратных клеток.",
        "21. Все перемещения Разрушителей должны быть анимированы.",
    ];

    private Settings settings;
    private BaseState currentState;

    protected override void LoadContent()
    {
        settings = Settings.Load(Content);

        AtlasLoader.Load(Content, settings.system.textureAtlases, spriteRenderer);

        base.LoadContent();
    }

    protected override void Initialize()
    {
        base.Initialize();

        foreach (string task in tasks)
        {
            Console.OutputEncoding = new UTF8Encoding();
            Console.WriteLine($"Task: {task}");
        }

        LoadFont(settings.system.fontName);

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
                return new MainMenu(spriteRenderer, textRenderer, Input, settings, Window.ClientBounds);
            case State.Gameplay:
                return new Gameplay(spriteRenderer, textRenderer, this, Input, settings);
            case State.GameOver:
                return new GameOver(spriteRenderer, textRenderer, Input, settings, Window.ClientBounds);
        }

        return null;
    }
}
