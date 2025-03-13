using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MGPong;

public class Player
{
    protected Paddle _paddle;
    protected int _speed;

    public int Speed => _speed;
    public int Score { get; set; }
    public Paddle Paddle => _paddle;

    public Player(Paddle paddle, int speed)
    {
        _paddle = paddle;
        _speed = speed;
    }

    public virtual void MoveUp(GameTime gt)
    {
        _paddle.MoveUp(gt, _speed);
    }

    public virtual void MoveDown(GameTime gt)
    {
        _paddle.MoveDown(gt, _speed);
    }

    public virtual void Draw(SpriteBatch sb)
    {
        _paddle.Draw(sb);
    }

    public virtual void PrepForNewBall()
    {
        _paddle.Reset();
    }

    public virtual void PrepForNewGame()
    {
        Score = 0;
        PrepForNewBall();
    }
}
