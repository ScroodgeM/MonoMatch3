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
