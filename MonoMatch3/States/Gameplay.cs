using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3.View;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3.States;

internal class Gameplay(
    SpriteRenderer spriteRenderer,
    RenderSystem renderSystem,
    IGameEvents gameEvents,
    InputManager inputManager,
    Settings settings,
    ProfileState profileState)
    : BaseState(spriteRenderer, renderSystem, settings)
{
    private MonoMatch3Core.Board.Board board;
    private Board boardView;
    private TimeSpan gameStartTime;
    private readonly StatefulEventInt<string> sessionCountdown = StatefulEventInt.Create(string.Empty);
    private readonly StatefulEventInt<string> scoreLabel = StatefulEventInt.Create(string.Empty);

    internal override void Start()
    {
        board = MonoMatch3Core.Board.Board.Create(gameEvents, inputManager, settings);
        boardView = new Board(spriteRenderer, renderSystem, settings, gameEvents, board);

        board.OnTileRemoved += OnTileRemoved;

        board.RunGame();

        gameStartTime = gameEvents.CurrentTime.Value;
        gameEvents.CurrentTime.OnValueChanged += OnTimeChanged;

        SpawnText();

        UpdateScoreLabel();
    }

    internal override void Die()
    {
        MonoMatch3Core.Board.Board.Destroy(board);
        boardView.Die();
        gameEvents.CurrentTime.OnValueChanged -= OnTimeChanged;
        base.Die();
    }

    private void SpawnText()
    {
        Transform countdownTransform = Transform.Default;
        countdownTransform.layerDepth = RenderLayer.Text.ToLayerDepth();
        countdownTransform.position = new Vector2(settings.view.countdownPositionX, settings.view.countdownPositionY);
        RegisterTextToRemoveOnDeath(renderSystem.AddText(sessionCountdown, countdownTransform));

        Transform scoreTransform = Transform.Default;
        scoreTransform.layerDepth = RenderLayer.Text.ToLayerDepth();
        scoreTransform.position = new Vector2(settings.view.scorePositionX, settings.view.scorePositionY);
        RegisterTextToRemoveOnDeath(renderSystem.AddText(scoreLabel, scoreTransform));
    }

    private void OnTileRemoved(TileBase tile, TileRemoveReason removeReason)
    {
        switch (removeReason)
        {
            case TileRemoveReason.DestroyedBySpecial:
                profileState.AddScore(settings.board.scorePerDestroyedTile);
                break;
            case TileRemoveReason.SuccessMatch:
                profileState.AddScore(settings.board.scorePerMatchedTile);
                break;
        }

        UpdateScoreLabel();
    }

    private void UpdateScoreLabel()
    {
        scoreLabel.Set($"Score: {profileState.GetLastScore():#,##0}");
    }

    private void OnTimeChanged(TimeSpan time)
    {
        TimeSpan timeElapsed = time - gameStartTime;
        float timeLimit = settings.board.timings.gameSessionTimeLimit;

        if (timeElapsed.TotalSeconds > timeLimit)
        {
            SwitchToState(State.GameOver);
        }
        else
        {
            TimeSpan timeLeft = TimeSpan.FromSeconds(timeLimit) - timeElapsed;
            sessionCountdown.Set($"Time left: {timeLeft.TotalSeconds:0.0}");
        }
    }
}
