using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.GameScreens;

namespace MetalSpace.Objects
{
    /// <summary>
    /// The <c>Object</c> class represents an object in the player inventory.
    /// </summary>
    public class Object
    {
        #region Fields

        /// <summary>
        /// Store for the Position property.
        /// </summary>
        protected int _position;

        /// <summary>
        /// Store for the Name property.
        /// </summary>
        protected string _name;

        /// <summary>
        /// Store for the TextureName property.
        /// </summary>
        protected string _textureName;

        /// <summary>
        /// Store for the IsEquipped property.
        /// </summary>
        protected bool _isEquipped;

        #endregion

        #region Properties

        /// <summary>
        /// Position property
        /// </summary>
        /// <value>
        /// Position of the object in the inventory.
        /// </value>
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Name property
        /// </summary>
        /// <value>
        /// Name of the object.
        /// </value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// TextureName property
        /// </summary>
        /// <value>
        /// Name of the texture used in the model.
        /// </value>
        public string TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        /// <summary>
        /// IsEquipped property
        /// </summary>
        /// <value>
        /// true if the object is equipped, false otherwise.
        /// </value>
        public bool IsEquipped
        {
            get { return _isEquipped; }
            set { _isEquipped = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Object</c> class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        /// <param name="textureName">Name of the texture used in the object.</param>
        /// <param name="position">Position of the object in the inventory.</param>
        /// <param name="isEquipped">true if the object is equipped, false otherwise.</param>
        public Object(string name, string textureName, int position, bool isEquipped)
        {
            _position = position;

            _name = name;
            _textureName = textureName;

            _isEquipped = isEquipped;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load all needed content of the <c>Object</c>.
        /// </summary>
        public virtual void Load()
        {

        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload all needed content of the <c>Object</c>.
        /// </summary>
        public virtual void Unload()
        {

        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the <c>Object</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>Object</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public virtual void Draw(GameTime gameTime)
        {
            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            Rectangle destination = new Rectangle();
            destination.X = (int)((_position % 3) * ((viewportSize.X / 8f) * 0.875f) +
                (viewportSize.X * 0.19f / 8f) +
                (viewportSize.X / 2f) + InventoryScreen.HPad);
            destination.Y = (int)((int)(_position / 3) * ((viewportSize.X / 8f) * 0.87f) +
                (viewportSize.X * 0.2f / 8f) +
                (viewportSize.Y / 8f) + InventoryScreen.VPad);
            destination.Width = (int)(viewportSize.X * 0.65f / 8f);
            destination.Height = (int)(viewportSize.X * 0.6f / 8f);

            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(_textureName).BaseTexture as Texture2D,
                destination, new Color(255, 255, 255, 200));
        }

        #endregion
    }
}
