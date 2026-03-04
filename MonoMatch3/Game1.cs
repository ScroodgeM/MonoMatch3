using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;

namespace MonoMatch3;

public class Game1() : Core("Mono Match 3", new Vector2(1024, 1024), false)
{
    private Sprite gameLogo;

    private TextureAtlas gemsAtlas;
    private readonly List<string> gemSpriteNames = new List<string>();

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        TextureAtlas logoAtlas = TextureAtlas.FromFile(Content, ContentStructure.images.logo);
        foreach (string logoSpriteName in logoAtlas.AllSpriteNames)
        {
            gameLogo = logoAtlas.GetSprite(logoSpriteName);
            gameLogo.AddAnimation(new PingPongColorChannels(0.5f, 1f, 0.20f, 0.25f, 0.33f));
            gameLogo.AddAnimation(new PingPongScale(1.0f, 1.1f, 0.16f));
            break;
        }

        gemsAtlas = TextureAtlas.FromFile(Content, ContentStructure.images.gems);
        gemSpriteNames.AddRange(gemsAtlas.AllSpriteNames);

        float initialRotationStep = MathF.PI * 2.0f / (float)gemSpriteNames.Count;
        for (int i = 0; i < gemSpriteNames.Count; i++)
        {
            string gemSpriteName = gemSpriteNames[i];
            Sprite gemSprite = gemsAtlas.GetSprite(gemSpriteName);
            gemSprite.AddAnimation(new RotateAround(i * initialRotationStep, 0.45f, 300f));
        }

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        Rectangle windowRect = Window.ClientBounds;

        GraphicsDevice.Clear(Color.LightSeaGreen);

        SpriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

        Sprite.Transform defaultTransform = Sprite.Transform.Default;
        defaultTransform.position = new Vector2(windowRect.Width, windowRect.Height) * 0.5f;

        defaultTransform.layerDepth = (int)RenderLayers.MainMenuLogo;
        gameLogo.Draw(SpriteBatch, defaultTransform, gameTime);

        defaultTransform.layerDepth = (int)RenderLayers.Elements;
        foreach (string spriteName in gemSpriteNames)
        {
            gemsAtlas.GetSprite(spriteName).Draw(SpriteBatch, defaultTransform, gameTime);
        }

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
