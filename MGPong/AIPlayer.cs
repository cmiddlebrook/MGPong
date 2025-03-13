﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;


namespace MGPong;

public class AIPlayer : Player
{
    private int _speedIncrease;

    public AIPlayer(Paddle paddle, int speed, int speedIncrease)
        : base(paddle, speed)
    {
        _speedIncrease = speedIncrease;
    }

    public void TrackBall(GameTime gt, Rectangle ballBounds)
    {
        Point ballPos = ballBounds.Center;
        Point paddlePos = _paddle.Bounds.Center;

        if (ballPos.Y < paddlePos.Y)
        {
            _paddle.MoveUp(gt, _speed);
        }
        else if (ballPos.Y > paddlePos.Y)
        {
            _paddle.MoveDown(gt, _speed);
        }
    }

    public void PowerUp()
    {
        _speed += _speedIncrease;
        _paddle.Grow();
    }

    public override void PrepForNewBall()
    {
        _paddle.ResetPositionOnly();
    }

}
