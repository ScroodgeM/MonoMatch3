using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoMatch3.Match3Core;

namespace MonoMatch3;

public class GameStarter() : Core("Mono Match 3", new Vector2(1024, 1024), false)
{
    private GameSettings gameSettings;

    protected override void LoadContent()
    {
        gameSettings = GameSettings.FromFile(Content);

        TextureAtlas logoAtlas = TextureAtlas.FromFile(Content, ContentStructure.images.logo);
        spriteRenderer.Pool.Add(logoAtlas);
        TextureAtlas gemsAtlas = TextureAtlas.FromFile(Content, ContentStructure.images.gems);
        spriteRenderer.Pool.Add(gemsAtlas);

        Sprite.Transform transform = Sprite.Transform.Default;
        Rectangle windowRect = Window.ClientBounds;
        transform.position = new Vector2(windowRect.Width, windowRect.Height) * 0.5f;
        transform.layerDepth = (int)RenderLayers.MainMenuLogo;

        ushort spriteId = spriteRenderer.AddSprite(gameSettings.view.mainMenu.logoSpriteId, transform);

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
        BoardInput boardInput = new BoardInput(this, Input, gameSettings);
        Board board = new Board(this, timer, boardInput, gameSettings);
        View.Board boardView = new View.Board(spriteRenderer, gameSettings, this, board);

        board.RunGame();
    }
}
