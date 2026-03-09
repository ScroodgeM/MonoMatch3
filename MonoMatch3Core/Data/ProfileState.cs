using System;

namespace MonoMatch3Core.Data;

public class ProfileState
{
    private ulong topScore = 0;
    private ulong lastScore = 0;

    public ulong GetTopScore() => topScore;

    public ulong GetLastScore() => lastScore;

    internal void SubmitScore(ulong score)
    {
        lastScore = score;
        topScore = Math.Max(topScore, score);
    }
}
