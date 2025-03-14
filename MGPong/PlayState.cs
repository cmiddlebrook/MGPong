using CALIMOE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;


namespace MGPong;

public class PlayState : GameState
{
    private Rectangle _playArea;
    private Texture2D _board;
    private ScoreBar _scoreBar;
    private Player _player;
    private AIPlayer _aiPlayer;
    private SoundEffect _winFx;
    private SoundEffect _loseFx;
    private Song _playMusic;
    private MainBall _ball;
    private PowerBall _powerBall;
    private bool _fastBall;
    private float _maxAngle = MathHelper.PiOver4;
    private bool _paused = false;
    public PlayState(StateManager sm, AssetManager am, InputHelper ih) :base(sm, am, ih)
    {
        _name = "Play";
    }

    public override void LoadContent()
    {
        _winFx = _am.LoadSoundFx("WinPoint");
        _loseFx = _am.LoadSoundFx("LosePoint");
        _playMusic = _am.LoadMusic("IcecapMountains");

        _board = _am.LoadTexture("Board2");
        _scoreBar = new ScoreBar(_am.LoadTexture("ScoreBar"), _am.LoadFont("Score"), _board.Width);
        _playArea = new Rectangle(0, _scoreBar.Height, _board.Width, _board.Height);

        _player = new Player(new Paddle(_playArea, _am.LoadTexture("LeftPaddle"), 4), 300);

        Texture2D aiPaddleTX = _am.LoadTexture("RightPaddle");
        Paddle aiPaddle = new Paddle(_playArea, aiPaddleTX, _playArea.Width - aiPaddleTX.Width - 4);
        _aiPlayer = new AIPlayer(aiPaddle, 100, 25);

        _ball = new MainBall(_playArea, _am.LoadTexture("WhiteBall"), _am.LoadSoundFx("WallHit"), _am.LoadSoundFx("PaddleHit"));
        _powerBall = new PowerBall(_playArea, _am.LoadTexture("WhiteBall"), _am.LoadSoundFx("WallHit"));
    }

    public override void Enter()
    {
        _player.PrepForNewGame();
        _aiPlayer.PrepForNewGame();
        ServeNewBall();
        MediaPlayer.Volume = 0.2f;
        //MediaPlayer.Play(_playMusic);
        base.Enter();
    }
    public override void HandleInput(GameTime gt)
    {
        base.HandleInput(gt);

        if (_ih.KeyDown(Keys.W))
        {
            _player.MoveUp(gt);
        }

        else if (_ih.KeyDown(Keys.S))
        {
            _player.MoveDown(gt);
        }

        else if (_ih.KeyPressed(Keys.P))
        {
            _paused = !_paused;
        }
        else if (_ih.KeyPressed(Keys.Escape))
        {
            _sm.SwitchState("Title");
        }
    }

    public override void Update(GameTime gt)
    {
        HandleInput(gt);

        if (!_paused)
        {
            _ball.Update(gt);
            _powerBall.Update(gt);
            _aiPlayer.TrackBall(gt, _ball.Bounds);
            CheckPaddleCollision();
            CheckPointScored();
        }

        base.Update(gt);
    }

    public override void Draw(SpriteBatch sb)
    {
        sb.Draw(_board, _playArea, Color.White);
        _scoreBar.Draw(sb, _player.Score, _aiPlayer.Score);
        _powerBall.Draw(sb);
        _player.Draw(sb);
        _aiPlayer.Draw(sb);
        _ball.Draw(sb);

        base.Draw(sb);
    }
    public int getWindowWidth()
    {
        return _board.Bounds.Width;
    }

    public int getWindowHeight()
    {
        return _board.Bounds.Height + _scoreBar.Height;
    }

    protected void CheckPaddleCollision()
    {
        Rectangle ballRect = _ball.Bounds;
        if (ballRect.Intersects(_player.Paddle.Bounds))
        {
            if (_fastBall)
            {
                _ball.FastBall();
            }
            
            BounceBallOffPaddle(_player.Paddle);
        }
        else if (ballRect.Intersects(_aiPlayer.Paddle.Bounds))
        {
            BounceBallOffPaddle(_aiPlayer.Paddle);
            _ball.RevertBallSpeed();
        }

        if (_powerBall.Bounds.Intersects(_player.Paddle.Bounds))
        {
            EnablePowerUp();
        }
    }

    protected void BounceBallOffPaddle(Paddle paddle)
    {
        float relativeIntersectY = (_ball.CenterY - paddle.CenterY) / (paddle.Bounds.Height / 2f);
        _ball.BounceOffPaddle(relativeIntersectY * _maxAngle);
    }

    protected void EnablePowerUp()
    {
        switch (_powerBall.Type)
        {
            case PowerBall.BallType.BigPaddle:
                {
                    _player.Paddle.Grow(true);
                    break;
                }

            case PowerBall.BallType.FastPaddle:
                {
                    _player.Speed = 800;
                    break;
                }

            case PowerBall.BallType.FastBall:
                {
                    _fastBall = true;
                    break;
                }

        }
        _powerBall.CyclePowerBall();
        _powerBall.NewBall();
    }

    protected void CheckPointScored()
    {
        Rectangle ballRect = _ball.Bounds;
        if (ballRect.X < 0)
        {
            _aiPlayer.Score++;
            _loseFx.Play();
            ServeNewBall();
        }
        else if (ballRect.X + ballRect.Width > _playArea.Width)
        {
            _player.Score++;
            _winFx.Play();
            _aiPlayer.PowerUp();
            ServeNewBall();
        }        
    }


    protected void ServeNewBall()
    {
        _fastBall = false;
        _player.PrepForNewBall();
        _aiPlayer.PrepForNewBall();
        _ball.NewBall();
        _powerBall.NewBall();
    }

}
