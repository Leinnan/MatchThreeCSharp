using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameCore
{
    public class Board 
    {
        public struct SymbolType
        {
            public String gfxPath;
            public int id;
            public Texture2D texture;
        }
        public struct Symbol
        {
            public int symbolType;
            Vector2 pos;
            bool highlighted;
            public void SetPos(int x, int y)
            {
                pos = new Vector2(x,y);
            }
            public void SetPos(Vector2 newPos)
            {
                pos = newPos;
            }
            public Vector2 GetPos(){ return pos; }
            public Vector2 GetGfxPos(){ return ( new Vector2(Constants.BOARD_POS_X,50)+pos*Constants.SYMBOL_GFX_SIZE); }
            public Texture2D GetGfx(SymbolType[] symbolsTypes){ return symbolsTypes[symbolType].texture; }
            public bool IsTouched(Vector2 touchPos)
            {
                Vector2 gfxMinPos = GetGfxPos();
                Vector2 gfxMaxPos = gfxMinPos + new Vector2(Constants.SYMBOL_GFX_SIZE,Constants.SYMBOL_GFX_SIZE);
                
                return (touchPos.X >= gfxMinPos.X && touchPos.Y >= gfxMinPos.Y &&
                        touchPos.X <= gfxMaxPos.X && touchPos.Y <= gfxMaxPos.Y );
            }
            public void SetHighlighted(bool enable = true) { highlighted = enable; }
            public bool IsHighlighted() { return highlighted; }
            public void DisplayValuesToConsole(string prefix="")
            {
                Console.WriteLine("------------------");
                Console.WriteLine(prefix + "Vector2 pos = ( " + pos.X.ToString() + ", " + pos.Y.ToString() + " );");
                Console.WriteLine(prefix + "int symbolType = " + symbolType.ToString() + ";\n");
            }
        }

        Random rnd = new Random();
        FilledRectangle m_background;
        SymbolType[] m_symbolsTypes = new SymbolType[8]; 
        Symbol[,] m_symbols = new Symbol[Constants.BOARD_SIZE, Constants.BOARD_SIZE];
        (bool isAny, Vector2 pos) m_selectedSymbol = (false, new Vector2(-1,-1));

        public Board(ContentManager content, GraphicsDevice graphics)
        {
            ReadSymbolsConfig();
            for(int i = 0; i < Constants.SYMBOLS_TYPES_AMOUNT; i++)
            {
                m_symbolsTypes[i].texture = content.Load<Texture2D>(Constants.SYMBOLS_PATH[i]);
            }
            m_background = new FilledRectangle(
                new Vector2(Constants.BOARD_POS_X, Constants.BOARD_POS_Y), // position
                new Vector2(Constants.BOARD_LENGTH,Constants.BOARD_LENGTH), // size
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

        public void SelectSymbolAtIndex(int x, int y)
        {
            if(m_selectedSymbol.isAny)
                m_symbols[(int)m_selectedSymbol.pos.X,(int)m_selectedSymbol.pos.Y].SetHighlighted(false);

            m_symbols[x,y].SetHighlighted();
            m_selectedSymbol.pos = new Vector2(x,y);
            m_selectedSymbol.isAny = true;
        }

        public void StopSymbolSelection()
        {
            if(!m_selectedSymbol.isAny)
                return;

            m_symbols[(int)m_selectedSymbol.pos.X,(int)m_selectedSymbol.pos.Y].SetHighlighted(false);
            m_selectedSymbol = (false, new Vector2(-1,-1));
        }

        public bool IsAnySymbolSelected(){ return m_selectedSymbol.isAny; }

        public Vector2 GetSelectedSymbolPos(){ return m_selectedSymbol.pos; }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            m_background.Draw(ref spriteBatch);
            for(int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                for(int j = 0; j < Constants.BOARD_SIZE; j++)
                {
                    spriteBatch.Draw(m_symbols[i,j].GetGfx(m_symbolsTypes), 
                                     m_symbols[i,j].GetGfxPos(), 
                                     m_symbols[i,j].IsHighlighted() ? Color.Red : Color.White);
                }
            }
        }

        public ref Symbol GetSymbolAtIndex(int x, int y){ return ref m_symbols[x, y]; }

        public void SwapSymbols(Vector2 first, Vector2 second)
        {
            int symbolTypeTmp = m_symbols[(int)first.X,(int)first.Y].symbolType;           
            
            m_symbols[(int)first.X,(int)first.Y].symbolType = m_symbols[(int)second.X,(int)second.Y].symbolType;
            m_symbols[(int)second.X,(int)second.Y].symbolType = symbolTypeTmp;
        }
        public bool CanSwapWithSelectedSymbol(Vector2 pos)
        {
            if(pos.X == m_selectedSymbol.pos.X)
            {
                return ( Math.Abs( (int)pos.Y - (int)m_selectedSymbol.pos.Y ) == 1 );
            }
            if(pos.Y == m_selectedSymbol.pos.Y)
            {
                return ( Math.Abs( (int)pos.X - (int)m_selectedSymbol.pos.X ) == 1 );
            }
            return false;
        }


    }
}