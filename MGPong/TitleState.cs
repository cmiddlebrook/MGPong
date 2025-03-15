using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CALIMOE;

namespace MGPong;

public class TitleState : GameState
{
    private TextObject _titleText;
    private TextObject _instructions;
    private string _instructionsText;
    private Song _titleMusic;
    private PowerBallData _ballData;
    private SpriteObject _pbBigPaddle, _pbFastPaddle, _pbFastBall;

    public TitleState(StateManager sm, AssetManager am, InputHelper ih, PowerBallData ballData) :base(sm, am, ih)
	{
        _name = "Title";
        _ballData = ballData;
	}

    public override void LoadContent()
    {
        _titleMusic = _am.LoadMusic("NaturalLife");

        _titleText = new TextObject(_am.LoadFont("Title"));
        _instructions = new TextObject(_am.LoadFont("Instructions"));
        _instructionsText += "You control the left paddle, the AI paddle starts weak,";
        _instructionsText += "\n       but gets stronger with each point scored";
        _instructionsText += "\n\n                      W - Move paddle up";
        _instructionsText += "\n                      S - Move paddle down";
        _instructionsText += "\n                      P - Pause the game";
        _instructionsText += "\n\n                         Big paddle";
        _instructionsText += "\n                         Fast paddle";
        _instructionsText += "\n                         Fast ball";
        _instructionsText += "\n\n                     ESCAPE - Quit to title";
        _instructionsText += "\n                  SPACEBAR - Start new game";

        _pbBigPaddle = new SpriteObject(_am.LoadTexture("WhiteBall"), new Vector2(385, 360), Vector2.Zero, Vector2.One * 1.5f);
        _pbBigPaddle.Colour = _ballData.GetColour(PowerBallData.BallType.BigPaddle);
        _pbFastPaddle = new SpriteObject(_am.LoadTexture("WhiteBall"), new Vector2(385, 395), Vector2.Zero, Vector2.One * 1.5f);
        _pbFastPaddle.Colour = _ballData.GetColour(PowerBallData.BallType.FastPaddle);
        _pbFastBall = new SpriteObject(_am.LoadTexture("WhiteBall"), new Vector2(385, 430), Vector2.Zero, Vector2.One * 1.5f);
        _pbFastBall.Colour = _ballData.GetColour(PowerBallData.BallType.FastBall);
    }

    public override void Enter()
    {
        MediaPlayer.Volume = 0.3f;
        //MediaPlayer.Play(_titleMusic);
        base.Enter();
    }

    public override void HandleInput(GameTime gt)
    {
        base.HandleInput(gt);

        if (_ih.KeyPressed(Keys.Space))
        {
            _sm.SwitchState("Play");
        }
    }

    public override void Update(GameTime gt)
    {
        HandleInput(gt);
        base.Update(gt);
    }

    public override void Draw(SpriteBatch sb)
    {
        _titleText.DrawText(sb, "MG Pong", TextObject.CenterText.Horizontal, 30);
        _instructions.DrawText(sb, _instructionsText, TextObject.CenterText.Horizontal, 100);
        _pbBigPaddle.Draw(sb);
        _pbFastPaddle.Draw(sb);
        _pbFastBall.Draw(sb);

        base.Draw(sb);
    }
}
