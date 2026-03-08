using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3.View;
using MonoMatch3Core.Data;

namespace MonoMatch3.States;

internal class Gameplay(
    SpriteRenderer spriteRenderer,
    TextRenderer textRenderer,
    IGameEvents gameEvents,
    InputManager inputManager,
    Settings settings)
    : BaseState(spriteRenderer, textRenderer, settings)
{
    private MonoMatch3Core.Board.Board board;
    private View.Board boardView;
    private TimeSpan gameStartTime;
    private readonly StatefulEventInt<string> sessionCountdown = StatefulEventInt.Create(string.Empty);

    private readonly List<ushort> mySprites = new List<ushort>();
    private readonly List<byte> myTexts = new List<byte>();

    internal override void Start()
    {
        board = MonoMatch3Core.Board.Board.Create(gameEvents, inputManager, settings);
        boardView = new View.Board(spriteRenderer, settings, gameEvents, board);
        board.RunGame();

        gameStartTime = gameEvents.CurrentTime.Value;
        gameEvents.CurrentTime.OnValueChanged += OnTimeChanged;

        Transform countdownTransform = Transform.Default;
        countdownTransform.layerDepth = RenderLayer.Text.ToLayerDepth();
        countdownTransform.position = new Vector2(settings.view.countdownPositionX, settings.view.countdownPositionY);
        myTexts.Add(textRenderer.AddText(sessionCountdown, countdownTransform));
    }

    internal override void Die()
    {
        MonoMatch3Core.Board.Board.Destroy(board);
        boardView.Die();

        foreach (ushort spriteId in mySprites)
        {
            spriteRenderer.RemoveSprite(spriteId);
        }

        foreach (byte textId in myTexts)
        {
            textRenderer.RemoveText(textId);
        }
    }

    private void OnTimeChanged(TimeSpan time)
    {
        TimeSpan timeElapsed = time - gameStartTime;
        TimeSpan timeLimit = TimeSpan.FromSeconds(settings.board.timings.gameSessionTimeLimit);
        if (timeElapsed > timeLimit)
        {
            SwitchToState(State.GameOver);
        }
        else
        {
            Console.WriteLine(timeLimit - timeElapsed);
            sessionCountdown.Set($"Time left: {(timeLimit - timeElapsed).TotalSeconds:0.0}");
        }
    }
}
