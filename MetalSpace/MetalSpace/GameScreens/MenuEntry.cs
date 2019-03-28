using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>MenuEntry</c> class represents an individual entry of the main 
    /// menu of the game.
    /// </summary>
    class MenuEntry
    {
        #region Fields

        /// <summary>
        /// Store for the Text property.
        /// </summary>
        private string _text;

        /// <summary>
        /// Store for the SelectionFace property.
        /// </summary>
        private float _selectionFade;

        #endregion

        #region Properties 

        /// <summary>
        /// Text property
        /// </summary>
        /// <value>
        /// Text that represents the entry.
        /// </value>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// SelectionFade property
        /// </summary>
        /// <value>
        /// Position in the animation of the text.
        /// </value>
        public float SelectionFade
        {
            get { return _selectionFade; }
            set { _selectionFade = value; }
        }

        #endregion
        
        #region Constructor

        /// <summary>
        /// Constructor of the <c>MenuEntry</c> class.
        /// </summary>
        /// <param name="text">Text that represents the entry.</param>
        public MenuEntry(string text)
        {
            _text = text;
        }

        #endregion

        public event EventHandler<EventArgs> Selected;

        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, EventArgs.Empty);
        }

        #region Update Method

        /// <summary>
        /// Update the state of the menu entry.
        /// </summary>
        /// <param name="screen">Reference to the screen where the drawing is done.</param>
        /// <param name="isSelected">true if the current entry is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        public virtual void Update(GameScreen screen, bool isSelected,
                                   GameTime gametime)
        {
            float fadeSpeed = (float)gametime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                SelectionFade = Math.Min(SelectionFade + fadeSpeed, 1);
            else
                SelectionFade = Math.Max(SelectionFade - fadeSpeed, 0);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the menu entry on the screen.
        /// </summary>
        /// <param name="screen">Reference to the screen where the drawing is done.</param>
        /// <param name="position">Position of the screen where the option is drawed.</param>
        /// <param name="isSelected">true if the current option is selected, false otherwise.</param>
        /// <param name="gameTime">Global time of the game.</param>
        public virtual void Draw(GameScreen screen, Vector2 position, 
                                 bool isSelected, GameTime gameTime)
        {
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * _selectionFade;

            Color color = isSelected ? Color.White : Color.LightSlateGray;
            color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

            if(isSelected)
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SelectedMenuEntry").BaseTexture,
                    new Rectangle((int)position.X - 50, (int)position.Y - 5, 50, 50), Color.White);

            ScreenManager.SpriteBatch.DrawString(isSelected ? ScreenManager.FontEntriesSelected : ScreenManager.FontEntries, 
                _text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        #endregion

        #region GetHeight Method

        /// <summary>
        /// Gets the height of the menu entry.
        /// </summary>
        /// <param name="screen">Reference to the menu screen where the drawing is done.</param>
        /// <returns></returns>
        public virtual int GetHeight(MenuScreen screen)
        {
            return ScreenManager.FontEntries.LineSpacing;
        }

        #endregion
    }
}
