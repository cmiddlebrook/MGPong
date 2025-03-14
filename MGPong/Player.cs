using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MGPong;

public class Player
{
    protected Paddle _paddle;

    private int _startSpeed;
    public int Speed { get; set; }
    public int Score { get; set; }
    public Paddle Paddle => _paddle;

    public Player(Paddle paddle, int speed)
    {
        _paddle = paddle;
        _startSpeed = speed;
        Speed = _startSpeed;
    }

    public virtual void MoveUp(GameTime gt)
    {
        _paddle.MoveUp(gt, Speed);
    }

    public virtual void MoveDown(GameTime gt)
    {
        _paddle.MoveDown(gt, Speed);
    }

    public virtual void Draw(SpriteBatch sb)
    {
        _paddle.Draw(sb);
    }

    public virtual void PrepForNewBall()
    {
        Speed = _startSpeed;
        _paddle.Reset();
    }

    public virtual void PrepForNewGame()
    {
        Score = 0;
        PrepForNewBall();
    }
}
