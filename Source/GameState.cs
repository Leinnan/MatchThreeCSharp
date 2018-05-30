using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoGameCore {
    public sealed class GameState : State {
        public override string Id { get{return "GameState";} }

        public enum Substate {
            IDLE,
            SWAPING,
            WINING,
            SWAPING_BACK,
            DROPPING
        }

        private SpriteFont font;
        private Texture2D background;
        public Board m_board;
        public int m_score;
        private Substate m_currentSubstate = Substate.IDLE;
        
        public GameState(){}

        public void SwitchState( Substate newSubstate ){ m_currentSubstate = newSubstate; }

        public override void OnInit()
        {
            Console.WriteLine("OnInit"); 
        }
        public override void OnShutdown()
        {
            Console.WriteLine("OnShutdown"); 
        }
        public override void OnUpdate(GameTime gameTime)
        {
            Console.WriteLine("OnUpdate"); 
            switch(m_currentSubstate)
            {
                case Substate.IDLE:
                {
                    break;
                }
                default:
                {
                    break;
                }
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
                    break;
            }
        }

        
        public override void OnLoad(ContentManager content, GraphicsDevice graphics)
        {
            Console.WriteLine("OnLoad"); 
            font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            background = content.Load<Texture2D>(Constants.INGAME_BG);
            m_board = new Board(content, graphics);
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            Console.WriteLine("OnDraw"); 
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            m_board.Draw(ref spriteBatch);
            spriteBatch.DrawString(font, "Score: " + m_score.ToString(), new Vector2(10, 10), Color.Black);
        }  
    }
}