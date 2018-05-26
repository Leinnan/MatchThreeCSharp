using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameCore {
    public sealed class GameScreen : Screen {
        private SpriteFont font;
        private Texture2D background;
        
        public override void OnLoad(ContentManager content)
        {
            font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            background = content.Load<Texture2D>(Constants.INGAME_BG);
        }
        public override void OnUnload(ContentManager content)
        {

        }
        public override void Draw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "Score", new Vector2(10, 10), Color.Black);
        }        
    }
}