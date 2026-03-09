using System;

namespace MonoMatch3;

public class ProfileState
{
    private ulong topScore = 0;
    private ulong lastScore = 0;

    public ulong GetTopScore() => topScore;

    public ulong GetLastScore() => lastScore;

    internal void StartNewGame()
    {
        lastScore = 0;
    }

    internal void AddScore(ulong score)
    {
        lastScore += score;
        topScore = Math.Max(topScore, lastScore);
    }
}
