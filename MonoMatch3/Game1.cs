using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace MonoMatch3;

public class Game1() : Core("Mono Match 3", new Vector2(1024, 1024), false)
{
    private Texture2D gameLogo;

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        gameLogo = Content.Load<Texture2D>(ContentStructure.images.logo);

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
        GraphicsDevice.Clear(Color.LightSeaGreen);

        SpriteBatch.Begin();
        SpriteBatch.Draw(gameLogo, Vector2.Zero, Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
