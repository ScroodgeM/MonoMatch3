using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

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
            break;
        }

        gemsAtlas = TextureAtlas.FromFile(Content, ContentStructure.images.gems);
        gemSpriteNames.AddRange(gemsAtlas.AllSpriteNames);

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
        float logoScale = 1.05f + 0.05f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds);

        float r = 0.75f + 0.25f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.2);
        float g = 0.75f + 0.25f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.6);
        float b = 0.75f + 0.25f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2.1);

        Rectangle windowRect = Window.ClientBounds;

        GraphicsDevice.Clear(Color.LightSeaGreen);

        SpriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

        Sprite.Transform logoTransform = Sprite.Transform.Default;
        logoTransform.position = new Vector2(windowRect.Width, windowRect.Height) * 0.5f;
        logoTransform.color = new Color(r, g, b, 1f);
        logoTransform.scale = Vector2.One * logoScale;
        logoTransform.layerDepth = (int)RenderLayers.MainMenuLogo;

        gameLogo.Draw(SpriteBatch, logoTransform, gameTime);

        Sprite.Transform gemTransform = Sprite.Transform.Default;
        gemTransform.layerDepth = (int)RenderLayers.Elements;

        float elementsRotation = (float)((gameTime.TotalGameTime.TotalSeconds * 0.45) % (Math.PI * 2.0));
        float rotationStepInRadians = MathF.PI * 2.0f / (float)gemSpriteNames.Count;
        for (var i = 0; i < gemSpriteNames.Count; i++)
        {
            float offsetX = MathF.Sin(elementsRotation + i * rotationStepInRadians);
            float offsetY = MathF.Cos(elementsRotation + i * rotationStepInRadians);

            gemTransform.position = new Vector2(windowRect.Width, windowRect.Height) * 0.5f + new Vector2(offsetX, offsetY) * 300f;

            gemsAtlas.GetSprite(gemSpriteNames[i]).Draw(SpriteBatch, gemTransform, gameTime);
        }

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
