using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Cameras;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>CameraManager</c> class represents the manager that controls
    /// the current existing cameras in the game.
    /// </summary>
    class CameraManager : GameComponent
    {
        #region Fields

        /// <summary>
        /// Id of the default camera of the game.
        /// </summary>
        public const int DefaultCamera = 1;

        /// <summary>
        /// Hashtable with the game cameras.
        /// </summary>
        private static Hashtable _cameras = new Hashtable();

        /// <summary>
        /// true if the <c>CameraManager</c> is initialized, false otherwise.
        /// </summary>
        private static bool _initialized = false;

        /// <summary>
        /// Store for the ActiveCamera property.
        /// </summary>
        private static Camera _activeCamera;

        #endregion

        #region Properties

        /// <summary>
        /// ActiveCamera property
        /// </summary>
        /// <value>
        /// Reference to the active camera of the game.
        /// </value>
        public static Camera ActiveCamera
        {
            get { return _activeCamera; }
            set { _activeCamera = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>CameraManager</c> class.
        /// </summary>
        /// <param name="game"></param>
        public CameraManager(Game game) : base(game)
        {
            Enabled = true;
        }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Initialize the content of the <c>CameraManager</c>.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _initialized = true;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the cameras.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(_activeCamera != null)
                _activeCamera.Update();
        }

        #endregion

        #region AddCamera Method

        /// <summary>
        /// Add a new camera to the game.
        /// </summary>
        /// <param name="id">Id of the camera.</param>
        /// <param name="newCamera">Reference to the camera.</param>
        public static void AddCamera(int id, Camera newCamera)
        {
            if(!_cameras.Contains(id))
                _cameras.Add(id, newCamera);
        }

        #endregion

        #region SetAllCamerasProjectionMatrix Method

        /// <summary>
        /// Set the projection matrix of all the cameras.
        /// </summary>
        public static void SetAllCamerasProjectionMatrix()
        {
            foreach (Camera camera in _cameras.Values)
                camera.GeneratePerspectiveProjectionMatrix();
        }

        #endregion

        #region SetActiveCamera Method

        /// <summary>
        /// Set the active camera to use.
        /// </summary>
        /// <param name="id">If of the camera.</param>
        public static void SetActiveCamera(int id)
        {
            if (_cameras.ContainsKey(id))
                _activeCamera = _cameras[id] as Camera;
        }

        #endregion
    }
}
