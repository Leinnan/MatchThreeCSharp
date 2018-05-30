using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGameCore {
    public abstract class State{
        public abstract string Id { get; }
        List<Keys> m_holdedKeys = new List<Keys> ();
        MouseState m_prevMouseState;
        private bool m_exitGameRequest = false;

        Vector2 m_pressedMousePos;
        Vector2 m_mousePos;

        public abstract void OnInit();
        public abstract void OnShutdown();
        public abstract void OnUpdate(GameTime gameTime);
        public abstract void OnEnter();
        public abstract void OnQuit();
        public abstract void OnLoad(ContentManager content, GraphicsDevice graphics);
        public abstract void OnUnload(ContentManager content, GraphicsDevice graphics);
        public abstract void OnDraw(ref SpriteBatch spriteBatch);

        // input handling
        public virtual void OnKeyPressed (Keys pressedKey) {}
        public virtual void OnKeyHold (Keys holdedKey) {}
        public virtual void OnKeyReleased (Keys releasedKey) {}
        public virtual void OnMousePressed (Vector2 mousePos) {}
        public virtual void OnMouseHold (Vector2 mousePos, Vector2 movement) {}
        public virtual void OnMouseReleased (Vector2 mousePos, Vector2 movement) {}

        public void HandleInput () {
            HandleKeyboard ();
            HandleMouse ();
        }
        public bool IsRequestingGameExit(){ return m_exitGameRequest; }
        public void RequestGameExit(){ m_exitGameRequest = true; }

        protected void HandleKeyboard () {
            List<Keys> pressedKeys = new List<Keys> (Keyboard.GetState ().GetPressedKeys ());

            foreach (var key in pressedKeys) {
                if (m_holdedKeys.Contains (key))
                    OnKeyHold (key);
                else {
                    OnKeyPressed (key);
                    m_holdedKeys.Add (key);
                }
            }

            foreach (var key in m_holdedKeys)
                if (!pressedKeys.Contains (key))
                    OnKeyReleased (key);

            m_holdedKeys.RemoveAll (key => !pressedKeys.Contains (key));
        }

        protected void HandleMouse () {
            MouseState currState = Mouse.GetState ();
            Vector2 movement = currState.Position.ToVector2 ();
            if (m_prevMouseState.LeftButton == ButtonState.Pressed &&
                currState.LeftButton == ButtonState.Pressed) {

                movement -= m_prevMouseState.Position.ToVector2 ();
                OnMouseHold (currState.Position.ToVector2 (), movement);
            } else if (m_prevMouseState.LeftButton == ButtonState.Pressed &&
                currState.LeftButton != ButtonState.Pressed) {
                movement -= m_pressedMousePos;
                OnMouseReleased (currState.Position.ToVector2 (), movement);
            } else if (currState.LeftButton == ButtonState.Pressed) {
                OnMousePressed (currState.Position.ToVector2 ());
            }
            m_prevMouseState = Mouse.GetState ();
        }
    }
}