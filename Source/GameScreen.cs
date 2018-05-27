using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameCore {
    public sealed class GameScreen : Screen {
        private SpriteFont font;
        private Texture2D background;
        public Board boardGfx;

        public struct Board
        {
            public Board(GraphicsDevice graphics)
            {
                m_background = new FilledRectangle(
                    new Vector2(20,50), // position
                    new Vector2(440,440), // size
                    new Color(0,0,0,200));
                m_background.GenerateTexture(graphics);
            }
            public void Draw(ref SpriteBatch spriteBatch)
            {
                m_background.Draw(ref spriteBatch);
            }
            public FilledRectangle m_background;
        }
        
        public override void OnLoad(ContentManager content, GraphicsDevice graphics)
        {
            font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            background = content.Load<Texture2D>(Constants.INGAME_BG);
            boardGfx = new Board(graphics);
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void Draw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            boardGfx.Draw(ref spriteBatch);
            spriteBatch.DrawString(font, "Score", new Vector2(10, 10), Color.Black);
        }        
    }
}