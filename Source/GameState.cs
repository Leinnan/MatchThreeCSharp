using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoGameCore {
    public sealed class GameState : State {
        public override string Id { get{return "GameState";} }

        public enum Substate {
            IDLE,
            SWAPING,
            WINING,
            SWAPING_BACK,
            DROPPING
        }

        private SpriteFont font;
        private Texture2D background;
        public Board m_board;
        public int m_score;
        private Substate m_currentSubstate = Substate.IDLE;
        SoundEffect m_swipeEffect;
        SoundEffect m_swipeBackEffect;
        SoundEffect m_matchingEffect;
        float m_delay = 0.0f; 
        (Vector2 one, Vector2 two) m_swappedSymbols;
        
        public GameState(){}

        public void SwitchState( Substate newSubstate ){ m_currentSubstate = newSubstate; }

        public override void OnInit()
        {
            Console.WriteLine("OnInit"); 
        }
        public override void OnShutdown()
        {
            Console.WriteLine("OnShutdown"); 
        }
        public override void OnUpdate(GameTime gameTime)
        {
            if(m_delay >= 0.0f)
            {
                m_delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
            //Console.WriteLine("OnUpdate"); 
            switch(m_currentSubstate)
            {
                case Substate.IDLE:
                {
                }
                break;
                case Substate.SWAPING:
                {
                    m_board.CalculateMatchingSymbols();
                    if(!m_board.AreAnyAnimationInProgress())
                    {
                        m_delay += 0.2f;
                        SwitchState(m_board.GetMatchingSymbolsAmount() > 0 ? Substate.WINING : Substate.SWAPING_BACK);
                    }
                }
                break;
                case Substate.SWAPING_BACK:
                {
                    if(!m_board.AreAnyAnimationInProgress())
                    {
                        m_board.SwapSymbols(m_swappedSymbols.one, m_swappedSymbols.two);
                        m_swipeBackEffect.Play();
                        SwitchState(Substate.IDLE);
                    }
                }
                break;
                case Substate.WINING:
                {
                    m_score += m_board.GetMatchingSymbolsAmount() * 100;
                    m_board.DestroyAllMatchingSymbols();
                    m_matchingEffect.Play();
                    if(!m_board.AreAnyAnimationInProgress())
                        SwitchState(Substate.IDLE);
                }
                break;
                default:
                {
                    break;
                }
            }

        }
        public override void OnEnter()
        {
            Console.WriteLine("OnEnter"); 
        }
        public override void OnQuit()
        {
            Console.WriteLine("OnQuit"); 
        }

        // input handling
        public override void OnKeyPressed (Keys pressedKey) 
        {            
            switch (pressedKey) {
                case Keys.Escape:
                    RequestGameExit();
                    break;
                default:
                    break;
            }
        }

        public override void OnMouseReleased (Vector2 mousePos, Vector2 movement) 
        {
            for(int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                for(int j = 0; j < Constants.BOARD_SIZE; j++)
                {
                    if(m_board.GetSymbolAtIndex(i,j).IsTouched(mousePos) && m_currentSubstate == Substate.IDLE)
                    {
                        if(m_board.IsAnySymbolSelected())
                        {
                            var selectedSymbolPos = m_board.GetSelectedSymbolPos();
                            bool canSwap = m_board.CanSwapWithSelectedSymbol(new Vector2(i,j));
                            m_board.StopSymbolSelection();
                            if(canSwap)
                            {
                                m_swappedSymbols = (selectedSymbolPos, new Vector2(i,j));
                                m_board.SwapSymbols(selectedSymbolPos, new Vector2(i,j));
                                m_swipeEffect.Play();
                                SwitchState(Substate.SWAPING);
                            }
                        }
                        else
                            m_board.SelectSymbolAtIndex(i,j);
                        Console.WriteLine("Symbol o nr [" + i.ToString() + "," + j.ToString() + "]\n\n");
                    }
                }
            }
        }
        
        public override void OnLoad(ContentManager content, GraphicsDevice graphics)
        {
            Console.WriteLine("OnLoad"); 
            font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            background = content.Load<Texture2D>(Constants.INGAME_BG);
            m_board = new Board(content, graphics);
            m_swipeEffect = content.Load<SoundEffect>(Constants.SWIPE_SOUND);
            m_swipeBackEffect = content.Load<SoundEffect>(Constants.SWIPE_BACK_SOUND);
            m_matchingEffect = content.Load<SoundEffect>(Constants.MATCHING_SOUND);
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            //Console.WriteLine("OnDraw"); 
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            m_board.Draw(ref spriteBatch);
            spriteBatch.DrawString(font, "Score: " + m_score.ToString(), new Vector2(10, 10), Color.Black);
        }  
    }
}