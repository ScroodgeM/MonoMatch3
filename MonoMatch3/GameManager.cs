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
        "таймер обратного отсчёта",
        "кнопка запуска в главном меню",
        "избавиться от клавиатуры",
        "выкинуть кор-синглтон",
        "свапы?",
        "1. При запуске игры открывается Главное Меню, в котором есть единственная кнопка Play.",
        "2. При нажатии на Play открывается Игровой Экран.",
        "3. Игровой Экран представляет из себя поле 8x8 квадратных клеток.",
        "12. За каждый исчезнувший элемент игрок получает очки. Количество набранных очков должно быть видно на Игровом Экране.",
        "13. Игроку даётся 60 секунд на игру. По истечении этого времени игра заканчивается. На Игровом Экране должно быть видно сколько времени осталось.",
        "14. По завершению игры должно появиться сообщение «Game Over», с единственной кнопкой Ok. При нажатии на Ok игрок попадёт в Главное Меню.",
        "21. Все перемещения Разрушителей должны быть анимированы.",
    ];

    private Settings settings;
    private MonoMatch3Core.Board.Board board;
    private View.Board boardView;
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
        if (Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Exit();
        }

        if (Input.Keyboard.WasKeyJustPressed(Keys.Space))
        {
            if (board == null)
            {
                StartNewState(State.Gameplay);
                StartGame();
            }
            else
            {
                FinishGame();
                StartNewState(State.MainMenu);
            }
        }

        base.Update(gameTime);
    }

    private void StartGame()
    {
        board = MonoMatch3Core.Board.Board.Create(this, Input, settings);
        boardView = new View.Board(spriteRenderer, settings, this, board);
        board.RunGame();
    }

    private void FinishGame()
    {
        MonoMatch3Core.Board.Board.Destroy(board);
        board = null;

        boardView.Die();
        boardView = null;
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
            currentState.Start();
        }
    }

    private BaseState CreateState(State state)
    {
        switch (state)
        {
            case State.MainMenu:
                return new MainMenu(spriteRenderer, textRenderer, settings, Window.ClientBounds);
        }

        return null;
    }
}
