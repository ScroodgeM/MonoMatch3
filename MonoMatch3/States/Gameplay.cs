using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
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

    internal override void Start()
    {
        board = MonoMatch3Core.Board.Board.Create(gameEvents, inputManager, settings);
        boardView = new View.Board(spriteRenderer, settings, gameEvents, board);
        board.RunGame();
    }

    internal override void Die()
    {
        MonoMatch3Core.Board.Board.Destroy(board);
        board = null;

        boardView.Die();
        boardView = null;
    }
}
