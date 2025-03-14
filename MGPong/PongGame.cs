using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using CALIMOE;

namespace MGPong;

public class PongGame : Calimoe
{    

    public PongGame()
    {
        Window.AllowUserResizing = false;
        Window.Title = "Game #1 - MG Pong";
        MediaPlayer.IsRepeating = true;
        _showFPS = false;
        _fallbackTextureSize = 64;
    }


    protected override void LoadContent()
    {
        base.LoadContent();

        _sm.AddState(new TitleState(_sm, _am, _ih));
        PlayState play = new PlayState(_sm, _am, _ih);
        _sm.AddState(play);
        _sm.SwitchState("Title");

        _graphics.PreferredBackBufferWidth = play.getWindowWidth();
        _graphics.PreferredBackBufferHeight = play.getWindowHeight();
        _graphics.ApplyChanges();
    }



    protected override void Update(GameTime gt)
    {
        if (_sm.Current == "Title" && _ih.KeyPressed(Keys.Escape))
        {
            Exit();
        }

        base.Update(gt);
        _sm.Update(gt);

    }


    protected override void Draw(GameTime gt)
    {        
        base.Draw(gt);

        GraphicsDevice.Clear(new Color(0x10, 0x10, 0x10));

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
        _sm.Draw(_spriteBatch);
        _spriteBatch.End();
    }




}
