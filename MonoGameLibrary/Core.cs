using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.StatefulEvent;
using MonoGameLibrary.Timers;

namespace MonoGameLibrary;

public class Core : Game, IGameEvents
{
    public event Action<GameTime> OnUpdate = time => { };
    public event Action<GameTime> OnDraw = time => { };
    public IStatefulEvent<TimeSpan> CurrentTime => currentTime;
    public ITimer Timer => timer;

    public GraphicsDeviceManager Graphics => graphicsDeviceManager;
    public new GraphicsDevice GraphicsDevice => graphicsDevice;
    public SpriteBatch SpriteBatch => spriteBatch;
    public new ContentManager Content => contentManager;
    public InputManager Input => inputManager;

    protected readonly SpriteRenderer spriteRenderer;
    protected readonly TextRenderer textRenderer;

    private readonly StatefulEventInt<TimeSpan> currentTime = new(TimeSpan.Zero, (a, b) => a == b);

    private readonly GraphicsDeviceManager graphicsDeviceManager;
    private GraphicsDevice graphicsDevice;
    private SpriteBatch spriteBatch;
    private readonly ContentManager contentManager;
    private readonly Timer timer;
    private InputManager inputManager;

    protected Core(string title, Vector2 screenSize, bool isFullScreen)
    {
        graphicsDeviceManager = new GraphicsDeviceManager(this);
        graphicsDeviceManager.PreferredBackBufferWidth = Math.Max(256, (int)screenSize.X);
        graphicsDeviceManager.PreferredBackBufferHeight = Math.Max(256, (int)screenSize.Y);
        graphicsDeviceManager.IsFullScreen = isFullScreen;
        graphicsDeviceManager.ApplyChanges();

        Window.Title = title;

        contentManager = base.Content;
        contentManager.RootDirectory = "Content";

        spriteRenderer = new SpriteRenderer();
        textRenderer = new TextRenderer();

        timer = new Timer(this);

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        graphicsDevice = base.GraphicsDevice;
        spriteBatch = new SpriteBatch(graphicsDevice);

        spriteRenderer.Init(spriteBatch);
        textRenderer.Init(spriteBatch);

        inputManager = new InputManager(this);
    }

    protected void LoadFont(string fontName) => textRenderer.SetFont(Content.Load<SpriteFont>(fontName));

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        currentTime.Set(gameTime.TotalGameTime);

        OnUpdate(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightSeaGreen);
        spriteRenderer.Draw(gameTime);
        textRenderer.Draw();

        base.Draw(gameTime);

        OnDraw(gameTime);
    }
}
