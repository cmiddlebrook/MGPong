using CALIMOE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Reflection.Metadata;


namespace MGPong;

public class PlayState : GameState
{
    private Rectangle _playArea;
    private Texture2D _board;
    private ScoreBar _scoreBar;
    private Paddle _playerPaddle;
    private Paddle _aiPaddle;
    private SoundEffect _winFx;
    private SoundEffect _loseFx;
    private Song _playMusic;
    private int _playerSpeed = 300;
    private int _aiSpeed = 100;
    private int _aiSpeedIncrease = 25;
    private Ball _ball;
    private int _playerScore = 0;
    private int _aiPlayerScore = 0;
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
        int maxPaddleHeight = (int)(_board.Height * .75);
        _scoreBar = new ScoreBar(_am.LoadTexture("ScoreBar"), _am.LoadFont("Score"), _board.Width);
        _playArea = new Rectangle(0, _scoreBar.Height, _board.Width, _board.Height);
        _playerPaddle = new Paddle(_playArea, _am.LoadTexture("LeftPaddle"), 4, maxPaddleHeight);
        Texture2D aiPaddleTX = _am.LoadTexture("RightPaddle");
        _aiPaddle = new Paddle(_playArea, aiPaddleTX, _playArea.Width - aiPaddleTX.Width - 4, maxPaddleHeight);
        _ball = new Ball(_playArea, _am.LoadTexture("WhiteBall"), _am.LoadSoundFx("PaddleHit"), _am.LoadSoundFx("PaddleHit2"));

    }

    public override void Enter()
    {
        MediaPlayer.Volume = 0.2f;
        //MediaPlayer.Play(_playMusic);
        base.Enter();
    }
    public override void HandleInput(GameTime gt)
    {
        base.HandleInput(gt);

        if (_ih.KeyDown(Keys.W))
        {
            _playerPaddle.MoveUp(gt, _playerSpeed);
        }

        else if (_ih.KeyDown(Keys.S))
        {
            _playerPaddle.MoveDown(gt, _playerSpeed);
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
            MoveAIPlayer(gt);
            CheckPaddleCollision();
            CheckPointScored();
        }

        base.Update(gt);
    }

    public override void Draw(SpriteBatch sb)
    {
        sb.Draw(_board, _playArea, Color.White);
        _scoreBar.Draw(sb, _playerScore, _aiPlayerScore);
        _playerPaddle.Draw(sb);
        _aiPaddle.Draw(sb);
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

    protected void MoveAIPlayer(GameTime gt)
    {
        float ballY = _ball.Bounds.Y;
        float paddleY = _aiPaddle.Bounds.Y;

        if (ballY < paddleY)
        {
            _aiPaddle.MoveUp(gt, _aiSpeed);
        }
        else if (ballY > paddleY)
        {
            _aiPaddle.MoveDown(gt, _aiSpeed);
        }
    }

    protected void CheckPaddleCollision()
    {
        Rectangle ballRect = _ball.Bounds;
        if (ballRect.Intersects(_playerPaddle.Bounds) || ballRect.Intersects(_aiPaddle.Bounds))
        {
            _ball.BounceOffPaddle();
        }
    }

    protected void CheckPointScored()
    {
        Rectangle ballRect = _ball.Bounds;
        if (ballRect.X < 0)
        {
            AIPlayerScored();
        }
        else if (ballRect.X + ballRect.Width > _playArea.Width)
        {
            PlayerScored();
        }
    }

    protected void PlayerScored()
    {
        _playerScore++;
        _winFx.Play();
        ServeNewBall();
        _aiSpeed += _aiSpeedIncrease;
        _aiPaddle.Grow();
    }

    protected void AIPlayerScored()
    {
        _aiPlayerScore++;
        _loseFx.Play();
        ServeNewBall();
    }

    protected void ServeNewBall()
    {
        _ball.Reset();
        _playerPaddle.Reset();
        _aiPaddle.ResetPosition();
    }

}
