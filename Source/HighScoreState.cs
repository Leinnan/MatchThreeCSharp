using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameCore
{
    public class HighScoreState : State 
    {
        public override string Id { get{return "HighScoreState";} }

        private SpriteFont _font;
        private Texture2D _background;
        FilledRectangle _veil;
        private Texture2D _logo;
        private float _delay = 0.5f;
        private HighScore _highScore = new HighScore();

        public HighScoreState()
        {
        }

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
            Console.WriteLine("OnEnter"); 
            _highScore.LoadFromFile();
            _delay = 0.5f; 
            DisableInput();
        }
        public override void OnQuit()
        {
            Console.WriteLine("OnQuit"); 
        }

        // input handling
        public override void OnKeyPressed (Keys pressedKey) 
        {
            RequestStateChange("MenuState");
        }

        public override void OnMouseReleased (Vector2 mousePos, Vector2 movement) 
        {}
        
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
            spriteBatch.DrawString(_font, _highScore.GetInStringFormat(), new Vector2(150, 350), new Color(217, 217, 217),0.0f,new Vector2(0.0f,0.0f),new Vector2(1.5f,1.5f),0,1 );
        }

        private Vector2 GetCurrentLogoPos()
        {
            Vector2 result = new Vector2( 40, -100);
            result.Y += 250.0f * (float)Math.Pow(1.0f - _delay,3);
            return result;
        }
    }
}