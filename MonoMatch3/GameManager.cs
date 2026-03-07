using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoMatch3Core.Board;
using MonoMatch3Core.Data;

namespace MonoMatch3;

public class GameManager() : Core("Mono Match 3", new Vector2(1024, 1024), false)
{
    private static readonly string[] tasks =
    [
        "бонус за 5 фишек",
        "ракета в действии",
        "бомба в действии",
        "таймер обратного отсчёта",
        "кнопка запуска в главном меню",
        "избавиться от клавиатуры",
        "выкинуть кор-синглтон",
        "1. При запуске игры открывается Главное Меню, в котором есть единственная кнопка Play.",
        "2. При нажатии на Play открывается Игровой Экран.",
        "3. Игровой Экран представляет из себя поле 8x8 квадратных клеток.",
        "12. За каждый исчезнувший элемент игрок получает очки. Количество набранных очков должно быть видно на Игровом Экране.",
        "13. Игроку даётся 60 секунд на игру. По истечении этого времени игра заканчивается. На Игровом Экране должно быть видно сколько времени осталось.",
        "14. По завершению игры должно появиться сообщение «Game Over», с единственной кнопкой Ok. При нажатии на Ok игрок попадёт в Главное Меню.",
        "19. В случае, если Line бонус участвует в матче, он срабатывает: элемент разрушается, и из него в противоположных направлениях, зависящих от направления бонуса, вылетают два Разрушителя, каждый из которых летит прямо, пока не долетит до края игрового поля.",
        "20. Если над элементом летит Разрушитель, элемент уничтожается.",
        "21. Все перемещения Разрушителей должны быть анимированы.",
        "22. Если Разрушитель пролетает над другим бонусом, бонус срабатывает.",
        "27. В случае, если бонус Bomb участвует в матче, он срабатывает: элемент разрушается, и через 250 мс разрушаются все элементы вокруг него в квадрате 3х3 с центром в том месте, где взорвался бонус.",
        "28. Если во взрывающейся области оказался другой бонус, он срабатывает.",
    ];

    private Settings settings;
    private Board board;
    private View.Board boardView;

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

        StartMainMenu();
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
                spriteRenderer.RemoveAll();
                StartGame();
            }
            else
            {
                FinishGame();
                StartMainMenu();
            }
        }

        base.Update(gameTime);
    }

    private void StartMainMenu()
    {
        Sprite.Transform transform = Sprite.Transform.Default;
        Rectangle windowRect = Window.ClientBounds;
        transform.position = new Vector2(windowRect.Width, windowRect.Height) * 0.5f;
        transform.layerDepth = (int)RenderLayers.MainMenuLogo;

        ushort spriteId = spriteRenderer.AddSprite(settings.view.mainMenuLogoSpriteId, transform);

        spriteRenderer.AddAnimation(spriteId, new PingPongColorChannels(0.5f, 1f, 0.20f, 0.25f, 0.33f));
        spriteRenderer.AddAnimation(spriteId, new PingPongScale(1.0f, 1.1f, 0.16f));
    }

    private void StartGame()
    {
        board = Board.Create(this, timer, Input, settings);
        boardView = new View.Board(spriteRenderer, settings, this, timer, board);
        board.RunGame();
    }

    private void FinishGame()
    {
        Board.Destroy(board);
        board = null;

        boardView.Die();
        boardView = null;
    }
}
