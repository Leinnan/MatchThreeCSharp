using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoGameCore {
    public class StateHandler{
        protected string m_curState;
        List<State> m_states = new List<State>();
        private bool m_exitGameRequest = false;

        public StateHandler(){}

        ~StateHandler()
        {
            m_states.Find(x => x.Id.Equals(m_curState)).OnQuit();
            foreach (var state in m_states)
            {
                state.OnShutdown();
            }
        }

        public void Update(GameTime gameTime)
        {
            // const bool contains = m_states.Contains(x => x.Id.Equals(m_curState));
            // Debug.Assert(contains, "There is no state with ID " + m_curState);
            m_states.Find(x => x.Id.Equals(m_curState)).HandleInput();
            m_states.Find(x => x.Id.Equals(m_curState)).OnUpdate(gameTime);

            if(m_states.Find(x => x.Id.Equals(m_curState)).IsRequestingGameExit())
                m_exitGameRequest = true;
            else if (m_states.Find(x => x.Id.Equals(m_curState)).IsRequestingStateChange())
            {
                SwitchState(m_states.Find(x => x.Id.Equals(m_curState)).GetRequestedStateName());
            }
        }
        public void RegisterState(State newState)
        {
            // Debug.Assert(!m_states.Contains(x => x.Id.Equals(newState.Id)), "There is already state with ID " + newState.Id);
            m_states.Add(newState);
            m_states.Find(x => x.Id.Equals(newState.Id)).OnInit();
        }
        public void UnregisterState(string stateToRemove)
        {
            // Debug.Assert(m_states.Contains(x => x.Id.Equals(stateToRemove)), "There is no state with ID " + stateToRemove);
            m_states.Find(x => x.Id.Equals(stateToRemove)).OnShutdown();
            m_states.RemoveAll(x => x.Id.Equals(stateToRemove));
        }

        public void SwitchState( string newState )
        {
            m_states.Find(x => x.Id.Equals(m_curState)).OnQuit();
            m_states.Find(x => x.Id.Equals(newState)).OnEnter();
            m_curState = newState;
        }

        public void Start( string startState )
        {
            m_states.Find(x => x.Id.Equals(startState)).OnEnter();
            m_curState = startState;
        }
        public void Draw(ref SpriteBatch spriteBatch)
        {
            m_states.Find(x => x.Id.Equals(m_curState)).OnDraw(ref spriteBatch);
        }

        public void LoadAssets(ContentManager content, GraphicsDevice graphics)
        {
            foreach(var state in m_states)
            {
                state.OnLoad(content, graphics);
            }
        }
        public bool IsRequestingGameExit(){ return m_exitGameRequest; }
    }
}