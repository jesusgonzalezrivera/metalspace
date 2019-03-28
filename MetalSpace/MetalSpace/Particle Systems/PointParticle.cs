using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MetalSpace.Managers;
using DPSF;

namespace MetalSpace.Particle_Systems
{
    /// <summary>
    /// The <c>PointParticle</c> class represents the group of points that
    /// leaves the enemy's body when it dies.
    /// </summary>
    class PointParticle : DefaultTexturedQuadParticleSystem
    {
        #region Fields

        /// <summary>
        /// Total time in seconds.
        /// </summary>
        private const float TotalTimeInSeconds = 5.0f;

        /// <summary>
        /// Total time that the points exist.
        /// </summary>
        private const float MaxParticleLifetime = TotalTimeInSeconds - 0.5f;
        
        /// <summary>
        /// Store for the IsComplete property.
        /// </summary>
        private bool _isComplete;

        /// <summary>
        /// Min distance of the points.
        /// </summary>
        private float _minDistance;

        /// <summary>
        /// Max distance of the points to be attracted.
        /// </summary>
        private float _maxDistance;

        /// <summary>
        /// Store for the MagnetsForce property.
        /// </summary>
        private float _magnetsForce;

        /// <summary>
        /// Store for the DestinationPosition property.
        /// </summary>
        private Vector3 _destinationPosition;

        /// <summary>
        /// Store for the MagnetsMode property.
        /// </summary>
        private DefaultParticleSystemMagnet.MagnetModes _magnetsMode;

        /// <summary>
        /// Store for the MagnetsDistanceFunction property.
        /// </summary>
        private DefaultParticleSystemMagnet.DistanceFunctions _magnetsDistanceFunction;

        #endregion

        #region Properties

        /// <summary>
        /// Position of the emitter (position of the enemy when it dies).
        /// </summary>
        public Vector3 EmitterPosition
        {
            get { return Emitter.PositionData.Position; }
            set { Emitter.PositionData.Position = value; }
        }

        /// <summary>
        /// MagnetsForce property
        /// </summary>
        /// <value>
        /// Force of the player to attract the points.
        /// </value>
        public float MagnetsForce
        {
            get { return _magnetsForce; }
            set
            {
                _magnetsForce = value;

                foreach (DefaultParticleSystemMagnet cMagnet in MagnetList)
                    cMagnet.MaxForce = _magnetsForce;
            }
        }

        /// <summary>
        /// MagnetsDistanceFunction property
        /// </summary>
        /// <value>
        /// Function to used for the distance functions.
        /// </value>
        public DefaultParticleSystemMagnet.DistanceFunctions MagnetsDistanceFunction
        {
            get { return _magnetsDistanceFunction; }
            set
            {
                _magnetsDistanceFunction = value;

                foreach (DefaultParticleSystemMagnet cMagnet in MagnetList)
                    cMagnet.DistanceFunction = _magnetsDistanceFunction;
            }
        }

        /// <summary>
        /// MagnetsMode property
        /// </summary>
        /// <value>
        /// Mode of the magnets force.
        /// </value>
        public DefaultParticleSystemMagnet.MagnetModes MagnetsMode
        {
            get { return _magnetsMode; }
            set
            {
                _magnetsMode = value;

                foreach (DefaultParticleSystemMagnet cMagnet in MagnetList)
                    cMagnet.Mode = _magnetsMode;
            }
        }

        /// <summary>
        /// DestinationPosition property
        /// </summary>
        /// <value>
        /// Position of the player as he moves.
        /// </value>
        public Vector3 DestionationPosition
        {
            get { return _destinationPosition; }
            set
            {
                _destinationPosition = value;

                foreach (DefaultParticleSystemMagnet cMagnet in MagnetList)
                    ((MagnetPoint)cMagnet).PositionData.Position = _destinationPosition;
            }
        }

        /// <summary>
        /// IsComplete property
        /// </summary>
        /// <value>
        /// true if the points have reached the player, false otherwise.
        /// </value>
        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                _isComplete = value;

                if (_isComplete)
                    PointsReceived(this, EventArgs.Empty);
            }
        }

        public event EventHandler PointsReceived = delegate(object sender, EventArgs e) { };

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>PointParticle</c> class.
        /// </summary>
        public PointParticle() : base(EngineManager.Game)
        {
            _isComplete = false;

            _minDistance = 0;
            _maxDistance = 50;
            _magnetsForce = 2.5f;

            _magnetsDistanceFunction = DefaultParticleSystemMagnet.DistanceFunctions.Constant;
        
            _magnetsMode = DefaultParticleSystemMagnet.MagnetModes.Attract;
        }

        #endregion

        #region AutoInitialize Method

        // 
        // Particle system properties should not be set until after this is called, as 
        // they are likely to be reset to their default values.
        /// <summary>
        /// Function to Initialize the Particle System with default values.
        /// </summary>
        /// <param name="graphicsDevice">Reference to the graphics device.</param>
        /// <param name="contentManager">Reference to the content manager.</param>
        /// <param name="spriteBatch">Reference to the sprite batch.</param>
        public override void AutoInitialize(GraphicsDevice graphicsDevice, 
            ContentManager contentManager, SpriteBatch spriteBatch)
        {
            // Initialize the Particle System before doing anything else
            InitializeTexturedQuadParticleSystem(graphicsDevice, contentManager, 100, 100,
                                                UpdateVertexProperties, "Content/Textures/ParticleTexture");

            // Finish loading the Particle System in a separate function call, so if
            // we want to reset the Particle System later we don't need to completely 
            // re-initialize it, we can just call this function to reset it.
            LoadParticleSystem();
        }

        #endregion

        #region LoadParticleSystem Method

        /// <summary>
        /// Load the Particle System Events and any other settings.
        /// </summary>
        public void LoadParticleSystem()
        {
            // Set the Particle Initialization Function, as it can be changed on the fly
            // and we want to make sure we are using the right one to start with to start with.
            ParticleInitializationFunction = InitializeParticleProperties;

            // Remove all Events first so that none are added twice if this function is called again
            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            // Setup the Emitter
            Emitter.ParticlesPerSecond = 100;
            //Emitter.PositionData.Position = new Vector3(-100, 50, 0);

            // Allow the Particle's Velocity, Rotational Velocity, Color, and Transparency to be updated each frame
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);

            // This function must be executed after the Color Lerp function as the Color Lerp will overwrite the Color's
            // Transparency value, so we give this function an Execution Order of 100 to make sure it is executed last.
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);

            // Update the particle to face the camera. Do this after updating it's rotation/orientation.
            ParticleEvents.AddEveryTimeEvent(UpdateParticleToFaceTheCamera, 200);

            ParticleEvents.AddEveryTimeEvent(CheckDistance, 200);
            ParticleSystemEvents.AddTimedEvent(TotalTimeInSeconds, MarkComplete);
            
            // Call function to add Magnet Particle Event
            ToogleMagnetsAffectPositionOrVelocity();

            // Specify to use the Point Magnet by default
            MagnetList.Clear();
            
            MagnetList.Add(new MagnetPoint(DestionationPosition,
                MagnetsMode, MagnetsDistanceFunction, 
                _minDistance, _maxDistance, MagnetsForce, 0));
        }

        #endregion

        #region InitializeParticleProperties Method

        /// <summary>
        /// Initialize the properties of a particle.
        /// </summary>
        /// <param name="particle">Reference to the particle to be initialized.</param>
        public void InitializeParticleProperties(DefaultTexturedQuadParticle particle)
        {
            // Set the Particle's Lifetime (how long it should exist for) and initial Position
            particle.Lifetime = 5;
            particle.Position = Emitter.PositionData.Position;

            // Give the Particle a random velocity direction to start with
            particle.Velocity = DPSFHelper.RandomNormalizedVector() * 0.5f;

            particle.Size = 0.2f;
            particle.Color = Color.Red;
        }

        #endregion

        #region ToogleMagnetsAffectPositionOrVelocity Method

        /// <summary>
        /// Toggles if Magnets should affect the Particles' Position or Velocity, and adds 
        /// the appropriate Particle Event to do so.
        /// </summary>
        public void ToogleMagnetsAffectPositionOrVelocity()
        {
            // Remove the previous Magnet Particle Events
            ParticleEvents.RemoveAllEventsInGroup(1);

            // Specify that Magnets should affect the Particles' Velocity
            //ParticleEvents.AddEveryTimeEvent(UpdateParticleVelocityAccordingToMagnets, 0, 1);
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAccordingToMagnets, 0, 1);
        }

        #endregion

        #region CheckDistance Method

        /// <summary>
        /// Check the distance between the points and the player.
        /// </summary>
        /// <param name="particle">Particle</param>
        /// <param name="elapsedTimeInSeconds">Elapsed time in seconds.</param>
        public void CheckDistance(DPSFDefaultBaseParticle particle, float elapsedTimeInSeconds)
        {
            if (Vector3.Distance(particle.Position, _destinationPosition) < 0.1f)
                particle.Visible = false;
        }

        #endregion

        #region MarkSplashScreenAsDonePlaying Method

        /// <summary>
        /// Mark the <c>PointParticle</c> completed.
        /// </summary>
        /// <param name="elapsedTimeInSeconds">Elapsed time in seconds.</param>
        public void MarkComplete(float elapsedTimeInSeconds)
        {
            IsComplete = true;
        }

        #endregion
    }
}
