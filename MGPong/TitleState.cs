using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CALIMOE;

namespace MGPong;

public class TitleState : GameState
{
    private TextObject _titleObj = null;
    private Song _titleMusic;

    public TitleState(StateManager sm, AssetManager am, InputHelper ih) :base(sm, am, ih)
	{
        _name = "Title";
	}

    public override void LoadContent()
    {
        _titleMusic = _am.LoadMusic("NaturalLife");
        _titleObj = new TextObject(_am.LoadFont("Title"));
    }

    public override void Enter()
    {
        MediaPlayer.Volume = 0.3f;
        MediaPlayer.Play(_titleMusic);
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
        _titleObj.DrawText(sb, "MG Pong", TextObject.CenterText.Horizontal, 50);
        _titleObj.DrawText(sb, "Press [SPACE] to play!", TextObject.CenterText.Both);

        base.Draw(sb);
    }
}
