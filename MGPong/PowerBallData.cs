using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace MGPong;

public class PowerBallData
{
    public enum BallType
    {
        FastBall,
        BigPaddle,
        FastPaddle,
    }

    private Dictionary<BallType, Color> _ballColors;

    public PowerBallData()
    {
        _ballColors = new Dictionary<BallType, Color>();
        _ballColors[BallType.FastBall] = new Color(255, 50, 50) * 0.7f; // red
        _ballColors[BallType.BigPaddle] = new Color(57, 255, 20) * 0.7f; // green
        _ballColors[BallType.FastPaddle] = new Color(4, 118, 208) * 0.8f; // blue
    }

    public Color GetColour(BallType type)
    {
        return _ballColors[type];
    }

}
