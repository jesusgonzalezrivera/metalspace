using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Events;
using MetalSpace.Textures;
using MetalSpace.Settings;
using MetalSpace.GameScreens;
using MetalSpace.GameComponents;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>MessageBoxScreen</c> class represents the message that the
    /// user wants to display, which can have with one menu entry or with
    /// two ones.
    /// </summary>
    class MessageBoxScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Const string that contains the name of the background texture.
        /// </summary>
        private const string texture = "DialogBackground";

        /// <summary>
        /// Horizontal pad.
        /// </summary>
        public const int HPad = 32;

        /// <summary>
        /// Vertical pad.
        /// </summary>
        public const int VPad = 16;

        /// <summary>
        /// true if the message box only have a message entry, false otherwise.
        /// </summary>
        private bool _oneOption;

        /// <summary>
        /// Message showed in the message box.
        /// </summary>
        private string _message;

        /// <summary>
        /// Selected option.
        /// </summary>
        private bool _selectedOption;

        /// <summary>
        /// Size of the text showed in the message box.
        /// </summary>
        private Vector2 _textSize;

        /// <summary>
        /// Size of the text of each menu entry.
        /// </summary>
        private Vector2 _shortTextSize;

        /// <summary>
        /// Position of the text showed in the message box.
        /// </summary>
        private Vector2 _textPosition;

        /// <summary>
        /// Position of the accept entry.
        /// </summary>
        private Vector2 _acceptPosition;

        /// <summary>
        /// Position of the cancel entry.
        /// </summary>
        private Vector2 _cancelPosition;

        public event EventHandler<EventArgs> Accepted;
        public event EventHandler<EventArgs> Cancelled;

        #endregion

        #region Constructor 

        /// <summary>
        /// Constructor of the <c>MessageBoxScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <param name="message">Message that appears in the screen.</param>
        public MessageBoxScreen(string name, string message)
            : this(name, message, true) { }

        /// <summary>
        /// Constructor of the <c>MessageBoxScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <param name="message">Message that appears in the screen.</param>
        /// <param name="oneOption">true if the message box constains one entry, false otherwise.</param>
        public MessageBoxScreen(string name, string message, bool oneOption = false)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            _oneOption = oneOption;

            _message = message;
            _selectedOption = false;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load and calculate all the necessary content of the game.
        /// </summary>
        public override void Load() 
        { 
            Vector2 viewportSize = new Vector2(
                EngineManager.GameGraphicsDevice.Viewport.Width, 
                EngineManager.GameGraphicsDevice.Viewport.Height);

            _textSize = ScreenManager.FontEntries.MeasureString(_message);
            _textPosition = new Vector2((viewportSize.X - _textSize.X) / 2, 
                (viewportSize.Y - _textSize.Y * 2) / 2);

            _shortTextSize = ScreenManager.FontEntries.MeasureString("accept");
            _shortTextSize.X += 10;

            if (_oneOption)
            {
                _acceptPosition = new Vector2(
                    (viewportSize.X - _shortTextSize.X) / 2, viewportSize.Y / 2);
            }
            else
            {
                _acceptPosition = new Vector2(
                    (viewportSize.X - _textSize.X) / 2, viewportSize.Y / 2);
                _cancelPosition = new Vector2(
                    (viewportSize.X + _textSize.X) / 2 - _shortTextSize.X, viewportSize.Y / 2);
            }
        }

        #endregion

        #region HandleInput Method

        /// <summary>
        /// Handle the input of the user relative to the <c>MessageBoxScreen</c> screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        /// <param name="gameTime">Current time of the game.</param>
        public override void HandleInput(Input input, GameTime gameTime)
        {
            if (input.Left)
            {
                _selectedOption = !_selectedOption;
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
            }

            if (input.Right)
            {
                _selectedOption = !_selectedOption;
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
            }

            if (input.Start || input.Action)
            {
                SoundManager.GetSound("MenuAccept").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuAccept").Play(true);
                if (_oneOption)
                {
                    // Raise the accepted event, then exit the message box.
                    if (Accepted != null)
                        Accepted(this, EventArgs.Empty);
                }
                else
                {
                    if (!_selectedOption)
                    {
                        // Raise the accepted event, then exit the message box.
                        if (Accepted != null)
                            Accepted(this, EventArgs.Empty);
                    }
                    else
                    {
                        // Raise the cancelled event, then exit the message box.
                        if (Cancelled != null)
                            Cancelled(this, EventArgs.Empty);
                    }
                }

                ExitScreen();
            }
            
            if (input.Select)
            {
                SoundManager.GetSound("MenuAccept").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuAccept").Play(true);

                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                    Cancelled(this, EventArgs.Empty);

                ExitScreen();
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the background screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            if (!TextureManager.GetTexture(texture).ReadyToRender)
                return;

            // Fade the popup alpha during transitions.
            Color color = new Color(255, 255, 255, TransitionAlpha);

            ScreenManager.SpriteBatch.Begin();

            // Draw the background rectangle
            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture(texture).BaseTexture as Texture2D,
                new Rectangle(
                    (int)_textPosition.X - HPad, (int)_textPosition.Y - VPad * 2,
                    (int)_textSize.X + HPad * 2, (int)_textSize.Y * 2 + VPad * 4), 
                color);

            // Draw the message
            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                _message, _textPosition, color);

            // Draw the options
            if (_oneOption)
            {
                ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                    StringHelper.DefaultInstance.Get("accept"), _acceptPosition, color);

                ScreenManager.SpriteBatch.Draw(
                    TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle((int)_acceptPosition.X, (int)_acceptPosition.Y,
                        (int)_shortTextSize.X, (int)_shortTextSize.Y),
                    Color.White);
            }
            else
            {
                ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                    StringHelper.DefaultInstance.Get("accept"), _acceptPosition, color);
                ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                    StringHelper.DefaultInstance.Get("cancel"), _cancelPosition, color);

                if (!_selectedOption)
                {
                    ScreenManager.SpriteBatch.Draw(
                        TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                        new Rectangle((int)_acceptPosition.X, (int)_acceptPosition.Y,
                            (int)_shortTextSize.X, (int)_shortTextSize.Y),
                        Color.White);
                }
                else
                {
                    ScreenManager.SpriteBatch.Draw(
                        TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                        new Rectangle((int)_cancelPosition.X, (int)_cancelPosition.Y,
                            (int)_shortTextSize.X, (int)_shortTextSize.Y),
                        Color.White);
                }
            }

            ScreenManager.SpriteBatch.End();
        }

        #endregion
    }
}