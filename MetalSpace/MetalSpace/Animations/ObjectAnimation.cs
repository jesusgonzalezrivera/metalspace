using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MetalSpace.Animations
{
    /// <summary>
    /// The <c>ObjectAnimation</c> class permits to create individual 
    /// animations for the differents meshes of a model, including 
    /// position, rotation and scalation with or without looping.
    /// </summary>
    class ObjectAnimation
    {
        #region Fields

        /// <summary>
        /// Store for the Position property.
        /// </summary>
        private Vector3 _position;

        /// <summary>
        /// Store for the StartPosition property.
        /// </summary>
        private Vector3 _startPosition;

        /// <summary>
        /// Store for the EndPosition property.
        /// </summary>
        private Vector3 _endPosition;

        /// <summary>
        /// Store for the Rotation property.
        /// </summary>
        private Vector3 _rotation;

        /// <summary>
        /// Store for the StartRotation property.
        /// </summary>
        private Vector3 _startRotation;

        /// <summary>
        /// Store for the EndRotation property.
        /// </summary>
        private Vector3 _endRotation;

        /// <summary>
        /// Store for the Scale property.
        /// </summary>
        private Vector3 _scale;

        /// <summary>
        /// Store for the StartScale property.
        /// </summary>
        private Vector3 _startScale;

        /// <summary>
        /// Store for the EndScale property.
        /// </summary>
        private Vector3 _endScale;

        /// <summary>
        /// Store for the Duration property.
        /// </summary>
        private TimeSpan _duration;

        /// <summary>
        /// Store for the ElapsedTime property.
        /// </summary>
        private TimeSpan _elapsedTime;

        /// <summary>
        /// Store for the Loop property.
        /// </summary>
        private bool _loop;

        #endregion

        #region Properties

        /// <summary>
        /// Position property
        /// </summary>
        /// <value>
        /// Return the current position of the model.
        /// </value>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// StartPosition property
        /// </summary>
        /// <value>
        /// Return the start position of the animation.
        /// </value>
        public Vector3 StartPosition
        {
            get { return _startPosition; }
            set { _startPosition = value; }
        }

        /// <summary>
        /// EndPosition property
        /// </summary>
        /// <value>
        /// Return the end position of the animation.
        /// </value>
        public Vector3 EndPosition
        {
            get { return _endPosition; }
            set { _endPosition = value; }
        }

        /// <summary>
        /// Rotation property
        /// </summary>
        /// <value>
        /// Return the current rotation of the model.
        /// </value>
        public Vector3 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// StartRotation property
        /// </summary>
        /// <value>
        /// Return the start rotation of the model.
        /// </value>
        public Vector3 StartRotation
        {
            get { return _startRotation; }
            set { _startRotation = value; }
        }

        /// <summary>
        /// EndRotation property
        /// </summary>
        /// <value>
        /// Return the finish rotation of the model.
        /// </value>
        public Vector3 EndRotation
        {
            get { return _endRotation; }
            set { _endRotation = value; }
        }

        /// <summary>
        /// Scale property
        /// </summary>
        /// <value>
        /// Return the current scale of the model.
        /// </value>
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// StartScale property
        /// </summary>
        /// <value>
        /// Return the start scale of the model.
        /// </value>
        public Vector3 StartScale
        {
            get { return _startScale; }
            set { _startScale = value; }
        }

        /// <summary>
        /// EndScale property
        /// </summary>
        /// <value>
        /// Return the end scale of the model.
        /// </value>
        public Vector3 EndScale
        {
            get { return _endScale; }
            set { _endScale = value; }
        }

        /// <summary>
        /// Duration property
        /// </summary>
        /// <value>
        /// Return the duration of the animation.
        /// </value>
        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        /// <summary>
        /// ElapsedTime property
        /// </summary>
        /// <value>
        /// Return the elapsed time of the animation.
        /// </value>
        public TimeSpan ElapsedTime
        {
            get { return _elapsedTime; }
            set { _elapsedTime = value; }
        }

        /// <summary>
        /// Loop property
        /// </summary>
        /// <value>
        /// True if it necessary repeat the animation and false otherwise.
        /// </value>
        public bool Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>ObjectAnimation</c> class in case of position
        /// animation of the model.
        /// </summary>
        /// <param name="startPosition">Start position of the model.</param>
        /// <param name="endPosition">End position of the model</param>
        /// <param name="duration">Duration of the animation.</param>
        /// <param name="loop">Specify if it is necessary repeat the animation</param>
        public ObjectAnimation(Vector3 startPosition, Vector3 endPosition,
            TimeSpan duration, bool loop)
        {
            _position = startPosition;
            _startPosition = startPosition;
            _endPosition = endPosition;

            _rotation = Vector3.Zero;
            _startRotation = Vector3.Zero;
            _endRotation = Vector3.Zero;

            _scale = Vector3.Zero;
            _startScale = Vector3.Zero;
            _endScale = Vector3.Zero;

            _duration = duration;
            _loop = loop;
        }

        /// <summary>
        /// Constructor of the <c>ObjectAnimation</c> class in case of position
        /// and rotation animation of the model.
        /// </summary>
        /// <param name="startPosition">Start position of the model.</param>
        /// <param name="endPosition">End position of the model.</param>
        /// <param name="startRotation">Start rotation of the model.</param>
        /// <param name="endRotation">End rotation of the model.</param>
        /// <param name="duration">Duration of the animation.</param>
        /// <param name="loop">Specify if it is necessary repeat the animation.</param>
        public ObjectAnimation(Vector3 startPosition, Vector3 endPosition,
            Vector3 startRotation, Vector3 endRotation, TimeSpan duration, bool loop)
        {
            _position = startPosition;
            _startPosition = startPosition;
            _endPosition = endPosition;

            _rotation = startRotation;
            _startRotation = startRotation;
            _endRotation = endRotation;

            _scale = Vector3.Zero;
            _startScale = Vector3.Zero;
            _endScale = Vector3.Zero;

            _duration = duration;
            _loop = loop;
        }

        /// <summary>
        /// Constructor of the <c>ObjectAnimation</c> class in case of position,
        /// rotation and scale animation of the model. 
        /// </summary>
        /// <param name="startPosition">Start position of the model.</param>
        /// <param name="endPosition">End position of the model.</param>
        /// <param name="startRotation">Start rotation of the model.</param>
        /// <param name="endRotation">End rotation of the model.</param>
        /// <param name="startScale">Start scale of the model.</param>
        /// <param name="endScale">End scale of the model.</param>
        /// <param name="duration">Duration of the animation.</param>
        /// <param name="loop">Specify if it is necessary repeat the animation.</param>
        public ObjectAnimation(Vector3 startPosition, Vector3 endPosition,
            Vector3 startRotation, Vector3 endRotation, Vector3 startScale,
            Vector3 endScale, TimeSpan duration, bool loop)
        {
            _position = startPosition;
            _startPosition = startPosition;
            _endPosition = endPosition;

            _rotation = startRotation;
            _startRotation = startRotation;
            _endRotation = endRotation;

            _scale = startScale;
            _startScale = startScale;
            _endScale = endScale;

            _duration = duration;
            _loop = loop;
        }

        #endregion

        #region Update

        /// <summary>
        /// Update the animation specified in the constructor.
        /// </summary>
        /// <param name="elapsed">Elapsed time between this and the last
        /// execution of the Update method.</param>
        public void Update(TimeSpan elapsed)
        {
            // Update the time
            _elapsedTime += elapsed;

            // Determine how far along the duration value we are (0 to 1)
            float amt = (float)_elapsedTime.TotalSeconds / (float)_duration.TotalSeconds;

            if (_loop)
                while (amt > 1)
                    amt -= 1;
            else
                amt = MathHelper.Clamp(amt, 0, 1);

            // Update the current position, rotation and scale
            _position = Vector3.Lerp(_startPosition, _endPosition, amt);
            _rotation = Vector3.Lerp(_startRotation, _endRotation, amt);
            _scale = Vector3.Lerp(_startScale, _endScale, amt);
        }

        #endregion
    }
}
