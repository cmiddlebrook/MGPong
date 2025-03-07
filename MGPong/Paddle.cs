using CALIMOE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGPong;

public class Paddle
{
    private SpriteObject _sprite;
    private Rectangle _playArea;

    public Rectangle Bounds => _sprite.Bounds;

    public Paddle(Rectangle playArea, Texture2D texture, int xPosition)
    {
        _playArea = playArea;
        int startY = (playArea.Height - (texture.Height / 2)) / 2;
        _sprite = new SpriteObject(texture, new Vector2(xPosition, startY), Vector2.Zero);
    }

    public void Reset()
    {
        _sprite.Reset();
    }

    public void MoveUp(GameTime gt, int speed = 300)
    {
        float newY = (float)(_sprite.Position.Y - speed * gt.ElapsedGameTime.TotalSeconds);
        _sprite.Position = new Vector2(_sprite.Position.X, Math.Max(newY, _playArea.Top));
    }

    public void MoveDown(GameTime gt, int speed = 300)
    {
        float newY = (float)(_sprite.Position.Y + speed * gt.ElapsedGameTime.TotalSeconds);
        _sprite.Position = new Vector2(_sprite.Position.X, Math.Min(newY, _playArea.Bottom - _sprite.Bounds.Height));
    }

    public void Draw(SpriteBatch sb) 
	{
        _sprite.Draw(sb);
	}
}
