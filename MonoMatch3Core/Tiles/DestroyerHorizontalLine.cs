using System;
using MonoGameLibrary;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Tiles;

internal class DestroyerHorizontalLine(Settings settings, IGameEvents gameEvents, Board.Board board, TileColor tileColor, TilePosition position)
    : TileBase(settings, gameEvents, board, tileColor, position)
{
    public override TileType Type => TileType.DestroyerHorizontalLine;

    internal override void ProcessSuccessMatch()
    {
        Console.WriteLine("process destroyer effect here");
        board.RemoveTile(position.Value, TileRemoveReason.SuccessMatch);
    }

    internal override void ChangeTypeTo(TileType newTileType) => throw new InvalidOperationException("we should not be here");
}
