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
        "кнопка запуска в главном меню",
        "избавиться от клавиатуры",
        "выкинуть кор-синглтон",
        "свапы?",
        "топ-очки и очки на экрае поражения",
        "1. При запуске игры открывается Главное Меню, в котором есть единственная кнопка Play.",
        "2. При нажатии на Play открывается Игровой Экран.",
        "3. Игровой Экран представляет из себя поле 8x8 квадратных клеток.",
        "14. По завершению игры должно появиться сообщение «Game Over», с единственной кнопкой Ok. При нажатии на Ok игрок попадёт в Главное Меню.",
        "21. Все перемещения Разрушителей должны быть анимированы.",
    ];

    private Settings settings;
    private BaseState currentState;

    protected override void LoadContent()
    {
        settings = Settings.Load(Content);

        TextureAtlas.Load(Content, settings.system.textureAtlases, spriteRenderer.Pool);

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

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.Space))
        {
            if (currentState is MainMenu)
            {
                StartNewState(State.Gameplay);
            }
        }

        base.Update(gameTime);
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
                return new MainMenu(spriteRenderer, textRenderer, settings, Window.ClientBounds);
            case State.Gameplay:
                return new Gameplay(spriteRenderer, textRenderer, this, Input, settings);
        }

        return null;
    }
}
