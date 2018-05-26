using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoGameCore {
    public class StateHandler{
        protected string m_curState;
        List<State> m_states;

        public StateHandler(){}

        void Update(GameTime gameTime)
        {
            // const bool contains = m_states.Contains(x => x.Id.Equals(m_curState));
            // Debug.Assert(contains, "There is no state with ID " + m_curState);
            
            m_states.Find(x => x.Id.Equals(m_curState)).HandleInput();
            m_states.Find(x => x.Id.Equals(m_curState)).OnUpdate(gameTime);
        }
        void RegisterState(State newState)
        {
            // Debug.Assert(!m_states.Contains(x => x.Id.Equals(newState.Id)), "There is already state with ID " + newState.Id);
            m_states.Add(newState);
            m_states.Find(x => x.Id.Equals(newState.Id)).OnInit();
        }
        void UnregisterState(string stateToRemove)
        {
            // Debug.Assert(m_states.Contains(x => x.Id.Equals(stateToRemove)), "There is no state with ID " + stateToRemove);
            m_states.Find(x => x.Id.Equals(stateToRemove)).OnShutdown();
            m_states.RemoveAll(x => x.Id.Equals(stateToRemove));
        }

        void SwitchState( string newState )
        {
            m_states.Find(x => x.Id.Equals(m_curState)).OnQuit();
            m_states.Find(x => x.Id.Equals(newState)).OnEnter();
            m_curState = newState;
        }

        void Start( string startState )
        {
            m_states.Find(x => x.Id.Equals(startState)).OnEnter();
            m_curState = startState;
        }
    }
}