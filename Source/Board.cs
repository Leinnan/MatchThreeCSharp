using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameCore
{
    public class Board 
    {
        Vector2 BoardMin = new Vector2((480-394)/2,50);
        struct SymbolType
        {
            public String gfxPath;
            public int id;
            public Texture2D texture;
        }
        struct Symbol
        {
            public int symbolType;
            Vector2 pos;
            public void SetPos(int x, int y)
            {
                pos = new Vector2(x,y);
            }
            public Vector2 GetGfxPos(){ return ( new Vector2((480-394)/2,50)+pos*Constants.FIELD_SIZE); }
            public Texture2D GetGfx(SymbolType[] symbolsTypes){ return symbolsTypes[symbolType].texture; }
        }

        Random rnd = new Random();
        FilledRectangle m_background;
        SymbolType[] m_symbolsTypes = new SymbolType[8]; 
        Symbol[,] m_symbols = new Symbol[Constants.BOARD_SIZE, Constants.BOARD_SIZE];

        public Board(ContentManager content, GraphicsDevice graphics)
        {
            ReadSymbolsConfig();
            for(int i = 0; i < Constants.SYMBOLS_TYPES_AMOUNT; i++)
            {
                m_symbolsTypes[i].texture = content.Load<Texture2D>(Constants.SYMBOLS_PATH[i]);
            }
            m_background = new FilledRectangle(
                BoardMin, // position
                new Vector2(394,394), // size
                new Color(0,0,0,200));
            m_background.GenerateTexture(graphics);
            GenerateSymbolsTable();
        }

        // for now preconfigured, maybe in future I will move this config to file
        void ReadSymbolsConfig()
        {
            for(int i = 0; i < Constants.SYMBOLS_TYPES_AMOUNT; i++)
            {
                m_symbolsTypes[i].id = i;
                m_symbolsTypes[i].gfxPath = Constants.SYMBOLS_PATH[i];
            }
        }
        void GenerateSymbolsTable()
        {
            for(int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                for(int j = 0; j < Constants.BOARD_SIZE; j++)
                {
                    m_symbols[i, j].symbolType = rnd.Next(0,Constants.SYMBOLS_TYPES_AMOUNT);
                    m_symbols[i, j].SetPos(i,j);
                }
            }
        }
        public void Draw(ref SpriteBatch spriteBatch)
        {
            m_background.Draw(ref spriteBatch);
            for(int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                for(int j = 0; j < Constants.BOARD_SIZE; j++)
                {
                    spriteBatch.Draw(m_symbols[i,j].GetGfx(m_symbolsTypes), m_symbols[i,j].GetGfxPos(), Color.White);
                }
            }
        }

        Symbol GetSymbolAtIndex(int x, int y){ return m_symbols[x, y]; }

    }
}