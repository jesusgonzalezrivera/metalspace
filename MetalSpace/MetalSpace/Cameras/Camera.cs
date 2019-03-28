using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Settings;

namespace MetalSpace.Cameras
{
    /// <summary>
    /// The <c>Camera</c> abstract class represents a basic camera that only
    /// see to a objetive point.
    /// </summary>
    public abstract class Camera
    {
        #region Fields

        /// <summary>
        /// Store for the View property.
        /// </summary>
        private Matrix _view;

        /// <summary>
        /// Store for the Projection property.
        /// </summary>
        private Matrix _projection;

        /// <summary>
        /// Store of the FieldOfView property.
        /// </summary>
        private float _fieldOfView;

        /// <summary>
        /// Store for the Aspect Ratio property.
        /// </summary>
        private float _aspectRatio;

        /// <summary>
        /// Store for the NearPlane property.
        /// </summary>
        private float _nearPlane;

        /// <summary>
        /// Store for the FarPlane property.
        /// </summary>
        private float _farPlane;

        /// <summary>
        /// Store for the Position property.
        /// </summary>
        private Vector3 _position;

        /// <summary>
        /// Store for the Target property.
        /// </summary>
        private Vector3 _target;

        #endregion

        #region Properties

        /// <summary>
        /// View property
        /// </summary>
        /// <value>
        /// Matrix that rotate the world according to the camera in 3D.
        /// </value>
        public Matrix View
        {
            get { return _view; }
            set { _view = value; }
        }

        /// <summary>
        /// Projection property
        /// </summary>
        /// <value>
        /// Matrix that translate the 3D data to the 2D screen.
        /// </value>
        public Matrix Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        /// <summary>
        /// FieldOfView property
        /// </summary>
        /// <value>
        /// Field of view in radians of the camera.
        /// </value>
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set { _fieldOfView = value; }
        }

        /// <summary>
        /// AspectRatio property
        /// </summary>
        /// <value>
        /// Aspect ratio (4:3, 16:9) of the camera.
        /// </value>
        public float AspectRatio
        {
            get { return _aspectRatio; }
            set { _aspectRatio = value; }
        }

        /// <summary>
        /// NearPlane property
        /// </summary>
        /// <value>
        /// Near plane where the camera start recording.
        /// </value>
        public float NearPlane
        {
            get { return _nearPlane; }
            set { _nearPlane = value; }
        }

        /// <summary>
        /// FarPlane property
        /// </summary>
        /// <value>
        /// Far plane where the camera end recording.
        /// </value>
        public float FarPlane
        {
            get { return _farPlane; }
            set { _farPlane = value; }
        }

        /// <summary>
        /// Position property
        /// </summary>
        /// <value>
        /// Position of the camera.
        /// </value>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Target property
        /// </summary>
        /// <value>
        /// Target position where the camara is pointed.
        /// </value>
        public Vector3 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Empty constructor of the <c>Camera</c> class.
        /// </summary>
        public Camera()
        {
            _fieldOfView = MathHelper.ToRadians(45);
            _aspectRatio = GameSettings.DefaultInstance.ResolutionWidth /
                GameSettings.DefaultInstance.ResolutionHeight;

            _nearPlane = 0.01f;
            _farPlane = 3500f;

            _position = Vector3.Zero;
            _target = new Vector3(0, 10, 0);

            GeneratePerspectiveProjectionMatrix();
        }

        /// <summary>
        /// Parametrized constructor of the <c>Camera</c> class.
        /// </summary>
        /// <param name="position">Position of the camera.</param>
        /// <param name="target">Target of the camera.</param>
        public Camera(Vector3 position, Vector3 target)
        {
            _fieldOfView = MathHelper.ToRadians(45);
            _aspectRatio = GameSettings.DefaultInstance.ResolutionWidth /
                GameSettings.DefaultInstance.ResolutionHeight;

            _nearPlane = 0.01f;
            _farPlane = 3500f;

            _position = position;
            _target = target;

            GeneratePerspectiveProjectionMatrix();
        }

        #endregion

        #region GeneratePerspectiveProjectionMatrix Method

        /// <summary>
        /// Generate the projection matrix of the class with the parameters
        /// of the camera.
        /// </summary>
        public void GeneratePerspectiveProjectionMatrix()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
                _fieldOfView, _aspectRatio, _nearPlane, _farPlane);
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update method (empty in the default camera).
        /// </summary>
        public virtual void Update()
        {

        }

        #endregion
    }
}
