using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoGameCore {
    public sealed class GameState : State {
        public override string Id { get{return "GameState";} }

        private SpriteFont font;
        private Texture2D background;
        public Board boardGfx;
        private Texture2D[] m_symbols;

        public GameState(){}

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
            font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            background = content.Load<Texture2D>(Constants.INGAME_BG);
            boardGfx = new Board(graphics);
            m_symbols = new Texture2D[16];

            for(int i = 0; i < 16; i++)
            {
                m_symbols[i] = content.Load<Texture2D>(Constants.SYMBOLS_PATH[i]);
            }
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            boardGfx.Draw(ref spriteBatch);
            for(int y = 0; y < 12; y++)
                for(int x = 0; x < 12; x++)
                    spriteBatch.Draw(m_symbols[((x+y)+16)%500%16], new Vector2((480-394)/2+x*32, 50+(32*y)), Color.White);
            spriteBatch.DrawString(font, "Score", new Vector2(10, 10), Color.Black);
        }  
        
        public struct Board
        {
            public Board(GraphicsDevice graphics)
            {
                m_background = new FilledRectangle(
                    new Vector2((480-394)/2,50), // position
                    new Vector2(394,394), // size
                    new Color(0,0,0,200));
                m_background.GenerateTexture(graphics);
            }
            public void Draw(ref SpriteBatch spriteBatch)
            {
                m_background.Draw(ref spriteBatch);
            }
            public FilledRectangle m_background;
        }   
    }
}