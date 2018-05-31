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

        public enum Substate {
            IDLE,
            SWAPING,
            WINING,
            SWAPING_BACK,
            DROPPING
        }

        private SpriteFont font;
        private Texture2D background;
        private Substate m_currentSubstate = Substate.IDLE;
        float m_delay = 0.0f; 
        (Vector2 one, Vector2 two) m_swappedSymbols;
        Vector2 m_selectedSymbol;
        
        public MenuState(){}

        public void SwitchState( Substate newSubstate ){ m_currentSubstate = newSubstate; }

        public override void OnInit()
        {
        }
        public override void OnShutdown()
        {
        }
        public override void OnUpdate(GameTime gameTime)
        {
            if(m_delay >= 0.0f)
            {
                m_delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

        }
        public override void OnEnter()
        {
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
            font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            background = content.Load<Texture2D>(Constants.INGAME_BG);
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            //Console.WriteLine("OnDraw"); 
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "PRESS ANY KEY TO START GAME", new Vector2(30, 400), Color.Black);
        }  
    }
}