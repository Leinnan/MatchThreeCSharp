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
            public bool matching;
            public void SetPos(int x, int y)
            {
                pos = new Vector2(x,y);
            }
            public void SetPos(Vector2 newPos)
            {
                pos = newPos;
            }
            public Vector2 GetPos(){ return pos; }
            public Vector2 GetGfxPos(){ return ( new Vector2(Constants.BoardPosX,50)+pos*Constants.SymbolGfxSize); }
            public Texture2D GetGfx(SymbolType[] symbolsTypes){ return symbolsTypes[symbolType].texture; }
            public bool IsTouched(Vector2 touchPos)
            {
                Vector2 gfxMinPos = GetGfxPos();
                Vector2 gfxMaxPos = gfxMinPos + new Vector2(Constants.SymbolGfxSize,Constants.SymbolGfxSize);
                
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
        SymbolType[] m_symbolsTypes = new SymbolType[Constants.SymbolsTypesAmount]; 
        Symbol[,] m_symbols = new Symbol[Constants.BoardSize, Constants.BoardSize];
        (bool isAny, Vector2 pos) m_selectedSymbol = (false, new Vector2(-1,-1));
        int m_matchingSymbolsAmount = 0;

        public Board(ContentManager content, GraphicsDevice graphics)
        {
            ReadSymbolsConfig();
            for(int i = 0; i < Constants.SymbolsTypesAmount; i++)
            {
                m_symbolsTypes[i].texture = content.Load<Texture2D>(Constants.SymbolsPath[i]);
            }
            m_background = new FilledRectangle(
                new Vector2(Constants.BoardPosX, Constants.BoardPosY), // position
                new Vector2(Constants.BoardLength,Constants.BoardLength), // size
                new Color(0,0,0,200));
            m_background.GenerateTexture(graphics);
        }

        // for now preconfigured, maybe in future I will move this config to file
        void ReadSymbolsConfig()
        {
            for(int i = 0; i < Constants.SymbolsTypesAmount; i++)
            {
                m_symbolsTypes[i].id = i;
                m_symbolsTypes[i].gfxPath = Constants.SymbolsPath[i];
            }
        }

        public void GenerateSymbolsTable()
        {
            for(int i = 0; i < Constants.BoardSize; i++)
            {
                for(int j = 0; j < Constants.BoardSize; j++)
                {
                    m_symbols[i, j].symbolType = rnd.Next(0,Constants.SymbolsTypesAmount);
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
            for(int i = 0; i < Constants.BoardSize; i++)
            {
                for(int j = 0; j < Constants.BoardSize; j++)
                {
                    spriteBatch.Draw(m_symbols[i,j].GetGfx(m_symbolsTypes), 
                                     m_symbols[i,j].GetGfxPos(), 
                                     m_symbols[i,j].IsHighlighted() ? Color.Red : Color.White);
                }
            }
        }

        public void DestroySymbol(int x, int y)
        {
            for(int i = y; i >= 1; i--)
            {
                SwapSymbols( new Vector2(x,i), new Vector2(x,i-1) );
            }
            m_symbols[x, 0].symbolType = rnd.Next(0,Constants.SymbolsTypesAmount);
        }

        public int GetMatchingSymbolsAmount(){ return m_matchingSymbolsAmount; }

        public void CalculateMatchingSymbols()
        {
            m_matchingSymbolsAmount = 0;

            for(int i=1;i<Constants.BoardSize-1;i++)
            for(int j=1;j<Constants.BoardSize-1;j++)
            {
                // X axis
                if (m_symbols[i,j].symbolType==m_symbols[i+1,j].symbolType &&
                    m_symbols[i,j].symbolType==m_symbols[i-1,j].symbolType)
                    for(int n=-1;n<=1;n++)
                    {
                        m_symbols[i+n,j].matching = true;
                        m_matchingSymbolsAmount++;
                    }

                // Y axis
                if (m_symbols[i,j].symbolType==m_symbols[i,j+1].symbolType &&
                    m_symbols[i,j].symbolType==m_symbols[i,j-1].symbolType)
                    for(int n=-1;n<=1;n++) 
                    {
                        m_symbols[i,j+n].matching = true;
                        m_matchingSymbolsAmount++;
                    }
            }
            for(int i=1;i<Constants.BoardSize-1;i++)
            {
                // X axis
                if (m_symbols[i,Constants.BoardSize-1].symbolType==m_symbols[i+1,Constants.BoardSize-1].symbolType &&
                    m_symbols[i,Constants.BoardSize-1].symbolType==m_symbols[i-1,Constants.BoardSize-1].symbolType)
                    for(int n=-1;n<=1;n++)
                    {
                        m_symbols[i+n,Constants.BoardSize-1].matching = true;
                        m_matchingSymbolsAmount++;
                    }
                // X axis
                if (m_symbols[i,0].symbolType==m_symbols[i+1,0].symbolType &&
                    m_symbols[i,0].symbolType==m_symbols[i-1,0].symbolType)
                    for(int n=-1;n<=1;n++)
                    {
                        m_symbols[i+n,0].matching = true;
                        m_matchingSymbolsAmount++;
                    }

                // Y axis
                if (m_symbols[Constants.BoardSize-1,i].symbolType==m_symbols[Constants.BoardSize-1,i+1].symbolType &&
                    m_symbols[Constants.BoardSize-1,i].symbolType==m_symbols[Constants.BoardSize-1,i-1].symbolType)
                    for(int n=-1;n<=1;n++) 
                    {
                        m_symbols[Constants.BoardSize-1,i+n].matching = true;
                        m_matchingSymbolsAmount++;
                    }
                
                if (m_symbols[0,i].symbolType==m_symbols[0,i+1].symbolType &&
                    m_symbols[0,i].symbolType==m_symbols[0,i-1].symbolType)
                    for(int n=-1;n<=1;n++) 
                    {
                        m_symbols[0,i+n].matching = true;
                        m_matchingSymbolsAmount++;
                    }
            }

        }
        //! returns true if destroy any symbol
        public bool DestroyAllMatchingSymbols()
        {
            bool wasAnySymbolDestroyed = false;
            for(int i=0;i<Constants.BoardSize;i++)
            for(int j=0;j<Constants.BoardSize;j++)
                if(m_symbols[i,j].matching)
                {
                    DestroySymbol(i,j);
                    m_symbols[i,j].matching = false;
                    wasAnySymbolDestroyed =true;
                }
            m_matchingSymbolsAmount = 0;

            return wasAnySymbolDestroyed;
        }

        // TODO Edit after animation implementation
        public bool AreAnyAnimationInProgress(){ return false; }

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

        public Vector2 GetSymbolIndexAtGfxPos(Vector2 pos)
        {
            for(int i = 0; i < Constants.BoardSize; i++)
                for(int j = 0; j < Constants.BoardSize; j++)
                    if( m_symbols[i, j].IsTouched(pos))
                    {
                        return new Vector2(i, j);
                    }

            return new Vector2(-1,-1);

        }

    }
}