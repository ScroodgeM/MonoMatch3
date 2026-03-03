using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary;

public class Core : Game
{
    public static Core Instance => instance;
    private static Core instance;

    public GraphicsDeviceManager Graphics => graphicsDeviceManager;
    public new GraphicsDevice GraphicsDevice => graphicsDevice;
    public SpriteBatch SpriteBatch => spriteBatch;
    public new ContentManager Content => contentManager;

    private readonly GraphicsDeviceManager graphicsDeviceManager;
    private GraphicsDevice graphicsDevice;
    private SpriteBatch spriteBatch;
    private readonly ContentManager contentManager;

    public Core(string title, Vector2 screenSize, bool isFullScreen)
    {
        if (instance != null)
        {
            throw new NotSupportedException("Only one instance of Core can be created");
        }

        instance = this;

        graphicsDeviceManager = new GraphicsDeviceManager(this);
        graphicsDeviceManager.PreferredBackBufferWidth = Math.Max(256, (int)screenSize.X);
        graphicsDeviceManager.PreferredBackBufferHeight = Math.Max(256, (int)screenSize.Y);
        graphicsDeviceManager.IsFullScreen = isFullScreen;
        graphicsDeviceManager.ApplyChanges();

        Window.Title = title;

        contentManager = base.Content;
        contentManager.RootDirectory = "Content";

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        graphicsDevice = base.GraphicsDevice;
        spriteBatch = new SpriteBatch(graphicsDevice);
    }
}
