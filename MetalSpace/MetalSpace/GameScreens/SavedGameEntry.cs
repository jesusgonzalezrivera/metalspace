using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Settings;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>SavedGameEntry</c> class represents an individual saved game
    /// in the <c>LoadGameScreen</c>. Each saved game shows the name of the
    /// map, the percent completed in the game, the date when the saved game
    /// was created and a capturedImage.
    /// </summary>
    class SavedGameEntry
    {
        #region Fields

        /// <summary>
        /// Store for the Name property.
        /// </summary>
        private string _name;

        /// <summary>
        /// Store for the Percent property.
        /// </summary>
        private float _percent;

        /// <summary>
        /// Store for the Date property.
        /// </summary>
        private string _date;

        /// <summary>
        /// Store for the CapturedImage property.
        /// </summary>
        private Texture2D _capturedImage;

        #endregion

        #region Properties

        /// <summary>
        /// Name property
        /// </summary>
        /// <value>
        /// Name of the map where the player moves.
        /// </value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Percent property
        /// </summary>
        /// <value>
        /// Percent of the game completed for the player.
        /// </value>
        public float Percent
        {
            get { return _percent; }
            set { _percent = value; }
        }

        /// <summary>
        /// Date property
        /// </summary>
        /// <value>
        /// Date when the saved game was created.
        /// </value>
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// CapturedImage property
        /// </summary>
        /// <value>
        /// CapturedImage of the game when the saved game was created.
        /// </value>
        public Texture2D CapturedImage
        {
            get { return _capturedImage; }
            set { _capturedImage = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>SavedGameEntry</c> class.
        /// </summary>
        /// <param name="values">Values of the saved game.</param>
        public SavedGameEntry(SavedGame values)
        {
            _name = values.MapName;
            _percent = values.Percent;
            _date = values.Date;
            _capturedImage = values.CapturedImage;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the game.
        /// </summary>
        /// <param name="screen">Reference of the screen where the saved game is ubicated.</param>
        /// <param name="isSelected">true if the current saved game is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        public void Update(LoadGameScreen screen, bool isSelected, GameTime gametime)
        {
            
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the game.
        /// </summary>
        /// <param name="screen">Reference of the screen where the saved game is ubicated.</param>
        /// <param name="position">Position of the saved game in the screen.</param>
        /// <param name="isSelected">true if the current saved game is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        public void Draw(LoadGameScreen screen, Vector2 position, bool isSelected, GameTime gameTime)
        {
            Color color = isSelected ? Color.White : Color.DimGray;
            color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

            Vector2 size = ScreenManager.FontEntries.MeasureString(_name);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected, _name, 
                new Vector2(position.X + 10, position.Y + 5), color, 0, Vector2.Zero, 0.75f,
                SpriteEffects.None, 0);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected, (_percent * 100).ToString() + "%",
                new Vector2(position.X + 10, position.Y + size.Y + 5*2), color, 0, Vector2.Zero, 0.75f,
                SpriteEffects.None, 0);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected, _date,
                new Vector2(position.X + 10, position.Y + size.Y * 2 + 5*3), color, 0, Vector2.Zero, 0.75f,
                SpriteEffects.None, 0);

            int xPosition = GameSettings.DefaultInstance.ResolutionWidth / 2;
            ScreenManager.SpriteBatch.Draw(
                _capturedImage, new Rectangle(xPosition-85, (int)position.Y+6, 130, 117),
                new Rectangle(0, 0, _capturedImage.Width, _capturedImage.Height),
                isSelected ? Color.White : Color.DarkGray);
        }

        #endregion
    }
}
