using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Settings;

namespace MetalSpace.Objects
{
    /// <summary>
    /// The <c>Weapon</c> class represents an weapon element in the inventory
    /// of the player. There are two types of weapons: normal and laser.
    /// </summary>
    class Weapon : Object
    {
        #region Fields

        /// <summary>
        /// Store for the CurrentAmmo property.
        /// </summary>
        private int _currentAmmo;

        /// <summary>
        /// Store for the TotalAmmo property.
        /// </summary>
        private int _totalAmmo;

        /// <summary>
        /// Store for the Power property.
        /// </summary>
        private int _power;

        /// <summary>
        /// Store for the Type property.
        /// </summary>
        private Gun.ShotType _type;

        #endregion

        #region Properties

        /// <summary>
        /// CurrentAmmo property
        /// </summary>
        /// <value>
        /// Current amount of ammo in the weapon.
        /// </value>
        public int CurrentAmmo
        {
            get { return _currentAmmo; }
            set { _currentAmmo = value; }
        }

        /// <summary>
        /// TotalAmmo property
        /// </summary>
        /// <value>
        /// Total amount of ammo that the weapon have.
        /// </value>
        public int TotalAmmo
        {
            get { return _totalAmmo; }
            set { _totalAmmo = value; }
        }

        /// <summary>
        /// Power property
        /// </summary>
        /// <value>
        /// Damage that the weapon can do.
        /// </value>
        public int Power
        {
            get { return _power; }
            set { _power = value; }
        }

        /// <summary>
        /// Type property
        /// </summary>
        /// <value>
        /// Type of ammo in the weapon.
        /// </value>
        public Gun.ShotType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Weapon</c> class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        /// <param name="textureName">Name of the texture that represents the object.</param>
        /// <param name="position">Position in the inventory.</param>
        /// <param name="isEquipped">true if the element is equipped, false otherwise.</param>
        /// <param name="type">Type of the weapon.</param>
        /// /// <param name="power">Power of the weapon.</param>
        /// <param name="totalAmmo">Total amount of ammo.</param>
        public Weapon(string name, string textureName, int position, 
            bool isEquipped, Gun.ShotType type, int power, int totalAmmo) :
            base(name, textureName, position, isEquipped)
        {
            _power = power;
            _type = type;
            _currentAmmo = totalAmmo;
            _totalAmmo = totalAmmo;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed content of the <c>Weapon</c>.
        /// </summary>
        public override void Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the needed content of the <c>Weapon</c>.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>Weapon</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>Weapon</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            if (_isEquipped)
            {
                Rectangle destination = new Rectangle();
                destination.X = (int)(viewportSize.X * 3.125f / 8f);
                destination.Y = (int)(viewportSize.Y * 1.41f / 8f);
                destination.Width = (int)(viewportSize.X * 0.7f / 8f);
                destination.Height = (int)(viewportSize.Y * 2.27f / 8f);

                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(_textureName).BaseTexture as Texture2D,
                    destination, new Color(255, 255, 255, 200));
            }
            else
                base.Draw(gameTime);
        }

        #endregion
    }
}
