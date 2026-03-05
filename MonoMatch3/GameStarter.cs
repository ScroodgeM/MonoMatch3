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
        foreach (string logoSpriteName in logoAtlas.AllSpriteNames)
        {
            Sprite sprite = logoAtlas.GetSprite(logoSpriteName);
            sprite.AddAnimation(new PingPongColorChannels(0.5f, 1f, 0.20f, 0.25f, 0.33f));
            sprite.AddAnimation(new PingPongScale(1.0f, 1.1f, 0.16f));

            Sprite.Transform transform = Sprite.Transform.Default;
            transform.position = screenCenter;
            transform.layerDepth = (int)RenderLayers.MainMenuLogo;

            spriteRenderer.Bank.Add(logoSpriteName, sprite);
            spriteRenderer.Add(logoSpriteName, transform);
            break;
        }

        TextureAtlas gemsAtlas = TextureAtlas.FromFile(Content, ContentStructure.images.gems);
        List<string> gemSpriteNames = new List<string>(gemsAtlas.AllSpriteNames);
        float initialRotationStep = MathF.PI * 2.0f / (float)gemSpriteNames.Count;
        for (int i = 0; i < gemSpriteNames.Count; i++)
        {
            string spriteName = gemSpriteNames[i];
            Sprite sprite = gemsAtlas.GetSprite(spriteName);
            sprite.AddAnimation(new RotateAround(i * initialRotationStep, 0.45f, 300f));

            Sprite.Transform transform = Sprite.Transform.Default;
            transform.position = screenCenter;
            transform.layerDepth = (int)RenderLayers.Elements;

            spriteRenderer.Bank.Add(spriteName, sprite);
            spriteRenderer.Add(spriteName, transform);
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
