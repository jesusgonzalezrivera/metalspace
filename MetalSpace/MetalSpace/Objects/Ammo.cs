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
    /// The <c>Ammo</c> class represents an ammo element in the inventory
    /// of the player. There are two types of ammo: normal and laser.
    /// </summary>
    class Ammo : Objects.Object
    {
        #region Fields

        /// <summary>
        /// Store for the Amount property.
        /// </summary>
        private int _amount;

        /// <summary>
        /// Store for the Type property.
        /// </summary>
        private Objects.Gun.ShotType _type;
        
        #endregion

        #region Properties

        /// <summary>
        /// Amount property
        /// </summary>
        /// <value>
        /// Amount of ammo of the <c>Type</c>.
        /// </value>
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        /// <summary>
        /// Type property
        /// </summary>
        /// <value>
        /// Type of ammo (normal, ammo).
        /// </value>
        public Objects.Gun.ShotType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Ammo</c> class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        /// <param name="textureName">Name of the texture that represents the object.</param>
        /// <param name="position">Position in the inventory.</param>
        /// <param name="isEquipped">true if the element is equipped, false otherwise.</param>
        /// <param name="amount">Amount of ammo.</param>
        /// <param name="type">Type of the ammo.</param>
        public Ammo(string name, string textureName, int position, 
            bool isEquipped, int amount, Objects.Gun.ShotType type) :
            base(name, textureName, position, isEquipped)
        {
            _amount = amount;
            _type = type;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed content of the <c>Ammo</c> object.
        /// </summary>
        public override void  Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the neede content of the <c>Ammo</c> object.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>Ammo</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void  Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region Draw Method
        
        /// <summary>
        /// Draw the current state of the <c>Ammo</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void  Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            ScreenManager.SpriteBatch.DrawString(
                ScreenManager.FontEntriesSelected, _amount.ToString(), 
                new Vector2(
                    (int)((_position % 3) * ((viewportSize.X / 8f) * 0.875f) +
                    (viewportSize.X * 0.19f / 8f) +
                    (viewportSize.X / 2f) + InventoryScreen.HPad),
                    (int)((int)(_position / 3) * ((viewportSize.X / 8f) * 0.87f) +
                    (viewportSize.X * 0.6f / 8f) +
                    (viewportSize.Y / 8f) + InventoryScreen.VPad)),
                Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
        }

        #endregion
    }
}
