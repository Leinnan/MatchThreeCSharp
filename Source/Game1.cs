using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoGameCore {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;

        List<Keys> m_holdedKeys = new List<Keys> ();
        MouseState m_prevMouseState;

        Vector2 m_pressedMousePos;
        Vector2 m_mousePos;
        public Game1 () {
            graphics = new GraphicsDeviceManager (this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize () {
            m_prevMouseState = Mouse.GetState ();
            base.Initialize ();
        }

        protected override void LoadContent () {
            spriteBatch = new SpriteBatch (GraphicsDevice);
        }

        protected override void Update (GameTime gameTime) {
            // TODO: Add your update logic here
            HandleInput ();
            base.Update (gameTime);
        }

        protected override void Draw (GameTime gameTime) {
            GraphicsDevice.Clear (Color.Gray);
            spriteBatch.Begin ();
            spriteBatch.End ();
            base.Draw (gameTime);
        }

        protected void HandleInput () {
            HandleKeyboard ();
            HandleMouse ();
        }

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

        protected void OnKeyPressed (Keys pressedKey) {
            switch (pressedKey) {
                case Keys.Escape:
                    Exit ();
                    break;
                default:
                    break;

            }
        }

        protected void OnKeyHold (Keys holdedKey) {
            switch (holdedKey) {
                default:
                    break;
            }
        }

        protected void OnKeyReleased (Keys releasedKey) {

        }

        protected void OnMousePressed (Vector2 mousePos) {
            m_pressedMousePos = mousePos;
        }

        protected void OnMouseHold (Vector2 mousePos, Vector2 movement) {

        }

        protected void OnMouseReleased (Vector2 mousePos, Vector2 movement) {

        }

    }
}