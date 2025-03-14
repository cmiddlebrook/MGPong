using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


namespace MGPong;

public class MainBall : Ball
{
    protected const float FAST_SPEED = 2500f;

    protected SoundEffect _paddleHitFx;
    protected float _speedIncrease = 40f;

    public MainBall(Rectangle playArea, Texture2D texture, SoundEffect wallHit, SoundEffect paddleHit)
        : base(playArea, texture, wallHit)
    {
        _paddleHitFx = paddleHit;
    }

    protected override Vector2 GetStartPosition()
    {
        int startX = (_playArea.Width - (_size / 2)) / 2;
        int startY = (_playArea.Height - (_size / 2)) / 2;
        return new Vector2(startX, startY);
    }
    protected override Vector2 GetStartVelocity()
    {
        Vector2 randomVelocity = new Vector2(_rand.Next(2) == 0 ? -100f : 100f, (_rand.Next(10, 50)));
        randomVelocity.Y *= _rand.Next(2) == 0 ? 1 : -1;
        randomVelocity.Normalize();
        randomVelocity *= _speed;

        return randomVelocity;
    }

    public void FastBall()
    {
        _speed = FAST_SPEED;
    }

    public void RevertBallSpeed()
    {
        _speed = NORMAL_SPEED;
    }

    public override void NewBall()
    {
        RevertBallSpeed();
        _sprite.Reset();
        _sprite.Velocity = GetStartVelocity();
    }
    public void BounceOffPaddle(float angle)
    {
        _sprite.ReverseXDirection();
        _speed += _speedIncrease;
        _sprite.Velocity = new Vector2(MathF.Cos(angle) * _speed * MathF.Sign(_sprite.Velocity.X), MathF.Sin(angle) * _speed);
        _paddleHitFx.Play();
    }
}
