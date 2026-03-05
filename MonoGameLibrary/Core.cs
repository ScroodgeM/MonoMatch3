using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

namespace MonoGameLibrary;

public class Core : Game, IGameEvents
{
    public event Action<GameTime> OnUpdate = time => { };
    public event Action<GameTime> OnDraw = time => { };

    public static Core Instance => instance;
    private static Core instance;

    public GraphicsDeviceManager Graphics => graphicsDeviceManager;
    public new GraphicsDevice GraphicsDevice => graphicsDevice;
    public SpriteBatch SpriteBatch => spriteBatch;
    public new ContentManager Content => contentManager;
    public InputManager Input => inputManager;

    protected readonly SpriteRenderer spriteRenderer;

    private readonly GraphicsDeviceManager graphicsDeviceManager;
    private GraphicsDevice graphicsDevice;
    private SpriteBatch spriteBatch;
    private readonly ContentManager contentManager;
    private InputManager inputManager;

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

        spriteRenderer = new SpriteRenderer();

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        graphicsDevice = base.GraphicsDevice;
        spriteBatch = new SpriteBatch(graphicsDevice);

        spriteRenderer.Init(spriteBatch);

        inputManager = new InputManager(this);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        OnUpdate(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightSeaGreen);
        spriteRenderer.Draw(gameTime);
        base.Draw(gameTime);

        OnDraw(gameTime);
    }
}
