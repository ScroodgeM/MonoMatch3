using System;
using Microsoft.Xna.Framework;
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

    protected InputManager Input => inputManager;
    protected RenderSystem RenderSystem => renderSystem;

    private readonly StatefulEventInt<TimeSpan> currentTime = new(TimeSpan.Zero, (a, b) => a == b);

    private readonly Timer timer;
    private readonly RenderSystem renderSystem;
    private readonly InputManager inputManager;

    protected Core(string title, Vector2 screenSize, bool isFullScreen)
    {
        GraphicsDeviceManager graphicsDeviceManager = new GraphicsDeviceManager(this);
        graphicsDeviceManager.PreferredBackBufferWidth = Math.Max(256, (int)screenSize.X);
        graphicsDeviceManager.PreferredBackBufferHeight = Math.Max(256, (int)screenSize.Y);
        graphicsDeviceManager.IsFullScreen = isFullScreen;
        graphicsDeviceManager.ApplyChanges();

        Window.Title = title;

        Content.RootDirectory = "Content";

        timer = new Timer(this);
        renderSystem = new RenderSystem();
        inputManager = new InputManager(this, renderSystem);

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        renderSystem.Init(new SpriteBatch(GraphicsDevice));
    }

    protected void LoadFont(string fontName) => renderSystem.TextRenderer.SetFont(Content.Load<SpriteFont>(fontName));

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        currentTime.Set(gameTime.TotalGameTime);

        OnUpdate(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightSeaGreen);

        renderSystem.Draw(gameTime);

        base.Draw(gameTime);

        OnDraw(gameTime);
    }
}
