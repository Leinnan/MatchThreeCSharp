using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoGameCore {
    public class StateHandler{
        protected string _curState;
        List<State> _states = new List<State>();
        private bool _exitGameRequest = false;

        public StateHandler(){}

        ~StateHandler()
        {
            _states.Find(x => x.Id.Equals(_curState)).OnQuit();
            foreach (var state in _states)
            {
                state.OnShutdown();
            }
        }

        public void Update(GameTime gameTime)
        {
            // const bool contains = m_states.Contains(x => x.Id.Equals(m_curState));
            // Debug.Assert(contains, "There is no state with ID " + m_curState);
            _states.Find(x => x.Id.Equals(_curState)).HandleInput();
            _states.Find(x => x.Id.Equals(_curState)).OnUpdate(gameTime);

            if(_states.Find(x => x.Id.Equals(_curState)).IsRequestingGameExit())
                _exitGameRequest = true;
            else if (_states.Find(x => x.Id.Equals(_curState)).IsRequestingStateChange())
            {
                SwitchState(_states.Find(x => x.Id.Equals(_curState)).GetRequestedStateName());
            }
        }
        public void RegisterState(State newState)
        {
            // Debug.Assert(!m_states.Contains(x => x.Id.Equals(newState.Id)), "There is already state with ID " + newState.Id);
            _states.Add(newState);
            _states.Find(x => x.Id.Equals(newState.Id)).OnInit();
        }
        public void UnregisterState(string stateToRemove)
        {
            // Debug.Assert(m_states.Contains(x => x.Id.Equals(stateToRemove)), "There is no state with ID " + stateToRemove);
            _states.Find(x => x.Id.Equals(stateToRemove)).OnShutdown();
            _states.RemoveAll(x => x.Id.Equals(stateToRemove));
        }

        public void SwitchState( string newState )
        {
            _states.Find(x => x.Id.Equals(_curState)).OnQuit();
            _states.Find(x => x.Id.Equals(_curState)).ResetRequestStateChange();
            _states.Find(x => x.Id.Equals(newState)).OnEnter();
            _curState = newState;
        }

        public void Start( string startState )
        {
            _states.Find(x => x.Id.Equals(startState)).OnEnter();
            _curState = startState;
        }
        public void Draw(ref SpriteBatch spriteBatch)
        {
            _states.Find(x => x.Id.Equals(_curState)).OnDraw(ref spriteBatch);
        }

        public void LoadAssets(ContentManager content, GraphicsDevice graphics)
        {
            foreach(var state in _states)
            {
                state.OnLoad(content, graphics);
            }
        }
        public bool IsRequestingGameExit(){ return _exitGameRequest; }
    }
}