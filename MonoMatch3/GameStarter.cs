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
    protected override void LoadContent()
    {
        Rectangle windowRect = Window.ClientBounds;
        Vector2 screenCenter = new Vector2(windowRect.Width, windowRect.Height) * 0.5f;

        TextureAtlas logoAtlas = TextureAtlas.FromFile(Content, ContentStructure.images.logo);
        spriteRenderer.Pool.Add(logoAtlas);

        foreach (string logoSpriteName in logoAtlas.AllSpriteNames)
        {
            Sprite.Transform transform = Sprite.Transform.Default;
            transform.position = screenCenter;
            transform.layerDepth = (int)RenderLayers.MainMenuLogo;

            ushort spriteId = spriteRenderer.AddSprite(logoSpriteName, transform);

            spriteRenderer.AddAnimation(spriteId, new PingPongColorChannels(0.5f, 1f, 0.20f, 0.25f, 0.33f));
            spriteRenderer.AddAnimation(spriteId, new PingPongScale(1.0f, 1.1f, 0.16f));

            break;
        }

        TextureAtlas gemsAtlas = TextureAtlas.FromFile(Content, ContentStructure.images.gems);
        spriteRenderer.Pool.Add(gemsAtlas);
        List<string> gemSpriteNames = new List<string>(gemsAtlas.AllSpriteNames);
        float initialRotationStep = MathF.PI * 2.0f / (float)gemSpriteNames.Count;
        for (int i = 0; i < gemSpriteNames.Count; i++)
        {
            Sprite.Transform transform = Sprite.Transform.Default;
            transform.position = screenCenter;
            transform.layerDepth = (int)RenderLayers.Elements;

            ushort spriteId = spriteRenderer.AddSprite(gemSpriteNames[i], transform);

            spriteRenderer.AddAnimation(spriteId, new RotateAround(i * initialRotationStep, 0.45f, 300f));
        }

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
        GameSettings gameSettings = GameSettings.FromFile(Content);
        BoardInput boardInput = new BoardInput(this, Input, gameSettings);
        Board board = new Board(this, boardInput, gameSettings);
        View.Board boardView = new View.Board(spriteRenderer, gameSettings, board);
    }
}
