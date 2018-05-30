using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameCore
{
    public class FilledRectangle
    {
        Rectangle m_rect;
        Color[] m_data;
        Texture2D m_txt;
        public FilledRectangle(Vector2 pos, Vector2 size, Color color)
        {
            m_rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            m_data = new Color[]{color};
        }
        public void GenerateTexture(GraphicsDevice graphics)
        {
            m_txt = new Texture2D(graphics, 1, 1);
            m_txt.SetData(m_data);
        }

        public void Move(Vector2 movement)
        {
            m_rect.X += (int)movement.X;
            m_rect.Y += (int)movement.Y;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_txt,m_rect,Color.White);
        }
    }
}
