using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace MonoMatch3;

public class Game1() : Core("Mono Match 3", new Vector2(1024, 1024), false)
{
    private Texture2D gameLogo;
    private Texture2D[] elements;

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        gameLogo = Content.Load<Texture2D>(ContentStructure.images.logo);

        elements = new Texture2D[]
        {
            Content.Load<Texture2D>(ContentStructure.images.element1),
            Content.Load<Texture2D>(ContentStructure.images.element2),
            Content.Load<Texture2D>(ContentStructure.images.element3),
            Content.Load<Texture2D>(ContentStructure.images.element4),
            Content.Load<Texture2D>(ContentStructure.images.element5),
        };

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float logoScale = 1.05f + 0.05f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds);

        float r = 0.75f + 0.25f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.2);
        float g = 0.75f + 0.25f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.6);
        float b = 0.75f + 0.25f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2.1);
        Color logoTintColor = new Color(r, g, b, 1f);

        Rectangle windowRect = Window.ClientBounds;
        Vector2 windowCenter = new Vector2(windowRect.Width, windowRect.Height) * 0.5f;

        float elementsRotation = (float)((gameTime.TotalGameTime.TotalSeconds * 1.45) % (Math.PI * 2.0));

        GraphicsDevice.Clear(Color.LightSeaGreen);

        SpriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

        SpriteBatch.Draw(
            gameLogo,
            windowCenter,
            null,
            logoTintColor,
            0f,
            new Vector2(gameLogo.Width, gameLogo.Height) * 0.5f,
            logoScale,
            SpriteEffects.None,
            (float)RenderLayers.MainMenuLogo
        );

        for (var i = 0; i < elements.Length; i++)
        {
            Texture2D element = elements[i];

            float offsetX = MathF.Sin(elementsRotation + MathF.PI * 2.0f * (float)i / (float)elements.Length);
            float offsetY = MathF.Cos(elementsRotation + MathF.PI * 2.0f * (float)i / (float)elements.Length);

            SpriteBatch.Draw(
                element,
                windowCenter + new Vector2(offsetX, offsetY) * 300f,
                null,
                Color.White,
                0f,
                new Vector2(element.Width, element.Height) * 0.5f,
                0.3f,
                SpriteEffects.None,
                (float)RenderLayers.Elements
            );
        }

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
