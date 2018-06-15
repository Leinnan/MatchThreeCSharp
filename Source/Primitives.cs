using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameCore
{
    public class FilledRectangle
    {
        Rectangle _rect;
        Color[] _data;
        Texture2D _txt;
        public FilledRectangle(Vector2 pos, Vector2 size, Color color)
        {
            _rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            _data = new Color[]{color};
        }
        public void GenerateTexture(GraphicsDevice graphics)
        {
            _txt = new Texture2D(graphics, 1, 1);
            _txt.SetData(_data);
        }

        public void Move(Vector2 movement)
        {
            _rect.X += (int)movement.X;
            _rect.Y += (int)movement.Y;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_txt,_rect,Color.White);
        }

        public bool IsVectorInRectangle(int x, int y)
        {
            return _rect.Contains(x, y);
        }
    }
}
