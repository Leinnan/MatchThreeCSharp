using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoGameCore {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;
        protected StateHandler m_stateHandler;

        public Game1 () {
            graphics = new GraphicsDeviceManager (this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;   // set this value to the desired height of your window
            graphics.ApplyChanges();
        }

        protected override void Initialize () {
            m_stateHandler = new StateHandler();
            GameState gameState = new GameState();
            m_stateHandler.RegisterState(new GameState());
            base.Initialize ();
        }

        protected override void LoadContent () {
            m_stateHandler.LoadAssets(Content, GraphicsDevice);
            spriteBatch = new SpriteBatch (GraphicsDevice);
            m_stateHandler.Start("GameState");
        }

        protected override void Update (GameTime gameTime) {
            m_stateHandler.Update(gameTime);
            if(m_stateHandler.IsRequestingGameExit())
                Exit();
            base.Update (gameTime);
        }

        protected override void Draw (GameTime gameTime) {
            GraphicsDevice.Clear (Color.Gray);
            spriteBatch.Begin ();
            m_stateHandler.Draw( ref spriteBatch );
            spriteBatch.End ();
            base.Draw (gameTime);
        }

    }
}