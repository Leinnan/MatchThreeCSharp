using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameCore {
    public sealed class MenuState : State {
        public override string Id { get{return "MenuState";} }

        private SpriteFont _font;
        private Texture2D _background;
        FilledRectangle _veil;
        private FilledRectangle _startGameBtnBg;
        private FilledRectangle _highScoreBtnBg;
        private Texture2D _logo;
        private float _delay = 0.5f;
        
        public MenuState(){}

        public override void OnInit()
        {
        }
        
        public override void OnShutdown()
        {
        }
        
        public override void OnUpdate(GameTime gameTime)
        {
            if(_delay >= 0.0f)
            {
                _delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
            if(_delay <= 0.0f)
                EnableInput();
        }
        public override void OnEnter()
        {
            _delay = 0.5f; 
            DisableInput();
            Console.WriteLine("OnEnter"); 
        }
        public override void OnQuit()
        {
            Console.WriteLine("OnQuit"); 
        }

        // input handling
        public override void OnKeyPressed (Keys pressedKey) 
        {            
            switch (pressedKey) {
                case Keys.Escape:
                    RequestGameExit();
                    break;
                case Keys.H:
                    RequestStateChange("HighScoreState");
                    break;
                default:
                    RequestStateChange("GameState");
                    break;
            }
        }

        public override void OnMouseReleased(Vector2 mousePos, Vector2 movement)
        {
            if (_startGameBtnBg.IsVectorInRectangle((int) mousePos.X, (int) mousePos.Y))
            {
                RequestStateChange("GameState");
            }
            else if(_highScoreBtnBg.IsVectorInRectangle((int) mousePos.X, (int) mousePos.Y))
            {
                RequestStateChange("HighScoreState");
            }
        }
        
        public override void OnLoad(ContentManager content, GraphicsDevice graphics)
        {
            Console.WriteLine("OnLoad"); 
            _font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            _logo = content.Load<Texture2D>(Constants.MenuLogo);
            _background = content.Load<Texture2D>(Constants.IngameBg);
            
            _veil = new FilledRectangle(
                new Vector2(0, 320), // position
                new Vector2(Constants.ScreenWidth,Constants.ScreenHeight-320), // size
                new Color(0,0,0,170));
            _veil.GenerateTexture(graphics);
            
            _startGameBtnBg = new FilledRectangle(
                new Vector2(20, 320 + 20), // position
                new Vector2(Constants.ScreenWidth-40,210), // size
                new Color(46,204,64,255));
            _startGameBtnBg.GenerateTexture(graphics);
            
            _highScoreBtnBg = new FilledRectangle(
                new Vector2(20, 320 + 20 + 210 + 20), // position
                new Vector2(Constants.ScreenWidth-40,210), // size
                new Color(0,116,217,255));
            _highScoreBtnBg.GenerateTexture(graphics);
        }
        
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {
        }
        
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            //Console.WriteLine("OnDraw"); 
            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(_logo, GetCurrentLogoPos(), Color.White);
            _veil.Draw(ref spriteBatch);
            _startGameBtnBg.Draw(ref spriteBatch);
            _highScoreBtnBg.Draw(ref spriteBatch);
            spriteBatch.DrawString(_font, "Start Game", new Vector2(130, 430), Color.WhiteSmoke);
            spriteBatch.DrawString(_font, "HighScore", new Vector2(130, 620), Color.WhiteSmoke);
        }

        private Vector2 GetCurrentLogoPos()
        {
            Vector2 result = new Vector2( 40, -100);
            result.Y += 250.0f * (float)Math.Pow(1.0f - _delay,3);
            return result;
        }
    }
}