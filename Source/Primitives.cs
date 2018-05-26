using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameCore
{
    public class MyRectangle
    {
        Vector2 m_size;
        Vector2 m_pos;
        Color m_color;
        Color[] m_data;
        Texture2D m_rect;
        public MyRectangle(Vector2 pos, Vector2 size, Color color)
        {
            m_pos = pos;
            m_size = size;
            m_color = color;
            m_data = new Color[(int)pos.X * (int)pos.Y];
        }
        public void GenerateTexture(ref GraphicsDeviceManager graphics)
        {
            m_rect = new Texture2D(graphics.GraphicsDevice, (int)m_pos.X, (int)m_pos.Y);
            for(int i=0; i < m_data.Length; ++i) m_data[i] = m_color;
                m_rect.SetData(m_data);
        }

        public void Move(Vector2 movement)
        {
            m_pos += movement;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_rect,m_pos,Color.White);
        }
    }
}
