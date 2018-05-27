using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameCore {
    public abstract class Screen{
        
        public abstract void OnLoad(ContentManager content, GraphicsDevice graphics);
        public abstract void OnUnload(ContentManager content, GraphicsDevice graphics);
        public abstract void Draw(ref SpriteBatch spriteBatch);
        
    }
}