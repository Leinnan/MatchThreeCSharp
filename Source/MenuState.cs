using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoGameCore {
    public sealed class MenuState : State {
        public override string Id { get{return "MenuState";} }

        private SpriteFont _font;
        private Texture2D _background;
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
                default:
                    RequestStateChange("GameState");
                    break;
            }
        }

        public override void OnMouseReleased (Vector2 mousePos, Vector2 movement) 
        {}
        
        public override void OnLoad(ContentManager content, GraphicsDevice graphics)
        {
            Console.WriteLine("OnLoad"); 
            _font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            _logo = content.Load<Texture2D>(Constants.MenuLogo);
            _background = content.Load<Texture2D>(Constants.IngameBg);
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            //Console.WriteLine("OnDraw"); 
            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(_logo, GetCurrentLogoPos(), Color.White);
            spriteBatch.DrawString(_font, "PRESS ANY KEY TO START GAME", new Vector2(30, 400), Color.Black);
        }

        private Vector2 GetCurrentLogoPos()
        {
            Vector2 result = new Vector2( 40, -100);
            result.Y += 250.0f * (float)Math.Pow(1.0f - _delay,3);
            return result;
        }
    }
}