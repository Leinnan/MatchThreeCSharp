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

        private SpriteFont _font;
        private Texture2D _background;
        private Board _board;
        private int _score;
        private Substate _currentSubstate = Substate.IDLE;
        private SoundEffect _swipeEffect;
        private SoundEffect _swipeBackEffect;
        private SoundEffect _matchingEffect;
        private float _delay = 0.0f; 
        private (Vector2 one, Vector2 two) _swappedSymbols;
        
        public GameState(){}

        public void SwitchState( Substate newSubstate ){ _currentSubstate = newSubstate; }

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
            if(_delay >= 0.0f)
            {
                _delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
            //Console.WriteLine("OnUpdate"); 
            switch(_currentSubstate)
            {
                case Substate.IDLE:
                {
                }
                break;
                case Substate.SWAPING:
                {
                    _board.CalculateMatchingSymbols();
                    if(!_board.AreAnyAnimationInProgress())
                    {
                        _delay += 0.2f;
                        SwitchState(_board.GetMatchingSymbolsAmount() > 0 ? Substate.WINING : Substate.SWAPING_BACK);
                    }
                }
                break;
                case Substate.SWAPING_BACK:
                {
                    if(!_board.AreAnyAnimationInProgress())
                    {
                        _board.SwapSymbols(_swappedSymbols.one, _swappedSymbols.two);
                        _swipeBackEffect.Play();
                        SwitchState(Substate.IDLE);
                    }
                }
                break;
                case Substate.WINING:
                {
                    _score += _board.GetMatchingSymbolsAmount() * 100;
                    _board.DestroyAllMatchingSymbols();
                    _matchingEffect.Play();
                    if(!_board.AreAnyAnimationInProgress())
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
            _delay = 0.5f;
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

            if(_currentSubstate != Substate.IDLE)
                return;

            var inputStartedSymbolIndex = _board.GetSymbolIndexAtGfxPos(mousePos-movement);
            var inputEndedSymbolIndex = _board.GetSymbolIndexAtGfxPos(mousePos);
            if(inputEndedSymbolIndex.X != -1)
            {
                if(_board.IsAnySymbolSelected())
                {
                    var selectedSymbolPos = _board.GetSelectedSymbolPos();
                    bool canSwap = _board.CanSwapWithSelectedSymbol(inputEndedSymbolIndex);
                    _board.StopSymbolSelection();
                    if(canSwap)
                    {
                        _swappedSymbols = (selectedSymbolPos, inputEndedSymbolIndex);
                        _board.SwapSymbols(selectedSymbolPos, inputEndedSymbolIndex);
                        _swipeEffect.Play();
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
                        _swappedSymbols = (symbolToSwap, inputStartedSymbolIndex);
                        _board.SwapSymbols(symbolToSwap, inputStartedSymbolIndex);
                        _swipeEffect.Play();
                        SwitchState(Substate.SWAPING);
                    }
                    else
                        _board.SelectSymbolAtIndex((int)inputEndedSymbolIndex.X, (int)inputEndedSymbolIndex.Y);
                }
            }
        }
        
        public override void OnLoad(ContentManager content, GraphicsDevice graphics)
        {
            Console.WriteLine("OnLoad"); 
            _font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            _background = content.Load<Texture2D>(Constants.INGAME_BG);
            _board = new Board(content, graphics);
            _swipeEffect = content.Load<SoundEffect>(Constants.SWIPE_SOUND);
            _swipeBackEffect = content.Load<SoundEffect>(Constants.SWIPE_BACK_SOUND);
            _matchingEffect = content.Load<SoundEffect>(Constants.MATCHING_SOUND);
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            //Console.WriteLine("OnDraw"); 
            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            _board.Draw(ref spriteBatch);
            spriteBatch.DrawString(_font, "Score: " + _score.ToString(), new Vector2(10, 10), Color.Black);
        }  
    }
}