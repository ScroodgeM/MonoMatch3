using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoMatch3Core;
using MonoMatch3Core.Board;
using MonoMatch3Core.Data;

namespace MonoMatch3;

public class GameStarter() : Core("Mono Match 3", new Vector2(1024, 1024), false)
{
    private Settings settings;

    protected override void LoadContent()
    {
        settings = Settings.Load(Content);

        TextureAtlas.Load(Content, settings.system.textureAtlases, spriteRenderer.Pool);

        Sprite.Transform transform = Sprite.Transform.Default;
        Rectangle windowRect = Window.ClientBounds;
        transform.position = new Vector2(windowRect.Width, windowRect.Height) * 0.5f;
        transform.layerDepth = (int)RenderLayers.MainMenuLogo;

        ushort spriteId = spriteRenderer.AddSprite(settings.view.mainMenuLogoSpriteId, transform);

        spriteRenderer.AddAnimation(spriteId, new PingPongColorChannels(0.5f, 1f, 0.20f, 0.25f, 0.33f));
        spriteRenderer.AddAnimation(spriteId, new PingPongScale(1.0f, 1.1f, 0.16f));

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Exit();
        }

        if (Input.Keyboard.WasKeyJustPressed(Keys.Space))
        {
            spriteRenderer.RemoveAll();
            StartGame();
        }

        base.Update(gameTime);
    }

    private void StartGame()
    {
        Board board = Board.Create(this, timer, Input, settings);
        View.Board boardView = new View.Board(spriteRenderer, settings, this, timer, board);
        board.RunGame();
    }
}
