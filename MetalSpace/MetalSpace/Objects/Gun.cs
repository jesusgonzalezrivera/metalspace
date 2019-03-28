using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Objects;
using MetalSpace.Models;
using MetalSpace.Managers;
using MetalSpace.Interfaces;
using SkinnedModel;

namespace MetalSpace.Objects
{
    /// <summary>
    /// The <c>Gun</c> class represents an gun element that the player
    /// can handle in the game. There are two types of guns: normal and laser.
    /// </summary>
    public class Gun
    {
        #region Fields

        /// <summary>
        /// Type of the gun.
        /// </summary>
        public enum ShotType
        {
            /// <summary>
            /// Nomal ammo.
            /// </summary>
            Normal,
            /// <summary>
            /// Laser ammo.
            /// </summary>
            Laser
        }

        /// <summary>
        /// Store for the GunType property.
        /// </summary>
        protected ShotType _gunType;

        /// <summary>
        /// Store for the CurrentAmmo property.
        /// </summary>
        protected int _currentAmmo;

        /// <summary>
        /// Store for the MaxAmmo property.
        /// </summary>
        protected int _maxAmmo;

        /// <summary>
        /// Store for the Attack property.
        /// </summary>
        protected int _attack;

        protected Player _player;

        /// <summary>
        /// Store for the DModel property.
        /// </summary>
        protected DrawableModel _model;

        #endregion

        #region Properties

        /// <summary>
        /// DModel property
        /// </summary>
        /// <value>
        /// Drawable model that represents the gun.
        /// </value>
        public DrawableModel DModel
        {
            get { return _model; }
            set { _model = value; }
        }

        /// <summary>
        /// Position property
        /// </summary>
        /// <value>
        /// Position of the model.
        /// </value>
        public Vector3 Position
        {
            get { return _model.Position; }
            set { _model.Position = value; }
        }

        /// <summary>
        /// Rotation property
        /// </summary>
        /// <value>
        /// Rotation of the model.
        /// </value>
        public Vector3 Rotation
        {
            get { return _model.Rotation; }
            set { _model.Rotation = value; }
        }

        /// <summary>
        /// Scale property
        /// </summary>
        /// <value>
        /// Scale of the model.
        /// </value>
        public Vector3 Scale
        {
            get { return _model.Scale; }
            set { _model.Scale = value; }
        }

        /// <summary>
        /// GunType property
        /// </summary>
        /// <value>
        /// Type of the ammo fo the gun.
        /// </value>
        public ShotType GunType
        {
            get { return _gunType; }
            set { _gunType = value; }
        }

        /// <summary>
        /// Attack property
        /// </summary>
        /// <value>
        /// Attack made by the gun.
        /// </value>
        public int Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        /// <summary>
        /// CurrentAmmo property
        /// </summary>
        /// <value>
        /// Current ammo of the gun.
        /// </value>
        public int CurrentAmmo
        {
            get { return _currentAmmo; }
            set { _currentAmmo = value; }
        }

        /// <summary>
        /// MaxAmmo property
        /// </summary>
        /// <value>
        /// Max ammo stored in the gun.
        /// </value>
        public int MaxAmmo
        {
            get { return _maxAmmo; }
            set { _maxAmmo = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Gun</c> class.
        /// </summary>
        /// <param name="modelName">Name of the model that represents the gun.</param>
        /// <param name="gunType">Type of the ammo of the gun.</param>
        /// <param name="player">Reference to the player.</param>
        /// <param name="attack">Attack made by the gun.</param>
        /// <param name="maxAmmo">Max ammo stored by the gun.</param>
        public Gun(string modelName, ShotType gunType, Player player, int attack, int maxAmmo)
        {
            _player = player;

            _gunType = gunType;

            _attack = attack;
            _currentAmmo = maxAmmo;
            _maxAmmo = maxAmmo;

            _model = new DrawableModel((GameModel)ModelManager.GetModel(modelName),
                _player.Position, _player.Rotation, _player.Scale, 0);
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed content of the <c>Gun</c>.
        /// </summary>
        public void Load()
        {

        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the needed content of the <c>Gun</c>.
        /// </summary>
        public void Unload()
        {
            _model = null;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>Gun</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Update(GameTime gameTime)
        {
            Position = _player.Position;
            Rotation = _player.Rotation;
            Scale = _player.Scale;

            if (_player.PlayerState == Player.State.Jumping)
            {
                if (_player.PlayerXDirection == Character.XDirection.Left ||
                    (_player.PlayerXDirection == Character.XDirection.None &&
                    _player.LastPlayerXDirection == Character.XDirection.Left))
                {
                    Position += new Vector3(-0.5f, 1.25f, 0);
                }
                else if (_player.PlayerXDirection == Character.XDirection.Right ||
                    (_player.PlayerXDirection == Character.XDirection.None &&
                    _player.LastPlayerXDirection == Character.XDirection.Right))
                {
                    Position += new Vector3(0.5f, 1.25f, 0);
                }
            }
            else if (_player.PlayerState == Player.State.Climbing)
            {
                if (_player.PlayerXDirection == Character.XDirection.Left ||
                    (_player.PlayerXDirection == Character.XDirection.None &&
                    _player.LastPlayerXDirection == Character.XDirection.Left))
                {
                    Position += new Vector3(-0.35f, 2.15f, 0);
                    Rotation += new Vector3(MathHelper.ToRadians(90), MathHelper.ToRadians(-90), 0);
                }
                else if (_player.PlayerXDirection == Character.XDirection.Right ||
                    (_player.PlayerXDirection == Character.XDirection.None &&
                    _player.LastPlayerXDirection == Character.XDirection.Right))
                {
                    Position += new Vector3(0.35f, 2.15f, 0);
                    Rotation += new Vector3(MathHelper.ToRadians(90), MathHelper.ToRadians(-90), 0);
                }
            }
            else if (_player.PlayerXDirection == Character.XDirection.None &&
                _player.PlayerYDirection == Character.YDirection.Up)
            {
                Position += new Vector3(0, 2.15f, 0);
                Rotation += new Vector3(0, MathHelper.ToRadians(-90), 0);
            }
            else if (_player.PlayerXDirection == Character.XDirection.Left ||
                (_player.PlayerXDirection == Character.XDirection.None && 
                 _player.LastPlayerXDirection == Character.XDirection.Left))
            {
                if (_player.PlayerYDirection == Character.YDirection.Up)
                {
                    Position += new Vector3(-0.5f, 1.75f, 0);
                    Rotation += new Vector3(MathHelper.ToRadians(180), MathHelper.ToRadians(225), MathHelper.ToRadians(180));
                }
                else
                {
                    Position += new Vector3(-0.5f, 1.25f, 0);
                }
            }
            else if (_player.PlayerXDirection == Character.XDirection.Right ||
                (_player.PlayerXDirection == Character.XDirection.None && _player.LastPlayerXDirection == Character.XDirection.Right))
            {
                if (_player.PlayerYDirection == Character.YDirection.Up)
                {
                    Position += new Vector3(0.5f, 1.75f, 0);
                    Rotation += new Vector3(0, MathHelper.ToRadians(-45), 0);
                }
                else
                {
                    Position += new Vector3(0.5f, 1.25f, 0);
                }
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>Gun</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in _model.Model.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                        Matrix.CreateScale(Scale) *
                        Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) *
                        Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z));
                    effect.View = CameraManager.ActiveCamera.View;
                    effect.Projection = CameraManager.ActiveCamera.Projection;
                }

                mesh.Draw();
            }
        }

        #endregion
    }
}
