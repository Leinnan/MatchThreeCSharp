using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGameCore {
    public abstract class State{
        public abstract string Id { get; }
        private List<Keys> _holdedKeys = new List<Keys> ();
        private MouseState _prevMouseState;
        private bool _exitGameRequest = false;
        private string _requestedStateChange = "";
        private bool _inputEnabled = true;

        Vector2 _pressedMousePos;

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
            if(!_inputEnabled)
                return;
            HandleKeyboard ();
            HandleMouse ();
        }
        public bool IsRequestingGameExit(){ return _exitGameRequest; }
        public void RequestGameExit(){ _exitGameRequest = true; }

        public void ResetRequestStateChange()
        {
            _requestedStateChange = "";
        }

        public bool IsRequestingStateChange()
        {
            return _requestedStateChange.Length > 0;
        }
        public string GetRequestedStateName()
        {
            return _requestedStateChange;
        }

        public void RequestStateChange(string name)
        {
            _requestedStateChange = name;
            
        }
        public bool IsInputEnabled(){ return _inputEnabled; }
        public void EnableInput(){ _inputEnabled = true; }
        public void DisableInput(){ _inputEnabled = false; }

        protected void HandleKeyboard () {
            List<Keys> pressedKeys = new List<Keys> (Keyboard.GetState ().GetPressedKeys ());

            foreach (var key in pressedKeys) {
                if (_holdedKeys.Contains (key))
                    OnKeyHold (key);
                else {
                    OnKeyPressed (key);
                    _holdedKeys.Add (key);
                }
            }

            foreach (var key in _holdedKeys)
                if (!pressedKeys.Contains (key))
                    OnKeyReleased (key);

            _holdedKeys.RemoveAll (key => !pressedKeys.Contains (key));
        }

        protected void HandleMouse () {
            MouseState currState = Mouse.GetState ();
            Vector2 movement = currState.Position.ToVector2 ();
            if (_prevMouseState.LeftButton == ButtonState.Pressed &&
                currState.LeftButton == ButtonState.Pressed) {

                movement -= _prevMouseState.Position.ToVector2 ();
                OnMouseHold (currState.Position.ToVector2 (), movement);
            } else if (_prevMouseState.LeftButton == ButtonState.Pressed &&
                currState.LeftButton != ButtonState.Pressed) {
                movement -= _pressedMousePos;
                OnMouseReleased (currState.Position.ToVector2 (), movement);
            } else if (currState.LeftButton == ButtonState.Pressed) {
                OnMousePressed (currState.Position.ToVector2 ());
                _pressedMousePos = currState.Position.ToVector2 ();
            }
            _prevMouseState = Mouse.GetState ();
        }
    }
}