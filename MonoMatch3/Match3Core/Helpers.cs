namespace MonoMatch3.Match3Core;

public static class Helpers
{
    public static bool IsPositionValid(int positionX, int positionY, byte boardSizeX, byte boardSizeY)
    {
        return
            positionX >= 0
            &&
            positionX < boardSizeX
            &&
            positionY >= 0
            &&
            positionY < boardSizeY;
    }
}
