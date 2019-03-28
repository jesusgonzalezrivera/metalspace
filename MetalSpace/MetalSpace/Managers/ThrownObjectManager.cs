using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Models;
using MetalSpace.Scene;
using MetalSpace.Events;
using MetalSpace.Objects;
using MetalSpace.Settings;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>ThrownObjectManager</c> represents the class that permits to control
    /// state of the diferent objects thrown by the user.
    /// </summary>
    class ThrownObjectManager
    {
        #region Fields

        /// <summary>
        /// Reference to the scene where the player moves.
        /// </summary>
        private SceneRenderer _scene;

        /// <summary>
        /// Reference to the main player.
        /// </summary>
        private Player _player;

        /// <summary>
        /// Store for the ThrownObjects property.
        /// </summary>
        private List<MoveableObject> _thrownObjects;

        #endregion

        #region Properties

        /// <summary>
        /// ThrownObjects property
        /// </summary>
        /// <value>
        /// List of thrown objects.
        /// </value>
        public List<MoveableObject> ThrownObjects
        {
            get { return _thrownObjects; }
            set { _thrownObjects = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>ThrownObjectManager</c> class.
        /// </summary>
        /// <param name="scene">Reference to the scene where the player moves.</param>
        /// <param name="player">Reference to the main player.</param>
        public ThrownObjectManager(SceneRenderer scene, Player player)
        {
            _thrownObjects = new List<MoveableObject>();

            _scene = scene;
            _player = player;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the content of the <c>ThrownObjectManager</c>.
        /// </summary>
        public void Load()
        {
            
        }

        #endregion

        #region AddObject Method

        /// <summary>
        /// Add a new object to the <c>ThrownObjectManager</c>.
        /// </summary>
        /// <param name="thrownObject">Reference to the object.</param>
        public void AddObject(MoveableObject thrownObject)
        {
            thrownObject.DModel.BSphere = ((DrawableModel)thrownObject.DModel).GetBoundingSphere();
            _thrownObjects.Add(thrownObject);
        }

        #endregion

        #region RemoveObject Method

        /// <summary>
        /// Remove an object from the <c>ThrownObjectManager</c>.
        /// </summary>
        /// <param name="thrownObject">Reference to the object.</param>
        public void RemoveObject(MoveableObject thrownObject)
        {
            if (_thrownObjects.Contains(thrownObject))
                _thrownObjects.Remove(thrownObject);
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the thrown objects.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Update(GameTime gameTime)
        {
            foreach (MoveableObject thrownObject in _thrownObjects)
                thrownObject.Rotation += new Vector3((float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f), 0, 0);

            // Check collisions with the player
            List<MoveableObject> aux = new List<MoveableObject>();
            foreach (MoveableObject thrownObject in _thrownObjects)
            {
                if (thrownObject.DModel.BSphere.Intersects(_player.DModel.BSphere))
                {
                    EventManager.Trigger(new EventData_PickedObject(_player, thrownObject));

                    SoundManager.GetSound("PickedObject").Volume = GameSettings.DefaultInstance.SoundVolume;
                    SoundManager.GetSound("PickedObject").Play(true, false);
                }
                else
                    aux.Add(thrownObject);
            }

            _thrownObjects = aux;
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the thrown objects.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Draw(GameTime gameTime)
        {
            foreach (MoveableObject thrownObject in _thrownObjects)
                thrownObject.Draw(gameTime);
        }

        #endregion
    }
}
