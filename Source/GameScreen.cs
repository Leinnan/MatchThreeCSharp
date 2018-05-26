using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameCore {
    public sealed class GameScreen : Screen {
        private SpriteFont font;
        
        public override void OnLoad(ContentManager content)
        {
            font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
        }
        public override void OnUnload(ContentManager content)
        {

        }
        public override void Draw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Score", new Vector2(10, 10), Color.Black);
        }        
    }
}