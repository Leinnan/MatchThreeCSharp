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
        Vector2 m_selectedSymbol;
        
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
                    RequestStateChange("MenuState");
                    break;
                default:
                    break;
            }
        }

        public override void OnMouseReleased (Vector2 mousePos, Vector2 movement) 
        {
            Console.WriteLine("[MOVEMENT] " + movement.X + " " + movement.Y );

            if(m_currentSubstate != Substate.IDLE)
                return;

            var inputStartedSymbolIndex = m_board.GetSymbolIndexAtGfxPos(mousePos-movement);
            var inputEndedSymbolIndex = m_board.GetSymbolIndexAtGfxPos(mousePos);
            if(inputEndedSymbolIndex.X != -1)
            {
                if(m_board.IsAnySymbolSelected())
                {
                    var selectedSymbolPos = m_board.GetSelectedSymbolPos();
                    bool canSwap = m_board.CanSwapWithSelectedSymbol(inputEndedSymbolIndex);
                    m_board.StopSymbolSelection();
                    if(canSwap)
                    {
                        m_swappedSymbols = (selectedSymbolPos, inputEndedSymbolIndex);
                        m_board.SwapSymbols(selectedSymbolPos, inputEndedSymbolIndex);
                        m_swipeEffect.Play();
                        SwitchState(Substate.SWAPING);
                    }
                }
                else
                {
                    bool isGestureCorrect = false;
                    Vector2 symbolToSwap = inputStartedSymbolIndex;

                    if(movement.X >= ( Constants.SYMBOL_GFX_SIZE * 0.6f) ||
                        movement.Y >= ( Constants.SYMBOL_GFX_SIZE * 0.6f))
                    {
                        if( Math.Abs(movement.X) > Math.Abs(movement.Y) )
                        {
                            symbolToSwap.X += movement.X > 0 ? 1 : -1;
                        }
                        else
                        {
                            symbolToSwap.Y += movement.Y > 0 ? 1 : -1;
                        }
                        isGestureCorrect = (symbolToSwap.X >= 0 && symbolToSwap.X < Constants.BOARD_SIZE &&
                                            symbolToSwap.Y >= 0 && symbolToSwap.Y < Constants.BOARD_SIZE );
                    }
                    
                    if(isGestureCorrect)
                    {
                        m_swappedSymbols = (symbolToSwap, inputStartedSymbolIndex);
                        m_board.SwapSymbols(symbolToSwap, inputStartedSymbolIndex);
                        m_swipeEffect.Play();
                        SwitchState(Substate.SWAPING);
                    }
                    else
                        m_board.SelectSymbolAtIndex((int)inputEndedSymbolIndex.X, (int)inputEndedSymbolIndex.Y);
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